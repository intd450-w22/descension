using UnityEngine;

namespace Util.Helpers
{
    public class Note : MonoBehaviour
    {
        [TextArea] public string notes = "Comment...";
        void Awake()
        {
            enabled = false;
        }
    }
}
