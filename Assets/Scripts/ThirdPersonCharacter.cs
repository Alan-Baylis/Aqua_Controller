using System.Collections;
using UnityEngine;
using System;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : MonoBehaviour
    {

        public bool lookAtPointer;
        Emotions emotions;
        [SerializeField]
        float m_MovingTurnSpeed = 360;
        [SerializeField]
        float m_StationaryTurnSpeed = 180;
        [SerializeField]
        float m_JumpPower = 12f;
        [SerializeField]
        float stepUpForce = 0.8f;
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
        float m_GroundCheckDistance;
        float temporaryGroundCheckDistance;

        //Stats
        [SerializeField]
        public float maxStamina;
        [SerializeField]
        public int maxHealth;
        [SerializeField]
        float recovery = 6f;
        bool dead;


        [Range(200, 1000)]
        [SerializeField]
        int slideForce = 500;
        public bool m_Crouching;
        AnimationClip[] animations;

        Rigidbody m_Rigidbody;
        Rigidbody[] ponyTail;
        public Animator m_Animator;
        const float k_Half = 0.5f;
        public float m_TurnAmount;
        public float m_ForwardAmount;

        Vector3 m_GroundNormal;
        float m_CapsuleHeight;
        Vector3 m_CapsuleCenter;
        CapsuleCollider m_Capsule;
        RaycastHit hitInfo; // Was inside CheckGroundStatus() but why ???

        //FacialEmotions





        //My movements
        float mouseWheel = 0.2f;
        public bool isRunning;
        public bool isWalking;
        public bool isIdle;
        public bool m_IsGrounded;
        public bool isExhausted;
        public bool isJumping;
        bool idleJump, runJump;
        bool animationJump;
        float jumpStart;
        public bool isFalling, landLight, landForwardHeavy;
        public bool runSlide;
        public float transition;
        bool playing, playingExhausted, playingFlip, playingFall, runPlaying, runStopPlaying;
        bool flipReady;
        public bool landed, landing;
        bool standUp;
        float standUpStart;
        public bool turnningAround;
        public bool RunJumpLeft, RunJumpRight;
        Vector3 moveDirection;
        float groundedTimer;
        bool crashedInAir, crashedOnGround;
        public bool runKick, walkKick;
        bool slideForceEnabled;
        float slideStart, _slideForce;
        bool slideSound;

        float timeInAir, timeInAirStart;
        bool timeInAirBool;
        bool WalkStepOverLeft, WalkStepOverRight, RunStepOverLeft, RunStepOverRight;

        //Stats
        float stamina, health, healthChange;
        float recoverySpeed = 0.01f;
        bool setPlayerStats;

        //Foot positioning
        RaycastHit hitSteps;
        internal static bool footIkOn;
        public bool _footIkOn;
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
        float footNewPos, leftFootNewPos, footHigh, rightFootHigh, leftFootHigh, leftFootLow, rightFootLow, hipToFootDist, toeEnd;
        float climbDist;
        float landedStart;
        string footName;
        Vector3 footNewPosition, leftFootNewPosition, rightFootNewPosition;
        Transform rightFoot, leftFoot, hips, foot;
        int stepUpOrDown;
        float charScale;
        float animClipSpeed;
        public bool leftLegStepUp, rightLegStepUp;
        public bool leftFootIkActive, rightFootIkActive;
        float rightFootTempX, leftFootTempX, rightFootTempY, leftFootTempY, rightFootTempZ, leftFootTempZ;
        float stepUpLeg;
        bool holdingStepUpLeg, holdingStepUpLegPlaying;
        float holdingRightPosZ, holdingRightPosX;
        float aimSmoother;


        //Ledge climbing
        public bool ledgeDetected, ledgeHang, ledgeHanging, startLedgeHang, endLedgeHang, ledgeClimbUp;
        GameObject ledgeObject;
        Transform lastLedgePosition;
        Vector3 armHangPoint, rotationDirection, climbPointer;
        Quaternion rotationToObject;
        private bool endLedgeHangAnim, ledgeHangStoped, endingLedgeHang, climbUpPositionChang;
        private float ledgeHangRotateAmount;



        public float breathingTempo = 1, myForward, wallDetectDist = 1f, timer;
        int ways;
        float jumpPrepare, fallStart, runStop, runStart, runBegin, runTime, exhaustTime, exhaustStart, climbStart, runSlideForward, runSlideSide, buttonTime = -1, interval = 0.1f;
        public bool facingWall, ragdolEnabled, inRagdol;


        GameObject head;
        Transform spine;
        AudioClip footSound, headSound;
        AudioSource soundSource;
        Vector3 fwd, down;
        float breathInterval = 0.8f;

        //Animation lengths and indexes
        RuntimeAnimatorController animatorController;
        int animIndex;

        float toeToKnucklesDist;


        void Start()
        {
            //Dissable Mouse                
            Cursor.visible = false;
            Screen.lockCursor = true;
            try
            {
                pointer = GameObject.Find("Pointer").transform;
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("No pointer");
            }
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            m_CapsuleHeight = m_Capsule.height;
            m_CapsuleCenter = m_Capsule.center;
            head = transform.Find("Aqua/Hips/Spine/Chest/Neck/Head").gameObject;
            spine = transform.Find("Aqua/Hips/Spine");
            m_Animator.SetLayerWeight(0, 1);
            leftFoot = transform.Find("Aqua/Hips/Left_leg/Left_knee/Left_foot");
            rightFoot = transform.Find("Aqua/Hips/Right_leg/Right_knee/Right_foot").transform;
            rightFootNewPosition = rightFoot.transform.position;
            leftFootNewPosition = leftFoot.transform.position;
            hips = transform.Find("Aqua/Hips").transform;
            toeEnd = transform.Find("Aqua/Hips/Right_leg/Right_knee/Right_foot/Right_toe/Right_toe_end").transform.position.y;
            hipToFootDist = hips.position.y - toeEnd;
            //print(hipToFootDisc + " hipToFootDisc");
            charScale = 100 / (transform.localScale.x * 100 / 1.635238f);
            m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            temporaryGroundCheckDistance = m_GroundCheckDistance;
            down = transform.TransformDirection(Vector3.down);
            stepUpSmoothing = 0.01f;

            //To copy all animations from animation controller to animations array
            animatorController = m_Animator.runtimeAnimatorController;
            animations = new AnimationClip[animatorController.animationClips.Length];
            animatorController.animationClips.CopyTo(animations, 0);

            //print animation names
            printAnimationNameIndex();

            //Assigning ponytail rigid bodies
            //ponyTail = GameObject.Find("Pony_tail_skeleton_2").GetComponent<Rigidbody>();

            ponyTail = head.GetComponentsInChildren<Rigidbody>();


            //eyeBlink = (int)GameObject.Find("Body").GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(4);
            emotions = new Emotions();
            //Emotions emotions;
            ragdolEnabled = true;
            stamina = maxStamina;
            health = maxHealth;

            //Get hand to hip position for climbing
            try
            {
                toeToKnucklesDist = transform.Find("Aqua/Hips/Spine/Chest").transform.position.y - transform.Find("Aqua/Hips/Right_leg/Right_knee/Right_foot/Right_toe/Right_toe_end").transform.position.y
                   + Vector3.Distance(transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm").transform.position, (transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand/Left_middle").transform.position));

                //print("toeToKnucklesDist " + toeToKnucklesDist);
            }
            catch (NullReferenceException ex)
            {
                print("No bones found to calculate distance from hip to palm");
            }
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
                if (i >= animatorController.animationClips.Length)
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
            fwd = (transform.TransformPoint((Vector3.forward) + new Vector3(0, 0, 100))).normalized;
            // Do not use Find function here - too slow
            hips = transform.Find("Aqua/Hips").transform;

            timer = Time.time;
            if (m_ForwardAmount < 0.01f) { m_ForwardAmount = 0; }
            //if (m_TurnAmount != 0) { isTurning = true; } else isTurning = false;

            //Runing
            if (m_ForwardAmount > 0.5 && m_Rigidbody.velocity.magnitude > 4 /*&& myForward > 0.5*/ && !isFalling)
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


            //Not Runnings
            if (!(m_ForwardAmount > 0.5 && m_Rigidbody.velocity.magnitude > 4 /*&& myForward > 0.5*/ && !isFalling))
            {
                // print("asdas");
                if (!runStopPlaying && turnningAround)
                {
                    runStop = Time.time;
                    runStopPlaying = true;
                    StopRun();
                }
                if (runStop + 1f < timer)
                {
                    isRunning = false;
                    runStopPlaying = false;
                }

            }


            //print(m_Rigidbody.velocity.magnitude);


            if (m_ForwardAmount <= 0.5 && m_ForwardAmount > 0.1) { isWalking = true; } else isWalking = false;
            if (m_ForwardAmount == 0) { isIdle = true; } else isIdle = false;


            if (!ledgeHanging && !ledgeClimbUp && landForwardHeavy || landLight)
            {
                landing = true;
                //m_Animator.applyRootMotion = false;
                isRunning = false;
            }

            if (Input.GetMouseButton(0) && isRunning)
            {
                runKick = true;

            }
            if (Input.GetMouseButton(0) && isWalking || Input.GetMouseButton(0) && isIdle && m_IsGrounded)
            {
                walkKick = true;

            }
            if (Input.GetMouseButtonUp(0))
            {
                runKick = false;
                walkKick = false;
            }
            if (Input.GetMouseButton(1))
            {
                inRagdol = true;
            }
            if (Input.GetKeyDown(KeyCode.Tab) && isRunning && m_IsGrounded && !isExhausted)
            {
                flipReady = true;
            }
            if (flipReady)
            {
                DoFlip();
            }

            if (isExhausted)
            {
                runSlide = false;
            }

            if (runSlide)
            {
                RunSlide();
            }
            detectWallsAndIdle();//m_ForwardAmount, detectWall);
            print("Root motion " + m_Animator.applyRootMotion);

        }
        void LateUpdate()
        {
            leftFoot = transform.Find("Aqua/Hips/Left_leg/Left_knee/Left_foot");
            rightFoot = transform.Find("Aqua/Hips/Right_leg/Right_knee/Right_foot").transform;

            stopAnimatingBone("rightLeg");

            //aimToMouse("Neck");
            if (!isIdle && stepUpPlaying)
            {
                smoothRotateBone(spine);
            }
            //print("Layer 0 is " + m_Animator.GetLayerWeight(0));
            //print("Layer 1 is " + m_Animator.GetLayerWeight(1));

            if (dead)
            {
                m_Animator.enabled = false;
                inRagdol = true;

            }

        }
        private void FixedUpdate() //Only for Physics
        {

            //Change gravity for the ponytail
            foreach (Rigidbody joints in ponyTail)
            {
                if (!crashedInAir && !inRagdol)
                {
                    //print("Adding force");
                    joints.AddForce(30 * Physics.gravity);
                }
            }
            emotions.EyeBlink();
            emotions.Breath(isRunning, isExhausted);
            if (runSlide)
            {
                m_Rigidbody.AddRelativeForce(Vector3.forward * _slideForce, ForceMode.Impulse);
            }

            Exhausted();
            setGuiStats();

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

            else if (mouseWheel > 0.1f && mouseWheel < 0.3f)
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

            mouseWheel += Input.GetAxis("Mouse ScrollWheel");
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                setForwardAmount(move.z);
            }


            m_Crouching = crouch;

            if (m_Crouching) // To crouch from layer 0 to 2
            {
                StartCoroutine(AnimatorLayerSmoother(1, m_Animator.GetLayerWeight(0), 1, 0.02f, 2));
            }
            else // To stand up, layer 2 to 0
            {
                StartCoroutine(AnimatorLayerSmoother(0, m_Animator.GetLayerWeight(0), 1, 0.02f, 2));
            }


            if (m_IsGrounded && !runSlide && !stepUpPlaying && !landing && !ledgeHang)
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
            // Turn on LegIk only on suitable conditions
            if (isFalling || m_Crouching || runSlide || landing || RunJumpLeft || RunJumpRight || facingWall || isJumping || runKick || walkKick)
            {
                footIkOn = false;
            }
            else
            {
                footIkOn = true; //ON
            }

            // send input and other state parameters to the animator
            UpdateAnimator(move);

        }
        //Print all animation names and index
        void printAnimationNameIndex()
        {
            for (int i = 0; i < animatorController.animationClips.Length; i++)
            {
                //print(i + " is animation " + GetAnimationClips(i).name);
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
                try
                {
                    print("Collision with " + collision.gameObject.name);
                    collision.rigidbody.AddRelativeForce(collision.relativeVelocity * 400, ForceMode.Impulse);
                }
                catch (NullReferenceException ex)
                {
                    //Debug.Log("Cannot kick without rigid body");
                }
            }
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.green);
            }


            if (isJumping && collision.gameObject && collision.gameObject.tag != "Leg" && collision.gameObject.tag != "Foot" && collision.gameObject.tag != "Torso" && collision.gameObject.tag != "Arm")
            {
                crashedInAir = true;
            }

            if ((walkKick || runKick) && collision.gameObject.name != "Foot")
            {
                try
                {
                    //print("Add Force");
                    collision.rigidbody.AddRelativeForce(collision.relativeVelocity * 40, ForceMode.Impulse);
                }
                catch (NullReferenceException ex)
                {
                    //Debug.Log("Cannot kick without rigid body");
                }
            }
            if (collision.gameObject.tag == "Foot" || collision.gameObject.tag == "Leg" || collision.gameObject.tag == "Torso" || collision.gameObject.tag == "Arm")
            {
                // print("Ignoring collision");
                Physics.IgnoreCollision(collision.collider, m_Capsule, true);
            }

        }

        bool frontFootRight()
        {
            if (Mathf.Abs(rightFoot.transform.localPosition.z) < Mathf.Abs(leftFoot.transform.localPosition.z))
            {
                return true;
            }
            else return false;
        }

        //Change, detect magnitude drop and then paly animation with isgrounded condition.
        public void StopRun()
        {

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
            {
                if (Input.GetKeyUp(KeyCode.W) && m_ForwardAmount > 0.5 || Input.GetKeyUp(KeyCode.D) && m_ForwardAmount > 0.5
                    || Input.GetKeyUp(KeyCode.A) && m_ForwardAmount > 0.5 || Input.GetKeyUp(KeyCode.S) && m_ForwardAmount > 0.5)
                {
                    if (m_IsGrounded && !runSlide)
                    {
                        runStopPlaying = true;
                        setForwardAmount(0);
                    }
                }
            }
            else { runStopPlaying = false; }
        }

        void detectWallsAndIdle()//float currentSpeed, float detectWall)
        {
            //Debug.DrawLine(m_Capsule.transform.position + new Vector3(0, m_Capsule.height / 2, 0), m_Capsule.transform.position + new Vector3(0, -m_GroundCheckDistance, 0), Color.yellow);

            //if (Physics.Raycast(m_Capsule.transform.position + new Vector3(0, m_Capsule.height / 2, 0), Vector3.down, out hitInfo, m_GroundCheckDistance))

            // Debug.DrawRay(hips.TransformPoint(0, 0, 0.15f), fwd, Color.yellow);
            //if (Physics.Raycast(hips.TransformPoint(0, 0, 0.15f), fwd, out hitInfo, detectWall) && m_IsGrounded)
            // print(hips.transform.position);
            Debug.DrawRay(m_Capsule.transform.position + new Vector3(0, hipToFootDist / 2, 0), fwd, Color.blue);
            if (Physics.Raycast(m_Capsule.transform.position + new Vector3(0, hipToFootDist / 2, 0), fwd, out hitInfo, wallDetectDist) && m_IsGrounded)
            {

                //Stop only if not pushable object (has a rigid body)
                if (hitInfo.rigidbody == null)
                {
                    facingWall = true;
                    m_Animator.applyRootMotion = false;
                    //print("Next to a wall, stoping!");
                }

            }
            else facingWall = false;
        }


        public void turnAround(string side)
        {
            if (m_ForwardAmount > 0.5 && myForward > 0.5 && !isExhausted && m_IsGrounded && isRunning && !runSlide && !isIdle)
            {
                turnningAround = true;
                if (side == "Right")
                {
                    //TO DO: bool instead
                    m_Animator.Play("TurnAroundRight");
                }
                if ((side == "Left"))
                {
                    m_Animator.Play("TurnAroundLeft");
                }
            }
        }

        void DoFlip()
        {
            //To-Do: Flip animations depending on conditions


            if (flipReady && playingFlip)
            {
                jumpPrepare = Time.time;
                playingFlip = false;
            }
            if (GetAnimationLength("FrontFlip") < jumpPrepare)
            {
                print(GetAnimationLength("FrontFlip"));
                flipReady = false;
                jumpPrepare = 0;
            }

            if (flipReady)
            {
                //m_Animator.Play("FrontFlip");
                //isJumping = true;
                emotions.Surprised();
                if (jumpPrepare + 0.3 < timer)
                {
                    m_Rigidbody.velocity = new Vector3(0, m_JumpPower * 1.2f, 0);
                    //m_Rigidbody.AddRelativeForce(0, 0, 5);
                    flipReady = false;
                    m_IsGrounded = false;
                    playingFlip = true;
                    //m_Animator.applyRootMotion = false;
                    temporaryGroundCheckDistance = 0.1f;
                    ScaleCapsule("frontFlip");
                    //print((rightFeet.transform.position.y + leftFeet.transform.position.y) / 2);
                }
            }
            else temporaryGroundCheckDistance = m_GroundCheckDistance;

        }

        void RunSlide()
        {
            if (!slideSound)
            {
                PlaySounds("gravslip");
                slideSound = true;
                slideStart = Time.time; //Could have indicidual if
                standUp = false;
            }

            ScaleCapsule("slide");

            //Add force after few seconds of sliding, approx when character is on ground
            if (GetAnimationClips(29).length / 2 < slideStart && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("RunSlideStart"))
            {
                _slideForce = slideForce * m_ForwardAmount;
            }
            else if (!slideForceEnabled && (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("RunSlideLeft") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("RunSlideRight")))
            {
                slideForceEnabled = true;
                _slideForce = slideForce * m_ForwardAmount;
            }
            //remove force linearnly
            if (_slideForce > 10)
            {
                _slideForce = _slideForce - 10; //* Mathf.Pow(0.99f, Time.time);
            }
            else _slideForce = 0;

            //Slide movements
            if (Input.GetKeyDown(KeyCode.A))
            {
                runSlideSide = 0;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                runSlideSide = 1;
            }
            if (Input.GetKeyDown(KeyCode.W) && !standUp)
            {
                standUp = true;
                standUpStart = timer;
            }

            if (standUp)
            {
                slideStart = 0;

                if (GetAnimationClips(41).length / 18 < timer - standUpStart)
                {
                    slideForceEnabled = false;
                    standUp = false;
                    runSlide = false;
                    slideSound = false;
                    standUpStart = 0;
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
                setStamina(0, Time.smoothDeltaTime / stamina);

                if (runTime > maxStamina && isRunning)
                {
                    StartCoroutine(AnimatorLayerSmoother(1, m_Animator.GetLayerWeight(1), 1, 0.02f, 1));
                    //exhaustTime = 0;
                    if (m_Animator.GetLayerWeight(1) >= 0.95)
                    {
                        isExhausted = true;
                    }
                }
            }
            else if (stamina < maxStamina)
            {
                setStamina(maxStamina, Time.smoothDeltaTime / recovery);
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

                if (stamina <= maxStamina)
                {
                    setStamina(maxStamina, Time.smoothDeltaTime / recovery);
                }


                if (exhaustTime > recovery && isExhausted)
                {
                    runTime = 0;
                    playing = false;

                    //m_Animator.SetLayerWeight(0, 1);
                    StartCoroutine(AnimatorLayerSmoother(0, m_Animator.GetLayerWeight(0), 1, 0.02f, 1));

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


        IEnumerator AnimatorLayerSmoother(int function, float start, float finish, float interval, int layer)
        {
            yield return new WaitForSeconds(interval);
            //for going back to Original layer 0
            if (function == 0)
            {
                if (m_Animator.GetLayerWeight(0) <= 1 && m_Animator.GetLayerWeight(layer) >= 0)
                {
                    m_Animator.SetLayerWeight(layer, m_Animator.GetLayerWeight(layer) - interval);
                    m_Animator.SetLayerWeight(0, m_Animator.GetLayerWeight(0) + interval);
                    //m_ForwardAmount += interval;

                }
            }
            //for going to new layer X
            if (function == 1)
            {
                if (m_Animator.GetLayerWeight(layer) <= 1 && m_Animator.GetLayerWeight(0) >= 0)
                {
                    //m_ForwardAmount -= interval;
                    m_Animator.SetLayerWeight(layer, m_Animator.GetLayerWeight(layer) + interval);
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
                    headSound = head.GetComponent<Sounds>().Clips[UnityEngine.Random.Range(0, 6)];
                    head.GetComponent<Sounds>().audioSources[UnityEngine.Random.Range(0, 6)].PlayOneShot(headSound, 0.1f);

                }
                //Delete when all specific sounds are implemented
                if (name == "jumpLand")
                {
                    //GameObject.Find("Camera").GetComponent<AudioEchoFilter>().delay = 1000;
                    footSound = GetComponent<Sounds>().Clips[20];
                    AudioSource.PlayClipAtPoint(footSound, transform.position);
                }
                //END Delete when all specific sounds are implemented
                if (name == "exhaustStop")
                {
                    headSound = head.GetComponent<Sounds>().Clips[6];
                    head.GetComponent<Sounds>().audioSources[6].PlayOneShot(headSound, 1);
                }
                if (Physics.Raycast(transform.position + new Vector3(0, m_Capsule.height / 2, 0), down, out hitSteps, 10) && hitSteps.transform.gameObject.tag == "Concrete")
                {
                    if (name == "jumpLand")
                    {
                        footSound = GetComponent<Sounds>().Clips[20];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "steps" && isWalking && !isRunning)
                    {
                        footSound = GetComponent<Sounds>().Clips[UnityEngine.Random.Range(25, 44)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);

                    }
                    if (name == "stepsRun" && isRunning && !isWalking || name == "stepsRun" && isExhausted)
                    {
                        footSound = GetComponent<Sounds>().Clips[UnityEngine.Random.Range(25, 44)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "gravslip")
                    {
                        footSound = GetComponent<Sounds>().Clips[UnityEngine.Random.Range(21, 24)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                }
                //Check if ground is with gravel surface
                if (Physics.Raycast(transform.position + new Vector3(0, m_Capsule.height / 2, 0), down, out hitSteps, 10) && hitSteps.transform.gameObject.tag == "Gravel")
                {
                    if (name == "jumpLand")
                    {
                        footSound = GetComponent<Sounds>().Clips[20];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "steps" && !isRunning && isWalking)
                    {
                        footSound = GetComponent<Sounds>().Clips[UnityEngine.Random.Range(0, 20)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);

                    }
                    if (name == "stepsRun" && isRunning && !isWalking)
                    {
                        footSound = GetComponent<Sounds>().Clips[UnityEngine.Random.Range(0, 20)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                    if (name == "gravslip")
                    {
                        footSound = GetComponent<Sounds>().Clips[UnityEngine.Random.Range(21, 24)];
                        AudioSource.PlayClipAtPoint(footSound, transform.position);
                    }
                }
            }

        }

        void ScaleCapsuleForCrouching(bool crouch)
        {
            /*
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
            */
        }


        void ScaleCapsule(string state)
        {

            if (state == "ground" && !landing)
            {
                //m_Capsule.height = Mathf.Lerp(m_Capsule.height, m_CapsuleHeight, 0.01f);
                m_Capsule.height = Mathf.Lerp(m_Capsule.height, m_CapsuleHeight, Time.time);
                m_Capsule.center = new Vector3(m_CapsuleCenter.x, Mathf.Lerp(m_Capsule.center.y, m_CapsuleCenter.y, Time.time), m_CapsuleCenter.z);
            }
            if (state == "jump")
            {
                //m_IsGrounded = false;
                m_Capsule.height = m_CapsuleHeight / 2;
                m_Capsule.center = m_CapsuleCenter * 2;
            }
            if (state == "slide")
            {
                m_Capsule.height = m_CapsuleHeight / 4;
                m_Capsule.center = m_CapsuleCenter / 5;
                m_Animator.applyRootMotion = false;
            }
            if (state == "frontFlip")
            {
                m_Capsule.height = 0.3f;
            }
            if (state == "falling")//&& !landForwardHeavy)
            {
                //m_Capsule.height = m_CapsuleHeight / 2; //Mathf.Lerp(m_Capsule.height, m_CapsuleHeight, 0.01f);
                // m_Capsule.center = m_CapsuleCenter * 2;
            }
            if (state == "heavyLanding")
            {
                //print("Call");
                m_Capsule.height = m_CapsuleHeight / 3f;
                m_Capsule.center = m_CapsuleCenter / 3f;
            }
            if (state == "stepUp")
            {
                //m_CapsuleHeight = Mathf.Lerp(m_CapsuleHeight, 0.3f, 0.1f);
                m_CapsuleCenter = new Vector3(0, Mathf.Lerp(m_CapsuleCenter.y, 0.38f, 0.001f), 0); /// TEST State 
            }
            if (state == "stepUpDone")
            {
                m_Capsule.center = new Vector3(0, Mathf.Lerp(m_CapsuleCenter.y, 0.038f, 0.001f), 0); /// TEST State 
            }
        }


        void PreventStandingInLowHeadroom()
        {
            // prevent standing up in crouch-only zones
            /*
            if (!m_Crouching)
            {
                Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
                float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
                if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, ~0, QueryTriggerInteraction.Ignore))
                {
                    m_Crouching = true;
                }
            }
            */
        }


        void UpdateAnimator(Vector3 move)
        {
            //Detecing walls to play stop animation

            if (!stepUpReady && !facingWall)
            {
                m_Animator.SetFloat("Forward", getForwardAmount(), 0.1f, Time.deltaTime);
            }
            else if (facingWall)
            {
                m_Animator.SetFloat("Forward", getForwardAmount(), getForwardAmount(), Time.deltaTime);
            }

            m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
            m_Animator.SetFloat("Forward", m_ForwardAmount);
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
            //m_Animator.SetFloat("stepUpLeg", stepUpLeg);
            m_Animator.SetBool("climbPlaying", stepUpPlaying);
            m_Animator.SetFloat("runSlideSide", runSlideSide);
            m_Animator.SetFloat("runSlideForward", runSlideForward);
            m_Animator.SetBool("standUp", standUp);
            m_Animator.SetBool("runKick", runKick);
            m_Animator.SetBool("walkKick", walkKick);
            m_Animator.SetBool("frontFootRight", frontFootRight());
            m_Animator.SetBool("RunStop", runStopPlaying);
            m_Animator.SetBool("inRagdol", inRagdol);
            m_Animator.SetBool("playingFlip", playingFlip);
            m_Animator.SetBool("StepOverLeft", WalkStepOverLeft);
            m_Animator.SetBool("StepOverRight", WalkStepOverRight);
            m_Animator.SetBool("idleJump", idleJump);
            m_Animator.SetBool("FacingWall", facingWall);
            m_Animator.SetBool("ledgeHang", ledgeHang);
            m_Animator.SetBool("ledgeClimbUp", ledgeClimbUp);

            if (!m_IsGrounded && isJumping)
            {
                m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);

            }

            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
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

            //Play leg step over animation
            if (!isRunning && !isWalking && !runSlide && m_IsGrounded)
            {
                if (m_TurnAmount > 0.3)
                {
                    WalkStepOverRight = true;
                }
                else WalkStepOverRight = false;

                if (m_TurnAmount < -0.3)
                {
                    WalkStepOverLeft = true;
                }
                else WalkStepOverLeft = false;
            }
        }


        void HandleAirborneMovement()
        {
            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);
            //m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? temporaryGroundCheckDistance : 0.01f;

            if (isFalling && !timeInAirBool)
            {
                timeInAirBool = true;
                timeInAirStart = timer;
            }
            timeInAir = timer - timeInAirStart;
            if (isJumping)
            {
                ScaleCapsule("jump");
            }
        }


        void HandleGroundedMovement(bool crouch, bool jump)
        {
            timeInAirBool = false;

            //Jump
            if (!isJumping && jump && !crouch && m_IsGrounded && !isExhausted && !isFalling && !runSlide)
            {
                //Jump if movin
                if (m_ForwardAmount > 0.3 && !idleJump)
                {
                    runJump = true;
                }
                //Jump while Idle
                else
                {
                    jumpStart = timer;
                    idleJump = true;
                }
            }
            else
            {
                temporaryGroundCheckDistance = m_GroundCheckDistance;
                ScaleCapsule("ground");
            }

            // Idle Jump
            if (idleJump)
            {
                isJumping = true;
                jump = false;
                runJump = false;

                if (animationJump)
                {
                    idleJump = false;
                    m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                    m_Animator.applyRootMotion = false;
                    temporaryGroundCheckDistance = m_GroundCheckDistance / 4;
                    emotions.Surprised();
                }
            }

            //Run Jump
            if (runJump && !isJumping)
            {

                jumpStart = timer;
                isJumping = true;
                jump = false;

                // if(!facingWall){
                m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
                //} else{}

                m_Animator.applyRootMotion = false;
                temporaryGroundCheckDistance = m_GroundCheckDistance / 4;
                emotions.Surprised();
                runJump = false;

            }

            //End of IsJumping
            if (isJumping && !jump && jumpStart + GetAnimationLength("RunJump") / 8 < timer && !ledgeHang)
            {
                isJumping = false;
                animationJump = false;


            }

            //Landing is over when animation is over
            if (timeInAir + GetAnimationLength("LandForwardHeavy") * 0.2f < timer)
            {
                landForwardHeavy = false;
                landLight = false;
                timeInAirBool = true;
                landing = false;
            }
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

            //to hang and climb the ledge
            if (ledgeDetected && !ledgeHanging)
            {
                if (isJumping /*|| isFalling*/)
                {
                    startLedgeHang = true;
                    RotateTowards(ledgeObject);
                }
                //Starts function of ledge hang
                if (!ledgeClimbUp && startLedgeHang)
                {
                    //check when hanging
                    if (!ledgeHanging && animationJump)
                    {
                        StartCoroutine(StartLedgeHang());
                        ledgeHangStoped = false;
                        SetAnimationJump();
                    }
                }
            }

            //Ledge climbing up
            if (ledgeClimbUp)
            {
                ledgeHang = false;
                isJumping = false;
                idleJump = false;
                climbUpPositionChang = true;
                m_Animator.applyRootMotion = true;
            }
            else if (climbUpPositionChang) //Change position right after Ledge climbing up ends
            {
                climbUpPositionChang = false;
                ledgeClimbUp = false;
                transform.position = lastLedgePosition.position;
            }

            // if dropping or climbin up (button c or m_jump while ledgehanging)
            if (endLedgeHang)
            {
                ledgeHangStoped = true;
                StopCoroutine(StartLedgeHang());
                ledgeHang = false;
                endLedgeHang = false;
                ledgeHangRotateAmount = -160f;
                endingLedgeHang = true;
                ledgeHanging = false;
            }

            //to handle rotations of character when endLedgeHang animation is playing
            if (endingLedgeHang && !m_IsGrounded)
            {
                //Vector3 ledgeHangEndLookAtPos = transform.localPosition - new Vector3(10, 0, -10);

                // transform.rotation = Quaternion.LookRotation(); 


                // transform.Find("Aqua").rotation = Quaternion.LookRotation(ledgeHangEndLookAtPos);

                //transform.Find("Aqua").transform.Rotate(0, -160, 0);
                //transform.Find("Aqua").localEulerAngles = new Vector4(0,0,0);
                m_Animator.applyRootMotion = false;

                //transform.rotation = hips.transform.rotation;
            }
        }

        // Setter getter for m_forwardAmount
        public void setForwardAmount(float preferedMovingSpeed)
        {

            if (facingWall)
            {
                m_ForwardAmount = Mathf.Lerp(getForwardAmount(), 0, 0.1f);
                //m_ForwardAmount = 0;
            }
            else
            {
                m_ForwardAmount = Mathf.Lerp(getForwardAmount(), preferedMovingSpeed, 0.06f);
            }

        }
        float getForwardAmount()
        {
            return m_ForwardAmount;
        }


        void setStamina(float newStamina, float staminaChangeSpeed)
        {
            if (stamina >= 0 && stamina <= maxStamina)
            {
                stamina = Mathf.Lerp(stamina, newStamina, staminaChangeSpeed);
            }
            /*           if (stamina < 0) { stamina = 0; }
            if (stamina > maxStamina) { stamina = maxStamina; }
            */
        }
        void setHealth(int healthChange)
        {
            if (health > 0 && health <= maxHealth)
            {
                health += healthChange;
                print("Health loss " + healthChange);
            }
            else if (health < 0) { health = 0; }
            else if (health > maxHealth) { health = maxHealth; }

            if (dead) { health = 0; }
        }

        void CheckGroundStatus()
        {
#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(m_Capsule.transform.position + new Vector3(0, m_Capsule.height / 2, 0), m_Capsule.transform.position - new Vector3(0, m_GroundCheckDistance, 0) /*_Capsule.transform.position - new Vector3(0, m_Capsule.height/2, 0)*/, Color.yellow);
#endif
            // 0.1f is a small offset to start the ray from inside the character
            // it is also good to note that the transform position in the sample assets is at the base of the character
            // Checking how far is ground from half size of main colider to ground side, with vector leangth of m_groundCheckdistance
            if (Physics.Raycast(m_Capsule.transform.position + new Vector3(0, m_Capsule.height / 2, 0), Vector3.down, out hitInfo, m_GroundCheckDistance))
            {
                //m_GroundNormal = hitInfo.normal;
                m_IsGrounded = true;

                if (!landed && isFalling)
                {
                    landedStart = Time.time;
                    landed = true;
                    isFalling = false;
                    //ScaleCapsule("heavyLanding");
                    PlaySounds("jumpLand");
                    //emotions.Surprised();
                }
                playingFall = false;
                isFalling = false;
            }
            else // In Air
            {
                m_IsGrounded = false;
                //landed = false;
                //m_GroundNormal = Vector3.up;
                ScaleCapsule("falling");
                if (!stepUpPlaying && !isJumping && !ledgeHang && !ledgeHanging && !ledgeClimbUp)
                {
                    //Check is trully falling. It waits for character for being 1 sec not grounded to go to falling state.
                    if (!playingFall)
                    {
                        fallStart = Time.time;
                        playingFall = true;
                        landed = true;
                    }

                    if (fallStart + 0.2 < timer && !ledgeHanging)
                    {
                        healthChange = -(timer - fallStart) * 3;
                        //print("healthChange " + healthChange);
                        isFalling = true;
                        landLight = true;
                        emotions.Surprised();

                    }
                    if (fallStart + 0.5 < timer && !ledgeHanging)
                    {
                        landLight = false;
                        isFalling = true;
                        landForwardHeavy = true;

                    }
                    if (fallStart + 0.6 < timer && Physics.Raycast(transform.position + (Vector3.down * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
                    {
                        //isFalling = false;
                        // landLight = false;
                        //landForwardHeavy = false;
                        //Die here when lands
                    }
                }

                if (isJumping)
                {
                    if (!playingFall)
                    {
                        fallStart = Time.time;
                        playingFall = true;
                        isFalling = false;
                    }
                    if (fallStart + 0.8 < timer)
                    {
                        healthChange = -(timer - fallStart) * 3;
                        isFalling = true;
                        //landLight = false;
                        landForwardHeavy = true;
                    }
                }

            }
            if (m_IsGrounded)
            {
                groundedTimer = Time.time;

            }
            else
            {
                groundedTimer = 0;
            }

            //Landed when touched the ground from fall
            if (landed && timer > landedStart + 0.1f)
            {
                landed = false;
                if (landForwardHeavy)
                {
                    setHealth((int)(healthChange)); //Set minus health according how long it is falling
                    healthChange = 0;
                }
            }

        }


        private void legIK(int _foot)
        {
            if (_foot == 1) { legRay = 0.1f; }
            if (_foot == 2) { legRay = -0.1f; }
            if (isRunning && !isExhausted) { rayLength = 0.4f; rayPoss = 0.6f; }
            if (isRunning && isExhausted) { rayLength = 0.38f; }
            if (isWalking) { rayLength = hipToFootDist / 2.4f; rayPoss = 0.3f; }
            if (isIdle) { rayLength = hipToFootDist / 2.2f; rayPoss = 0.3f; } //Test org 0.1f

            //Debug.DrawRay(new Vector3(legRay, hipToFootDisc / 2, rayPoss), new Vector3(0, -hipToFootDisc / 2, 0), Color.blue);
            Debug.DrawRay(hips.TransformPoint(legRay, -hipToFootDist / 4, rayPoss), new Vector3(0, -hipToFootDist / 2, 0), Color.red);

            //Raycast according state, ignore feet
            if (Physics.Raycast(hips.TransformPoint(legRay, -hipToFootDist / 4, rayPoss), new Vector3(0, -hipToFootDist / 2, 0), out hitLeg, rayLength) && !(hitLeg.collider.gameObject.tag == "Foot"))
            {

                //print("Found an object - distance: " + rightFootNewPos + "Object name: " + hitRightLeg.collider.gameObject.name);
                if (_foot == 1 && !leftFootIkActive)
                {
                    slopeRight = Vector3.Cross(hitLeg.normal, -rightFoot.transform.right);
                    rightFootRot = Quaternion.LookRotation(Vector3.Exclude(hitLeg.normal, slopeRight), hitLeg.normal);

                    rightFootIkActive = true;
                    stepUpLeg = 1;

                    if (!stepUpPlaying && !stepUpReady && stepUpDone)
                    {
                        rightFootTempY = hipToFootDist / 10 + hitLeg.point.y;
                        rightFootTempX = hitLeg.point.x;
                        rightFootTempZ = hitLeg.point.z;
                    }

                    rightFootHigh = hipToFootDist / 1.45f - hitLeg.distance;
                    rightFootLow = rightFootHigh;
                    rightFootNewPosition = new Vector3(rightFootTempX, rightFootTempY, rightFootTempZ);
                    if (footSmoothingRight <= 0.99) { footSmoothingRight += 0.01f; }
                    if (footSmoothingRight > 0.95 && getForwardAmount() > 0.2f)
                    {
                        if (!ledgeHanging && !ledgeClimbUp)
                        {
                            stepUpReady = true;

                            holdingStepUpLegPlaying = false;
                            if (!isIdle)
                            {
                                setForwardAmount(0.1f);
                            }
                        }
                    }
                    else
                    {
                        stepUpReady = false;
                        //setForwardAmount(0.1f);
                    }
                }


                if (_foot == 2 && !rightFootIkActive && stepUpDone)
                {
                    slopeLeft = Vector3.Cross(hitLeg.normal, -leftFoot.transform.right);
                    leftFootRot = Quaternion.LookRotation(Vector3.Exclude(hitLeg.normal, slopeLeft), hitLeg.normal);

                    leftFootIkActive = true;
                    stepUpLeg = 0;

                    if (!stepUpPlaying && !stepUpReady && stepUpDone)
                    {
                        leftFootTempY = hipToFootDist / 10 + hitLeg.point.y;
                        if (leftFoot.transform.position.y >= leftFootTempY - leftFootTempY / 1.4f)
                        {
                            leftFootTempX = hitLeg.point.x;
                            leftFootTempZ = hitLeg.point.z;
                        }
                        else
                        {
                            leftFootTempX = leftFoot.position.x;
                            leftFootTempZ = leftFoot.position.z;
                        }

                    }

                    leftFootHigh = hipToFootDist / 1.45f - hitLeg.distance;
                    leftFootLow = leftFootHigh;
                    leftFootNewPosition = new Vector3(leftFootTempX, leftFootTempY, leftFootTempZ);
                    if (footSmoothingLeft <= 0.99) { footSmoothingLeft += 0.01f; }
                    if (footSmoothingLeft > 0.95 && getForwardAmount() > 0.2f)
                    {
                        if (!ledgeHanging && !ledgeClimbUp)
                        {
                            stepUpReady = true;

                            if (!isIdle)
                            {
                                setForwardAmount(0.1f);
                            }
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
                    ScaleCapsule("stepUp");
                    m_Rigidbody.AddRelativeForce(0, stepUpForce, 0f);


                }

                /*
                //Stop following leg from animating while climbing
                // maybe raycast down, or check last frames position and keep it there on climb
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
                stepUpOrDown = 1;
            }

            //print(leftFootTempY + " Left foot Y");
            //print(rightFootTempY + " Right foot Y");

            if (!(Physics.Raycast(hips.TransformPoint(legRay, -hipToFootDist / 4, rayPoss), new Vector3(0, -hipToFootDist / 2, 0), out hitLeg, rayLength)))
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

                if (!ledgeHanging && !ledgeClimbUp && !ledgeClimbUp)
                {
                    stepUpDone = true;
                    //Step up Stop
                    if (stepUpDone)
                    {
                        ScaleCapsule("stepUpDone");
                        //m_CapsuleCenter = new Vector3(0, 0.76f, 0f);
                        stepUpSmoothing = 0;
                        m_Rigidbody.useGravity = true;
                        stepUpPlaying = false;
                        stepUpReady = false;
                    }
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
                stepUpOrDown = 0;
            }


            if (_foot == 1)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootNewPosition);
                m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, Mathf.Lerp(footSmoothingRight, stepUpOrDown, 0.01f));
                m_Animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, Mathf.Lerp(footSmoothingRight, stepUpOrDown, 0.01f));
            }
            if (_foot == 2)
            {
                m_Animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootNewPosition);
                m_Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, Mathf.Lerp(footSmoothingLeft, stepUpOrDown, 0.01f));
                m_Animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, Mathf.Lerp(footSmoothingLeft, stepUpOrDown, 0.01f));
            }

            //m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);
        }
        //called from late update, used to stop following leg on climb up from anmating
        void stopAnimatingBone(string bone)
        {
            if (bone == "rightLeg")
            {
                if (!rightFootIkActive && stepUpReady)
                {

                }
            }
            if (bone == "leftLeg")
            {

            }

        }
        void LootAtPointer()
        {
            //print(aimSmoother);
            if (!isRunning && !runSlide && !m_Crouching)
            {
                aimSmoother += 0.01f;
                if (aimSmoother > 0.5f) aimSmoother = 0.5f;

                m_Animator.SetLookAtWeight(aimSmoother, aimSmoother - 0.3f, aimSmoother, 0);
            }

            if (isRunning)
            {
                aimSmoother -= 0.01f;
                if (aimSmoother < 0.01f) aimSmoother = 0;
                m_Animator.SetLookAtWeight(aimSmoother, aimSmoother, aimSmoother, 0);
            }

            if (runSlide)
            {
                aimSmoother += 0.01f;
                if (aimSmoother > 0.7f) aimSmoother = 0.7f;
                m_Animator.SetLookAtWeight(aimSmoother, 0, aimSmoother, 0);
            }

            if (m_Crouching)
            {
                aimSmoother -= 0.01f;
                if (aimSmoother < 0.2f) aimSmoother = 0.2f;
                m_Animator.SetLookAtWeight(aimSmoother, aimSmoother, aimSmoother, 0);
            }
            m_Animator.SetLookAtPosition(pointer.position);

        }
        void OnAnimatorIK()
        {
            if (lookAtPointer)
            {
                LootAtPointer();
            }
            if (m_Animator)
            {
                if (footIkOn && _footIkOn)
                {
                    if (rightFoot != null && leftFoot != null)
                    {
                        legIK(1);
                        legIK(2);
                    }
                }
            }
        }

        //what happens untill character reaches and stops at the ledge
        IEnumerator StartLedgeHang()
        {
            idleJump = false;
            print("LedgeHang");
            armHangPoint = ledgeObject.transform.Find("Point").transform.position;
            transform.position = Vector3.Slerp(transform.position, armHangPoint - new Vector3(0, toeToKnucklesDist, 0), 0.4f);
            RotateTowards(ledgeObject);
            ScaleCapsule("ground");
            ledgeHang = true;
            isJumping = false;
            isFalling = false;
            m_Rigidbody.useGravity = false;
            m_Rigidbody.velocity = Vector3.zero;
            yield return new WaitForSeconds(1);
        }

        //called only by animation
        public void LedgeHanging()
        {
            animationJump = false;
            //m_Rigidbody.isKinematic = true;
            startLedgeHang = false;
            ledgeHanging = true;
        }

        //called only by animation LedgeHangDrop and ledgeClimbUp
        public void EndLedgeHang()
        {
            print("ending Ledge hang");
            m_Rigidbody.useGravity = true;
            m_Rigidbody.isKinematic = false;
            ledgeHanging = false;
            ledgeClimbUp = false;
            ledgeDetected = false;

        }

        void setGuiStats()
        {

            GameManager.Get().stamina = stamina;
            GameManager.Get().health = health;
            if (!setPlayerStats) //Set once
            {
                GameManager.Get().maxStamina = stamina;
                GameManager.Get().maxHealth = health;
                setPlayerStats = true;

            }
        }

        void OnTriggerStay(Collider collider)
        {
            if (collider.tag == "Ledge")
            {
                ledgeObject = collider.gameObject;
                ledgeDetected = true;
                lastLedgePosition = ledgeObject.gameObject.transform;
            }
            else
            {
                //ledgeDetected = false;
            }
        }
        void OnTriggerExit(Collider collider)
        {
            if (collider.tag == "Ledge")
            {
                ledgeDetected = false;
            }
        }




        private void RotateTowards(GameObject gameObject)
        {

            if (gameObject == ledgeObject)
            {
                /* if (startLedgeHang)
                 {*/
                //Rotate towards ledge when jumping to ledge
                //Get direction
                try
                {
                    climbPointer = ledgeObject.transform.Find("ClimbPointer").transform.position;
                }
                catch (NullReferenceException ex)
                {
                    print("Ledge without LookAt pointer detected.");
                }
                /* }
                 if (ledgeHangStoped)
                 {
                     climbPointer = ledgeObject.transform.Find("ClimbPointer").transform.position;
                 }
                 */
            }
            else
            {
                climbPointer = gameObject.transform.position;

            }
            rotationDirection = (climbPointer - transform.position);
            rotationDirection.y = 0;
            //Rotation towards object
            rotationToObject = Quaternion.LookRotation(rotationDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToObject, 10);
        }
        public void SetAnimationJump()
        {
            animationJump = true;
        }

    }
}

