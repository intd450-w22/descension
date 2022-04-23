using UnityEngine;
using Util.Enums;

namespace Actor.Projectiles
{
    public abstract class Projectile : MonoBehaviour
    {
        protected abstract void Initialize(Vector2 direction, float damage, float knockBack, Tag target);
        
        public static void Instantiate(GameObject prefab, Vector3 position, Vector3 direction, float damage, float knockBack, Tag targetTag)
        {
            GameObject projectileObject = Object.Instantiate(prefab, position, Quaternion.identity);
            projectileObject.GetComponent<Projectile>().Initialize(direction, damage, knockBack, targetTag);
        }
    }
}
