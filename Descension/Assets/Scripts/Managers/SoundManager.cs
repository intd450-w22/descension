using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource removeRockSound;
        public AudioSource goldFoundSound;
        public AudioSource itemFoundSound;
        public AudioSource arrowAttackSound;
        public AudioSource inspectionSound;
        public AudioSource errorSound;
        
        private static SoundManager _instance;
        private static SoundManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<SoundManager>();
                return _instance;
            }
        }
        
        void Awake()
        {
            if (_instance == null) _instance = this;
            else if (_instance != this) Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

        public static void RemoveRock() => Instance._RemoveRock();
        private void _RemoveRock() 
        {
            removeRockSound.Play();
        }

        public static void GoldFound() => Instance._GoldFound();
        private void _GoldFound() 
        {
            goldFoundSound.Play();
        }

        public static void ItemFound() => Instance._ItemFound();
        private void _ItemFound() 
        {
            itemFoundSound.Play();
        }

        public static void Inspection() => Instance._Inspection();
        private void _Inspection()
        {
            inspectionSound.Play();
        }

        public static void ArrowAttack() => Instance._ArrowAttack();
        private void _ArrowAttack() 
        {
            arrowAttackSound.Play();
        }

        public static void Error() => Instance._Error();
        public void _Error()
        {
            //TODO Add error sound effect
            //errorSound.Play();
        }
    }
}
