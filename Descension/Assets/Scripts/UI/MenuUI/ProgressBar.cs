using System;
using System.Linq;
using UnityEngine;
using Util.Helpers;

namespace UI.MenuUI
{
    public class ProgressBar : MonoBehaviour
    {
        public enum ProgressBarDirection
        {
            HorizontalFromLeft,
            HorizontalFromRight,
            VerticalFromBottom,
            VerticalFromTop
        }

        [Header("Progress Bar Range")]
        public float Min = 0f;
        public float Max = 1f;
        public bool UseWholeNumbers = false;

        [SerializeField]
        private float _value;
        public float Value
        {
            get => _value;
            set
            {
                _value = Mathf.Clamp(value, Min, Max);
                if (UseWholeNumbers) _value = Mathf.RoundToInt(_value);

                UpdateProgress();
            }
        }

        [Header("Configuration")]
        public ProgressBarDirection Direction;
        public bool Invert = true;

        // References
        private RectTransform containerRectTransform;
        private RectTransform progressRectTransform;

        // Helpers
        public float GetPercentage() => Invert ? InvPercentage : Percentage;
        public float Percentage => (_value - Min) / (Max - Min);
        public float InvPercentage => 1 - Percentage;

        void Awake()
        {
            containerRectTransform = GetComponent<RectTransform>();
            progressRectTransform = GetComponentsInChildren<RectTransform>().SingleOrDefault(x => x.name == "Progress");
        }

        public void SetMax(float newMax)
        {
            Max = newMax;
            UpdateProgress();
        }

        public void SetMin(float newMin)
        {
            Min = newMin;
            UpdateProgress();
        }

        public void SetWidth(float width)
        {
            containerRectTransform.SetWidth(width);
            UpdateProgress();
        }

        public void SetHeight(float height)
        {
            containerRectTransform.SetWidth(height);
            UpdateProgress();
        }

        private void UpdateProgress()
        {
            if (progressRectTransform == null)
            {
                Debug.LogWarning("Cannot find rect transform with name 'Progress'");
                return;
            };

            switch (Direction)
            {
                case ProgressBarDirection.HorizontalFromLeft:
                    progressRectTransform.SetRight(containerRectTransform.GetWidth() * GetPercentage());
                    break;
                case ProgressBarDirection.HorizontalFromRight:
                    progressRectTransform.SetLeft(containerRectTransform.GetWidth() * GetPercentage());
                    break;
                case ProgressBarDirection.VerticalFromBottom:
                    progressRectTransform.SetTop(containerRectTransform.GetHeight() * GetPercentage());
                    break;
                case ProgressBarDirection.VerticalFromTop:
                    progressRectTransform.SetBottom(containerRectTransform.GetHeight() * GetPercentage());
                    break;
            }
        }
    }
}
