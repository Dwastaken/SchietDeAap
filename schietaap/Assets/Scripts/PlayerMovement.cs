using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   
    public CharacterController controller;
    public float speed;
    public float walkspeed = 5f;
    public float sprintspeed = 8f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;
    Vector3 velocity;
    

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGroundend;

   
    void Update()
    {
        isGroundend = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGroundend && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = sprintspeed;
        }
        else
        {
            speed = walkspeed;
        }



        if (Input.GetButtonDown("Jump") && isGroundend)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
