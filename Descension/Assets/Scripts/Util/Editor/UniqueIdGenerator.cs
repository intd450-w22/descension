using System.Collections.Generic;
using Actor.Interface;
using Environment;
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
            if (GUILayout.Button("Generate Id's")) GenerateIds();
            
            if (GUILayout.Button("Clear Used Id")) ClearUsedIds();
            
        }
        
        private void GenerateIds()
        {
            Debug.Log("Generating Id's from " + _ids.Count);

            foreach (var uniqueObject in FindObjectsOfType<UniqueMonoBehaviour>())
            {
                var serializedObject = new SerializedObject(uniqueObject);
                var property = serializedObject.FindProperty("uniqueId");
                var id = uniqueObject.GetInstanceID();
                while (_ids.Contains(id)) ++id;
                property.intValue = id;
                _ids.Add(id);
                serializedObject.ApplyModifiedProperties();
            }
            
            Debug.Log("Id's Generated to " + _ids.Count + ", make sure to save the scene.");
        }

        private void ClearUsedIds()
        {
            _ids.Clear();
        }
        
    }
}