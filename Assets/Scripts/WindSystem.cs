using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSystem : MonoBehaviour
{

    private Rigidbody[] windRigidBody;
    private GameObject[] windGameObjects;

    public int _windStrength = 10;
    public int _maxTimeBetweenWind = 5;
    public float _maxWindDuration = 2;
    private int timeInBetweenWind;
    private float windStrength, exponentialWindStrength;
    private float windDuration, windDurationInitial;
    private Vector3 windDirection;
    private float timer, lastWind;
    private bool windReady, windSet;
    public int windAngleVariation = 10;
    private int newWindAngle;
    private MeshRenderer meshRenderer;
    private Color initialColor, newColor;
    public float _windAcceleration;
    private float windAcceleration;


    // Use this for initialization
    void Start()
    {
        windGameObjects = GameObject.FindGameObjectsWithTag("WindEffect");
        windRigidBody = new Rigidbody[windGameObjects.Length];
        for (int i = 0; i < windGameObjects.Length; i++)
        {
            if (windGameObjects[i].GetComponent<Rigidbody>())
            {
                windRigidBody[i] = windGameObjects[i].GetComponent<Rigidbody>();
            }
        }


        timeInBetweenWind = _maxTimeBetweenWind;
        windDuration = _maxWindDuration;
        windStrength = _windStrength;

        if (_windAcceleration == 0) _windAcceleration = 1;
        if (_maxWindDuration == 0) _maxWindDuration = 1;
        if (_windStrength == 0) _windStrength = 1;
        if (_windAcceleration == 0) _windAcceleration = 1;

        if (GetComponent<MeshRenderer>())
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }
        initialColor = meshRenderer.material.color;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        windAcceleration = _windAcceleration / 10;
        //Change wind direction
        if (windReady)
        {
            newWindAngle = (int)Random.Range(Mathf.Abs(transform.localEulerAngles.y) - windAngleVariation, Mathf.Abs(transform.localEulerAngles.y) + windAngleVariation);
            transform.localEulerAngles = new Vector3(0, Mathf.Lerp(transform.localEulerAngles.y, newWindAngle, Time.deltaTime), 0);
        }

        timer = Time.time;

        //Rotation of the Wind game object will change wind direction in real time.
        windDirection = transform.forward;


        if (windReady && 0 < windDuration)
        {
            //print("Wind is Blowing");
            windDuration -= Time.deltaTime;

            //increase wind till one 1/4th of the windDuration
            if (windDuration > windDurationInitial / 4)
            {
                if (exponentialWindStrength <= windStrength)
                {
                    if ((exponentialWindStrength + windStrength / 20) < windStrength)
                    {
                        exponentialWindStrength += windStrength / (windStrength / windAcceleration) ;
                    }
                }

                //Change arrow color depending on wind strength
                initialColor.r = exponentialWindStrength * 100 / 255 / windStrength * 10;
                meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, initialColor, Time.deltaTime);
            }
            //Decrease wind after one 1/4th of the windDuration is left
            if (windDuration < windDurationInitial / 4)
            {
                if ((exponentialWindStrength - windStrength / 20) > 0)
                {
                    exponentialWindStrength -= windStrength / (windStrength / windAcceleration);
                }

                //Change arrow color depending on wind strength
                initialColor.r = exponentialWindStrength * 100 / 255 / windStrength * 10;
                meshRenderer.material.color = Color.Lerp(initialColor, Color.black,  Time.deltaTime);
            }
            
            //print("ExponentialWindStrength " + exponentialWindStrength);
            //print("windDuration " + windDuration);
            //print("windDurationInitial " + windDurationInitial);

            for (int i = 0; i < windGameObjects.Length; i++)
            {
                //print("Wind is Blowing");
                if (windRigidBody[i] != null)
                {
                    //Affecting all game objects with rigid body marked by "WindEffect"
                    windRigidBody[i].AddForce(Vector3.Normalize(windDirection) * exponentialWindStrength / 10, ForceMode.Impulse);
                }
            }


            lastWind = timer;
        }
        else if (!windSet)
        {
            //print("Setting new wind parameters");
            windDuration = Random.Range(1, _maxWindDuration);
            windDurationInitial = windDuration;
            exponentialWindStrength = 0;
            windSet = true;
            windReady = false;
            int colorValue = (int)exponentialWindStrength;
        }
        else if (!windReady && lastWind + timeInBetweenWind < timer)
        {
            timeInBetweenWind = Random.Range(1, _maxTimeBetweenWind);
            windStrength = Random.Range(_windStrength / 2, windStrength);

            windReady = true;
            windSet = false;
        }
        else
        {
            //print("Wind is Not Blowing");

        }




    }
}
