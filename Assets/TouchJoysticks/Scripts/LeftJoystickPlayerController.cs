using UnityEngine;

public class LeftJoystickPlayerController : MonoBehaviour
{
    public LeftJoystick leftJoystick; // the game object containing the LeftJoystick script
    private Vector3 leftJoystickInput; // holds the input of the Left Joystick
    private Rigidbody rigidBody; // rigid body component of the player character

    void Start()
    {
        rigidBody = transform.GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        leftJoystickInput = leftJoystick.GetInputDirection();


        if (leftJoystickInput != Vector3.zero)
        {
            if(leftJoystickInput.x > 0.3f)
            {
                PlayerMinsu.PlayerInstance.RightButton();
            }

            else if (leftJoystickInput.x < -0.3f)
            {
                PlayerMinsu.PlayerInstance.LeftButton();
            }
            else
            {
                PlayerMinsu.PlayerInstance.ButtonUp();
            }

            if (leftJoystickInput.y > 0.5f)
            {
                PlayerMinsu.PlayerInstance.UpButton();
            }

            else if (leftJoystickInput.y < -0.5f)
            {
                PlayerMinsu.PlayerInstance.DownButton();
            }
        }
        else
        {
            PlayerMinsu.PlayerInstance.ButtonUp();
        }
    }
}