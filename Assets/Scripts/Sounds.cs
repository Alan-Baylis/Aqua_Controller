using UnityEngine;
using System.Collections;



public class Sounds : MonoBehaviour {

    public AudioClip[] Clips;

    public AudioSource[] audioSources;

    // Use this for initialization
    void Start()
    {
        audioSources = new AudioSource[Clips.Length];

        int i = 0;

        while (i < Clips.Length)

        {

            GameObject child = new GameObject("Player");

            child.transform.parent = gameObject.transform;

            audioSources[i] = child.AddComponent<AudioSource>() as AudioSource;

            audioSources[i].clip = Clips[i];

            i++;

        }
    }

    // Update is called once per frame
    void Update () {
 
    }
}
