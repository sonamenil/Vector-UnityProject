#if UNITY_EDITOR
using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO;

[ScriptedImporter(1, "plist")]
public class PlistImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        string text = File.ReadAllText(ctx.assetPath);

        TextAsset asset = new TextAsset(text);

        ctx.AddObjectToAsset("main", asset);
        ctx.SetMainObject(asset);
    }
}
#endif