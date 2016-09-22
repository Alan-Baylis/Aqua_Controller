using UnityEngine;
using System.Collections;
//using NLinear;



public class ColorChanger : MonoBehaviour {

    Renderer [] renderers;
    // Use this for initialization
    void Start () {
        renderers = GetComponentsInChildren<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        print(this.transform.rotation.x);
        //GetComponentInChildren<Renderer>().material.color = new Vector4(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, 1);

    }
    void FixedUpdate(){
        foreach (Renderer i in renderers)
        {
            i.material.color = new Vector4(Mathf.Abs(transform.rotation.x), Mathf.Abs(transform.rotation.y), Mathf.Abs(transform.rotation.z), 1);

        }
    }


}
