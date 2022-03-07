using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource removeRockSound;
        public AudioSource goldFoundSound;
        public AudioSource itemFoundSound;
        public AudioSource arrowAttackSound;
        public AudioSource inspectionSound;
        
        
        private static SoundManager _instance;
        public static SoundManager Instance
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

        public void RemoveRock() 
        {
            removeRockSound.Play();
        }

        public void GoldFound() 
        {
            goldFoundSound.Play();
        }

        public void ItemFound() 
        {
            itemFoundSound.Play();
        }

        public void inspection()
        {
            inspectionSound.Play();
        }

        public void ArrowAttack() 
        {
            arrowAttackSound.Play();
        }
    }
}
