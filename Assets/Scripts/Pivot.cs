using UnityEngine;
using System.Collections;

[ExecuteInEditMode] 

//this class is a sub class of a followTarget class
public class Pivot : FollowTarget {

    protected Transform cam;
    protected Transform pivot;
    protected Vector3 lastTargetPosition;

    protected virtual void Awake()
    {
        cam = GetComponentInChildren<Camera>().transform;
        pivot = cam.parent; 
    }
	protected override void Start () {
        //Will use parents (superClass) class' Start
        base.Start();
	
	}
	
	// Update is called once per frame
	virtual protected void Update () {
        if (!Application.isPlaying)
        {
            if(target != null)
            {
                Follow(1000);
                lastTargetPosition = target.position;
            }
            if(Mathf.Abs(cam.localPosition.x)>0.5f || Mathf.Abs(cam.localPosition.y) > 0.5f)
            {
                cam.localPosition = Vector3.Scale(cam.localPosition, Vector3.forward);
            }
            cam.localPosition = Vector3.Scale(cam.localPosition, Vector3.forward);
        }
	}
    // must inlude this class becasue of... inheritance ?
    protected override void Follow(float deltaTime)
    {

    }
}
