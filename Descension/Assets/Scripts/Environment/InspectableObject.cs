using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.Enums;

namespace Environment
{
    public class InspectableObject : AInteractable
    {
        public string name;
        public string[] linesOfDialogue;
        public bool destroyAfterInteraction = true;
        public FactKey Fact;

        private bool _inspected;

        public void Inspect()
        {
            _inspected = true;
                
            SoundManager.Inspection();
            FactManager.SetFact(Fact, true);
                
            DialogueManager.StartDialogue(name, linesOfDialogue, () =>
            {
                if (destroyAfterInteraction) Destroy(transform.parent.gameObject);
            });
        }

        public override void Interact() => Inspect();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press F to interact";
    }
}
