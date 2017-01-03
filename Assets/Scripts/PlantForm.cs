using UnityEngine;
using System.Collections;

public class PlantForm : MonoBehaviour {
    public UIManager uimanager;
    Transform thisT;
	// Use this for initialization
	void Start () {
        thisT = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Appear_Tower_Select()
    {
        GameObject tower_select = UIManager.Send_Tower_Select();
        uimanager.Tower_UI_Disappear();
        Vector3 pt = Camera.main.WorldToScreenPoint(new Vector3(thisT.position.x, thisT.position.y, thisT.position.z));
        tower_select.transform.position = pt;
        uimanager.Set_State(1);
        
        uimanager.palnt = this;
    }

    public void PlantForm_DisAppear()
    {
        this.gameObject.SetActive(false);
    }

    public void PlantForm_Appear()
    {
        this.gameObject.SetActive(true);
    }

    //public void Appear_Tower_Up()
    //{
    //    GameObject tower_up = uimanager.Send_Tower_Up();
    //    Vector3 pt = Camera.main.WorldToScreenPoint(new Vector3(thisT.position.x, thisT.position.y, thisT.position.z));
    //    tower_up.transform.position = pt;
    //    uimanager.Set_State(3);
    //}

    //public void Appear_Tower_Top()
    //{
    //    GameObject tower_top = uimanager.Send_Tower_Top();
    //    Vector3 pt = Camera.main.WorldToScreenPoint(new Vector3(thisT.position.x, thisT.position.y, thisT.position.z));
    //    tower_top.transform.position = pt;
    //    uimanager.Set_State(5);
    //}
}
