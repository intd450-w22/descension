using System.Collections.Generic;
using Actor.Interface;
using Environment;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Util.Editor
{
    public class UniqueIdGenerator : EditorWindow
    {
        private static HashSet<int> _ids = new HashSet<int>();

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
            _ids.Clear();
            GenerateIds();

            // workaround for false dirty mark
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex));
        }

        private int GenerateIds([CanBeNull] string scenePath = null)
        {
            scenePath ??= SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
                
            EditorSceneManager.OpenScene(scenePath);
            
            var uniqueObjects = FindObjectsOfType<UniqueMonoBehaviour>();
            foreach (var uniqueObject in uniqueObjects)
            {
                var serializedObject = new SerializedObject(uniqueObject);
                var property = serializedObject.FindProperty("uniqueId");
                var id = uniqueObject.GetInstanceID();
                while (_ids.Contains(id) || id == 0) ++id;
                property.intValue = id;
                _ids.Add(id);
                serializedObject.ApplyModifiedProperties();
            }
            
            // only save if changes were made
            if (uniqueObjects.Length != 0) EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
            
            Debug.Log("Generated " + uniqueObjects.Length + " Unique Id's for " + scenePath);
            
            return uniqueObjects.Length;
        }
        
        private void GenerateAllIds()
        {
            _ids.Clear();
            
            Debug.Log("Generating unique Id's for all scenes.");

            var startScenePath = 
                SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);;
            
            var sceneCount = SceneManager.sceneCountInBuildSettings;

            for (var i = 0; i < sceneCount; i++)
                GenerateIds(SceneUtility.GetScenePathByBuildIndex(i));
            
            // return to original scene
            EditorSceneManager.OpenScene(startScenePath);
            
            Debug.Log("Generated " + _ids.Count + " unique Id's in " + sceneCount + " scenes.");
            
            _ids.Clear();
        }
        
    }
}