using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Util.EditorHelpers;

namespace Actor.Interface
{
    public interface IUnique
    {
        public int GetInstanceID();
        
        public int GetUniqueId();

        public void SetUniqueId(int id);
        
        
        

        
        
    }
}
