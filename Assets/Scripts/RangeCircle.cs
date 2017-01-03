using UnityEngine;
using System.Collections;

public class RangeCircle : MonoBehaviour {
    public static RangeCircle rangeCircle;
    [HideInInspector]
    public Transform ReceiveT;
    Transform thisT;
	// Use this for initialization
	void Start () {
        thisT = this.transform;
        rangeCircle = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AppearCircle(Transform transforms,float range)
    {
        thisT.position = new Vector3(transforms.position.x, thisT.position.y, transforms.position.z);
        thisT.localScale = new Vector3(1.5f*range,thisT.localScale.y,1.5f*range);
    }

    public void DisAppearCircle()
    {
        thisT.position = new Vector3(40f, thisT.position.y, 40f);
    }
}
