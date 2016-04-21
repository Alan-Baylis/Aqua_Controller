using UnityEngine;
using System.Collections;

public class legIk : MonoBehaviour
{
    Transform rightFoot, leftFoot;
    float legRay, rayPoss, rayLength;
    RaycastHit hitleg;
    Vector3 down, footNewPosition;
    float footHigh, footLow;
    Transform hips;
    float footSmoothing, climbSmoothing;
    bool climbReady, climbPlaying, climbDone;
    int upOrDown;
    Animation m_Animator;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void legIK(Transform _foot, AvatarIKGoal leg)
    {
        if (_foot == rightFoot) { legRay = 0.1f; }
        if (_foot == leftFoot) { legRay = -0.1f; }
        if (isRunning) { rayLength = 0.5f; rayPoss = 0.6f; }
        if (isWalking) { rayLength = 0.6f; rayPoss = 0.3f; }
        if (isIdle) { rayLength = 0.7f; rayPoss = 0.1f; }

        Debug.DrawRay(hips.TransformPoint(new Vector3(legRay, -0.3f, rayPoss)), down, Color.green);

        if (Physics.Raycast(hips.TransformPoint(new Vector3(legRay, -0.3f, rayPoss)), down, out hitleg, rayLength))
        {
            footHigh = 0.94f - hitleg.distance;
            footLow = footHigh;
            //slopeRight = Vector3.Cross(hitleg.normal, rightFoot.transform.right);
            //rightFootRot = Quaternion.LookRotation(Vector3.Exclude(hitleg.normal, slopeRight), hitleg.normal);
            //print("Found an object - distance: " + rightFootNewPos + "Object name: " + hitRightLeg.collider.gameObject.name);
            footNewPosition = new Vector3(_foot.transform.position.x, footHigh, _foot.transform.position.z);
            if (footSmoothing <= 0.99) { footSmoothing += 0.02f; }
            if (footSmoothing > 0.95) { climbReady = true; }

            //Climb up

            if (climbReady && !isIdle)
            {
                if (!climbPlaying && !climbDone)
                {
                    //climbDist = footHigh;
                    climbPlaying = true;
                }
                if (climbSmoothing <= 1) { climbSmoothing += 0.02f; }
                m_Rigidbody.useGravity = false;
                climbDone = false;
                //transform.transform.position = new Vector3(transform.position.x, transform.position.y + climbDist * climbSmoothing, transform.position.z);
                m_Rigidbody.AddRelativeForce(0, 10, 1);
                m_CapsuleCenter = new Vector3(0, 1, -0.5f);
            }

            upOrDown = 1;
        }

        if (!(Physics.Raycast(hips.TransformPoint(new Vector3(legRay, -0.3f, 0.1f)), down, out hitleg, 0.6f)))
        {
            if (footSmoothing >= 0.01) { footSmoothing -= 0.01f; }
            else footSmoothing = 0;
            climbDone = true;

            //Climb up stop
            if (climbReady && climbDone)
            {
                m_CapsuleCenter = new Vector3(0, 0.76f, 0f);
                climbSmoothing = 0;
                m_Rigidbody.useGravity = true;
                climbPlaying = false;
                climbReady = false;
            }

            footLow = Mathf.Lerp(footLow, _foot.transform.position.y, 0.01f);
            footNewPosition = new Vector3(_foot.transform.position.x, footLow, _foot.transform.position.z);
            upOrDown = 0;
        }
        m_Animator.SetIKPositionWeight(leg, Mathf.Lerp(footSmoothing, upOrDown, 0.01f));
        m_Animator.SetIKRotationWeight(leg, Mathf.Lerp(footSmoothing, upOrDown, 0.01f));
        m_Animator.SetIKPosition(leg, footNewPosition);

        //m_Animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRot);

    }
}
