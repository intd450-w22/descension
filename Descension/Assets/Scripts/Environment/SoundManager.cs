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
