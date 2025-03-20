using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

namespace Unity.PackageExample.Editor
{
    public class PersistentSceneLoaderWindowEditor : EditorWindow
    {
        private static bool enable
        {
            get => EditorPrefs.GetBool("enablePersistentSceneLoader", false);
            set => EditorPrefs.SetBool("enablePersistentSceneLoader", value);
        }


        [MenuItem("Tools/Persistent Scene Loader", false)]
        private static void OpenWindow()
        {
            GetWindow<PersistentSceneLoaderWindowEditor>().Show();
        }

        static PersistentSceneLoaderWindowEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeSceneStateChange;
        }

        private void OnGUI()
        {
            GUILayout.TextArea(
                "If you enable this tool, " +
                "the first scene in Build Settings will always be the one loaded " +
                "first when you play the game in the Editor, " +
                "regardless of which scene you are in.", EditorStyles.helpBox);

            enable =  EditorGUILayout.Toggle("Enable:", enable);
        }

        private static void OnPlayModeSceneStateChange(PlayModeStateChange state)
        {
            SceneAsset firstScene = GetFirstSceneAsset();

            if (enable && EditorSceneManager.playModeStartScene == null)
                EditorSceneManager.playModeStartScene = firstScene;

            if (!enable && EditorSceneManager.playModeStartScene == firstScene)
                EditorSceneManager.playModeStartScene = null;
        }

        private static SceneAsset GetFirstSceneAsset()
        {
            EditorBuildSettingsScene editorFirstScene = EditorBuildSettings.scenes.Length > 0 ? EditorBuildSettings.scenes[0] : null;

            if (editorFirstScene == null)
                return null;

            string path = AssetDatabase.GUIDToAssetPath(editorFirstScene.guid);
            return AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
        }
    }
}