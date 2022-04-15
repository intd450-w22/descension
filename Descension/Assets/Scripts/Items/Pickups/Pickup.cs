using System;
using System.Collections.Generic;
using Actor.Interface;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.EditorHelpers;
using Util.Helpers;

namespace Items.Pickups
{
    public class Pickup : AInteractable, IUnique
    {

        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;

        [ReadOnly] public GameObject prefab;
        [ReadOnly] public Vector3 position;
        
        public EquippableItem item;
        public int quantity = 1;
        public string[] pickupMessage;
        public bool autoPickup;
        public bool spawned;

        protected void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this)) Destroy(gameObject);
        }
        
        protected void OnEnable() 
        {
            if (GetUniqueId() == 0)
            {
                spawned = true;
                GameManager.AddPreSceneChangeCallback(SceneChangeCallback);
            }
        }

        private void OnDestroy() => GameManager.RemovePreSceneChangeCallback(SceneChangeCallback);

        void SceneChangeCallback() => SpawnManager.CachePickup(this);

        public void TryPickup()
        {
            if (!InventoryManager.PickupItem(item, ref quantity))
            {
                SoundManager.Error(); //TODO fail to pick up sound
                UIManager.GetHudController().ShowDialogue("Inventory full");
                return;
            }
            
            SoundManager.ItemFound();

            if (quantity == 0)
            {
                if (!spawned) GameManager.DestroyUnique(this);
                Destroy(gameObject);
            }
                
            // only show pickup dialogue once
            if (!FactManager.IsFactTrue(item.Fact))
            {
                DialogueManager.StartDialogue(item.GetName(), pickupMessage);
                FactManager.SetFact(item.Fact, true);
            }
        }

        private void OnValidate() => gameObject.GetChildObject("ItemSprite").GetComponent<SpriteRenderer>().sprite = item.inventorySprite;

        public override void Interact() => TryPickup();
        public override Vector2 Location() => gameObject.transform.position;
        public override string GetPrompt() => "Press F to pick up " + item.GetName();
    }
}
