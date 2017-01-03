using UnityEngine;
using System.Collections;

public class playanima : MonoBehaviour {

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Time.timeScale = 1;
        GetComponent<Animation>().Play("Run");
	}
}
