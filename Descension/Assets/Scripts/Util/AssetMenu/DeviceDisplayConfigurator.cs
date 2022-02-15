using System.Collections.Generic;
using System.Linq;
using Util.Structs;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * Original code by Andy Touch (andyt@unity3d.com)
 * Source code https://github.com/UnityTechnologies/InputSystem_Warriors
 */
namespace Util.AssetMenu
{
    [CreateAssetMenu(fileName = "Device Display Configurator",
        menuName = "Scriptable Objects/Device Display Configurator", order = 1)]
    public class DeviceDisplayConfigurator : ScriptableObject
    {
        

        [System.Serializable]
        public struct DisconnectedSettings
        {
            public string disconnectedDisplayName;
            public Color disconnectedDisplayColor;
        }

        public List<DeviceSet> listDeviceSets = new List<DeviceSet>();

        public DisconnectedSettings disconnectedDeviceSettings;

        private Color fallbackDisplayColor = Color.white;


        public string GetDeviceName(PlayerInput playerInput)
        {

            string currentDeviceRawPath = playerInput.devices[0].ToString();

            string newDisplayName = null;

            for (int i = 0; i < listDeviceSets.Count; i++)
            {

                if (listDeviceSets[i].deviceRawPath == currentDeviceRawPath)
                {
                    newDisplayName = listDeviceSets[i].deviceDisplaySettings.deviceDisplayName;
                }
            }

            if (newDisplayName == null)
            {
                newDisplayName = currentDeviceRawPath;
            }

            return newDisplayName;

        }


        public Color GetDeviceColor(PlayerInput playerInput)
        {
            var currentDeviceRawPath = playerInput.devices[0].ToString();
            var newDisplayColor = fallbackDisplayColor;

            var color = listDeviceSets
                .SingleOrDefault(x => x.deviceRawPath == currentDeviceRawPath)
                .deviceDisplaySettings?.deviceDisplayColor ?? fallbackDisplayColor;
            return color;

        }

        public Sprite GetDeviceBindingIcon(PlayerInput playerInput, string playerInputDeviceInputBinding)
        {

            var currentDeviceRawPath = playerInput.devices[0].ToString();

            Sprite displaySpriteIcon = null;

            for (var i = 0; i < listDeviceSets.Count; i++)
            {
                if (listDeviceSets[i].deviceRawPath == currentDeviceRawPath && 
                    listDeviceSets[i].deviceDisplaySettings.deviceHasContextIcons) 
                    displaySpriteIcon = FilterForDeviceInputBinding(listDeviceSets[i], playerInputDeviceInputBinding);
            }

            return displaySpriteIcon;
        }

        private Sprite FilterForDeviceInputBinding(DeviceSet targetDeviceSet, string inputBinding)
        {
            Sprite spriteIcon = null;

            switch (inputBinding)
            {
                case "Button North":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.buttonNorthIcon;
                    break;

                case "Button South":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.buttonSouthIcon;
                    break;

                case "Button West":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.buttonWestIcon;
                    break;

                case "Button East":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.buttonEastIcon;
                    break;

                case "Right Shoulder":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.triggerRightFrontIcon;
                    break;

                case "Right Trigger":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.triggerRightBackIcon;
                    break;

                case "rightTriggerButton":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.triggerRightBackIcon;
                    break;

                case "Left Shoulder":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.triggerLeftFrontIcon;
                    break;

                case "Left Trigger":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.triggerLeftBackIcon;
                    break;

                case "leftTriggerButton":
                    spriteIcon = targetDeviceSet.deviceDisplaySettings.triggerLeftBackIcon;
                    break;

                default:

                    for (int i = 0; i < targetDeviceSet.deviceDisplaySettings.customContextIcons.Count; i++)
                    {
                        if (targetDeviceSet.deviceDisplaySettings.customContextIcons[i].customInputContextString ==
                            inputBinding)
                        {
                            if (targetDeviceSet.deviceDisplaySettings.customContextIcons[i].customInputContextIcon !=
                                null)
                            {
                                spriteIcon = targetDeviceSet.deviceDisplaySettings.customContextIcons[i]
                                    .customInputContextIcon;
                            }
                        }
                    }


                    break;

            }

            return spriteIcon;
        }

        public string GetDisconnectedName()
        {
            return disconnectedDeviceSettings.disconnectedDisplayName;
        }

        public Color GetDisconnectedColor()
        {
            return disconnectedDeviceSettings.disconnectedDisplayColor;
        }
    }
}
