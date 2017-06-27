using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour {

    private int randomSound, soundAmount;
    AudioClip rockSounds;
    private bool called, underWater;

    // Use this for initialization
    void Start () {
        soundAmount = GetComponent<Sounds>().Clips.Length-1; 
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!called)
        {
            called = true;
            SetSoundProperties();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!underWater)
        {
            randomSound = Random.Range(0, soundAmount);
            rockSounds = GetComponent<Sounds>().Clips[randomSound];
            GetComponent<Sounds>().audioSources[randomSound].PlayOneShot(rockSounds, 1f);
            GetComponent<Sounds>().audioSources[randomSound].transform.position = transform.position;
        }

        //if (collision.gameObject.tag == "Player")
        // {
        //if (Vector3.Distance(collider.gameObject.transform.position, transform.position) < 1.2)
        // {
        //  canOpen = true;
        //      //}
        //   }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Water")
        {
            if (underWater)
            {
                underWater = false;
            }
            else
            {
                underWater = true;
                rockSounds = GetComponent<Sounds>().Clips[GetComponent<Sounds>().Clips.Length - 1];
                GetComponent<Sounds>().audioSources[GetComponent<Sounds>().Clips.Length - 1].PlayOneShot(rockSounds, 1f);
                GetComponent<Sounds>().audioSources[GetComponent<Sounds>().Clips.Length - 1].transform.position = transform.position;
            }

        }
    }

    private void SetSoundProperties()
    {
        for (int i = 0; i < soundAmount; i++)
        {
            GetComponent<Sounds>().audioSources[i].spatialBlend = 1;
            GetComponent<Sounds>().audioSources[i].maxDistance = 5;
        }
    }
}
