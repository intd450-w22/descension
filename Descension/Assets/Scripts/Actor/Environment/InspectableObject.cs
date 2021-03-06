using Actor.Interface;
using Managers;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;

namespace Actor.Environment
{
    // public class InspectableObject : UniqueMonoBehaviour
    public class InspectableObject : AInteractable, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
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
