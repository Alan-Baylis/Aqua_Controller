using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
//using UnityStandardAssets.CrossPlatformInput;


public class MuscleSystem : MonoBehaviour
{
    private ThirdPersonCharacter m_Character;

    private SkinnedMeshRenderer bodyRenderer, pantiesRenderer, braRenderer;

    //Arm muscles
    private int rightBiceps, rightTriceps, rightDeltoid, leftBiceps, leftTriceps, leftDeltoid, leftBrachioradialis, rightBrachioradialis, leftExtensors, rightExtensors;
    //Leg muscles
    private int leftGastrocnemius, rightGastrocnemius, leftSemitendinosus, rightSemitendinosus, leftRectusFemoris, rightRectusFemoris, leftGastrocnemiusTemp, rightGastrocnemiusTemp;
    //Head muscles
    private int leftSternal, rightSternal;
    //Body Muscles
    private int leftTeres, rightTeres, leftTrapezius, rightTrapezius, abdominusRectus, leftLatissimus, rightLatissimus;

    public bool enableMuscles;
    [Range(0, 10)]
    public int allMuscleEffect;
    float _allMuscleEffect;
    [Range(0, 10)]
    public int buttocksEffect;
    float _buttocksEffect;
    [Range(0, 10)]
    public int backArmsEffect;
    float _backArmsEffect;
    [Range(0, 10)]
    public int frontArmsEffect;
    float _frontArmsEffect;
    [Range(0, 10)]
    public int deltoidEffect;
    float _deltoidEffect;
    [Range(0, 10)]
    public int teresMajorEffect;
    float _teresMajorEffect;
    [Range(0, 10)]
    public int trapeziusEffect;
    float _trapeziusEffect;
    [Range(0, 10)]
    public int abdominusEffect;
    float _abdominusEffect;
    [Range(0, 10)]
    public int legsEffect;
    float _legsEffect;
    float _muscleSpeed = 0.2f;
    float muscleTimer;



    void Start()
    {
        enableMuscles = true;
        m_Character = GetComponent<ThirdPersonCharacter>();
        //Get mesh renderers
        try
        {
            bodyRenderer = transform.Find("Aqua/Body").GetComponent<SkinnedMeshRenderer>();
            pantiesRenderer = transform.Find("Aqua/Panties").GetComponent<SkinnedMeshRenderer>();
            braRenderer = transform.Find("Aqua/Bra").GetComponent<SkinnedMeshRenderer>();
        }
        catch (NullReferenceException ex)
        {
            print("Can't find Mesh Renderer");
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //enable muscles if m is pressed
        if (!enableMuscles && Input.GetKeyDown(KeyCode.M))
        {
            enableMuscles = true;

        }
        // dissable muscles if m is pressed, set all blend shapes to 0
        else if (Input.GetKeyDown(KeyCode.M))
        {
            enableMuscles = false;

            leftBiceps = 0;
            leftTriceps = 0;
            leftDeltoid = 0;
            leftExtensors = 0;
            leftBrachioradialis = 0;

            rightBiceps = 0;
            rightTriceps = 0;
            rightDeltoid = 0;
            rightExtensors = 0;
            rightBrachioradialis = 0;

            leftGastrocnemius = 0;
            leftSemitendinosus = 0;

            rightGastrocnemius = 0;
            rightSemitendinosus = 0;

            leftRectusFemoris = 0;
            rightRectusFemoris = 0;

            leftSternal = 0;
            rightSternal = 0;

            rightTeres = 0;
            leftTeres = 0;
            leftTrapezius = 0;
            rightTrapezius = 0;

            abdominusRectus = 0;
            rightLatissimus = 0;
            leftLatissimus = 0;
        }


        _allMuscleEffect = allMuscleEffect / 10f;
        _buttocksEffect = buttocksEffect / 10f;
        _backArmsEffect = backArmsEffect / 10f;
        _frontArmsEffect = frontArmsEffect / 10f;
        _deltoidEffect = deltoidEffect / 10f;
        _trapeziusEffect = trapeziusEffect / 10f;
        _teresMajorEffect = teresMajorEffect / 10f;
        _abdominusEffect = abdominusEffect / 10f;
        _legsEffect = legsEffect / 10f;

        //Increase decrease muscle effect on spot
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (allMuscleEffect > 1)
            {
                allMuscleEffect -= 1;
            }
            else allMuscleEffect =  0;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            if (allMuscleEffect < 10)
            {
                allMuscleEffect += 1;
            }
            else
            {
                allMuscleEffect = 10;

            }
        }


        if (enableMuscles)
        {
            //To tell gui about muscle system status
            GUIStats.enableMuscles = true;

            muscleTimer = Time.time;
            try
            {
                leftBiceps = (int)(80 - (100 * Vector3.Distance(transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm").transform.position, transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.position)));
                rightBiceps = (int)(80 - (100 * Vector3.Distance(transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm").transform.position, transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.position)));

                if (m_Character.isJumping)
                {
                    abdominusRectus = 100;
                }
                //Condition 2
                if (m_Character.ledgeHanging || m_Character.ledgeClimbUp || m_Character.startLedgeHang)
                {
                    leftTriceps = (int)Mathf.Lerp(leftTriceps, 100f, _muscleSpeed);
                    rightTriceps = (int)Mathf.Lerp(rightTriceps, 100f, _muscleSpeed);
                    abdominusRectus = (int)Mathf.Lerp(abdominusRectus, 100f, _muscleSpeed);

                    leftBrachioradialis = (int)Mathf.Lerp(leftBrachioradialis, 100f, _muscleSpeed);
                    rightBrachioradialis = (int)Mathf.Lerp(rightBrachioradialis, 100f, _muscleSpeed);
                    leftExtensors = (int)Mathf.Lerp(leftExtensors, 100f, _muscleSpeed);
                    rightExtensors = (int)Mathf.Lerp(rightExtensors, 100f, _muscleSpeed);

                    leftDeltoid = (int)Mathf.Lerp(leftDeltoid, 100f, _muscleSpeed);
                    rightDeltoid = (int)Mathf.Lerp(rightDeltoid, 100f, _muscleSpeed);
                    leftLatissimus = (int)Mathf.Lerp(leftLatissimus, 100f, _muscleSpeed);
                    rightLatissimus = (int)Mathf.Lerp(rightLatissimus, 100f, _muscleSpeed);
                    leftExtensors = (int)Mathf.Lerp(leftExtensors, 100f, _muscleSpeed);
                    rightExtensors = (int)Mathf.Lerp(rightExtensors, 100f, _muscleSpeed);
                    leftBrachioradialis = (int)Mathf.Lerp(leftBrachioradialis, 100f, _muscleSpeed);
                    rightBrachioradialis = (int)Mathf.Lerp(rightBrachioradialis, 100f, _muscleSpeed);

                    leftSternal = (int)Mathf.Lerp(leftSternal, 100f, _muscleSpeed);
                    rightSternal = (int)Mathf.Lerp(rightSternal, 100f, _muscleSpeed);

                    leftTeres = (int)Mathf.Lerp(leftTeres, 100f, _muscleSpeed);
                    leftTrapezius = (int)Mathf.Lerp(leftTrapezius, 100f, _muscleSpeed);
                    rightTeres = (int)Mathf.Lerp(rightTeres, 100f, _muscleSpeed);
                    rightTrapezius = (int)Mathf.Lerp(rightTrapezius, 100f, _muscleSpeed);
                }
                else // Condition 1
                {

                    leftTriceps = Mathf.Abs(leftBiceps - 80);
                    rightTriceps = Mathf.Abs(rightBiceps - 80);
                    abdominusRectus = (int)(transform.Find("Aqua/Hips/Spine/Chest").transform.position.y - transform.Find("Aqua/Hips").transform.position.y);

                    if (transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.localEulerAngles.y > 0)
                    {
                        leftBrachioradialis = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.localEulerAngles.y));
                        leftExtensors = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.localEulerAngles.y) * -1);
                    }
                    if (transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.localEulerAngles.y > 0)
                    {
                        rightBrachioradialis = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.localEulerAngles.y));
                        rightExtensors = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.localEulerAngles.y) * -1);
                    }
                    leftDeltoid = (int)Mathf.Max(0, (transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm").transform.position.y - 0.5f -
                        transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm").transform.position.y) * -100);
                    rightDeltoid = (int)Mathf.Max(0, (transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm").transform.position.y - 0.5f -
                        transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm").transform.position.y) * -100);

                    leftLatissimus = leftDeltoid;
                    rightLatissimus = rightDeltoid;

                    //Neck muscles
                    rightSternal = Mathf.Max(0, 90 - (int)transform.Find("Aqua/Hips/Spine/Chest/Neck").transform.localEulerAngles.y) * 2;
                    leftSternal = Mathf.Max(0, (int)transform.Find("Aqua/Hips/Spine/Chest/Neck").transform.localEulerAngles.y - 80) * 2;

                    //Body muscles
                    leftTeres = (int)Mathf.Abs((transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm").transform.localRotation.x) * 400);
                    leftTrapezius = leftTeres;
                    rightTeres = (int)Mathf.Abs((transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm").transform.localRotation.x) * 400);
                    rightTrapezius = rightTeres;
                }


                //Leg muscles
                if (m_Character.m_IsGrounded)
                {
                //Left leg muscles
                    //Condition 1 Shifting of weight from one leg to another
                    leftGastrocnemius = (int)Mathf.Max(0, transform.Find("Aqua/Hips").transform.localPosition.x * -3000);
                    //Condition 2 squating 
                    //Possible to raycast from each leg to see if a single leg is grounded
                    leftGastrocnemiusTemp = (int)((transform.Find("Aqua/Hips/Left_leg/Left_knee/Left_foot").transform.position.y - transform.Find("Aqua/Hips").transform.position.y) * 400) + 440;
                    if (leftGastrocnemius < leftGastrocnemiusTemp) //Check if other condition of 
                    {
                        leftGastrocnemius = leftGastrocnemiusTemp;
                    }

                 //Right Leg muscles
                    //Condition 1 Shifting of weight from one leg to another
                    rightGastrocnemius = (int)Mathf.Max(0, transform.Find("Aqua/Hips").transform.localPosition.x * 3000);
                    //Condition 2 squating 
                    //Possible to raycast from each leg to see if a single leg is grounded
                    rightGastrocnemiusTemp = (int)((transform.Find("Aqua/Hips/Right_leg/Right_knee/Right_foot").transform.position.y - transform.Find("Aqua/Hips").transform.position.y) * 400) + 440;
                    if (rightGastrocnemius < rightGastrocnemiusTemp) //Check if other condition of 
                    {
                        rightGastrocnemius = rightGastrocnemiusTemp;
                    }
                }else
                {
                    leftGastrocnemius = 0;
                    rightGastrocnemius = 0;
                }


                leftSemitendinosus = leftGastrocnemius;
                leftRectusFemoris = leftGastrocnemius;
                rightSemitendinosus = rightGastrocnemius;
                rightRectusFemoris = rightGastrocnemius;







            }
            catch (NullReferenceException ex)
            {
                print("Can't find one of the bones for muscle system");
            }
            StartCoroutine(Flex());
        }
        else
        {
            if (muscleTimer + 3 > Time.time)
            {
                StartCoroutine(Flex());
            }
            GUIStats.enableMuscles = false;
        }
    }

    private IEnumerator Flex()
    {
        try
        {
            //Left arm muscles
            bodyRenderer.SetBlendShapeWeight(14, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(14), leftBiceps * _allMuscleEffect * _frontArmsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(16, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(16), (int)(leftTriceps * _allMuscleEffect * _backArmsEffect), 1));
            bodyRenderer.SetBlendShapeWeight(18, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(18), (int)(leftDeltoid * _allMuscleEffect * _deltoidEffect), 1));
            bodyRenderer.SetBlendShapeWeight(26, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(26), leftBrachioradialis * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(28, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(28), leftExtensors * _allMuscleEffect * _frontArmsEffect, 1));

            //Right arm muscles
            bodyRenderer.SetBlendShapeWeight(15, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(15), rightBiceps * _allMuscleEffect * _frontArmsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(17, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(17), rightTriceps * _allMuscleEffect * _backArmsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(19, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(19), (int)(rightDeltoid * _allMuscleEffect * _deltoidEffect), 1));
            bodyRenderer.SetBlendShapeWeight(27, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(27), (int)(rightBrachioradialis * _allMuscleEffect * _frontArmsEffect), 1));
            bodyRenderer.SetBlendShapeWeight(29, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(29), rightExtensors * _allMuscleEffect * _frontArmsEffect, 1));

            //Back leg muscles
            bodyRenderer.SetBlendShapeWeight(20, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(20), leftGastrocnemius * _allMuscleEffect * _legsEffect, 0.5f));
            bodyRenderer.SetBlendShapeWeight(23, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(23), rightSemitendinosus * _allMuscleEffect * _legsEffect, 0.5f));

            bodyRenderer.SetBlendShapeWeight(21, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(21), rightGastrocnemius * _allMuscleEffect * _legsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(22, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(22), leftSemitendinosus * _allMuscleEffect * _legsEffect, 1));

            //Front leg muscles
            bodyRenderer.SetBlendShapeWeight(36, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(36), leftRectusFemoris * _allMuscleEffect * _legsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(37, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(37), rightRectusFemoris * _allMuscleEffect * _legsEffect, 1));

            //Buttocks & panties
            bodyRenderer.SetBlendShapeWeight(24, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(24), (int)(leftGastrocnemius * _allMuscleEffect * _buttocksEffect), 0.5f));
            bodyRenderer.SetBlendShapeWeight(25, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(25), (int)(rightGastrocnemius * _allMuscleEffect * _buttocksEffect), 1));
            pantiesRenderer.SetBlendShapeWeight(0, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(0), (int)(leftGastrocnemius * _allMuscleEffect * _buttocksEffect), 0.5f));
            pantiesRenderer.SetBlendShapeWeight(1, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(1), (int)(rightGastrocnemius * _allMuscleEffect * _buttocksEffect), 0.5f));

            //Body
            //Back && bra back
            bodyRenderer.SetBlendShapeWeight(32, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(32), leftTeres * _teresMajorEffect * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(33, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(33), rightTeres * _teresMajorEffect * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(34, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(34), leftTrapezius * _trapeziusEffect * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(35, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(35), rightTrapezius * _trapeziusEffect * _allMuscleEffect, 1));
            braRenderer.SetBlendShapeWeight(0, Mathf.Lerp(braRenderer.GetBlendShapeWeight(0), leftTrapezius * 2 * _trapeziusEffect * _allMuscleEffect, 1));
            braRenderer.SetBlendShapeWeight(1, Mathf.Lerp(braRenderer.GetBlendShapeWeight(1), leftTrapezius * 2 * _trapeziusEffect * _allMuscleEffect, 1));
            braRenderer.SetBlendShapeWeight(2, Mathf.Lerp(braRenderer.GetBlendShapeWeight(2), leftLatissimus * 2 * _deltoidEffect * _allMuscleEffect, 1));
            braRenderer.SetBlendShapeWeight(3, Mathf.Lerp(braRenderer.GetBlendShapeWeight(3), rightLatissimus * 2 * _deltoidEffect * _allMuscleEffect, 1));
            //Neck
            bodyRenderer.SetBlendShapeWeight(31, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(31), rightSternal * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(30, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(30), leftSternal * _allMuscleEffect, 1));
            //Belly
            bodyRenderer.SetBlendShapeWeight(38, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(38), abdominusRectus * _abdominusEffect * _allMuscleEffect, 0.3f));
            //BodySides
            bodyRenderer.SetBlendShapeWeight(39, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(39), leftLatissimus * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(40, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(40), rightLatissimus * _allMuscleEffect, 1));
        }
        catch (NullReferenceException ex)
        {
            print("There is no muscle with such index.");
        }

        yield return new WaitForSeconds(1);
    }
}
