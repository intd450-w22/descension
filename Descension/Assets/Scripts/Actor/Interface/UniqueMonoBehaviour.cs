using System;
using UnityEngine;
using UnityEngine.Assertions;
using Util.EditorHelpers;

namespace Actor.Interface
{
    [Serializable]
    public class UniqueMonoBehaviour : MonoBehaviour
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;
        private void OnEnable() => Assert.AreNotEqual(0,uniqueId, "Unique Id not generated for UniqueMonoBehaviour, go to Window->'Unique Id Generator' and generate Id's.");
    }
}
