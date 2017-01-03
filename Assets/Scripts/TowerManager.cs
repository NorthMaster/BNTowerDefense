using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TowerManager : MonoBehaviour {
  public GameObject[] tower_prefab;//防御塔的预制件
  public GameObject rangetower_Effect;//范围塔的攻击特效
  public UIManager uimanager;//界面管理器的引用
  public GameObject[] shootPrefab;//防御塔的攻击武器
  public GameObject appearEffect;//防御塔生成特效的引用
  public void SetTowerPrefab(int receive_index, PlantForm plant) {//要生成的防御塔的设置
//       var tower = tower_prefab[receive_index].GetComponent<Tower>();
//       tower.rotateSpeed = GameData.rotateSpeed_Hero;
    tower_prefab[receive_index].GetComponent<Tower>().rotateSpeed = GameData.rotateSpeed_Hero;//旋转速度设置
    tower_prefab[receive_index].GetComponent<Tower>().attackRange = GameData.attackRange_Tower[receive_index];//攻击范围
    tower_prefab[receive_index].GetComponent<Tower>().attackSpeed = GameData.attackSpeed_Tower[receive_index];//攻击速度
    tower_prefab[receive_index].GetComponent<Tower>().attackDamage = GameData.attackDamage_Tower[receive_index];//攻击力
    tower_prefab[receive_index].GetComponent<Tower>().pursePrice = GameData.pursePrice_Tower[receive_index];//购塔金钱
    tower_prefab[receive_index].GetComponent<Tower>().shootPrefab = shootPrefab[receive_index];//攻击武器
    tower_prefab[receive_index].GetComponent<Tower>().plantform = plant;//种塔板子的引用

    if (tower_prefab[receive_index].GetComponent<Tower>().isRangeTower) {//如果是范围塔
      tower_prefab[receive_index].GetComponent<Tower>().Range_Tower_Effect = rangetower_Effect;//范围塔的攻击特效赋值
    }
    InitTower(tower_prefab[receive_index], plant);//生成防御塔的方法
  }

  public void InitTower(GameObject tower, PlantForm plant) {//生成防御塔的方法
    if (uimanager.tower_exit != null && tower.GetComponent<Tower>().tower_index == uimanager.tower_exit.GetComponent<Tower>().tower_index + 4) { //避免误删不是当下索引
      Destroy(uimanager.tower_exit);//删除已经存在的防御塔
    }
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    AudioManager.PlayAudio(11);//播放生成塔时的声音
    Instantiate(tower, plant.gameObject.transform.position, plant.gameObject.transform.rotation);//生成防御塔
    if (uimanager.gameObject.GetComponent<UIFirstHelp>() != null) uimanager.gameObject.GetComponent<UIFirstHelp>().countTower = 1;    //生成塔以后此索引值为1
    GameObject go = Instantiate(appearEffect, plant.gameObject.transform.position, plant.gameObject.transform.rotation) as GameObject;//防御塔生成特效
    Destroy(go, 0.7f);//生成特效消失
    UIManager.remainMonney -= tower.GetComponent<Tower>().pursePrice;//游戏金钱减少
    uimanager.Tower_UI_Disappear();//无关UI消失
    plant.PlantForm_DisAppear();      //种塔板子消失
  }
}
