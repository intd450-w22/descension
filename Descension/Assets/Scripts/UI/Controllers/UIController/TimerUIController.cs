using Managers;
using UnityEngine;
using Util.Enums;

namespace UI.Controllers.UIController
{
    public class TimerUIController : UIController
    {
        public Scene NextScene = Scene.None;
        public string OtherScene;
        public UIType NextUi = UIType.None;
        public int TimeBeforeTransition = 0;

        private bool _timerStarted = false;
        private float _timeRemaining;

        public override void OnStart()
        {
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

                if(NextScene == Scene.Other)
                    UIManager.SwitchScene(OtherScene);
                else if(NextScene != Scene.None)
                    UIManager.SwitchScene(NextScene);
                
                UIManager.SwitchUi(NextUi);
            }


        }
    }
}