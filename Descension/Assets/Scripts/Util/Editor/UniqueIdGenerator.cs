using System.Collections.Generic;
using System.Linq;
using Actor.Interface;
using Environment;
using JetBrains.Annotations;
using Managers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util.Editor
{
    public class UniqueIdGenerator : EditorWindow
    {
        // private static HashSet<int> _ids = new HashSet<int>();

        [MenuItem("Window/Unique Id Generator")]
        public static void ShowWindow()
        {
            GetWindow<UniqueIdGenerator>("Unique Id Generator");
        }
 
        private void OnGUI()
        {
            GUILayout.Box("Generates Unique Id's for all UniqueMonoBehaviour objects.\n\n" +
                          "All Scenes is recommended to ensure no collisions.\n\n" +
                          "If working with a scene not included Scenes In Build, use Current Scene Only.\n\n" +
                          "WARNING: Will also save all modified scenes.");
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("All Scenes In\nFile->Build Settings->Scenes In Build")) GenerateAllIds();

            GUILayout.Space(10);
            
            if (GUILayout.Button("Current Scene Only")) GenerateIdsForScene();

        }

        private void GenerateIdsForScene()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            
            GenerateIds();

            // workaround for false dirty mark
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex));
        }

        public static IEnumerable<T> FindInterfacesOfType<T>(bool includeInactive = false)
        {
            return SceneManager.GetActiveScene().GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<T>(includeInactive));
        }
        
        private int GenerateIds([CanBeNull] string scenePath = null)
        {
            scenePath ??= SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
                
            EditorSceneManager.OpenScene(scenePath);
            
            var uniqueObjects = FindObjectsOfType<MonoBehaviour>().Where(x => x is IUnique).ToArray();

            foreach (var uniqueObject in uniqueObjects)
            {
                var serializedObject = new SerializedObject(uniqueObject);
                var property = serializedObject.FindProperty("uniqueId");
                
                property.intValue = GameManager.GenerateNewUniqueId(uniqueObject.GetComponent<IUnique>());
                serializedObject.ApplyModifiedProperties();
            }

            // only save if changes were made
            if (uniqueObjects.Length != 0) EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

            Debug.Log("Generated " + uniqueObjects.Length + " Unique Id's for " + scenePath);

            return uniqueObjects.Length;
            
        }
        
        private void GenerateAllIds()
        {
            GameManager.ClearUniqueIds();
            
            Debug.Log("Generating unique Id's for all scenes.");
            
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

            var startScenePath = 
                SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);;
            
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            
            var count = 0;
            for (var i = 0; i < sceneCount; i++)
                count += GenerateIds(SceneUtility.GetScenePathByBuildIndex(i));
            
            // return to original scene
            EditorSceneManager.OpenScene(startScenePath);
            
            Debug.Log("Generated " + count + " unique Id's in " + sceneCount + " scenes.");
            
        }
        
    }
}