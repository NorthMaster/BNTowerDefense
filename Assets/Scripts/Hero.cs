using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {
  //英雄的转向速度
  [HideInInspector]
  public float rotateSpeed = 4;
  //英雄的生命值
  [HideInInspector]
  public int healthPoint = 400;
  //英雄攻击速度
  [HideInInspector]
  public float attackSpeed = 1f;
  //英雄伤害值
  [HideInInspector]
  public int attackDamage = 10;
  //英雄的移动速度
  [HideInInspector]
  public float moveSpeed = 4f;
  //英雄的攻击范围
  [HideInInspector]
  public float attackRange = 4.0f;
  //英雄脚底下的光圈
  public GameObject circle;
  //英雄的攻击特效
  [HideInInspector]
  public GameObject effect;
  [HideInInspector]
  public bool isBeAttack = false;
  //鼠标点击的位置，英雄移动到此处
  [HideInInspector]
  public Vector3 targetPosition;
  //英雄要攻击的小兵
  [HideInInspector]
  public GameObject FoundEnemy;
  float m_timer;
  private Transform thisT;
  bool isGo = false;
  [HideInInspector]
  public bool isDead = false;
  // Use this for initialization
  void Start() {
    thisT = this.transform;
    m_timer = attackSpeed;
  }

  // Update is called once per frame
  void Update() {

    if (healthPoint < 0) die(); //如果英雄死亡，则调用英雄死亡的方法
    if (isGo) {//根据标志位决定是否要移动到当前位置
      GoToPosition(targetPosition);
      rotate(targetPosition);
    }
    else {
      PlayAnimation(4); //1-3攻击，4待机，5跑动，6死亡
      SearchEnemy(); //英雄不进行移动时，此时待机检测身边是否有敌人
    }
  }

  public void BeSelected() {//光圈选中
    circle.layer = 12;
  }

  public void MoveSelected() {  //光圈取消
    circle.layer = 13;
  }

  public void GoToPosition(Vector3 pos) {//移动到鼠标点击的位置
    targetPosition = pos;    //否则进行移动操作
    if (isMovePosition(targetPosition, thisT.position, 0.5f)) { //如果已经移动到目标点
      MoveSelected();//移除选定状态
      isGo = false;//停止移动
      return;
    }
    isGo = true;    //开始移动

    Vector3 dir = (pos - thisT.position).normalized;    //移动方向单位向量
    dir.y = 0;//y轴为0
    PlayAnimation(5);    //1-3攻击，4待机，5跑动，6死亡
    this.transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);//移动
  }


  void rotate(Vector3 pos) {//转向目标点
    pos = pos - thisT.position;//  获取目标位置
    pos.y = 0;//y轴不存在转向
    Quaternion qtemp = Quaternion.LookRotation(pos);//获取转向角
    thisT.rotation = Quaternion.Slerp(thisT.rotation, qtemp, Time.deltaTime * rotateSpeed);//转向敌人
  }

  bool isMovePosition(Vector3 v1, Vector3 v2, float near) {
    if ((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z) < near) {
      return true;
    }
    return false;
  }

  void SearchEnemy() {
    if (FoundEnemy != null) {
      AttackEnemy();
    }
    if (FoundEnemy == null) {
      for (int i = GameManager.game.EnemyList.Count - 1;i >= 0;i--) {
        if (isMovePosition(thisT.position, GameManager.game.EnemyList[i].transform.position, attackRange)) {
          FoundEnemy = GameManager.game.EnemyList[i];
          break;
        }
      }
      return;
    }
  }

  //攻击敌人的方法
  void AttackEnemy() {
    //转向敌人
    rotate(FoundEnemy.transform.position);
    //1-3攻击，4待机，5跑动，6死亡
    PlayAnimation(1);
    m_timer -= Time.deltaTime;
    if (m_timer > 0) return;
    m_timer = attackSpeed;
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    //AudioManager.PlayAudio(9);
    FoundEnemy.GetComponent<Enemy>().Be_Attack_Hero(attackDamage, this.gameObject);
  }

  //当前英雄被攻击的方法
  public void AttackHero(int enemydamage) {
    isBeAttack = true;
    healthPoint -= enemydamage;
  }

  void die() {
    //1-3攻击，4待机，5跑动，6死亡
    PlayAnimation(6);
    //调用彻底死亡的方法
    StartCoroutine(dieComplete());
  }

  //协程用法，敌人死后尸体存在三秒自动消失
  IEnumerator dieComplete() {
    yield return new WaitForSeconds(3f);
    Destroy(this.gameObject);
    //是否已经死亡的标志位
    isDead = true;
  }

  int attackRangeInt(int i) {
    return (int)Random.Range(1, i);
  }

  public void PlayAnimation(int index) {
    switch (index) {
      //攻击
      case 1:
      case 2:
      case 3: GetComponent<Animator>().SetInteger("changeState", attackRangeInt(3)); break;
      //待机动作
      case 4: GetComponent<Animator>().SetInteger("changeState", 4); break;
      //跑动
      case 5: GetComponent<Animator>().SetInteger("changeState", 5); break;
      //死亡
      case 6: GetComponent<Animator>().SetInteger("changeState", 6); break;
    }
  }
}
