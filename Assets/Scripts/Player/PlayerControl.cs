using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Player))]
public class PlayerControl : MonoBehaviour
{

    /*modified version of Standard Assets : UnityStandardAssets._2D. Platformer2DUserControl
*/
    private Player player;
    private bool attemptedJump;


    private void Awake()
    {
        player = GetComponent<Player>();
    }


    private void Update()
    {
        if (!attemptedJump)
        {
            // Read the jump input in Update so button presses aren't missed.
            attemptedJump = CrossPlatformInputManager.GetButtonDown("Jump");
        }
    }


    private void FixedUpdate()
    {
        // Read the inputs.
        bool attemptedCrouch=Input.GetKey(KeyCode.LeftControl);
        float horizontalSpeed = CrossPlatformInputManager.GetAxis("Horizontal");


        if (player.constantFowardMotion)
        {
            horizontalSpeed = 1f;
        }

        // Pass all parameters to the character control script.
        player.Move(horizontalSpeed, attemptedCrouch, attemptedJump);
        attemptedJump = false;
    }
}