#if UNITY_EDITOR

using UnityEditor.AssetImporters;
using UnityEngine;
using System.IO;

[ScriptedImporter(1, "bin")]
public class BinImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        byte[] bytes = File.ReadAllBytes(ctx.assetPath);

        TextAsset asset = new TextAsset(bytes);

        ctx.AddObjectToAsset("main", asset);
        ctx.SetMainObject(asset);
    }
}

#endif