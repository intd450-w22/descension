using UnityEngine;

namespace Actor.Interface
{
    public interface IDamageable
    {
        void InflictDamage(GameObject instigator, float damage, float knockBack = 0);
    }
}
