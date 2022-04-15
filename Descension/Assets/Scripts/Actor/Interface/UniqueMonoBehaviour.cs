using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Util.EditorHelpers;

namespace Actor.Interface
{
    [Serializable]
    public abstract class UniqueMonoBehaviour : MonoBehaviour
    {
        [SerializeField, ReadOnly] private int uniqueId;
        public int GetUniqueId() => uniqueId;

        #if UNITY_EDITOR
        protected void OnEnable() => Assert.AreNotEqual(0,uniqueId, "Unique Id not generated for UniqueMonoBehaviour, go to Window->'Unique Id Generator' and generate Id's.");
        private bool _cacheLocationOnDestroyed;
        private string _assertMessage = 
            ": Inconsistent use of DestroyUnique and IsUniqueDestroyed. Should always call both either with or without location.";
        #endif
        
        // cache object destroyed in level. Will only be permanent if the player completes the level.
        protected virtual void DestroyUnique()
        {
            #if UNITY_EDITOR
            Assert.IsFalse(_cacheLocationOnDestroyed, this + _assertMessage);
            #endif
            
            _destroyedUnique.Add(GetUniqueId());
        }
        
        // cache object destroyed in level. Will only be permanent if the player completes the level.
        protected virtual void DestroyUnique(Vector3 location)
        {
            #if UNITY_EDITOR
            Assert.IsTrue(_cacheLocationOnDestroyed, this + _assertMessage);
            #endif
            
            _destroyedUniqueWithLocation.Add(GetUniqueId(), location);
        }
        
        // permanently destroy object
        protected virtual void DestroyUniquePermanent()
        {
           
            #if UNITY_EDITOR
            Assert.IsFalse(_cacheLocationOnDestroyed, this + _assertMessage);
            #endif
            _permanentDestroyedUnique.Add(GetUniqueId());
        }

        // permanently destroy object
        protected virtual void DestroyUniquePermanent(Vector3 location)
        {
            #if UNITY_EDITOR
            Assert.IsTrue(_cacheLocationOnDestroyed, this + _assertMessage);
            #endif
            
            _permanentDestroyedUniqueWithLocation.Add(GetUniqueId(), location);
        }
        
        // returns true if object is permanently destroyed
        protected virtual bool IsUniqueDestroyed()
        {
            #if UNITY_EDITOR
            _cacheLocationOnDestroyed = false;
            #endif
            
            return _permanentDestroyedUnique.Contains(GetUniqueId());
        }

        // returns true if object is permanently destroyed and outputs location set when DestroyUnique was called
        protected virtual bool IsUniqueDestroyed(out Vector3 location)
        {
            #if UNITY_EDITOR
            _cacheLocationOnDestroyed = true;
            #endif
            
            if (_permanentDestroyedUniqueWithLocation.ContainsKey(GetUniqueId()))
            {
                location = _permanentDestroyedUniqueWithLocation[uniqueId];
                return true;
            }

            location = Vector3.zero;
            return false;
        }
        
        
        
        #region static caching interface
        
        private static Dictionary<int, Vector2> _destroyedUniqueWithLocation = new Dictionary<int, Vector2>();
        private static Dictionary<int, Vector2> _permanentDestroyedUniqueWithLocation = new Dictionary<int, Vector2>();
        private static HashSet<int> _destroyedUnique = new HashSet<int>();
        private static HashSet<int> _permanentDestroyedUnique = new HashSet<int>();
        private static HashSet<int> _uniqueIds = new HashSet<int>();

        public static void ClearUniqueIds() => _uniqueIds.Clear();

        public int GetNewUniqueId()
        {
            var id = GetInstanceID();
            while (_uniqueIds.Contains(id)) ++id;
            _uniqueIds.Add(id);
            uniqueId = id;
            return id;
        }
        
        public static void ClearDestroyedCache()
        {
            _destroyedUniqueWithLocation.Clear();
            _permanentDestroyedUniqueWithLocation.Clear();

            _destroyedUnique.Clear();
            _permanentDestroyedUnique.Clear();
        }

        public static void OnSceneComplete()
        {
            foreach (var destroyed in _destroyedUniqueWithLocation)
                _permanentDestroyedUniqueWithLocation[destroyed.Key] = destroyed.Value;
            
            _destroyedUniqueWithLocation.Clear();
            
            foreach (var destroyed in _destroyedUnique)
                _permanentDestroyedUnique.Add(destroyed);
            
            _destroyedUnique.Clear();
        }
        public static void OnReloadScene()
        {
            _destroyedUniqueWithLocation.Clear();
            _destroyedUnique.Clear();
        }
        
        #endregion
        

        
        
    }
}
