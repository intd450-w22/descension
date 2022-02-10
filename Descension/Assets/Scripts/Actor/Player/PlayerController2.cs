using UnityEngine;

namespace Actor.Player
{
    public class PlayerController2 : MonoBehaviour
    {
        public float movementSpeed = 1;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
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