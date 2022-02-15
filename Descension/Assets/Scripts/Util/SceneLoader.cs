using UnityEngine.SceneManagement;

namespace Assets.Scripts.Util
{
    public static class SceneLoader
    {
        public static void Load(Util.Enums.Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }

        public static void Load(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
