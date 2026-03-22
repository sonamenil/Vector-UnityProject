// Assets/Editor/ExportSpritesToPNGsAndRemap.cs
// Unity 2020+ (tested newer too). Place in an Editor/ folder.

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ExportSpritesToPNGsAndRemap : EditorWindow
{
    private Texture2D spritesheet;         // Source texture (Sprite 2D, Multiple, sliced)
    private DefaultAsset outputFolder;      // Destination folder for .png files
    private bool remapReferences = true;    // Update all serialized references across project
    private bool remapAllAssets = true;
    private DefaultAsset remapFolder;
    private bool skipExisting = true;       // Skip if a .png with same name already exists

    [MenuItem("Tools/Sprites/Export Sub-Sprites to PNGs (+ Remap)")]
    public static void Open()
    {
        GetWindow<ExportSpritesToPNGsAndRemap>("Export Sprites to PNGs");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Source spritesheet (Sprite Mode: Multiple)", EditorStyles.boldLabel);
        spritesheet = (Texture2D)EditorGUILayout.ObjectField("Spritesheet", spritesheet, typeof(Texture2D), false);

        EditorGUILayout.Space(8);
        EditorGUILayout.LabelField("Output", EditorStyles.boldLabel);
        outputFolder = (DefaultAsset)EditorGUILayout.ObjectField("Folder", outputFolder, typeof(DefaultAsset), false);

        EditorGUILayout.Space(8);
        remapReferences = EditorGUILayout.Toggle("Remap references in project", remapReferences);
        using (new EditorGUI.DisabledScope(!remapReferences))
        {
            remapAllAssets = EditorGUILayout.Toggle("Remap every asset in project", remapAllAssets);
        }
        using (new EditorGUI.DisabledScope(!remapReferences || remapAllAssets))
        {
            remapFolder = (DefaultAsset)EditorGUILayout.ObjectField("Remap Folder", remapFolder, typeof(DefaultAsset), false);
        }
        skipExisting = EditorGUILayout.Toggle("Skip existing PNGs", skipExisting);

        EditorGUILayout.Space(12);
        using (new EditorGUI.DisabledScope(spritesheet == null || outputFolder == null || (remapReferences && (!remapAllAssets && remapFolder == null))))
        {
            if (GUILayout.Button("Export PNGs & Remap"))
            {
                ExportAndRemapPNGs();
            }
        }

        EditorGUILayout.Space(8);
        EditorGUILayout.HelpBox(
            "• The source texture must be Sprite (2D and UI) with Sprite Mode = Multiple and sliced.\n" +
            "• This creates one .png per sub-sprite and imports each as a Single-sprite texture.\n" +
            "• If 'Remap references' is enabled, any references to the old sub-sprites are replaced with the new .png sprites.",
            MessageType.Info
        );
    }

    private void ExportAndRemapPNGs()
    {
        string srcPath = AssetDatabase.GetAssetPath(spritesheet);
        if (string.IsNullOrEmpty(srcPath))
        {
            EditorUtility.DisplayDialog("Error", "Invalid spritesheet asset path.", "OK");
            return;
        }

        // Collect sub-sprites from the sliced texture
        Object[] subs = AssetDatabase.LoadAllAssetRepresentationsAtPath(srcPath);
        List<Sprite> oldSubSprites = new List<Sprite>();
        foreach (var o in subs)
            if (o is Sprite s) oldSubSprites.Add(s);

        if (oldSubSprites.Count == 0)
        {
            EditorUtility.DisplayDialog("No sub-sprites found",
                "The texture doesn’t appear to be sliced (Sprite Mode = Multiple).", "OK");
            return;
        }

        string outFolderPath = AssetDatabase.GetAssetPath(outputFolder);
        if (!AssetDatabase.IsValidFolder(outFolderPath))
        {
            EditorUtility.DisplayDialog("Error", "Select a valid output folder.", "OK");
            return;
        }

        // Ensure source is temporarily readable (so we can copy pixels)
        var srcImporter = (TextureImporter)AssetImporter.GetAtPath(srcPath);
        bool originalReadable = srcImporter.isReadable;
        var originalCompression = srcImporter.textureCompression;

        try
        {
            if (!originalReadable)
            {
                srcImporter.isReadable = true;
                // Lossless path helps avoid artifacts when copying
                srcImporter.textureCompression = TextureImporterCompression.Uncompressed;
                AssetDatabase.ImportAsset(srcPath, ImportAssetOptions.ForceUpdate);
            }

            // Read all pixels from the source into a readable Texture2D copy (handles compressed formats)
            Texture2D readableSource = GetReadableCopy(spritesheet);

            // Create map: old sub-sprite -> new imported .png sprite
            var remapMap = new Dictionary<Sprite, Sprite>();

            for (int i = 0; i < oldSubSprites.Count; i++)
            {
                var oldS = oldSubSprites[i];
                EditorUtility.DisplayProgressBar("Exporting PNGs", oldS.name, (float)i / oldSubSprites.Count);

                string safeName = SanitizeFileName(oldS.name);
                string pngPath = Path.Combine(outFolderPath, safeName + ".png").Replace("\\", "/");

                if (skipExisting && File.Exists(pngPath))
                {
                    // Load the sprite created by importer
                    var existingSprite = LoadSingleSpriteAtPath(pngPath);
                    if (existingSprite != null)
                    {
                        remapMap[oldS] = existingSprite;
                        continue;
                    }
                }

                // Crop pixels from the readable source by rect
                Rect r = oldS.rect;
                int w = Mathf.RoundToInt(r.width);
                int h = Mathf.RoundToInt(r.height);
                int x = Mathf.RoundToInt(r.x);
                int y = Mathf.RoundToInt(r.y);

                // Note Unity’s texture origin is bottom-left; rect coordinates already match that convention
                Color[] pixels = readableSource.GetPixels(x, y, w, h);

                var newTex = new Texture2D(w, h, TextureFormat.RGBA32, false);
                newTex.SetPixels(pixels);
                newTex.Apply(false, false);

                // Encode and write PNG
                byte[] png = newTex.EncodeToPNG();
                DestroyImmediate(newTex);
                File.WriteAllBytes(pngPath, png);

                // Import the PNG as a Single Sprite with matching settings
                AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceSynchronousImport);
                var newImp = (TextureImporter)AssetImporter.GetAtPath(pngPath);
                newImp.textureType = TextureImporterType.Sprite;
                newImp.spriteImportMode = SpriteImportMode.Single;
                newImp.spritePixelsPerUnit = oldS.pixelsPerUnit;
                newImp.spriteBorder = oldS.border;

                // Preserve pivot (convert from pixel pivot to normalized)
                Vector2 pivotNormalized = new Vector2(
                    oldS.pivot.x / r.width,
                    oldS.pivot.y / r.height
                );
                newImp.spritePivot = pivotNormalized;

                // Avoid compression artifacts if these are UI/pixel art
                newImp.textureCompression = TextureImporterCompression.Uncompressed;
                newImp.isReadable = false; // no need to keep readable

                AssetDatabase.ImportAsset(pngPath, ImportAssetOptions.ForceUpdate);

                // Load the resulting Sprite sub-asset produced by importer
                var newSprite = LoadSingleSpriteAtPath(pngPath);
                if (newSprite == null)
                {
                    Debug.LogWarning($"Could not load Sprite from {pngPath}");
                }
                else
                {
                    // Keep the same logical name for convenience
                    newSprite.name = oldS.name;
                    remapMap[oldS] = newSprite;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (remapReferences)
                RemapAllReferences(remapMap, remapAllAssets, AssetDatabase.GetAssetPath(remapFolder));
        }
        finally
        {
            // Restore importer settings
            srcImporter.isReadable = originalReadable;
            srcImporter.textureCompression = originalCompression;
            AssetDatabase.ImportAsset(srcPath, ImportAssetOptions.ForceUpdate);

            EditorUtility.ClearProgressBar();
        }

        EditorUtility.DisplayDialog(
            "Done",
            $"Exported sprites to PNGs in:\n{outFolderPath}\n" +
            (remapReferences ? "and remapped references across the project." : " (no remap)."),
            "OK"
        );
    }

    // ---- Helpers ----

    private static Texture2D GetReadableCopy(Texture2D source)
    {
        // Works for compressed/non-readable textures by GPU blit to a temporary RT, then ReadPixels
        RenderTexture rt = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.ARGB32);
        Graphics.Blit(source, rt);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D tex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
        tex.Apply(false, false);

        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rt);
        return tex;
    }

    private static Sprite LoadSingleSpriteAtPath(string texturePath)
    {
        // For a Single-sprite texture, the Sprite is a sub-asset; this loads it directly.
        var s = AssetDatabase.LoadAssetAtPath<Sprite>(texturePath);
        if (s != null) return s;

        // Fallback: search sub-assets
        var subs = AssetDatabase.LoadAllAssetRepresentationsAtPath(texturePath);
        foreach (var o in subs)
            if (o is Sprite sp) return sp;

        return null;
    }

    private static string SanitizeFileName(string n)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            n = n.Replace(c, '_');
        return n;
    }

    private static void RemapAllReferences(Dictionary<Sprite, Sprite> map, bool remapAll, string remapFolder)
    {
        if (map == null || map.Count == 0) return;
        if (remapAll)
        {
            remapFolder = "Assets";
        }
        // ---------- PASS 1: Non-scene/prefab assets ----------
        string[] assetGuids = AssetDatabase.FindAssets("",
            new[] { remapFolder }); // search whole project; restrict if you prefer
        int changedAssetFiles = 0;
        AssetDatabase.StartAssetEditing();
        try
        {
            for (int i = 0; i < assetGuids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(assetGuids[i]);
                string ext = Path.GetExtension(path).ToLowerInvariant();

                // Skip scenes/prefabs here; handled in passes 2/3
                if (ext == ".prefab" || ext == ".unity") continue;

                EditorUtility.DisplayProgressBar("Remapping (assets)", path, (float)i / assetGuids.Length);

                bool changed = false;
                var main = AssetDatabase.LoadMainAssetAtPath(path);
                if (main != null && RemapObjectReferences(main, map))
                    changed = true;

                var subs = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);
                foreach (var o in subs)
                    if (RemapObjectReferences(o, map))
                        changed = true;

                if (changed && main != null)
                {
                    EditorUtility.SetDirty(main);
                    changedAssetFiles++;
                }
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }

        // ---------- PASS 2: Prefabs ----------
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { remapFolder });
        int changedPrefabs = 0;

        for (int i = 0; i < prefabGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(prefabGuids[i]);
            EditorUtility.DisplayProgressBar("Remapping (prefabs)", path, (float)i / prefabGuids.Length);

            // Load prefab contents into a temporary editable scene
            GameObject root = PrefabUtility.LoadPrefabContents(path);
            if (root == null) continue;

            bool changed = RemapInGameObjectHierarchy(root, map);

            if (changed)
            {
                changedPrefabs++;
                PrefabUtility.SaveAsPrefabAsset(root, path);
            }

            PrefabUtility.UnloadPrefabContents(root);
        }
        EditorUtility.ClearProgressBar();

        // ---------- PASS 3: Scenes ----------
        string[] sceneGuids = AssetDatabase.FindAssets("t:Scene", new[] { remapFolder });
        int changedScenes = 0;

        for (int i = 0; i < sceneGuids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
            EditorUtility.DisplayProgressBar("Remapping (scenes)", path, (float)i / sceneGuids.Length);

            // Open additively so we don't disturb the current scene
            var scene = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
            bool changed = false;

            foreach (var root in scene.GetRootGameObjects())
                if (RemapInGameObjectHierarchy(root, map))
                    changed = true;

            if (changed)
            {
                changedScenes++;
                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
            }

            EditorSceneManager.CloseScene(scene, true);
        }
        EditorUtility.ClearProgressBar();

        Debug.Log($"Sprite PNG Remap: assets changed={changedAssetFiles}, prefabs changed={changedPrefabs}, scenes changed={changedScenes}");
    }

    // Walks all components & children, remapping any Sprite refs.
    private static bool RemapInGameObjectHierarchy(GameObject root, Dictionary<Sprite, Sprite> map)
    {
        bool changed = false;
        var comps = root.GetComponentsInChildren<Component>(true);

        foreach (var c in comps)
        {
            if (!c) continue;
            // SerializedObject walk replaces both single refs and array/list elements
            var so = new SerializedObject(c);
            var it = so.GetIterator();

            while (it.Next(true))
            {
                if (it.propertyType == SerializedPropertyType.ObjectReference)
                {
                    var curr = it.objectReferenceValue;
                    if (curr is Sprite s && s != null && map.TryGetValue(s, out var replacement))
                    {
                        Undo.RecordObject(c, "Remap Sprite");
                        it.objectReferenceValue = replacement;
                        changed = true;
                    }
                }
            }

            if (changed) so.ApplyModifiedProperties();
            if (changed) EditorUtility.SetDirty(c);
        }

        return changed;
    }

    private static bool RemapObjectReferences(Object obj, Dictionary<Sprite, Sprite> map)
    {
        if (obj == null) return false;

        bool changed = false;
        var so = new SerializedObject(obj);
        var prop = so.GetIterator();

        while (prop.Next(true))
        {
            if (prop.propertyType == SerializedPropertyType.ObjectReference)
            {
                var curr = prop.objectReferenceValue;
                if (curr is Sprite s && s != null && map.TryGetValue(s, out var replacement))
                {
                    prop.objectReferenceValue = replacement;
                    changed = true;
                }
            }
        }

        if (changed) so.ApplyModifiedProperties();
        return changed;
    }
}
