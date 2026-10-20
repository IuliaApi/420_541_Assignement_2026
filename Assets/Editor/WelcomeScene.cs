using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class WelcomeScene
{
    [InitializeOnLoadMethod]
    static void FirstOpen()
    {
        if (Application.isBatchMode) return;
        EditorApplication.delayCall += () =>
        {
            string key = "IuliasGame.Opened." + Application.dataPath;
            if (EditorPrefs.GetBool(key, false) || EditorApplication.isPlayingOrWillChangePlaymode) return;
            string path = "Assets/Scenes/MainMenu.unity";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(path) == null) return;
            var scene = SceneManager.GetActiveScene();
            if (!scene.isDirty && (string.IsNullOrEmpty(scene.path) || scene.path == "Assets/Scenes/SampleScene.unity"))
                EditorSceneManager.OpenScene(path);
            EditorPrefs.SetBool(key, true);
        };
    }
}
