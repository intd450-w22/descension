using Managers;
using UnityEditor;
using UnityEngine;
using Util.EditorHelpers;
using Util.Enums;

namespace UI.Controllers.UIController
{
    public class TimerUIController : UIController
    {
        #if UNITY_EDITOR
        public SceneAsset nextLevel;
        private void OnValidate() { if (nextLevel != null) _nextLevel = nextLevel.name; }
        #endif
        [SerializeField, ReadOnly] private string _nextLevel;
        
        public UIType NextUi = UIType.None;
        public int TimeBeforeTransition = 0;

        
        private bool _timerStarted = false;
        private float _timeRemaining;

        public override void OnStart()
        {
            SoundManager.StopMainMenuBackgroundAudio();

            _timeRemaining = TimeBeforeTransition;
            _timerStarted = true;
        }

        void Update()
        {
            if (!_timerStarted) return;

            if(_timeRemaining > 0)
                _timeRemaining -= Time.deltaTime;
            else
            {
                _timerStarted = false;
                _timeRemaining = 0;
                
                GameManager.SwitchScene(_nextLevel, NextUi);
            }
        }
    }
}