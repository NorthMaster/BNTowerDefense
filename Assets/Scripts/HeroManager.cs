using UnityEngine;
using System.Collections;

public class HeroManager : MonoBehaviour {
  public static HeroManager heromanger;
  public GameObject[] hero_prefab;
  //public GameObject[] effect;
  public Transform[] appear_point;
  [HideInInspector]
  public Hero currentHero;
  void Start() {
    //Debug.Log("开始设置英雄");
    SetHeroPrefab();
  }
  //对英雄进行相关的设置
  public void SetHeroPrefab() {
    int receive_index = UIData.heroselectindex_int;
    //Debug.Log("开始创建英雄，索引值为"+receive_index);
    //转速
    hero_prefab[receive_index].GetComponent<Hero>().rotateSpeed = GameData.rotateSpeed_Hero;
    //生命值
    hero_prefab[receive_index].GetComponent<Hero>().healthPoint = GameData.healthPoint_Hero[receive_index];
    //攻击速度
    hero_prefab[receive_index].GetComponent<Hero>().attackSpeed = GameData.attackSpeed_Hero[receive_index];
    //攻击力
    hero_prefab[receive_index].GetComponent<Hero>().attackDamage = GameData.damage_Hero[receive_index];
    //移动速度
    hero_prefab[receive_index].GetComponent<Hero>().moveSpeed = GameData.moveSpeed_Hero[receive_index];
    //攻击范围
    hero_prefab[receive_index].GetComponent<Hero>().attackRange = GameData.attackRange;
    //攻击特效
    //hero_prefab[receive_index].GetComponent<Hero>().effect =effect[receive_index];
    InitHero(hero_prefab[receive_index], receive_index);
  }

  void InitHero(GameObject hero, int receive_index) {
    //Debug.Log("生成了英雄");
    currentHero = (Instantiate(hero, appear_point[receive_index].position, Quaternion.identity) as GameObject).GetComponent<Hero>();
  }
}
