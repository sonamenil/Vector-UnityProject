#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Xml2Prefab;

public static class VectorLevelEditorMenu
{
    private const string EmptyTransformXml =
        "<Properties><Dynamic /></Properties>";

    private const string IdentityMatrixXml =
        "<Matrix A=\"1\" B=\"0\" C=\"0\" D=\"1\" />";

    [MenuItem("Vector Level Editor/Create Level Root")]
    public static void CreateLevelRoot()
    {
        var root = new GameObject("EditorLevelRoot");
        Undo.RegisterCreatedObjectUndo(root, "Create Level Root");

        root.AddComponent<Xml2PrefabLevelContainer>();

        var visualLayer = new GameObject("VisualLayer_Factor_1");
        Undo.RegisterCreatedObjectUndo(visualLayer, "Create Visual Layer");
        visualLayer.transform.SetParent(root.transform, false);

        var visual = visualLayer.AddComponent<Xml2PrefabVisualContainer>();
        visual.Init(1f);

        var objectGo = new GameObject("LevelObject_01");
        Undo.RegisterCreatedObjectUndo(objectGo, "Create Level Object");
        objectGo.transform.SetParent(visualLayer.transform, false);

        var objectContainer = objectGo.AddComponent<Xml2PrefabObjectRunnerContainer>();
        objectContainer.Init(
            "LevelObject_01",
            0,
            0,
            1f,
            0,
            "",
            "",
            new System.Collections.Generic.List<Component>(),
            null
        );

        Selection.activeGameObject = root;
    }

    [MenuItem("Vector Level Editor/Create Platform")]
    public static void CreatePlatform()
    {
        var parent = GetSelectedOrFirstObjectContainer();
        if (parent == null)
        {
            Debug.LogError("Select an Xml2PrefabObjectRunnerContainer first.");
            return;
        }

        var go = new GameObject("Platform");
        Undo.RegisterCreatedObjectUndo(go, "Create Platform");
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = Vector3.zero;

        var platform = go.AddComponent<Xml2PrefabPlatformContainer>();
        platform.Init(
            "Platform",
            0,
            0,
            300,
            40,
            false,
            EmptyTransformXml,
            null
        );

        platform.CreateInnerController();

        Selection.activeGameObject = go;
    }

    [MenuItem("Vector Level Editor/Create Trigger")]
    public static void CreateTrigger()
    {
        var parent = GetSelectedOrFirstObjectContainer();
        if (parent == null)
        {
            Debug.LogError("Select an Xml2PrefabObjectRunnerContainer first.");
            return;
        }

        var go = new GameObject("Platform");
        Undo.RegisterCreatedObjectUndo(go, "Create Platform");
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = Vector3.zero;

        var trigger = go.AddComponent<Xml2PrefabTriggerContainer>();
        trigger.Init(
            "",
            40,
            300,
            null
        );

        trigger.CreateInnerController();

        Selection.activeGameObject = go;
    }

    [MenuItem("Vector Level Editor/Create Area")]
    public static void CreateArea()
    {
        var parent = GetSelectedOrFirstObjectContainer();
        if (parent == null)
        {
            Debug.LogError("Select an Xml2PrefabObjectRunnerContainer first.");
            return;
        }

        var go = new GameObject("Platform");
        Undo.RegisterCreatedObjectUndo(go, "Create Platform");
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = Vector3.zero;

        var area = go.AddComponent<Xml2PrefabAreaContainer>();
        area.Init(
            EmptyTransformXml,
            "Animation",
            "Area",
            0,
            0,
            100,
            200,
            null
        );

        area.CreateInnerController();

        Selection.activeGameObject = go;
    }

    [MenuItem("Vector Level Editor/Create Spawn")]
    public static void CreateSpawn()
    {
        var parent = GetSelectedOrFirstObjectContainer();
        if (parent == null)
        {
            Debug.LogError("Select an Xml2PrefabObjectRunnerContainer first.");
            return;
        }

        var go = new GameObject("Spawn");
        Undo.RegisterCreatedObjectUndo(go, "Create Spawn");
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = Vector3.zero;

        var spawn = go.AddComponent<Xml2PrefabSpawnContainer>();
        spawn.Init(
            0,
            0,
            "spawn_0",
            "",
            EmptyTransformXml,
            null
        );

        Selection.activeGameObject = go;
    }

    [MenuItem("Vector Level Editor/Create Camera")]
    public static void CreateCamera()
    {
        var parent = GetSelectedOrFirstObjectContainer();
        if (parent == null)
        {
            Debug.LogError("Select an Xml2PrefabObjectRunnerContainer first.");
            return;
        }

        var go = new GameObject("Camera");
        Undo.RegisterCreatedObjectUndo(go, "Create Camera");
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = Vector3.zero;

        var camera = go.AddComponent<Xml2PrefabCameraContainer>();
        camera.Init(
            0,
            0,
            "camera_0",
            "",
            EmptyTransformXml,
            null
        );

        Selection.activeGameObject = go;
    }

    [MenuItem("Vector Level Editor/Create Visual")]
    public static void CreateVisual()
    {
        var parent = GetSelectedOrFirstObjectContainer();
        if (parent == null)
        {
            Debug.LogError("Select an Xml2PrefabObjectRunnerContainer first.");
            return;
        }

        var go = new GameObject("Visual");
        Undo.RegisterCreatedObjectUndo(go, "Create Visual");
        go.transform.SetParent(parent.transform, false);
        go.transform.localPosition = Vector3.zero;

        var visual = go.AddComponent<Xml2PrefabVisualRunnerContainer>();
        visual.Init(
            1,
            "your_sprite_name_here",
            100,
            100,
            Color.white,
            0,
            IdentityMatrixXml,
            0,
            0,
            EmptyTransformXml,
            null
        );

        Selection.activeGameObject = go;
    }

    private static Xml2PrefabObjectRunnerContainer GetSelectedOrFirstObjectContainer()
    {
        if (Selection.activeGameObject != null)
        {
            var selected = Selection.activeGameObject.GetComponentInParent<Xml2PrefabObjectRunnerContainer>();

            if (selected != null)
            {
                return selected;
            }
        }

        return Object.FindObjectOfType<Xml2PrefabObjectRunnerContainer>();
    }
}
#endif