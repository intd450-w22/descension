using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        private void OnEnable() => Assert.AreNotEqual(0,uniqueId, "UniqueId not Generated for UniqueMonoBehaviour, must go to Window->Unique Id Generator and Generate Id's.");
    }
}
