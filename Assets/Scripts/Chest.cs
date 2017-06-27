using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool openChest, chestOpened, chestClosed, canOpen, hingeBroken, called;
    private int randomSound, currentSound, soundAmount;
    private AudioClip openSound;
    HingeJoint hinge;
    JointLimits hingeLimits;
    JointMotor hingeMotor;


    // Use this for initialization
    void Start()
    {
        hingeBroken = !GameObject.Find("Lid").GetComponent<HingeJoint>();
        soundAmount = GetComponent<Sounds>().Clips.Length - 1;
        hinge = GameObject.Find("Lid").GetComponent<HingeJoint>();
        hingeLimits.max = 90;
    }

    void LateUpdate()
    {
        if (!called)
        {
            called = true;
            SetSoundProperties();
        }

        if (canOpen && hinge != null)
        {
            //Open Chest
            if (Input.GetKeyDown(KeyCode.E) && !chestOpened)
            {
                hingeMotor.targetVelocity = 100;
                hingeMotor.force = 10;
                hinge.motor = hingeMotor;
                openChest = true;
                chestOpened = true;
                hinge.useMotor = true;
                hingeLimits.bounciness = 0;
                hinge.limits = hingeLimits;
                //Play opening sound
                randomSound = Random.Range(0, soundAmount);
                openSound = GetComponent<Sounds>().Clips[randomSound];
                GetComponent<Sounds>().audioSources[randomSound].transform.position = transform.position;
                GetComponent<Sounds>().audioSources[randomSound].PlayOneShot(openSound, 1f);
            }
            //Close Chest
            else if (Input.GetKeyDown(KeyCode.E) && chestOpened)
            {
                chestOpened = false;
                openChest = false;

                hingeLimits.bounciness = 0.3f;
                hinge.limits = hingeLimits;
                //Play clwosing sound
                currentSound = randomSound;
                if (randomSound + 1 < soundAmount)
                {
                    randomSound += 1;
                }
                else if (randomSound - 1 >= 0)
                {
                    randomSound -= 1;
                }
                openSound = GetComponent<Sounds>().Clips[randomSound];
                GetComponent<Sounds>().audioSources[randomSound].PlayOneShot(openSound, 1f);
                GetComponent<Sounds>().audioSources[randomSound].transform.position = transform.position;
                chestClosed = false;
            }
            //Initiate motor to close the chest
            if (!chestOpened && !chestClosed && hinge.GetComponent<HingeJoint>().angle > 80)
            {
                hingeMotor.targetVelocity = -100;
                hingeMotor.force = 10;
                hinge.motor = hingeMotor;
                
            }

            // Sound when lid hits the box
            if (!chestOpened && !chestClosed && hinge.GetComponent<HingeJoint>().angle < 10)
            {
                chestClosed = true;
                openSound = GetComponent<Sounds>().Clips[4];
                GetComponent<Sounds>().audioSources[4].transform.position = transform.position;
                GetComponent<Sounds>().audioSources[4].PlayOneShot(openSound, 0.6f);
                hinge.useMotor = false;
            }


        }
    }

    private void SetSoundProperties()
    {
        for (int i = 0; i < soundAmount + 1; i++)
        {
            GetComponent<Sounds>().audioSources[i].spatialBlend = 1;
            GetComponent<Sounds>().audioSources[i].maxDistance = 5;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (Vector3.Distance(collider.gameObject.transform.position, transform.position) < 1.2)
            {
                canOpen = true;
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (canOpen)
        {
            if (collider.gameObject.tag == "Player")
            {
                canOpen = false;
            }
        }
    }
}
