using Managers;
using UnityEngine;
using Util.Enums;

namespace Actor.Objects
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract void Initialize(Vector2 direction, float damage, Tag target);
        
        public static void Instantiate(GameObject prefab, Vector3 position, Vector3 direction, float damage, Tag targetTag)
        {
            GameObject projectileObject = Object.Instantiate(prefab, position, Quaternion.identity);
            projectileObject.transform.localScale = GameManager.PlayerController.transform.localScale;
            projectileObject.GetComponent<Projectile>().Initialize(direction, damage, targetTag);
        }
    }
}
