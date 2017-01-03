using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
  public GameObject rangeCircle;//显示防御塔攻击范围的圈
  public EventSystem eventsystem;//事件系统
  public GraphicRaycaster graphicRaycaster;//检测射线碰撞器
  public Camera mainCamera;//主摄像机
  public LayerMask PlantForm;//种塔的板子所在的层级
  public LayerMask Tower;//防御塔所在层级
  public LayerMask UI;//UI所在层级
  public LayerMask Terrain;//地形所在层级
  public LayerMask Hero;//英雄所在层级
  public static GameManager game;//当前类的引用
  public SpawnManager spawnmanager;//出兵管理器
  public TowerManager towermanager;//防御塔管理器
  public EffectManager effectmanager;//技能管理器
  [HideInInspector]
  public List<GameObject> EnemyList = new List<GameObject>();//怪物列表
  bool Click = true;//进行点击操作的标志位，避免二次点击
  [HideInInspector]
  public bool isSelectHero = false;//是否选择了英雄的标志位
  [HideInInspector]
  public GameObject hero;//引用
  public GameObject hitTerrainEffect;//点击地面出现点击的特效
  [HideInInspector]
  public int effect_index = -1;    //特效的索引值

  void Start() {
    game = this;//获取当前类的引用
  }

  // Update is called once per frame
  void Update() {
    Check_Select();//检测拾取状态的方法
  }

  void Check_Select()//检测拾取状态的方法
  {
    //点击,包括点击地面已经选中人物
    if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved) { Click = false; }//如果点击到屏幕的触控点移动了，则没有点击对象
    if (!Input.GetMouseButtonUp(0)) { return; }//如果点击后没有抬起手指，则没有实现点击
    if (!Click) { Click = true; return; }//点击实现
    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);//由主摄像机发一条射线
    RaycastHit hit;//射线内容
    if (CheckGuiRaycastObjects()) { return; }        //防止点穿UI
    if (Physics.Raycast(ray, out hit, 40f, PlantForm)) {  //如果点击了种塔的板子
      GameObject plantform = hit.transform.gameObject;//获取该板子的引用
      plantform.GetComponent<PlantForm>().Appear_Tower_Select();//获取板子上的PlantForm脚本
      return;
    }
    if (Physics.Raycast(ray, out hit, 40f, Tower)) {//如果点击了防御塔
      Tower tower = hit.transform.gameObject.GetComponent<Tower>();            //获取选中塔的引用
      rangeCircle.gameObject.GetComponent<RangeCircle>().AppearCircle(tower.transform, tower.attackRange);            //出现塔的攻击范围
      if (tower.tower_index >= 8) {//如果选中塔的索引大于8，则为顶级塔
        tower.Appear_Tower_Top();//出现卖出顶级塔的UI
      }
      else {
        tower.Appear_Tower_Up();//出现升级塔和卖塔的UI
      }
      return;
    }
    if (Physics.Raycast(ray, out hit, 40f, Terrain)) {//如果点击了地面
      UIManager.uimanager.Tower_UI_Disappear();            //添加的ui消失的方法
      if (isSelectHero) {//如果已经选中了英雄
        Instantiate(hitTerrainEffect, hit.point, Quaternion.identity);//实例化一个点击地面的特效
        hero.GetComponent<Hero>().GoToPosition(hit.point);//调用英雄移动到点击的点的方法
        isSelectHero = false;//英雄取消选中状态
        return;
      }
      else if (effect_index != -1 && UIManager.uimanager.CheckIsOn()) {//如果已经选中了技能施放的UI
        effectmanager.DamageAllEnemy(effect_index, hit.point);//调用施放技能的方法
        UIManager.uimanager.effects[effect_index].gameObject.GetComponentInParent<ColdTime>().StartCold(UIManager.uimanager.coldtime[effect_index]); //调用技能冷却的方法
        effect_index = -1;//技能施放结束
        return;
      }

    }
    if (Physics.Raycast(ray, out hit, 40f, Hero)) {//如果选中了英雄
      isSelectHero = true;//选中英雄的标志位设置为真
      hero = hit.transform.gameObject;//获取选中的英雄对象
      hero.GetComponent<Hero>().BeSelected();//调用英雄被选中的方法
      return;
    }
  }

  bool CheckGuiRaycastObjects() {//防止点穿UI的方法
    PointerEventData eventData = new PointerEventData(eventsystem);
    eventData.pressPosition = Input.mousePosition;
    eventData.position = Input.mousePosition;
    List<RaycastResult> list = new List<RaycastResult>();
    graphicRaycaster.Raycast(eventData, list);
    //如果UI叠加>1
    return list.Count > 0;
  }


}
