using Actor.Interface;
using Items;
using Managers;
using UnityEngine;
using Util.Helpers;

namespace Environment
{
    public class RemovableRock : MonoBehaviour, IUnique
    {
        [SerializeField] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        public void SetUniqueId(int id) => uniqueId = id;
        
        
        public int goldDropMin = 1;
        public int goldDropMax = 20;
        public int goldDropChance = 40;  // percent chance of dropping gold in range (goldDropMin, goldDropMax)
        public ItemSpawner.DropStruct[] itemDrops;
        

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
                    
                UIManager.GetHudController().ShowFloatingText(transform.position, "Gold +" + gold, Color.yellow);
            }
            
            ItemSpawner.SpawnRandom(transform.position, itemDrops);
            
            GameManager.DestroyUnique(this);
            
            Destroy(gameObject);
        }
    }
}
