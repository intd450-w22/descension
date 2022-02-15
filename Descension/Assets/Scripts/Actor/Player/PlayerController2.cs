using UnityEngine;

namespace Assets.Scripts.Actor.Player
{
    public class PlayerController2 : MonoBehaviour
    {
        public float movementSpeed = 1;

        void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (x != 0 || y != 0)
            {
                transform.Translate(x * movementSpeed * Time.deltaTime,  y * movementSpeed * Time.deltaTime, 0);
                
            }
        }
    }
}