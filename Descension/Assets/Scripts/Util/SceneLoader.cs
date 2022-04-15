using UnityEditor;
using UnityEngine.SceneManagement;

namespace Util
{
    public static class SceneLoader
    {
        
        public static void Load(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        
        public static void Load(Scene scene)
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
