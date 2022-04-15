using System;
using Actor.Interface;
using Actor.Player;
using Managers;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;
using Random = UnityEngine.Random;

namespace Items
{
    public class RopeItem : MonoBehaviour, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        public int quantity = 1;
        public Vector2[] potentialPositions;

        private bool _isPickedUp = false;

        private string[] _description = 
            new string[] {"Rope Collected.", "Be sure to hold on with both hands while descending. When ascending...well...God help you."};

        void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this, out var location)) transform.position = location;
            else transform.position = potentialPositions[Random.Range(0, potentialPositions.Length)];
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isPickedUp) return;

            SoundManager.ItemFound();
            if (other.gameObject.CompareTag("Player"))
            {
                _isPickedUp = true;
                GameManager.DestroyUnique(this, transform.position);
                PlayerController.AddRope(quantity);
                DialogueManager.StartDialogue("Rope", _description);
                FactManager.SetFact(FactKey.HasSeenRope, true);
                Destroy(gameObject);
            }
        }
    }
}
