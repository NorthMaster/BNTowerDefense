using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class set : MonoBehaviour {

    public Button qd;
    public Button backscene;
	// Use this for initialization
	void Start () {
        qd.onClick.AddListener(EnterScene);
        backscene.onClick.AddListener(BackScene);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void EnterScene()
    {
        Application.LoadLevel("scene1");
    }

    void BackScene()
    {
        Application.LoadLevel("main");
    }
}
