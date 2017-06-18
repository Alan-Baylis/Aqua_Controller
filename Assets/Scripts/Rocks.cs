using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour {

    private int randomSound, soundAmount;
    AudioClip rockSounds;

    // Use this for initialization
    void Start () {
        soundAmount = GetComponent<Sounds>().Clips.Length; 
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        randomSound = Random.Range(0, soundAmount);
        rockSounds = GetComponent<Sounds>().Clips[randomSound];
        GetComponent<Sounds>().audioSources[randomSound].PlayOneShot(rockSounds, 1f);
        GetComponent<Sounds>().audioSources[randomSound].transform.position = transform.position;


        //if (collision.gameObject.tag == "Player")
        // {
        //if (Vector3.Distance(collider.gameObject.transform.position, transform.position) < 1.2)
        // {
        //  canOpen = true;
        //      //}
        //   }
    }
}
