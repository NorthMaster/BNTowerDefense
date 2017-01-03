using UnityEngine;
using System.Collections;

public class DestoryEffect : MonoBehaviour {

	public float time=1f;
	// Use this for initialization
	void Start () {
		Destroy (this.gameObject, time);
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
