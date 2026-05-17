// Assets/Editor/ConvertSpriteAssetsToPNGs_Exact.cs
// Converts Sprite .asset (or any Sprite) to PNG by rendering its mesh+UVs.
// Handles tight mesh, trimming, and atlas rotation. No reference remap.

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ConvertSpriteAssetsToPNGs : EditorWindow
{
    private DefaultAsset outputFolder;
    private bool useSelection = true;         // Convert selected Sprites/Textures
    private DefaultAsset scanFolder;          // Or scan a folder for Sprites
    private bool onlyAssetFiles;      // If true, only Sprites that are .asset files
    private bool skipExisting = true;         // Don't overwrite existing PNGs
    private bool deleteOriginalAsset; // Delete original .asset after export

    private static Material _spriteMat;       // Lazy Sprites/Default material

    [MenuItem("Tools/Sprites/Convert Sprite → PNG (Exact Mesh)")]
    public static void Open() => GetWindow<ConvertSpriteAssetsToPNGs>("Sprite → PNG (Exact)");

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "Renders Sprites using their real mesh + UVs (tight/trimmed/rotated OK) to PNG.\n" +
            "Use this when rect-cropping doesn't match what Unity displays.", MessageType.Info);

        useSelection = EditorGUILayout.ToggleLeft("Use current Selection (Sprites / Textures)", useSelection);
        using (new EditorGUI.DisabledScope(useSelection))
        {
            scanFolder = (DefaultAsset)EditorGUILayout.ObjectField("Or scan this Folder", scanFolder, typeof(DefaultAsset), false);
        }

        onlyAssetFiles = EditorGUILayout.Toggle("Only .asset Sprite files", onlyAssetFiles);
        skipExisting = EditorGUILayout.Toggle("Skip existing PNGs", skipExisting);
        deleteOriginalAsset = EditorGUILayout.Toggle("Delete original .asset after export", deleteOriginalAsset);

        EditorGUILayout.Space(8);
        outputFolder = (DefaultAsset)EditorGUILayout.ObjectField("Output Folder", outputFolder, typeof(DefaultAsset), false);

        EditorGUILayout.Space(12);
        using (new EditorGUI.DisabledScope(outputFolder == null || (!useSelection && scanFolder == null)))
        {
            if (GUILayout.Button("Export PNGs (Exact)")) ExportPNGsExact();
        }
    }

    private void ExportPNGsExact()
    {
        string outDir = AssetDatabase.GetAssetPath(outputFolder);
        if (!AssetDatabase.IsValidFolder(outDir))
        {
            EditorUtility.DisplayDialog("Invalid Output", "Please select a valid output folder.", "OK");
            return;
        }

        var sprites = CollectSprites(useSelection, scanFolder, onlyAssetFiles);
        if (sprites.Count == 0)
        {
            EditorUtility.DisplayDialog("Nothing to convert", "No matching Sprites found.", "OK");
            return;
        }

        EnsureMaterial();

        int exported = 0, skipped = 0, deleted = 0;

        for (int i = 0; i < sprites.Count; i++)
        {
            var s = sprites[i];
            EditorUtility.DisplayProgressBar("Exporting (Exact Mesh)", s.name, (float)i / sprites.Count);

            string safe = SanitizeFileName(s.name);
            string pngPath = Path.Combine(outDir, safe + ".png").Replace(
				Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar
			);
            if (skipExisting && File.Exists(pngPath)) { skipped++; continue; }

            Texture2D result = RenderSpriteToTexture(s);
            if (result == null) { Debug.LogWarning($"Failed to render '{s.name}'"); skipped++; continue; }

            byte[] bytes = result.EncodeToPNG();
            DestroyImmediate(result);
            File.WriteAllBytes(pngPath, bytes);

            // Import back as Single Sprite, preserving PPU, border, pivot
            AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceSynchronousImport);
            var ti = (TextureImporter)AssetImporter.GetAtPath(pngPath);
            ti.textureType = TextureImporterType.Sprite;
            ti.spriteImportMode = SpriteImportMode.Single;
            ti.spritePixelsPerUnit = s.pixelsPerUnit;
            ti.spriteBorder = s.border;
            //ti.spriteAlignment = (int)SpriteAlignment.Custom;
            var w = Mathf.RoundToInt(s.rect.width);
            var h = Mathf.RoundToInt(s.rect.height);
            ti.spritePivot = new Vector2(s.pivot.x / w, s.pivot.y / h);
            ti.textureCompression = TextureImporterCompression.Uncompressed;
            ti.isReadable = false;
            AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate);

            exported++;

            if (deleteOriginalAsset)
            {
                string origPath = AssetDatabase.GetAssetPath(s);
                if (!string.IsNullOrEmpty(origPath) && Path.GetExtension(origPath).ToLowerInvariant() == ".asset")
                {
                    if (AssetDatabase.DeleteAsset(origPath)) deleted++;
                    else Debug.LogWarning($"Could not delete original .asset: {origPath}");
                }
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();

        EditorUtility.DisplayDialog("Done",
            $"Exported: {exported}\nSkipped (exists): {skipped}\nDeleted original .asset: {deleted}", "OK");
    }

    private static List<Sprite> CollectSprites(bool fromSelection, DefaultAsset folder, bool onlyAssets)
    {
        var list = new List<Sprite>();

        if (fromSelection)
        {
            foreach (var o in Selection.objects)
            {
                if (o is Sprite sp)
                {
                    if (!onlyAssets || Path.GetExtension(AssetDatabase.GetAssetPath(sp)).ToLowerInvariant() == ".asset")
                        list.Add(sp);
                }
                else if (o is Texture2D)
                {
                    string p = AssetDatabase.GetAssetPath(o);
                    foreach (var rep in AssetDatabase.LoadAllAssetRepresentationsAtPath(p))
                        if (rep is Sprite sub)
                            if (!onlyAssets || Path.GetExtension(AssetDatabase.GetAssetPath(sub)).ToLowerInvariant() == ".asset")
                                list.Add(sub);
                }
            }
        }
        else
        {
            string dir = AssetDatabase.GetAssetPath(folder);
            if (!AssetDatabase.IsValidFolder(dir)) return list;

            foreach (var guid in AssetDatabase.FindAssets("t:Sprite", new[] { dir }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var sp = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                if (sp == null) continue;
                if (onlyAssets && Path.GetExtension(path).ToLowerInvariant() != ".asset") continue;
                list.Add(sp);
            }
        }

        return list;
    }

    private static void EnsureMaterial()
    {
        if (_spriteMat == null)
        {
            var shader = Shader.Find("Sprites/Default");
            _spriteMat = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        }
    }

    // Renders the exact visual sprite (mesh + UVs) into a Texture2D sized to sprite.rect (pixels)
    private static Texture2D RenderSpriteToTexture(Sprite s)
    {
        if (s == null || s.texture == null) return null;

        int w = Mathf.RoundToInt(s.rect.width);
        int h = Mathf.RoundToInt(s.rect.height);
        if (w <= 0 || h <= 0) return null;

        // Build a Mesh from sprite geometry
        var mesh = new Mesh();
        var verts = s.vertices;              // local units, pivot at (0,0)
        var tris = s.triangles;             // ushort indices
        var uvs = s.uv;                    // matches verts

        // Convert local-unit verts → pixel space within [0..w],[0..h]
        float ppu = s.pixelsPerUnit;
        var v3 = new Vector3[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            // Local (0,0) is pivot; move into rect pixel space
            float px = verts[i].x * ppu + s.pivot.x;
            float py = verts[i].y * ppu + s.pivot.y;
            v3[i] = new Vector3(px, py, 0f);
        }

        var tri32 = new int[tris.Length];
        for (int i = 0; i < tris.Length; i++) tri32[i] = tris[i];

        mesh.vertices = v3;
        mesh.uv = uvs;
        mesh.triangles = tri32;
        mesh.RecalculateBounds();

        // Render to RT in pixel space
        var rt = new RenderTexture(w, h, 0, RenderTextureFormat.ARGB32);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, w, 0, h);
        GL.Clear(true, true, Color.clear);

        _spriteMat.mainTexture = s.texture;
        _spriteMat.SetPass(0);
        Graphics.DrawMeshNow(mesh, Matrix4x4.identity);

        // Read back
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, w, h), 0, 0, false);
        tex.Apply(false, false);

        // Cleanup
        GL.PopMatrix();
        RenderTexture.active = prev;
        rt.Release();
        DestroyImmediate(rt);
        DestroyImmediate(mesh);

        return tex;
    }

    private static string SanitizeFileName(string n)
    {
        foreach (char c in Path.GetInvalidFileNameChars()) n = n.Replace(c, '_');
        return n;
    }
}
