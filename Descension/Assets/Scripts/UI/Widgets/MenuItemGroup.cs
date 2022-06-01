using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Widgets
{
    public class MenuItemGroup : MonoBehaviour
    {
        private int currIndex;
        private List<MenuItem> MenuItems;

        void Awake()
        {
            MenuItems = GetComponentsInChildren<MenuItem>().ToList();
        }
    }
}
