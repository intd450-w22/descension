using System.Collections.Generic;
using System.Linq;
using Rules;
using UnityEngine;
using Util.AssetMenu;
using Util.Enums;

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
                Init();
            }
        }

        private void Init()
        {
            Facts = new Dictionary<string, int>();
            foreach(var fact in FactsLists.SelectMany(x => x.Facts))
            {
                Facts.Add(fact.Key, fact.Value);
            }
        }

        public List<FactList> FactsLists;
        public Dictionary<string, int> Facts;

        public static int GetFact(string key) => Instance.Facts[key];
        public static int GetFact(FactKey key) => GetFact(key.ToString());
        
        public static bool IsFactTrue(string key) => Instance.Facts[key] > 0;
        public static bool IsFactTrue(FactKey key) => IsFactTrue(key.ToString());
        
        public static void SetFact(string key, int val)
        {
            Debug.Log($"[FactManager] Setting {key} to {val}");
            if (key == FactKey.None.ToString()) return;
            Instance.Facts[key] = val;
        }
        public static void SetFact(FactKey key, int val) => SetFact(key.ToString(), val);
        
        public static void SetFact(string key, bool val) => SetFact(key, val ? 1 : 0);
        public static void SetFact(FactKey key, bool val) => SetFact(key.ToString(), val);

        public static void IncrementFact(string key, int val) => SetFact(key, Instance.Facts[key] + 1);
        public static void IncrementFact(FactKey key, int val) => IncrementFact(key.ToString(), val);

        public static bool Query(Rule rule)
        {
            // print($"[FactManager] Querying {rule.Criteria.FirstOrDefault()?.Key ?? ""}");
            var query = new Query(Instance.Facts);
            return rule.Evaluate(query);
        }
    }
}
