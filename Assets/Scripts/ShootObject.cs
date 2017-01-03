using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour {
  //移动速度
  [HideInInspector]
  public float moveSpeed = 10f;
  Transform thisT;
  //伤害值
  [HideInInspector]
  public int attackDamage;
  private float touchTarget = 0.1f;
  Enemy e;
  Tower t;
  float dis = 0;
  void Start() {
    thisT = this.transform;
  }

  // Update is called once per frame
  void Update() {
    ShootTarget();//每帧调用炮弹飞向敌人的方法
  }
  public void Shoot(GameObject enemy, Tower tower) {//外部调用此脚本的射击方法
    e = enemy.GetComponent<Enemy>();
    t = tower;
  }

  void ShootTarget() {//武器飞向敌人的方法
    if (e.isDead) {//如果敌人已经死亡
      Destroy(this.gameObject);//销毁当前武器
      return;
    }
    dis = GetDistance(thisT.position, e.transform.position);//获取武器与敌人的距离
    if (dis < touchTarget) {//距离达到阈值
      if (!(t.tower_index % 4 == 1)) {      //如果攻击的不是火炮台,即普通炮塔
        e.Be_Attack(attackDamage);//调用敌人减血的方法
        Destroy(this.gameObject);//销毁当前武器
      }
      else {//如果是火炮塔
        Destroy(this.gameObject);//销毁当前武器
      }
    }
    else {//距离未达到阈值
      if (t.tower_index % 4 == 2 || t.tower_index % 4 == 0) {
        Vector3 dir = (e.transform.position - thisT.position).normalized;
        thisT.Translate(dir * moveSpeed * Time.deltaTime * 5, Space.World);
      }
      else {
        Vector3 dir = (e.transform.position - thisT.position).normalized;
        thisT.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
      }
    }
  }

  float GetDistance(Vector3 v1, Vector3 v2) {
    return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
  }
}
