using System;
using System.Collections.Generic;
using System.Linq;
using Util.Enums;
using Util.Helpers;

namespace Rules
{
    public enum Comparison
    {
        EQUAL_TO,
        NOT_EQUAL_TO,
        LESS_THAN,
        GREATER_THAN,
        LESS_THAN_EQUAL_TO,
        GREATER_THAN_EQUAL_TO
    }

    [Serializable]
    public class Criteria
    {
        public FactKey FactKey;
        public string Key => FactKey.ToString();
        public Comparison ComparisonType;
        public int Value;
    }

    [Serializable]
    public class Rule
    {
        public List<Criteria> Criteria;

        public Rule() => Criteria = new List<Criteria>();
        public Rule(List<Criteria> criteria) => Criteria = criteria;

        public bool Any() => Criteria.Any();
        public bool IsEmpty() => Criteria.IsNullOrEmpty();
        public int Size() => Criteria?.Count ?? 0;

        public bool Evaluate(Query query)
        {
            if (Criteria == null) return false;
            foreach (var criterion in Criteria)
            {
                var fact = query[criterion.Key];
                if (fact == null) continue;

                switch(criterion.ComparisonType)
                {
                    case Comparison.EQUAL_TO:
                        if (fact != criterion.Value) 
                            return false;
                        break;
                    case Comparison.NOT_EQUAL_TO:
                        if (fact == criterion.Value)
                            return false;
                        break;
                    case Comparison.LESS_THAN:
                        if (fact >= criterion.Value)
                            return false;
                        break;
                    case Comparison.GREATER_THAN:
                        if (fact <= criterion.Value)
                            return false;
                        break;
                    case Comparison.LESS_THAN_EQUAL_TO:
                        if (fact > criterion.Value) 
                            return false;
                        break;
                    case Comparison.GREATER_THAN_EQUAL_TO:
                        if (fact < criterion.Value) 
                            return false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return true;
        }
    }
}