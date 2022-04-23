using Actor.Interface;
using Managers;
using UnityEngine;
using Util.EditorHelpers;

namespace Actor.Environment
{
    public class Message : MonoBehaviour, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        public new string name;
        public string[] linesOfDialogue;

        public bool triggered;
        
        void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this)) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!triggered) {
                triggered = true;
                
                SoundManager.Inspection();
                
                DialogueManager.StartDialogue(name, linesOfDialogue, () =>
                {
                    GameManager.DestroyUniquePermanent(this);
                    Destroy(gameObject);
                });
            }
        }
    }
}
