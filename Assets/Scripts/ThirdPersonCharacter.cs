using System.Collections;
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
        [SerializeField]
        float stamina = 5f;
        [SerializeField]
        float recovery = 5f;
        public bool m_Crouching;
        AnimationClip [] animations;

        Rigidbody m_Rigidbody;
        public Animator m_Animator;
        float m_OrigGroundCheckDistance;
        const float k_Half = 0.5f;
        public float m_TurnAmount;
        public float m_ForwardAmount;

        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        RaycastHit hitInfo; // Was inside CheckGroundStatus() but why ???


        //My movements
        float mouseWheel = 0.2f;
        public bool isRunning;
        public bool isWalking;
        public bool isIdle;
        public bool m_IsGrounded;
        public bool isExhausted;
        public bool isJumping;
        public bool isFalling, landLight, landForwardHeavy;
        public bool runSlide;
        public float transition;
        bool playing, playingExhausted, playingFlip, playingFall, runPlaying, runStopPlaying;
        bool flipReady;
        bool landed, landing;
        bool standUp;
        public bool turnningAround;
        public bool RunJumpLeft, RunJumpRight;
        Vector3 moveDirection;
        float groundedTimer;

        //Foot positioning
        RaycastHit hitSteps;
        public bool footIkOn;
        RaycastHit hitLeg, noLegHit;
        Transform pointer;
        Vector3 slopeRight, slopeLeft;
        Quaternion rightFootRot, leftFootRot;
        float footSmoothing, stepUpSmoothing, footSmoothingRight, footSmoothingLeft;
        float rightLegHitPoint, leftLegHitPoint;
        float climbSpeed;
        bool stepUpDone;
        public bool stepUpPlaying, stepUpReady;
        float rayLength, rayPoss, legRay; // dynamic for right or left leg
        float footNewPos, leftFootNewPos, footHigh, rightFootHigh, leftFootHigh, leftFootLow, rightFootLow, hipToFootDisc, toeEnd;
        float climbDist;
        float landedStart;
        string footName;
        Vector3 footNewPosition, leftFootNewPosition, rightFootNewPosition;
        Transform rightFoot, leftFoot, hips, foot;
        int stepUporDown;
        float charScale;
        float animClipSpeed;
        public bool leftLegStepUp, rightLegStepUp;
        public bool leftFootIkActive, rightFootIkActive;
        float rightFootTempX, leftFootTempX, rightFootTempY, leftFootTempY, rightFootTempZ, leftFootTempZ;
        float stepUpLeg;



        //spublic bool isTurning;
        public float breathingTempo = 1;
        int ways;
        float  jumpPrepare, fallStart, runStop, runStart, runBegin, runTime, exhaustTime, exhaustStart, climbStart;
        public float timer;
        float buttonTime = -1;
        float interval = 0.1f;
        public float myForward;
        public float detectWall = 1f;
        public bool facingWall;
        float runSlideForward, runSlideSide;

        /*
                //For head movements according camera
                float xHead, yHead, yCam, yHip;
                GameObject cameraGroup;
                GameObject playerNeck;
                GameObject playerHips;
                GameObject pivot;
        */

        GameObject head;
        Transform spine;
        AudioClip footSound, headSound;
        AudioSource soundSource;
        Vector3 fwd, down;
        float breathInterval = 0.8f;

        //Animation lengths and indexes
        RuntimeAnimatorController animatorController;
        int animIndex;




        void Start()
        {
            //Dissable Mouse                
            Cursor.visible = false;
            Screen.lockCursor = true;
            pointer = GameObject.Find("Pointer").transform;
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;
            head = GameObject.Find("Head");
            spine = GameObject.Find("Spine").transform;
            m_Animator.SetLayerWeight(0, 1);
            playingFlip = true;
            leftFoot = GameObject.Find("Left_foot").transform;
            rightFoot = GameObject.Find("Right_foot").transform;
            hips = GameObject.Find("Hips").transform;
            toeEnd = GameObject.Find("Right_toe_end").transform.position.y;
            hipToFootDisc = hips.position.y - toeEnd;
            //print(hipToFootDisc + " hipToFootDisc");
            charScale = 100 / (transform.localScale.x * 100 / 1.635238f);
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            m_OrigGroundCheckDistance = m_GroundCheckDistance;
            down = transform.TransformDirection(Vector3.down);
            stepUpSmoothing = 0.01f;

            //To copy all animations from animation controller to animations array
            animatorController = m_Animator.runtimeAnimatorController;
            animations = new AnimationClip[animatorController.animationClips.Length];
            animatorController.animationClips.CopyTo(animations, 0);

            //print animation names
            //printAnimationNameIndex();
        }

        //To get Animation Lenghts
        public float GetAnimationLength(string name)
        {
            for (int i = 0; i < animatorController.animationClips.Length; i++)
            {
                if (name == GetAnimationClips(i).name)
                {
                    animIndex = i;
                }
                if(i >= animatorController.animationClips.Length)
                {
                    print("No animation found for GetAnimationLength");
                    return 0;
                }
            }
            float animLength = (GetAnimationClips(animIndex).length);
            return animLength;
        }
        private AnimationClip GetAnimationClips(int animIndex)
        {
            return animations[animIndex];
        }


        void Update()
        {
            //TODO: Better set these two according size of the player, dynamic
            fwd = transform.TransformPoint((Vector3.forward) + new Vector3(0, 0, 999));
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.visible = true;
                Screen.lockCursor = false;
            }

            timer = Time.time;
            //if (m_ForwardAmount < 0) { m_ForwardAmount = 0; }                                          // MAYBE NOT NEEDED
            //if (m_TurnAmount != 0) { isTurning = true; } else isTurning = false;

            //Runing
            if (m_ForwardAmount > 0.5 && m_Rigidbody.velocity.magnitude > 4 && myForward > 0.5)
            {
                if (!runPlaying)
                {
                    runBegin = Time.time;
                    runPlaying = true;
                }
                if (runBegin + 2 < timer)
                {
                    isRunning = true;
                    runPlaying = false;
                }

            }
            //Not Running
            if (!(m_ForwardAmount > 0.5 && m_Rigidbody.velocity.magnitude > 4 && myForward > 0.5))
            {

                if (!runStopPlaying && isRunning)
                {
                    runStop = Time.time;
                    runStopPlaying = true;
                }
                if (runStop + 4 < timer)
                {
                    //print(timer);
                    isRunning = false;
                    runStopPlaying = false;
                }
            }

            if (m_ForwardAmount <= 0.5 && m_ForwardAmount > 0.1) { isWalking = true; } else isWalking = false;
            if (m_ForwardAmount == 0) { isIdle = true; } else isIdle = false;
            Exhausted();
            doFlip();

            if (landForwardHeavy || landLight)
            {
                landing = true;
                //m_Animator.applyRootMotion = false;
                isRunning = false;
            }
        }
        void LateUpdate()
        {
            
            stopAnimatingBone("rightLeg");

            //aimToMouse("Neck");
            if (!isIdle && stepUpPlaying)
            {
                smoothRotateBone(spine);
            }
            //print("Layer 0 is " + m_Animator.GetLayerWeight(0));
            //print("Layer 1 is " + m_Animator.GetLayerWeight(1));
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

            //m_ForwardAmount = move.z * mouseWheel;

               setForwardAmount(move.z * mouseWheel);



            if (m_IsGrounded && !landing && !runSlide)
            {
                ApplyExtraTurnRotation();
            }

            // control and velocity handling is different when grounded and airborne:
            if (m_IsGrounded)
            {
                HandleGroundedMovement(crouch, jump);
            }
            else
            {
                HandleAirborneMovement();
            }

            //ScaleCapsuleForCrouching(crouch);
            //PreventStandingInLowHeadroom();

            if (isRunning && Input.GetKey(KeyCode.C))
            {
                runSlide = true;
            }
            if (runSlide)
            {
                m_Animator.Play("RunSlide");
                PlaySounds("gravslip");
                standUp = false;
                //m_Rigidbody.useGravity = false;
                //m_Capsule.material.dynamicFriction = 0;
                //m_Capsule.material.staticFriction = 0;
                //m_Capsule.material.bounciness = 999f;
                //m_Animator.applyRootMotion = false;
                //m_Rigidbody.isKinematic = true;
                //m_Rigidbody.AddRelativeForce(0, 0, m_Rigidbody.velocity.z * 2);

                if (Input.GetKeyDown(KeyCode.A))
                {
                    runSlideSide = 0;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    runSlideSide = 1;
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    standUp = true;

                }
                // TODO: 
                if (runSlide && standUp && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("RunSlidefinish"))
                {
                    runSlide = false;
                }
            }
            

            
            //else runSlide = false;


            // Turn on LegIk only on suitable conditions
            if (isFalling || crouch || runSlide || landForwardHeavy || landLight || RunJumpLeft || RunJumpRight || facingWall || isJumping)
            {
                footIkOn = false;

                if (runSlide)
                {
                    ScaleCapsule("slide");
                }
            }
            else
            {
                footIkOn = true;
            }


            // send input and other state parameters to the animator
            UpdateAnimator(move);
                
        }
        //Print all animation names and index
        void printAnimationNameIndex()
        {
            for (int i = 0; i < animatorController.animationClips.Length; i++)
            {
                print(i+ " is animation "+ GetAnimationClips(i).name);
            }
            //print(" is animation " + GetAnimationClips(25).name);
        }

        void smoothRotateBone(Transform bone)
        {
            if (bone == spine)
            {
                spine.localRotation = Quaternion.Euler(Mathf.LerpAngle(spine.localRotation.x, 10, 0.1f), spine.localRotation.y, spine.localRotation.z);
                //print(Mathf.LerpAngle(spine.rotation.x, 10, 0.1f));
            }
        }
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Lift")
            {
                print("Collision with " + collision.gameObject.name);
                m_Rigidbody.AddRelativeForce(0, 1000, 10);
            }
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }

        //Change, detect magnitude drop and then paly animation with isgrounded condition.
        public void stopRun()
        {
            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyUp(KeyCode.W) && m_ForwardAmount > 0.5 || Input.GetKeyUp(KeyCode.D) && m_ForwardAmount > 0.5
                    || Input.GetKeyUp(KeyCode.A) && m_ForwardAmount > 0.5 || Input.GetKeyUp(KeyCode.S) && m_ForwardAmount > 0.5)
                {
                    if (m_IsGrounded && !runSlide)
                    {
                        m_Animator.Play("RunStop");
                        setForwardAmount(0);
                    }
                }
            }
        }

        void detectWallsAndIdle()//float currentSpeed, float detectWall)
        {   
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), fwd, detectWall) && m_IsGrounded && !isIdle)
            {
                facingWall = true;
                m_Animator.applyRootMotion = false;
                //m_Animator.Play("WallStop");
                setForwardAmount(0);
                print("Next to a wall !");
            } else facingWall = false;

            /*
            if (!(Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), fwd, detectWall) && m_IsGrounded && !isIdle))
            {
                facingWall = false;
            }
            */
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



        public void turnAround(string side)
        {
            if (myForward > 0.5 && !isExhausted && m_IsGrounded && isRunning && !runSlide)
            {
                turnningAround = true;
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
            if (flipReady && playingFlip)
            {
                jumpPrepare = Time.time;
                playingFlip = false;

            }
            if (flipReady)
            {
                m_Animator.Play("FrontFlip");
                isJumping = true;
                if (jumpPrepare + 0.3 < timer)
                {
                    m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower * 1.2f, m_Rigidbody.velocity.z);
                    //m_Rigidbody.AddRelativeForce(0, 0, 5);
                    flipReady = false;
                    m_IsGrounded = false;
                    playingFlip = true;
                    //m_Animator.applyRootMotion = false;
                    m_GroundCheckDistance = 0.1f;
                    ScaleCapsule("frontFlip");
                    //print((rightFeet.transform.position.y + leftFeet.transform.position.y) / 2);
                }
            }
        }

        void Exhausted()
        {
            if (isRunning && !playing)
            {
                runStart = Time.time;
                playing = true;
            }
            if (isRunning)
            {
                runTime = timer - runStart;
                if (runTime > stamina && isRunning)
                {
                    StartCoroutine(Smoother(1, m_Animator.GetLayerWeight(1), 1, 0.02f));
                    //exhaustTime = 0;
                    if (m_Animator.GetLayerWeight(1) >= 0.95)
                    {
                        isExhausted = true;
                    }
                }

            }
            // to become not exhausted
            if (isExhausted)
            {
                if (isExhausted && !playingExhausted)
                {
                    exhaustStart = Time.time;
                    playingExhausted = true;
                }
                exhaustTime = timer - exhaustStart;
                if (exhaustTime > recovery /*&& isExhausted*/)
                {
                    runTime = 0;
                    playing = false;

                    //m_Animator.SetLayerWeight(0, 1);
                    StartCoroutine(Smoother(0, m_Animator.GetLayerWeight(0), 1, 0.02f));

                    //StartCoroutine(Exhaust());
                    if (m_Animator.GetLayerWeight(0) > 0.1)
                    {
                        exhaustTime = 0;
                        playingExhausted = false;
                        isExhausted = false;
                    }
                }
            }
        }

        //Using only for breathing sound
        IEnumerator Exhaust()
        {
            while (isExhausted)
            {
                yield return new WaitForSeconds(breathInterval);
                //PlaySounds("exhausted");
            }
        }
        public void setBool(string setBoolean)
        {
            if (setBoolean == "runSlide")
            {
                runSlide = false;
            }
        }

        //Smoothes out numbers from start to finish wit hgiven interval. function is specific cases,
        IEnumerator Smoother(int function, float start, float finish, float interval)
        {
            yield return new WaitForSeconds(interval);
            //for going back to fit state
            if (function == 0)
            {
                if (m_Animator.GetLayerWeight(0) <= 1 && m_Animator.GetLayerWeight(1) >= 0)
                {
                    m_Animator.SetLayerWeight(1, m_Animator.GetLayerWeight(1) - interval);
                    m_Animator.SetLayerWeight(0, m_Animator.GetLayerWeight(0) + interval);
                    //m_ForwardAmount += interval;

                }
            }
            //for going to tired state
            if (function == 1)
            {
                if (m_Animator.GetLayerWeight(1) <= 1 && m_Animator.GetLayerWeight(0) >= 0)
                {
                    //m_ForwardAmount -= interval;
                    m_Animator.SetLayerWeight(1, m_Animator.GetLayerWeight(1) + interval);
                    m_Animator.SetLayerWeight(0, m_Animator.GetLayerWeight(0) - interval);


                }
            }
        }

        public void PlaySounds(string name)
        {

            {
                if (name == "exhausted")
                {   /*
                footSound = GetComponent<Sounds>().Clips[Random.Range(45, 49)];
                AudioSource.PlayClipAtPoint(footSound, transform.position);
                soundSource.Play();
                */
                    //headSound is an audio clip
                    headSound = head.GetComponent<Sounds>().Clips[Random.Range(0, 6)];
                    head.GetComponent<Sounds>().audioSources[Random.Range(0, 6)].PlayOneShot(headSound, 0.1f);
                }
                if (name == "exhaustStop")
                {
                    headSound = head.GetComponent<Sounds>().Clips[6];
                    head.GetComponent<Sounds>().audioSources[6].PlayOneShot(headSound, 1);
                }

                if (Physics.Raycast(transform.position, down, out hitSteps, m_GroundCheckDistance) && hitSteps.transform.gameObject.tag == "Concrete")
                {
                    if (name == "jumpLand")
                    {
                        footSound = GetComponent<Sounds>().Clips[20];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "steps" && isWalking && !isRunning)
                    {
                        footSound = GetComponent<Sounds>().Clips[Random.Range(25, 44)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);

                    }
                    if (name == "stepsRun" && isRunning && !isWalking)
                    {
                        footSound = GetComponent<Sounds>().Clips[Random.Range(25, 44)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "gravslip")
                    {
                        footSound = GetComponent<Sounds>().Clips[Random.Range(21, 24)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                }
                //Check if ground is with gravel surface
                if (Physics.Raycast(transform.position, down, out hitSteps, m_GroundCheckDistance) && hitSteps.transform.gameObject.tag == "Gravel")
                {
                    if (name == "jumpLand")
                    {
                        footSound = GetComponent<Sounds>().Clips[20];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "steps" && !isRunning && isWalking)
                    {
                        footSound = GetComponent<Sounds>().Clips[Random.Range(0, 20)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);

                    }
                    if (name == "stepsRun" && isRunning && !isWalking)
                    {
                        footSound = GetComponent<Sounds>().Clips[Random.Range(0, 20)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "gravslip")
                    {
                        footSound = GetComponent<Sounds>().Clips[Random.Range(21, 24)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                }
            }

        }
        /*
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
        */


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
                //m_Capsule.height = Mathf.Lerp(m_Capsule.height, m_CapsuleHeight, 0.01f);
                m_Capsule.height = m_CapsuleHeight;
                m_Capsule.center = m_CapsuleCenter;
            }
            if (state == "jump")
            {
                //m_IsGrounded = false;
                m_Capsule.height = 0.5f; //Mathf.Abs(head.transform.position.y - (rightFeet.transform.position.y + leftFeet.transform.position.y) / 2);
                //m_CapsuleCenter = hips.transform.position;
            }
            if (state == "slide")
            {
                m_Capsule.height = m_CapsuleHeight / 4f;
                m_Capsule.center = m_CapsuleCenter / 5f;
                m_Animator.applyRootMotion = false;
            }
            if (state == "frontFlip")
            {
                m_Capsule.height = 0.3f;
            }
            if (state == "falling")
            {
                m_Capsule.height = m_CapsuleHeight; //Mathf.Lerp(m_Capsule.height, m_CapsuleHeight, 0.01f);
                m_Capsule.center = m_CapsuleCenter;
            }
            if(state == "heavyLanding")
            {
                m_Capsule.height = m_CapsuleHeight/5; 
                m_Capsule.center = m_CapsuleCenter/5;
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
            //Detecing walls to play stop animation
            detectWallsAndIdle();//m_ForwardAmount, detectWall);
            if (Input.GetKeyDown(KeyCode.Tab) && isRunning && m_IsGrounded && !isExhausted)
            {
                flipReady = true;
            }

            if (!stepUpReady && !facingWall)
            {
                m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
            }
            if (facingWall)
            {
                m_Animator.SetFloat("Forward", m_ForwardAmount, 0.5f, 0);
            }

            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetBool("Crouch", m_Crouching);
            m_Animator.SetBool("OnGround", m_IsGrounded);
            m_Animator.SetBool("isExhausted", isExhausted);
            m_Animator.SetBool("isJumping", isJumping);
            m_Animator.SetBool("isRunning", isRunning);
            m_Animator.SetBool("isIdle", isIdle);
            m_Animator.SetBool("runSlide", runSlide);
            m_Animator.SetBool("isFalling", isFalling);
            m_Animator.SetBool("landLight", landLight);
            m_Animator.SetBool("landForwardHeavy", landForwardHeavy);
            m_Animator.SetBool("RunJumpRight", RunJumpRight);
            m_Animator.SetBool("RunJumpLeft", RunJumpLeft);
            m_Animator.SetFloat("animClipSpeed", animClipSpeed);
            m_Animator.SetFloat("stepUpLeg", stepUpLeg);
            m_Animator.SetBool("climbPlaying", stepUpPlaying);
            m_Animator.SetFloat("runSlideSide", runSlideSide);
            m_Animator.SetFloat("runSlideForward", runSlideForward);
            m_Animator.SetBool("standUp", standUp);

            if (!m_IsGrounded && isJumping)
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

            //m_Animator.applyRootMotion = true;
        }


        void HandleAirborneMovement()
        {

            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
            if (isJumping)
            {
                ScaleCapsule("jump");
            }

        }


        void HandleGroundedMovement(bool crouch, bool jump)
        {
            // check whether conditions are right to allow a jump:
            if (!isJumping && jump && !crouch && m_IsGrounded && !isExhausted && !isFalling    /*m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")*/)
            {
                // jump!
                jump = false;
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                m_IsGrounded = false;
                isJumping = true;
                //m_Animator.applyRootMotion = false;
                m_GroundCheckDistance = 0.1f;
            }

            if (isJumping && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
                {
                    isJumping = false;
                }
            }

            
       



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

        // Setter getter for m_forwardAmount
        void setForwardAmount(float preferedMovingSpeed)
        {

            m_ForwardAmount = Mathf.Lerp(getForwardAmount(), preferedMovingSpeed, 0.06f);


        }
        float getForwardAmount()
        {
            return m_ForwardAmount;
        }

        void CheckGroundStatus()
        {

#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {

                if (!landed)
                {
                    landedStart = Time.time;
                    landed = true;
                    ScaleCapsule("heavyLanding");
                }

                isFalling = false;
                m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;

                m_Animator.applyRootMotion = true;
                playingFall = false;
            }
            else
            {
                m_IsGrounded = false;
                landed = false;
                m_GroundNormal = Vector3.up;
                ScaleCapsule("falling");
                if (!isJumping && !stepUpPlaying)
                {
                    //Check is trully falling. It waits for character for being 1 sec not grounded to go to falling state.
                    if (!playingFall)
                    {
                        fallStart = Time.time;
                        playingFall = true;
                    }
                    if (fallStart + 0.3 < timer)
                    {
                        landLight = true;
                        isFalling = true;
                    }
                    if (fallStart + 0.5 < timer)
                    {
                        landLight = false;
                        landForwardHeavy = true;
                    }
                }
            }
            if (m_IsGrounded)
            {
                groundedTimer = Time.time;
            }
            else
                groundedTimer = 0;

            //Landing is over when player is stood up
            if (landed && timer > landedStart + 1)
            {
                landForwardHeavy = false;
                landLight = false;
                landed = false;
                landing = false;
            }
        }


        void legIK(int _foot)
        {
            if (_foot == 0) { return; }
            if (_foot == 1) { legRay = 0.1f; }
            if (_foot == 2) { legRay = -0.1f; }
            if (isRunning && !isExhausted) { rayLength = 0.4f; rayPoss = 0.6f; }
            if(isRunning && isExhausted) { rayLength = 0.38f; }
            if (isWalking) { rayLength = hipToFootDisc / 2.4f; rayPoss = 0.2f; }
            if (isIdle) { rayLength = hipToFootDisc / 2.2f; rayPoss = 0.1f; }

            Debug.DrawRay(new Vector3(legRay, hipToFootDisc / 2, rayPoss), new Vector3(0, -hipToFootDisc / 2, 0), Color.blue);
            Debug.DrawRay(hips.TransformPoint(legRay, -hipToFootDisc / 4, rayPoss), new Vector3(0, -hipToFootDisc / 2, 0), Color.red);

            if (Physics.Raycast(hips.TransformPoint(legRay, -hipToFootDisc / 4, rayPoss), new Vector3(0, -hipToFootDisc / 2, 0), out hitLeg, rayLength))
            {
                //slopeRight = Vector3.Cross(hitleg.normal, rightFoot.transform.right);
                //rightFootRot = Quaternion.LookRotation(Vector3.Exclude(hitleg.normal, slopeRight), hitleg.normal);
                //print("Found an object - distance: " + rightFootNewPos + "Object name: " + hitRightLeg.collider.gameObject.name);
                if (_foot == 1 && !leftFootIkActive)
                {
                    rightFootIkActive = true;

                    //Lockign position of left foot if climbing
                    stepUpLeg = 1;
                    if (!stepUpPlaying && !stepUpReady)
                    {
                        rightFootTempX = hitLeg.point.x;
                        rightFootTempY = hipToFootDisc / 10 + hitLeg.point.y;
                        rightFootTempZ = hitLeg.point.z;

                        //lock the other foot

                    }

                    rightFootHigh = hipToFootDisc / 1.45f - hitLeg.distance;
                    rightFootLow = rightFootHigh;
                    rightFootNewPosition = new Vector3(rightFootTempX, rightFootTempY, rightFootTempZ);
                    if (footSmoothingRight <= 0.99) { footSmoothingRight += 0.02f; }
                    if (footSmoothingRight > 0.95 && getForwardAmount() > 0.2f) {
                        stepUpReady = true;
                        if (!isIdle)
                        {
                            setForwardAmount(0.5f);
                        }
                    }
                    else
                    {
                        stepUpReady = false;
                        //setForwardAmount(0f);
                    }
                }

                if (_foot == 2 && !rightFootIkActive)
                {
                    leftFootIkActive = true;
                    

                    stepUpLeg = 0;
                    if (!stepUpPlaying && !stepUpReady)
                    {
                        leftFootTempX = hitLeg.point.x;
                        leftFootTempY = hipToFootDisc / 10 + hitLeg.point.y;
                        leftFootTempZ = hitLeg.point.z;
                    }
                    leftFootHigh = hipToFootDisc / 1.45f - hitLeg.distance;
                    leftFootLow = leftFootHigh;
                    leftFootNewPosition = new Vector3(leftFootTempX, leftFootTempY, leftFootTempZ);
                    if (footSmoothingLeft <= 0.99) { footSmoothingLeft += 0.02f; }
                    if (footSmoothingLeft > 0.95 && getForwardAmount() > 0.2f) {
                        stepUpReady = true;
                        if (!isIdle)
                        {
                            setForwardAmount(0.5f);
                        }
                    }
                    else
                    {
                        stepUpReady = false;
                    }
                }

                //Step Up Start
                if (stepUpReady && !isIdle && getForwardAmount() > 0.2f)
                {
                    if (!stepUpPlaying && !stepUpDone)
                    {
                        stepUpPlaying = true;
                    }
                    if (stepUpSmoothing <= 1) { stepUpSmoothing += 0.01f; }

                    m_Rigidbody.useGravity = false;
                    stepUpDone = false;
                    m_Rigidbody.AddRelativeForce(0, 0.8f, 0f);
                    m_CapsuleHeight = Mathf.Lerp(m_CapsuleHeight, 0.4f, 0.1f);
                }
                /*
                //Stop following leg from animating while climbing
                if (stepUpPlaying)
                {
                    if (!rightFootIkActive) {
                        rightFootNewPosition = new Vector3(0, 0, 0);
                    }
                    if (!leftFootIkActive)
                    {
                        leftFootNewPosition = new Vector3(0, 0, 0);
                    }
                }
                */
                stepUporDown = 1;
            }

            //print(leftFootTempY + " Left foot Y");
            //print(rightFootTempY + " Right foot Y");

            if (!(Physics.Raycast(hips.TransformPoint(legRay, -hipToFootDisc / 4, rayPoss), new Vector3(0, -hipToFootDisc / 2, 0), out hitLeg, rayLength)))
            {
                leftFootIkActive = false;
                rightFootIkActive = false;

                if (_foot == 1)
                {
                    if (footSmoothingRight >= 0.01) { footSmoothingRight -= 0.01f; }
                    else footSmoothingRight = 0;
                    leftLegStepUp = false;
                }
                if (_foot == 2)
                {
                    if (footSmoothingLeft >= 0.01) { footSmoothingLeft -= 0.01f; }
                    else footSmoothingLeft = 0;
                    rightLegStepUp = false;
                }
                stepUpDone = true;

                //Step up Stop
                if (stepUpDone)
                {
                    m_CapsuleHeight = Mathf.Lerp(m_CapsuleHeight, 1.52f, 0.2f);
                    //m_CapsuleCenter = new Vector3(0, 0.76f, 0f);
                    stepUpSmoothing = 0;
                    m_Rigidbody.useGravity = true;
                    stepUpPlaying = false;
                    stepUpReady = false;
                }


                if (_foot == 1)
                {
                    rightFootLow = Mathf.Lerp(rightFootLow, rightFoot.transform.position.y, 0.01f);
                    rightFootNewPosition = new Vector3(rightFoot.transform.position.x, rightFootLow, rightFoot.transform.position.z);
                }
                if (_foot == 2)
                {
                    leftFootLow = Mathf.Lerp(leftFootLow, leftFoot.transform.position.y, 0.01f);
                    leftFootNewPosition = new Vector3(leftFoot.transform.position.x, leftFootLow, leftFoot.transform.position.z);
                }
                stepUporDown = 0;
            }


            if (_foot == 1)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootNewPosition);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, Mathf.Lerp(footSmoothingRight, stepUporDown, 0.01f));
                m_Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, Mathf.Lerp(footSmoothingRight, stepUporDown, 0.01f));
            }
            if (_foot == 2)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootNewPosition);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, Mathf.Lerp(footSmoothingLeft, stepUporDown, 0.01f));
                m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, Mathf.Lerp(footSmoothingLeft, stepUporDown, 0.01f));
            }

            //m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
        }
        //called from late update, used to stop following leg on climb up from anmating
        void stopAnimatingBone(string bone)
        {
            if (bone== "rightLeg")
            {
                if(!rightFootIkActive && stepUpReady)
                {

                }
            }
            if (bone == "leftLeg")
            {

            }

        }

        void OnAnimatorIK()
        {
            if (m_Animator)
            {
                //if the IK is active, set the position and rotation directly to the goal. 
                if (footIkOn)
                {
                    // Set the look target position, if one has been assigned
                    if (pointer != null)
                    {
                        //m_Animator.SetLookAtWeight(0.5f);
                        //m_Animator.SetLookAtPosition(pointer.position);
                    }

                    // Set the right hand target position and rotation, if one has been assigned
                    if (rightFoot != null && leftFoot != null)
                    {
                        //m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
                        //m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
                        //Check which leg is in front, then raycast from it.
                        //print(leftFoot.localPosition.z + " leftFoot.localPosition.z");
                        //print(rightFoot.localPosition.z + " rightFoot.localPosition.z");

                        // Different states as running walking idle, requires diffferent footIk settings
                        //
                        
                        legIK(1);
                        legIK(2);
                        
                        /*
                        if ((rightFoot.transform.position.z > leftFoot.transform.position.z && !isIdle ) && !leftLegStepUp)
                        {
                            legIK(1);
                        }
                        if ((rightFoot.transform.position.z < leftFoot.transform.position.z && !isIdle) && !rightLegStepUp)
                        {
                            legIK(2);
                        }
                        */


                        /*
                        if (isIdle)
                        {
                            legIK(1);
                            legIK(2);
                        }
                        /*/


                        /*
                        if (isIdle)
                        {
                            legIK(leftFoot, AvatarIKGoal.LeftFoot, leftFootNewPosition);
                            legIK(rightFoot, AvatarIKGoal.RightFoot, rightFootNewPosition);
                        }
                        
                        if (rightFoot.position.z > leftFoot.position.z && !isIdle)
                        {
                            legIK(rightFoot, AvatarIKGoal.RightFoot, rightFootNewPosition);
                        }
                        if (rightFoot.position.z < leftFoot.position.z && !isIdle)
                        {
                            //legIK(leftFoot, AvatarIKGoal.LeftFoot);
                        } 
                        */
                    }
                    //m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                    //stepUpPlaying = false;
                    //stepUpReady = false;
                }
            }
        }
    }
}

