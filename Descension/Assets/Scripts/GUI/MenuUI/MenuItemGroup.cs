using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GUI.MenuUI
{
    public class MenuItemGroup : MonoBehaviour
    {
        private int currIndex;
        private List<MenuItem> MenuItems;

        void Awake()
        {
            MenuItems = GetComponentsInChildren<MenuItem>().ToList();
            Debug.Log(name + " " + MenuItems.Count);
        }
    }
}
