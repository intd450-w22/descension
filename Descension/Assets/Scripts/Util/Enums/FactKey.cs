using System;

namespace Util.Enums
{
    [Serializable]
    public enum FactKey
    {
        None = 0,

        // Items
        HasSeenPick = 1,
        HasSeenTorch = 2,
        HasSeenSword = 3,
        HasSeenBow = 4,
        HasSeenArrow = 5,
        HasSeenRope = 6,
        HasSeenHealthPotion = 7,
        HasSeenGold = 8,
        HasSeenExplosives = 9,
        HasSeenTrigger = 10,
        HasSeenTimer = 11,

        // Enemies
        NumKilledEnemy1 = 12,
        NumKilledEnemy2 = 13,
        NumKilledEnemy3 = 14,

        // World
        HasMetShopkeeper = 15,
        NumItemsPurchased = 16,
        HasFoundNote1 = 17,
        HasFoundNote2 = 18,
        HasFoundNote3 = 19,
        HasFoundNote4 = 20,
        HasFoundNote5 = 21,
        HasFoundNote6 = 22,
        HasFoundJournal1 = 23,
        HasFoundJournal2 = 24,
        HasFoundJournal3 = 25,
        HasFoundDossier1 = 26,
        HasFoundDossier2 = 27,
        HasFoundDossier3 = 28,
        HasFoundDossier4 = 29,
        TriggerEnding = 30,
    }
}