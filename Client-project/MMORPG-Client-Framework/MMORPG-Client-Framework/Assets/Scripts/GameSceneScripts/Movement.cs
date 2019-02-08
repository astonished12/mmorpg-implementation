using UnityEngine;

namespace GameSceneScripts
{
    public class Movement : MonoBehaviour
    {
        public float speed = 6.0F;
        public float jumpSpeed = 8.0F;
        public float gravity = 20.0F;
        public float rotateSpeed = 3.0F;
        private Vector3 moveDirection = Vector3.zero;
        private Animator characterAnimator;

        private void Awake()
        {
            characterAnimator = GetComponent<Animator>();
        }
        // Update is called once per frame
        public void Move()
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                characterAnimator.SetBool("Jumping", false);
                moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;

                if (moveDirection.magnitude > 1f)
                {
                    moveDirection = moveDirection.normalized;
                }

                characterAnimator.SetFloat("Speed", moveDirection.magnitude);

                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = jumpSpeed;
                    characterAnimator.SetBool("Jumping", true);
                }

            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);

            //Rotate Player
            transform.Rotate(0, Input.GetAxis("Horizontal"), 0);

        }

        void FootR()
        {
        }

        void FootL()
        {
        }
    }
}
