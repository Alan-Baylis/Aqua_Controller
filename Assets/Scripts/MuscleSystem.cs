﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuscleSystem : MonoBehaviour
{
    private SkinnedMeshRenderer bodyRenderer, pantiesRenderer;

    //Arm muscles
    private int rightBicepsFlex, rightTricepsFlex, rightDeltoidFlex, leftBicepsFlex, leftTricepsFlex, leftDeltoidFlex, leftBrachioradialis, rightBrachioradialis, leftExtensors, rightExtensors;
    //Leg muscles
    private int leftGastrocnemius, rightGastrocnemius, leftSemitendinosus, rightSemitendinosus;

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

    void Start()
    {
        enableMuscles = true;

        //Get mesh renderers
        try
        {
            bodyRenderer = transform.Find("Aqua/Body").GetComponent<SkinnedMeshRenderer>();
            pantiesRenderer = transform.Find("Aqua/Panties").GetComponent<SkinnedMeshRenderer>();
        }
        catch (NullReferenceException ex)
        {
            print("Can't find Mesh Renderer");
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        _allMuscleEffect = allMuscleEffect / 10f;
        _buttocksEffect = buttocksEffect / 10f;
        _backArmsEffect = backArmsEffect / 10f;
        _frontArmsEffect = frontArmsEffect / 10f;
        _deltoidEffect = deltoidEffect / 10f;

        if (enableMuscles)
        {

            try
            {
                //Left Arm muscles
                leftBicepsFlex = (int)Mathf.Max(0, (transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm").transform.localEulerAngles.x - 285) * 1.5f);
                leftTricepsFlex = (int)(385 - transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm").transform.localEulerAngles.x);
                leftDeltoidFlex = (int)Mathf.Max(0, transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm").transform.localEulerAngles.y);

                if (transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.localEulerAngles.y > 0)
                {
                    leftBrachioradialis = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.localEulerAngles.y));
                    leftExtensors = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Left_shoulder/Left_arm/Left_forearm/Left_hand").transform.localEulerAngles.y) * -1);
                }

                //Right Arm muscles
                rightBicepsFlex = (int)Mathf.Max(0, (transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm").transform.localEulerAngles.x - 285) * 1.5f);
                rightTricepsFlex = (int)(385 - transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm").transform.localEulerAngles.x);
                rightDeltoidFlex = (int)Mathf.Max(0, transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm").transform.localEulerAngles.y);
                if (transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.localEulerAngles.y > 0)
                {
                    rightBrachioradialis = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.localEulerAngles.y));
                    rightExtensors = Mathf.Max(0, (int)(270f - transform.Find("Aqua/Hips/Spine/Chest/Right_shoulder/Right_arm/Right_forearm/Right_hand").transform.localEulerAngles.y) * -1);
                }

                //Left Leg muscles
                if (transform.Find("Aqua/Hips").transform.localPosition.x < 0)
                {
                    // print(GameObject.Find("Hips").transform.localPosition.x);
                    //Or if character not idle check z
                    leftGastrocnemius = (int)Mathf.Max(0, transform.Find("Aqua/Hips").transform.localPosition.x * -3000);
                    leftSemitendinosus = leftGastrocnemius;
                }
                else
                {
                    rightGastrocnemius = (int)Mathf.Max(0, transform.Find("Aqua/Hips").transform.localPosition.x * 3000);
                    rightSemitendinosus = rightGastrocnemius;
                }
            }

            catch (NullReferenceException ex)
            {
                print("Can't find one of the bones for muscle system");
            }


        }
        else
        {

            leftBicepsFlex = 0;
            leftTricepsFlex = 0;
            leftDeltoidFlex = 0;
            leftExtensors = 0;
            leftBrachioradialis = 0;

            rightBicepsFlex = 0;
            rightTricepsFlex = 0;
            rightDeltoidFlex = 0;
            rightExtensors = 0;
            rightBrachioradialis = 0;

            leftGastrocnemius = 0;
            leftSemitendinosus = 0;

            rightGastrocnemius = 0;
            rightSemitendinosus = 0;

        }
        if (enableMuscles)
        {
            Flex();
        }

    }

    private void Flex()
    {
        try
        {
            //Left arm muscles
            bodyRenderer.SetBlendShapeWeight(14, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(14), leftBicepsFlex * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(16, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(16), (int)(leftTricepsFlex * _allMuscleEffect * _backArmsEffect), 1));
            bodyRenderer.SetBlendShapeWeight(18, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(18), (int)(leftDeltoidFlex * _allMuscleEffect * _deltoidEffect), 1));
            bodyRenderer.SetBlendShapeWeight(26, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(26), leftBrachioradialis * _allMuscleEffect * frontArmsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(28, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(28), leftExtensors * _allMuscleEffect * frontArmsEffect, 1));

            //Right arm muscles
            bodyRenderer.SetBlendShapeWeight(15, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(15), rightBicepsFlex * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(17, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(17), (int)(rightTricepsFlex * _allMuscleEffect * _backArmsEffect), 1));
            bodyRenderer.SetBlendShapeWeight(19, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(19), (int)(rightDeltoidFlex * _allMuscleEffect * _deltoidEffect), 1));
            bodyRenderer.SetBlendShapeWeight(27, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(27), rightBrachioradialis * _allMuscleEffect * frontArmsEffect, 1));
            bodyRenderer.SetBlendShapeWeight(29, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(29), rightExtensors * _allMuscleEffect * frontArmsEffect, 1));

            //Back leg muscles
            bodyRenderer.SetBlendShapeWeight(20, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(20), leftGastrocnemius * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(22, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(22), rightSemitendinosus * _allMuscleEffect, 1));

            bodyRenderer.SetBlendShapeWeight(21, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(21), rightGastrocnemius * _allMuscleEffect, 1));
            bodyRenderer.SetBlendShapeWeight(23, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(23), leftSemitendinosus * _allMuscleEffect, 1));

            //Buttocks & panties
            bodyRenderer.SetBlendShapeWeight(24, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(24), (int)(leftGastrocnemius * _allMuscleEffect * _buttocksEffect), 1));
            bodyRenderer.SetBlendShapeWeight(25, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(25), (int)(rightGastrocnemius * _allMuscleEffect * _buttocksEffect), 1));
            pantiesRenderer.SetBlendShapeWeight(1, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(1), (int)(rightGastrocnemius * _allMuscleEffect * _buttocksEffect), 1));
            pantiesRenderer.SetBlendShapeWeight(0, Mathf.Lerp(bodyRenderer.GetBlendShapeWeight(0), (int)(leftGastrocnemius * _allMuscleEffect * _buttocksEffect), 1));


        }
        catch (NullReferenceException ex)
        {
            print("There is no muscle with such index.");
        }
    }
}
