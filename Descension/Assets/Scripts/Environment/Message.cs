using Actor.Interface;
using Managers;
using UI.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Util.EditorHelpers;

namespace Environment
{
    public class Message : MonoBehaviour, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        public new string name;
        public string[] linesOfDialogue;
        public Image dialogueBox;
        public Text dialogueText;

        public bool triggered = false;

        private HUDController _hudController;

        void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this)) Destroy(gameObject);
            else _hudController = UIManager.GetHudController();
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
