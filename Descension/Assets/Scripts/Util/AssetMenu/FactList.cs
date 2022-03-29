using System.Collections.Generic;
using System.Linq;
using Rules;
using UnityEngine;

namespace Util.AssetMenu
{
    [CreateAssetMenu(fileName = "Fact List", menuName = "Scriptable Objects/Fact List", order = 1)]
    public class FactList : ScriptableObject
    {
        public List<Fact> Facts;
    }
}