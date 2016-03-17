using UnityEngine;

public abstract class FollowTarget : MonoBehaviour {

    //SerializeField means that inspector can save these values even when component is subclass of this class
    [SerializeField] public Transform target;
    [SerializeField] public bool autoTargetPlayer = true;



    //Means that it can be overrided by subclases
    virtual protected void Start () {
	 if (autoTargetPlayer)
        {
            FindTargetPlayer();
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (autoTargetPlayer && (target == null || !target.gameObject.activeSelf))
        {
            FindTargetPlayer();
        }
        if(target != null && (target.GetComponent <Rigidbody>() != null && !target.GetComponent<Rigidbody>().isKinematic))
        {
            Follow(Time.deltaTime);
        }
	}

    protected abstract void Follow(float deltaTime);

    public void FindTargetPlayer()
    {
        if(target == null)
        {
            //target object is equals to "Player"
            GameObject targetObj = GameObject.FindGameObjectWithTag("Player");
            if(targetObj)
            {
                SetTarget(targetObj.transform);
            }
        }
    }

    public virtual void SetTarget(Transform newTransform)
    {
        target = newTransform;
    }

    public Transform Target { get { return this.target;  } }
}
