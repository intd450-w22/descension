using UnityEditor;
using UnityEngine.SceneManagement;

namespace Util
{
    public static class SceneLoader
    {
        
        public static void Load(SceneAsset scene)
        {
            SceneManager.LoadScene(scene.name);
        }
        
        public static void Load(Scene scene)
        {
            SceneManager.LoadScene(scene.name);
        }
    }
}
