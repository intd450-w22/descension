using System.Collections.Generic;
using System.Linq;
using Actor.Interface;
using JetBrains.Annotations;
using Managers;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Helpers;

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
            
            if (GUILayout.Button("Current Scene Only")) GenerateIdsForThisScene();
            
            GUILayout.Space(10);

            IUnique unique;
            if ((unique = Selection.activeObject.GetComponent<IUnique>()) != null)
                if (GUILayout.Button("Selected Object Only")) GenerateIdForSelected(unique);

        }

        private void GenerateIdForSelected(IUnique unique)
        {
            var serializedObject = new SerializedObject(unique as MonoBehaviour);
            var property = serializedObject.FindProperty("uniqueId");
            
            property.intValue = GenerateNewUniqueId(unique);
            serializedObject.ApplyModifiedProperties();
        }
        
        private void GenerateIdsForThisScene()
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

            GenerateIdsForScene();

            // workaround for false dirty mark
            EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex));
        }
        
        private int GenerateIdsForScene([CanBeNull] string scenePath = null)
        {
            scenePath ??= SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);
                
            EditorSceneManager.OpenScene(scenePath);
            
            var uniqueMonoBehaviour = FindObjectsOfType<MonoBehaviour>().Where(x => x is IUnique).ToArray();

            foreach (var unique in uniqueMonoBehaviour)
                GenerateIdForSelected(unique as IUnique);

            // only save if changes were made
            if (uniqueMonoBehaviour.Length != 0) EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

            Debug.Log("Generated " + uniqueMonoBehaviour.Length + " Unique Id's for " + scenePath);

            return uniqueMonoBehaviour.Length;
        }
        
        private void GenerateAllIds()
        {
            ClearUniqueIds();
            
            Debug.Log("Generating unique Id's for all scenes.");
            
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

            var startScenePath = 
                SceneUtility.GetScenePathByBuildIndex(SceneManager.GetActiveScene().buildIndex);;
            
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            
            var count = 0;
            for (var i = 0; i < sceneCount; i++)
                count += GenerateIdsForScene(SceneUtility.GetScenePathByBuildIndex(i));
            
            // return to original scene
            EditorSceneManager.OpenScene(startScenePath);
            
            Debug.Log("Generated " + count + " unique Id's in " + sceneCount + " scenes.");
            
            ClearUniqueIds();
        }
        
        private static readonly HashSet<int> UniqueIds = new HashSet<int>();

        private static void ClearUniqueIds() => UniqueIds.Clear();

        private static int GenerateNewUniqueId(IUnique unique)
        {
            var id = unique.GetInstanceID();
            while (UniqueIds.Contains(id)) ++id;
            UniqueIds.Add(id);
            unique.SetUniqueId(id);
            return id;
        }
        
    }
}