using UnityEngine;

namespace Actor.Interface
{
    public interface IDamageable
    {
        void InflictDamage(float damage, float direction,  float knockBack = 0);
        void InflictDamage(float damage, Vector2 direction,  float knockBack = 0);
        void InflictDamage(float damage, GameObject instigator, float knockBack = 0);
    }
}
