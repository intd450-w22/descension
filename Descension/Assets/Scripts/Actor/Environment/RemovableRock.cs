using Actor.Interface;
using Managers;
using UnityEngine;
using Util.EditorHelpers;
using Util.Helpers;

namespace Actor.Environment
{
    public class RemovableRock : MonoBehaviour, IUnique
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;

        public int goldDropMin = 1;
        public int goldDropMax = 20;
        public int goldDropChance = 40;  // percent chance of dropping gold in range (goldDropMin, goldDropMax)
        public SpawnManager.DropStruct[] itemDrops;
        

        void Awake()
        {
            if (GameManager.IsUniqueDestroyed(this))
            {
                GameDebug.Log("Destroying " + GetUniqueId());
                Destroy(gameObject);
            }
        }
        
        public void OnDestroyed()
        {
            if (Random.Range(0, 100) < goldDropChance)
            {
                int gold = Random.Range(goldDropMin, goldDropMax + 1);
                    
                SoundManager.GoldFound();
                    
                InventoryManager.Gold += gold;
                    
                UIManager.GetHudController().ShowFloatingText(transform.position, $"{gold} gold", Color.yellow);
            }
            
            SpawnManager.SpawnRandom(transform.position, itemDrops);
            
            GameManager.DestroyUnique(this);
            
            Destroy(gameObject);
        }
    }
}
