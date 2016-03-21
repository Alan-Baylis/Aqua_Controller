using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : MonoBehaviour
    {
        [SerializeField]
        float m_MovingTurnSpeed = 360;
        [SerializeField]
        float m_StationaryTurnSpeed = 180;
        [SerializeField]
        float m_JumpPower = 12f;
        [Range(1f, 4f)]
        [SerializeField]
        float m_GravityMultiplier = 2f;
        [SerializeField]
        float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
        [SerializeField]
        float m_MoveSpeedMultiplier = 1f;
        [SerializeField]
        float m_AnimSpeedMultiplier = 1f;
        [SerializeField]
        float m_GroundCheckDistance = 0.1f;

        Rigidbody m_Rigidbody;
        Animator m_Animator;
        public bool m_IsGrounded;
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        public float m_TurnAmount;
        public float m_ForwardAmount;
        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        bool m_Crouching;

        //My movements
        RaycastHit hit;
        float mouseWheel = 0.2f;
        public float movingRight;
        public float movingLeft;
        public bool isRunning;
        public bool isWalking;
        public bool isIdle;
        public bool isTurning;
        public bool isJumping;
        public bool exhausted;
        public float stamina;
        int ways;
        float timer;
        float buttonTime = -1;
        float interval = 1;
        public float myForward;
        public float detectWall = 1f;


        /*
        Vector3 headControl;
        Vector3 mouseClickPosition;       ///DONT NEED ?
        Transform neckBone;
        */

/*
        //For head movements according camera
        float xHead, yHead, yCam, yHip;
        GameObject cameraGroup;
        GameObject playerNeck;
        GameObject playerHips;
        GameObject pivot;
*/

        //GameObject head, rightFeet, leftFeet, hips;


        AudioClip sound;
        AudioSource sounds;
        Vector3 fwd, down;
        float runTime;

        void Start()
        {

            stamina = 5;
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;
            /*
            head = GameObject.Find("Head");
            leftFeet = GameObject.Find("Left_toe_end");
            rightFeet = GameObject.Find("Right_toe_end");
            hips = GameObject.Find("Hips");
            */
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;
            /*
            playerNeck = GameObject.Find("Neck");
            playerHips = GameObject.Find("Hips");
            pivot = GameObject.Find("Pivot");
            cameraGroup = GameObject.Find("Camera_group");
            */
            fwd = transform.TransformDirection(Vector3.forward);
            down = transform.TransformDirection(Vector3.down);
        }

        //Aims bone towards the mouse on screen
        void aimToMouse(string bone)
        {/*
            xHead = pivot.transform.eulerAngles.x;
            yCam = cameraGroup.transform.eulerAngles.y;
            yHip = playerHips.transform.localEulerAngles.y;
            yHead = -1 * (yHip - yCam);
            /*
            if(yHead > 0.3) { yHead = 0.3f; }
            if (yHead < -0.3) { yHead = -0.3f; }
            */

            // print(playerNeck.transform.localEulerAngles.y);

            //smoothY = Mathf.SmoothDamp(x, Screen.width / 2, ref z, 5, 2);



            //playerNeck.transform.rotation = Quaternion.Euler(xHead, yHead, transform.eulerAngles.z);
        }

        //animation["Walk"].AddMixingTransform(upperSpine);

        void Update()
        {
            if (m_ForwardAmount < 0) { m_ForwardAmount = 0; }                                          // MAYBE NOT NEEDED
            if (m_ForwardAmount > 0.5 || myForward > 0.5 ) { isRunning = true; } else isRunning = false;
            if (m_ForwardAmount <= 0.5 && m_ForwardAmount > 0.1) { isWalking = true; } else isWalking = false;
            if (m_ForwardAmount == 0) { isIdle = true; } else isIdle = false;
            if (m_TurnAmount != 0) { isTurning = true; } else isTurning = false;
        }
        void LateUpdate()
        {
            aimToMouse("Neck");

            if (isRunning)
            {
                runTime = Time.time;

                if(runTime > stamina && !exhausted)
                {
                    PlaySounds("exhausted");
                    exhausted = true;
                }
            }if (!isRunning) { runTime = 0; exhausted = false; }
            print(runTime);
        }

        public void Move(Vector3 move, bool crouch, bool jump)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.

            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            CheckGroundStatus();
            move = Vector3.ProjectOnPlane(move, m_GroundNormal);
            m_TurnAmount = Mathf.Atan2(move.x, move.z);

            mouseWheel += Input.GetAxis("Mouse ScrollWheel");

            if (mouseWheel > 1)
            {
                mouseWheel = 1;
                myForward = 1;
            }
            else if (mouseWheel < 0)
            {
                mouseWheel = 0;
                myForward = 0;
            }
            else if (mouseWheel > 0 && mouseWheel < 0.3f)
            {
                mouseWheel = 0.2f;
                myForward = 0.2f;
            }
            if (mouseWheel > 0.5f && mouseWheel < 0.7f)
            {
                mouseWheel = 0.5f;
                myForward = 0.5f;
            }
            else if (mouseWheel > 0.7f && mouseWheel < 0.9f)
            {
                mouseWheel = 0.9f;
                myForward = 0.9f;
            }
            else
            {
                mouseWheel += Input.GetAxis("Mouse ScrollWheel");
            }
            m_ForwardAmount = move.z * mouseWheel;


            ApplyExtraTurnRotation();


            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);
            }
            else
            {
                HandleAirborneMovement();
            }

            ScaleCapsuleForCrouching(crouch);
            PreventStandingInLowHeadroom();

            // send input and other state parameters to the animator
            UpdateAnimator(move);
        }

        public void stopRun()
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyUp(KeyCode.W) && m_ForwardAmount > 0.5 || Input.GetKeyUp(KeyCode.D) && m_ForwardAmount > 0.5
                    || Input.GetKeyUp(KeyCode.A) && m_ForwardAmount > 0.5 || Input.GetKeyUp(KeyCode.S) && m_ForwardAmount > 0.5)
                {
                    m_Animator.Play("RunStop");
                    //m_Animator.SetFloat("Forward", 0);
                }
            }
        }
        /*
        public bool directionSwitch(int direction)
        {
            int ways = direction;
            timer = Time.time;

            switch (ways)
            {
                case 1: direction = 1;
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        buttonTime = Time.time;
                        buttonTime = buttonTime + interval;
                       // print(buttonTime);
                    }
                    if (timer <= buttonTime && Input.GetKeyDown(KeyCode.S) )
                    {
                        print("S is down after releasing W");
                        return true;
                     }
                    break;
                  
                case 2:
                    direction = 2;
                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        buttonTime = Time.time;
                        buttonTime = buttonTime + interval;
                        // print(buttonTime);
                    }
                    if (timer <= buttonTime && Input.GetKeyDown(KeyCode.D))
                    {
                        print("D is down after releasing A");
                        return true;
                    }
                    break;
                case 3:
                    direction = 3;
                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        buttonTime = Time.time;
                        buttonTime = buttonTime + interval;
                        // print(buttonTime);
                    }
                    if (timer <= buttonTime && Input.GetKeyDown(KeyCode.A))
                    {
                        print("A is down after releasing D");
                        return true;
                    }
                    break;
                case 4:
                    direction = 4;
                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        buttonTime = Time.time;
                        buttonTime = buttonTime + interval;
                        // print(buttonTime);
                    }
                    if (timer <= buttonTime && Input.GetKeyDown(KeyCode.W))
                    {
                        print("W is down after releasing S");
                        return true;
                    }
                    break;
                   
            }
            return false;
        } // Direction switch
        */

        void detectWallsAndIdle(float currentSpeed, float detectWall)
        {

            if (Physics.Raycast(transform.position, fwd, detectWall) && m_IsGrounded)
            {

                m_Animator.Play("WallStop");
                /*
                m_ForwardAmount = 0.1f;
                print("Wall stopping");
                isIdle = true;*/
                //m_Animator.SetFloat ("Forward", Mathf.Lerp (1, -0.1f, Time.time*0.5f));
                //print(currentSpeed);
            }
        }
        public void turnAround(string side)
        {
            if (myForward > 0.5)
            {
                if (side == "Right")
                {
                    m_Animator.Play("TurnAroundRight");
                }
                if ((side == "Left"))
                {
                    m_Animator.Play("TurnAroundLeft");
                }
            }
        }
        void doFlip()
        {
            //To-Do: Flip animations depending on conditions
            //To-Do: Scale colider
            m_Animator.Play("FrontFlip");
            isJumping = true;
            //print((rightFeet.transform.position.y + leftFeet.transform.position.y) / 2);
            print(m_CapsuleHeight);
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            print(m_Capsule);
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }

        public void PlaySounds(string name)
        {
            if(name == "exhausted")
            {

                sound = GetComponent<Sounds>().Clips[45];
                sounds.PlayOneShot(sound, 1);

            }
            if (Physics.Raycast(transform.position, down, out hit, m_GroundCheckDistance) && hit.transform.gameObject.tag == "Concrete")
            {
                if (name == "jumpLand")
                {
                    sound = GetComponent<Sounds>().Clips[20];
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                }
                if (name == "steps" && isWalking)
                {
                    sound = GetComponent<Sounds>().Clips[Random.Range(25, 44)];
                    AudioSource.PlayClipAtPoint(sound, transform.position);

                }
                if (name == "stepsRun" && isRunning)
                {
                    sound = GetComponent<Sounds>().Clips[Random.Range(25, 44)];
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                }
                if (name == "gravslip")
                {
                    sound = GetComponent<Sounds>().Clips[Random.Range(21, 24)];
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                }
            }
                //Check if ground is with gravel surface
                if (Physics.Raycast(transform.position, down, out hit, m_GroundCheckDistance) && hit.transform.gameObject.tag == "Gravel")
            {
                if (name == "jumpLand")
                {
                    sound = GetComponent<Sounds>().Clips[20];
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                }
                if (name == "steps" && isWalking)
                {
                    sound = GetComponent<Sounds>().Clips[Random.Range(0, 20)];
                    AudioSource.PlayClipAtPoint(sound, transform.position);

                }
                if (name == "stepsRun" && isRunning)
                {
                    sound = GetComponent<Sounds>().Clips[Random.Range(0, 20)];
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                }
                if (name == "gravslip")
                {
                    sound = GetComponent<Sounds>().Clips[Random.Range(21, 24)];
                    AudioSource.PlayClipAtPoint(sound, transform.position);
                }
            }
        }

        //Check if button was pressed longer than 1 sec
        bool buttonTimer() //NOT USED
        {
            timer = Time.time;

            if (Input.GetKeyUp(KeyCode.W) || Input.GetKey(KeyCode.W))
            {
                buttonTime = Time.time + interval;
            }
            if (Input.GetKeyDown(KeyCode.S) && timer < buttonTime)
            {
                print("S is down after releasing W");
                return true;

            }

            return false;
        }


        void ScaleCapsuleForCrouching(bool crouch)
        {
            if (m_IsGrounded && crouch)
            {
                if (m_Crouching) return;
                m_Capsule.height = m_Capsule.height / 2f;
                m_Capsule.center = m_Capsule.center / 2f;
                m_Crouching = true;
            }
            else
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                    return;
                }
                m_Capsule.height = m_CapsuleHeight;
                m_Capsule.center = m_CapsuleCenter;
                m_Crouching = false;
            }
        }


        void ScaleCapsule(string state)
        {
            if (state == "ground")
            {
                m_Capsule.height = m_CapsuleHeight;
                //m_Capsule.center = m_CapsuleCenter;
            }
            if (state == "jump")
            {
                //m_IsGrounded = false;
                m_Capsule.height = 1f; //Mathf.Abs(head.transform.position.y - (rightFeet.transform.position.y + leftFeet.transform.position.y) / 2);
                //m_CapsuleCenter = hips.transform.position;
            }
        }

        void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                }
            }
        }


        void UpdateAnimator(Vector3 move)
        {
            // update the animator parameters

            //Detecing walls to play stop animation
            if (m_ForwardAmount < 1 && m_ForwardAmount > 0.7f)
            {
                detectWallsAndIdle(m_ForwardAmount, detectWall);
            }
            if (Input.GetKeyDown(KeyCode.Tab) && isRunning)
            {
                doFlip();
            }
            m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetBool("Crouch", m_Crouching);
            m_Animator.SetBool("OnGround", m_IsGrounded);


            if (!m_IsGrounded)
            {
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
            }


            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
            float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
            if (m_IsGrounded)
            {
                m_Animator.SetFloat("JumpLeg", jumpLeg);
            }

            // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
            // which affects the movement speed because of the root motion.
            if (m_IsGrounded && move.magnitude > 0)
            {
                m_Animator.speed = m_AnimSpeedMultiplier;
            }
            else
            {
                // don't use that while airborne
                m_Animator.speed = 1;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
            if (isJumping) {
                print("ssdad");
                ScaleCapsule("jump");
            }
        }


        void HandleGroundedMovement(bool crouch, bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
            }
            isJumping = false;
            ScaleCapsule("ground");
        }

        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        }


        public void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (m_IsGrounded && Time.deltaTime > 0)
            {
                Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = m_Rigidbody.velocity.y;
                m_Rigidbody.velocity = v;
            }
        }


        void CheckGroundStatus()
        {
            RaycastHit hitInfo;
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;
                m_Animator.applyRootMotion = true;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundNormal = Vector3.up;
                m_Animator.applyRootMotion = false;
            }
        }
    }
}

