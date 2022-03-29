using System;

namespace Util.Enums
{
    [Serializable]
    public enum FactKey
    {
        None,

        // Items
        HasSeenPick,
        HasSeenTorch,
        HasSeenSword,
        HasSeenBow,
        HasSeenArrow,
        HasSeenRope,
        HasSeenHealthPotion,
        HasSeenGold,

        // Enemies
        NumKilledEnemy1,
        NumKilledEnemy2,
        NumKilledEnemy3,

        // World
        HasMetShopkeeper,
        NumItemsPurchased
    }
}