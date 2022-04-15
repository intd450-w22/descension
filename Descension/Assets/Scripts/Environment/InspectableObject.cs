using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Environment
{
    // public class InspectableObject : UniqueMonoBehaviour
    public class InspectableObject : AInteractable, IUnique
    {
        [SerializeField] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        public new string name;
        public string[] linesOfDialogue;
        
        public FactKey Fact;
        private bool _inspected;

        void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this)) Destroy(transform.parent.gameObject);
        }
        
        public void Inspect()
        {
            if (_inspected) return;
            
            _inspected = true;
                
            SoundManager.Inspection();
            FactManager.SetFact(Fact, true);
                
            DialogueManager.StartDialogue(name, linesOfDialogue, () =>
            {
                GameManager.DestroyUniquePermanent(this);
                Destroy(transform.parent.gameObject);
            });
        }

        public override void Interact() => Inspect();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press F to interact";
    }
}
