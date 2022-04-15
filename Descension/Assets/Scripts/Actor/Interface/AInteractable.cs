using Actor.Player;
using UnityEngine;

namespace Actor.Interface
{
    public abstract class AInteractable : MonoBehaviour
    {
        protected bool _inRange;

        public abstract void Interact();
        public abstract Vector2 Location();
        public abstract string GetPrompt();

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _inRange = true;
                PlayerController.AddInteractableInRange(gameObject.GetInstanceID(), this);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _inRange = false;
                PlayerController.RemoveInteractableInRange(gameObject.GetInstanceID());
            }
        }
    }
}