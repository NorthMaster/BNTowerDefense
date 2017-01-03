using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tower : MonoBehaviour {
  GameObject target; //塔攻击的目标
  public GameObject turretObject;  //旋转体
  public GameObject baseObject;  //基座
  Transform thisT; //缓存
  Transform turrentT;
  [HideInInspector]
  public GameObject shootPrefab; //发射的武器
  public Transform shootPoint;  //发射武器的点
  public int tower_index;//每种塔对应一个索引坐标值
  //旋转的速度
  [HideInInspector]
  public float rotateSpeed;
  //攻击范围
  [HideInInspector]
  public float attackRange;
  //塔的攻击速度
  [HideInInspector]
  public float attackSpeed;
  //塔的伤害值
  [HideInInspector]
  public int attackDamage;
  //塔的价值
  [HideInInspector]
  public int pursePrice;
  //塔的名称
  [HideInInspector]
  public string towerName;
  //范围塔的攻击特效
  [HideInInspector]
  public GameObject Range_Tower_Effect;
  //种塔板子的引用
  [HideInInspector]
  public PlantForm plantform;
  //是否为范围塔
  public bool isRangeTower = false;
  float attack_s;
  void Start() {
    thisT = this.transform;//获取塔的位置引用
    if (turretObject != null) {//如果防御塔旋转体存在
      turrentT = turretObject.transform;//获取旋转体的位置引用
    }
    attack_s = 1 / attackSpeed;//记录防御塔攻击速度
  }

  void Update() {
    if (!isRangeTower) {Normal_Tower_Attack();}  //如果不是范围型防御塔调用单打型防御塔攻击的方法
    else {Range_Tower_Attack();}//如果是范围型防御塔调用范围塔攻击的方法
  }

  void Normal_Tower_Attack() {//单打型防御塔攻击的方法
    SearchTarget();    //寻找敌人
    if (target != null) {    //如果存在敌人，完成转向
      Vector3 targetPosition = target.transform.position;//获取发射点的位置
      targetPosition.y = turretObject.transform.position.y;      //固定y轴，避免出现转动错误
      Quaternion wantedRotate = Quaternion.LookRotation(targetPosition - turretObject.transform.position);//获取旋转角度的四元数
      turrentT.rotation = Quaternion.Slerp(turrentT.rotation, wantedRotate, rotateSpeed * Time.deltaTime);///旋转
      Normal_Attack();//普通攻击
    }
  }

  void Normal_Attack() {//普通攻击
    attack_s -= Time.deltaTime; //控制发射速度
    if (attack_s > 0) {
      return;
    }
    attack_s = 1 / attackSpeed;
    if (target.GetComponent<Enemy>().healthPoint > 0) {//如果要攻击的敌人生命值不低于0
      GameObject go = Instantiate(shootPrefab, shootPoint.position, shootPoint.rotation) as GameObject;//实例化发射武器
      ShootObject so = go.GetComponent<ShootObject>();      // 获取引用射击敌人
      so.attackDamage = attackDamage;//发射武器伤害值传递
      so.Shoot(target, this);//调用发射的方法
      AudioManager.PlayAudio((tower_index + 1) % 4);//播放防御塔攻击声效
    }
    else {
      target = null;//否则攻击对象为空
    }
  }

  void Range_Tower_Attack() {//范围型防御塔攻击方法
    if (Range_Tower_Effect == null) {    //如果丢失攻击特效则返回
      return;
    }
    attack_s -= Time.deltaTime;    //控制范围塔的攻击时间
    if (attack_s > 0) return;
    attack_s = 1 / attackSpeed;
    Range_Attack();    //范围攻击
  }
  void Range_Attack() {  //范围攻击的塔的实现
    for (int i = GameManager.game.EnemyList.Count - 1;i >= 0;i--) {    //搜寻敌人，遍历敌人的集合
      if (CheckRange(thisT.position, GameManager.game.EnemyList[i].transform.position)) { //在攻击范围内的敌人受到攻击
        Enemy e = GameManager.game.EnemyList[i].GetComponent<Enemy>(); //获取受到攻击敌人的引用
        e.Be_Attack(attackDamage);//调用敌人受到范围性塔攻击的方法
        //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
        AudioManager.PlayAudio(4);//播放范围塔攻击声效
        Instantiate(Range_Tower_Effect, thisT.position, thisT.rotation);        //实现攻击特效
      }
    }
  }


  void Judge_Audio(int tower_index) {
    switch (tower_index) {
      case 0:
      case 4:
      case 8:
        //1出兵；2种塔；3卖塔；4红塔，5范围塔；6绿塔；7箭塔；8英雄攻击；9英雄技能；10技能1；11技能2
        AudioManager.PlayAudio(4); break;
      case 1:
      case 5:
      case 9:
        //1出兵；2种塔；3卖塔；4红塔，5范围塔；6绿塔；7箭塔；8英雄攻击；9英雄技能；10技能1；11技能2
        AudioManager.PlayAudio(5); break;
      case 2:
      case 6:
      case 10:
        //1出兵；2种塔；3卖塔；4红塔，5范围塔；6绿塔；7箭塔；8英雄攻击；9英雄技能；10技能1；11技能2
        AudioManager.PlayAudio(6); break;
      case 3:
      case 7:
      case 11:
        //1出兵；2种塔；3卖塔；4红塔，5范围塔；6绿塔；7箭塔；8英雄攻击；9英雄技能；10技能1；11技能2
        AudioManager.PlayAudio(7); break;
    }
  }

  //寻找敌人的方法
  void SearchTarget() {
    //此方法的目的是获取一个敌人target
    if (target == null) {
      //搜寻敌人，遍历敌人的集合
      for (int i = GameManager.game.EnemyList.Count - 1;i >= 0;i--) {
        //在攻击范围内的敌人受到攻击
        if (CheckRange(thisT.position, GameManager.game.EnemyList[i].transform.position)) {
          target = GameManager.game.EnemyList[i];
        }
      }
    }
    else {
      //若大于攻击范围则判定目标为null
      if (!CheckRange(thisT.position, target.transform.position)) {
        target = null;
      }
    }
  }
  bool CheckRange(Vector3 v1, Vector3 v2) {
    if ((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z) < attackRange * attackRange) {
      return true;
    }
    return false;
  }

  public void Appear_Tower_Up() {//出现升级塔的UI
    GameObject tower_up = UIManager.Send_Tower_Up();    //获取升级塔的UI
    GameObject.FindGameObjectWithTag("uptower").GetComponent<Image>().sprite = UIManager.uimanager.towerTextures[tower_index + 4];//获取比当前塔高一级的图片
    Vector3 pt = Camera.main.WorldToScreenPoint(new Vector3(thisT.position.x, thisT.position.y, thisT.position.z));    //种塔板子位置3D转2D
    tower_up.transform.position = pt;    //升级塔的UI移动到板子位置
    UIManager.uimanager.Set_State(3);    //播放UI出现动画
    UIManager.uimanager.palnt = plantform;//种塔的板子引用传递
    UIManager.uimanager.tower_index_up = tower_index + 4;//当前塔的索引+4，为升级以后塔的索引
    //传递当前塔的引用
    UIManager.uimanager.tower_exit = this.gameObject;
    //改变买塔买塔数值
    UIManager.uimanager.ChangeText();
  }
  //出现顶级塔出售的UI
  public void Appear_Tower_Top() {
    GameObject tower_top = UIManager.Send_Tower_Top();
    Vector3 pt = Camera.main.WorldToScreenPoint(new Vector3(thisT.position.x, thisT.position.y, thisT.position.z));
    tower_top.transform.position = pt;
    UIManager.uimanager.Set_State(5);
    UIManager.uimanager.palnt = plantform;
    UIManager.uimanager.tower_exit = this.gameObject;
    //改变买塔买塔数值
    UIManager.uimanager.ChangeText();
  }
}
