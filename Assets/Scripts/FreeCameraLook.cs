﻿using UnityEngine;
//using UnityEditor;
public class FreeCameraLook : Pivot { 

[SerializeField] private float moveSpeed = 5f;
[SerializeField] private float turnSpeed = 1.5f;
[SerializeField] private float turnSmoothing = 0.1f;
[SerializeField] private float tiltMax = 75f;
[SerializeField] private float tiltMin = 45f;
[SerializeField] private bool lockCursor = false;

    private float lookAngle;
    private float tiltAngle;

    private const float LookDistance = 100f;

    private float smoothX = 0;
    private float smoothY = 0;
    private float smoothXVelocity = 0;
    private float smoothYVelocity = 0;
    private float tempMouseZ;


    protected override void Awake()
    {
        base.Awake();
        //Screen.lockCursor = lockCursor;
        cam = GetComponentInChildren<Camera>().transform;
        pivot = cam.parent;
    }



	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        HandleRotationMovement();

	}
    void OnDisable() {

       // Screen.lockCursor = false; 
    }

    protected override void Follow(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, target.position, deltaTime + moveSpeed);
    }

    void HandleRotationMovement()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            SetMouseZ(pivot.localPosition.z + Input.GetAxis("Mouse ScrollWheel"));
            pivot.localPosition = new Vector4(pivot.localPosition.x, pivot.localPosition.y, tempMouseZ);
        }
        

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        if(turnSmoothing > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, x, ref smoothXVelocity, turnSmoothing);
            smoothY = Mathf.SmoothDamp(smoothY, y, ref smoothYVelocity, turnSmoothing);
        }
        else
        {
            smoothX = x;
            smoothY = y;
        }

        lookAngle += smoothX * turnSpeed;
        transform.rotation = Quaternion.Euler(0f, lookAngle, 0);
        tiltAngle -= smoothY * turnSpeed;
        tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);

        pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
    }
    private void SetMouseZ(float _tempMouseZ)
    {
        if (tempMouseZ >= -1 && tempMouseZ <= 1)
        {
            tempMouseZ = Mathf.Lerp(pivot.localPosition.z, _tempMouseZ, 0.5f);
        }
        else
        {
            if (tempMouseZ > 0)
            {
                tempMouseZ = 1f;
            }else
                tempMouseZ = -1;
        }
    }

}
