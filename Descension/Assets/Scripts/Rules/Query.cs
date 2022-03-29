using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.ComTypes;

namespace Rules
{
    public class Query
    {
        private Dictionary<string, int> _query;

        public Query() => _query = new Dictionary<string, int>();
        public Query(Dictionary<string, int> facts) => _query = facts;
        
        public void Add(string key, int value) => _query.Add(key, value);
        public void Add(KeyValuePair<string, int> kvp) => Add(kvp.Key, kvp.Value);
        public void Add(Fact fact) => Add(fact.Key, fact.Value);

        public void AddRange(IEnumerable<KeyValuePair<string, int>> kvps)
        {
            foreach(var kvp in kvps) Add(kvp.Key, kvp.Value);
        }
        public void AddRange(IEnumerable<Fact> facts)
        {
            foreach(var fact in facts) Add(fact.Key, fact.Value);
        }

        public int? this[string key]
        {
            get => Get(key);
            set => Set(key, value ?? throw new NoNullAllowedException());
        }

        public int? Get(string key) => _query.TryGetValue(key, out var value) ? value : (int?) null;
        public void Set(string key, int value) => _query[key] = value;
    }
}