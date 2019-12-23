using UnityEngine;

public class RightJoystickPlayerController : MonoBehaviour
{
    public RightJoystick rightJoystick; // the game object containing the RightJoystick script
    public Transform rotationTarget; // the game object that will rotate to face the input direction
    public float moveSpeed = 6.0f; // movement speed of the player character
    public int rotationSpeed = 8; // rotation speed of the player character
    public float xMovementRightJoystick;
    private Vector3 rightJoystickInput; // hold the input of the Right Joystick
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
        // get input from joystick
        rightJoystickInput = rightJoystick.GetInputDirection();

        xMovementRightJoystick = rightJoystickInput.x; // The horizontal movement from joystick 02
        float yMovementRightJoystick = rightJoystickInput.y; // The vertical movement from joystick 02

        // if there is no input on the right joystick

        // if there is only input from the right joystick
        if (rightJoystickInput != Vector3.zero)
        {
            // calculate the player's direction based on angle
            float tempAngle = Mathf.Atan2(yMovementRightJoystick, xMovementRightJoystick);
            xMovementRightJoystick *= Mathf.Abs(Mathf.Cos(tempAngle));
            yMovementRightJoystick *= Mathf.Abs(Mathf.Sin(tempAngle));

            // rotate the player to face the direction of input
            Vector3 temp = transform.position;
            temp.y += yMovementRightJoystick;
            temp.x += xMovementRightJoystick;
            Vector3 lookDirection = (temp - transform.position) * -1;
            if (lookDirection != Vector3.zero)
            {
                rotationTarget.localRotation = Quaternion.Slerp(rotationTarget.localRotation, Quaternion.LookRotation(lookDirection) * Quaternion.Euler(0f, 90f, 0f), rotationSpeed);
            }

            PlayerMinsu.PlayerInstance.weapon.WeaponCenter.gameObject.transform.localScale = new Vector3(1, 1, 1);
            if (xMovementRightJoystick < 0f)
            {
                PlayerMinsu.PlayerInstance.weapon.direction_Weapon = false;
            }
            else
            {
                PlayerMinsu.PlayerInstance.weapon.direction_Weapon = true;
            }
        }
    }
}