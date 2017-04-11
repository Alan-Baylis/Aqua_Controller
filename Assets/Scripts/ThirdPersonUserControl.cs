using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]

    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private bool jumpToSidePressed;
        float jumpToSideStart;
        public bool RunJumpLeft, RunJumpRight;
        bool crouch;

        Animator m_Animator;


        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();

            m_Animator = m_Character.m_Animator;

        }


        private void Update()
        {


            if (Input.GetMouseButtonDown(0))
            {
                Cursor.visible = true;
                Screen.lockCursor = false;
            }
            if (!m_Jump)
            {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (m_Character.isRunning)
            {
                m_Character.StopRun();
            }

            if (m_Character.m_TurnAmount < -0.9)
            { /* || m_Character.directionSwitch(1) == true || m_Character.directionSwitch(2) == true*/

                m_Character.turnAround("Left");
            } else if (m_Character.m_TurnAmount > 0.9) /* || m_Character.directionSwitch(4) == true || m_Character.directionSwitch(3) == true*/

            {
                m_Character.turnAround("Right");
            }
            else m_Character.turnningAround = false;


            // To jump right or left while running
            if (!jumpToSidePressed && m_Character.isRunning && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)))
            {
                jumpToSideStart = Time.time;
                jumpToSidePressed = true;
            }
            if (jumpToSidePressed && jumpToSideStart + 0.2f < Time.time)
            {
                jumpToSidePressed = false;
            }

            if (Input.GetKeyDown(KeyCode.A) && jumpToSidePressed && jumpToSideStart + 0.2f > Time.time)
            {
                m_Character.RunJumpLeft = true;
            }
            if (Input.GetKeyDown(KeyCode.D) && jumpToSidePressed && jumpToSideStart + 0.2f > Time.time)
            {
                m_Character.RunJumpRight = true;
            }

            //Declare animation lengths only once in the Setup

            /* if ((m_Character.RunJumpLeft || m_Character.RunJumpRight) && jumpToSideStart + m_Character.m_Animator.GetCurrentAnimatorStateInfo(0).length
                 - m_Character.m_Animator.GetCurrentAnimatorStateInfo(0).length / 3 < m_Character.timer)*/

            if ((m_Character.RunJumpLeft || m_Character.RunJumpRight) && jumpToSideStart + m_Character.GetAnimationLength("RunJumpLeft")/12- m_Character.GetAnimationLength("RunJumpLeft")/3/12
            < m_Character.timer)
            {
                m_Character.RunJumpRight = false;
                m_Character.RunJumpLeft = false;
                jumpToSidePressed = false;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl) && !m_Character.isIdle && GameManager.Get().stamina > GameManager.Get().maxStamina/5) //Check if player has at least 10/100 of stamina, then slide.
            {
                m_Character.runSlide = true;
            }


        }








        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");

            if(crouch && Input.GetKeyDown(KeyCode.C)){
                crouch = false;
            }
            else if (!crouch && Input.GetKeyDown(KeyCode.C)){
                crouch = true;
            }


            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) { m_Character.setForwardAmount(0.46f); }
#endif

            // pass all parameters to the character control script
            if (!m_Character.inRagdol)
            {
                m_Character.Move(m_Move, crouch, m_Jump);
            }
            m_Jump = false;



        }
    }
}
