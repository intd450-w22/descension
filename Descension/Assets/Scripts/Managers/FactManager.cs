using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class FactManager : MonoBehaviour
    {
        private static FactManager _instance;
        private static FactManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<FactManager>();
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
        }

        private Dictionary<Fact, bool> _facts = Enum.GetValues(typeof(Fact))
            .Cast<Fact>().ToDictionary(x => x, _ => true);

        public static bool GetFact(Fact fact) => Instance._facts[fact];
        public static bool SetFact(Fact fact, bool val) => Instance._facts[fact] = val;
    }

    [Serializable]
    public enum Fact
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
        HasKilledEnemy1,
        HasKilledEnemy2,
        HasKilledEnemy3,

        // World
        HasMetShopkeeper,
    }
}
