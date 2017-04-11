using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleSystem : MonoBehaviour
{
    SkinnedMeshRenderer bodyRenderer, pantiesRenderer;
    Vector3 leftElbow, rightElbow, leftArm, rightArm, leftHand, rightHand;
    Vector3 rightFoot;

    //Arm muscles
    int rightBicepsFlex, rightTricepsFlex, rightDeltoidFlex, leftBicepsFlex, leftTricepsFlex, leftDeltoidFlex;
    //Leg muscles
    int leftGastrocnemius, rightGastrocnemius, leftSemitendinosus, rightSemitendinosus;

    public bool enableMuscles;
    // Use this for initialization
    void Start()
    {
        enableMuscles = true;



        //Get bones
        try
        {

        }
        catch (NullReferenceException ex)
        {

        }
        //Get mesh renderers
        try
        {
            bodyRenderer = GameObject.Find("Body").GetComponent<SkinnedMeshRenderer>();
            pantiesRenderer = GameObject.Find("Panties").GetComponent<SkinnedMeshRenderer>();
        }
        catch (NullReferenceException ex)
        {
            print("Can't find Mesh Renderer");
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (enableMuscles)
        {
            //Right Arm muscles
            rightBicepsFlex = (int)Mathf.Max(0, (GameObject.Find("Right_forearm").transform.localEulerAngles.x - 285) * 1.5f);
            rightTricepsFlex = (int)(385 - GameObject.Find("Right_forearm").transform.localEulerAngles.x);
            rightDeltoidFlex = (int)Mathf.Max(0, GameObject.Find("Right_arm").transform.localEulerAngles.y);

            //Left Arm muscles
            leftBicepsFlex = (int)Mathf.Max(0, (GameObject.Find("Left_forearm").transform.localEulerAngles.x - 285) * 1.5f);
            leftTricepsFlex = (int)(385 - GameObject.Find("Left_forearm").transform.localEulerAngles.x);
            leftDeltoidFlex = (int)Mathf.Max(0, GameObject.Find("Left_arm").transform.localEulerAngles.y);

            //Left Leg muscles
            if (GameObject.Find("Hips").transform.localPosition.x < 0)
            {
                //Or if character not idle check z
                leftGastrocnemius = (int)Mathf.Max(0, GameObject.Find("Hips").transform.localPosition.x * -3000);
                leftSemitendinosus = leftGastrocnemius;
            }
            else
            {
                rightGastrocnemius = (int)Mathf.Max(0, GameObject.Find("Hips").transform.localPosition.x * 3000);
                rightSemitendinosus = rightGastrocnemius;
            }
        }
        else
        {
            leftBicepsFlex = 0;
            leftTricepsFlex = 0;
            leftDeltoidFlex = 0;

            rightBicepsFlex = 0;
            rightTricepsFlex = 0;
            rightDeltoidFlex = 0;

            leftGastrocnemius = 0;
            leftSemitendinosus = 0;

            rightGastrocnemius = 0;
            rightSemitendinosus = 0;

        }

        //print("RightDeltoidFlex " + RightDeltoidFlex);

        Flex();
    }

    void Flex()
    {
        try
        {
            //Right arm muscles
            bodyRenderer.SetBlendShapeWeight(15, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(15), rightBicepsFlex, 1));
            bodyRenderer.SetBlendShapeWeight(17, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(17), rightTricepsFlex, 1));
            bodyRenderer.SetBlendShapeWeight(19, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(19), rightDeltoidFlex, 1));

            //Left arm muscles
            bodyRenderer.SetBlendShapeWeight(14, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(14), leftBicepsFlex, 1));
            bodyRenderer.SetBlendShapeWeight(16, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(16), leftTricepsFlex, 1));
            bodyRenderer.SetBlendShapeWeight(18, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(18), leftDeltoidFlex, 1));

            //Left Leg muscles
            bodyRenderer.SetBlendShapeWeight(20, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(20), leftGastrocnemius, 1));
            bodyRenderer.SetBlendShapeWeight(22, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(22), rightSemitendinosus, 1));

            //Right Leg muscles
            bodyRenderer.SetBlendShapeWeight(21, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(21), rightGastrocnemius, 1));
            bodyRenderer.SetBlendShapeWeight(23, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(23), leftSemitendinosus, 1));

        }
        catch (NullReferenceException ex)
        {
            print("Can't find Blend Shape");
        }
    }
}
