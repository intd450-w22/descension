using Managers;
using UnityEngine;
using Util.Helpers;

namespace UI.HUD
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private float _timeToLive = 1.5f;
        [SerializeField] private float _speed = 0.02f;

        void Start()
        {
            Invoke(nameof(_Destroy), _timeToLive);
        }

        private void _Destroy() => Destroy(gameObject);


        private bool _isFrozen;
        // Update is called once per frame
        void Update()
        {
            if (GameManager.IsFrozen)
            {
                OnFrozen();
                return;
            }

            transform.position = new Vector3(transform.position.x, transform.position.y + _speed, transform.position.z);
        }

        void OnFrozen()
        {
            if (_isFrozen) return;
                
            _isFrozen = true;
            CancelInvoke(nameof(_Destroy));
                
            // reactivate destroy timer when unfrozen
            this.InvokeWhen(
                () => { _isFrozen = false; Invoke(nameof(_Destroy), _timeToLive); }, 
                () => !GameManager.IsFrozen, 
                1);
        }

    }
}
