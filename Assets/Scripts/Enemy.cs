using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour {
  [HideInInspector]
  public Path path;    //敌人行进路径
  private List<Vector3> waypoints;    //路经点的集合
  private Vector3 target;    //每一次朝向的目标点
  [HideInInspector]
  public float moveSpeed;    //移动速度  
  private float rotateSpeed = 4f;    //转体速度
  [HideInInspector]
  public Texture2D blood_red;    //血条图片的引用，红色为上层
  [HideInInspector]
  public Texture2D blood_black;  //黑色为下层
  [HideInInspector]
  public int healthPoint;    //生命值，会有统一分配
  [HideInInspector]
  public int attackDamage;    //小兵的攻击力
  int HealthPoint;    //记录生命值
  [HideInInspector]
  public bool isDead = false;    //是否死掉了
  [HideInInspector]
  public int deadPrice;    //声明被消灭以后获得的钱数
  [HideInInspector]
  public float attackSpeed;//攻击速度
  float m_timer;//攻击速度缓存
  private Transform thisT;    //缓存当前位置
  GameObject hero_cur;    //获取攻击英雄的引用
  bool isBeAttack = false;//是否被攻击,控制移动
  int huoDamage = 0;//火炮台的伤害值
  bool isBeAttack_Other = false;//是否被第三方攻击，控制血条显示
  float Scale = 0.5f;    //定义血条缩放比
  void Start() {
    isBeAttack_Other = false;//没有被第三方攻击，血条不显示
    thisT = this.transform;        //第一次调用脚本，缓存当前的位置
    HealthPoint = healthPoint;        //记录初始血量，用于以后进度条的更新
    m_timer = attackSpeed;        //初始攻击速度
  }
  public void init() {//路径点初始化
    waypoints = new List<Vector3>();        //声明路径点的集合
    for (int i = 0;i < path.waypoints.Length;i++) {//遍历路径点集合  
      waypoints.Add(path.waypoints[i].position); //将path中的路径点添加到waypoints集合中
    }
    GetNextPoint();        //获取下一个路径点
  }
  void GetNextPoint() {
    if (waypoints.Count > 0) {//如果集合中有路径点
      target = waypoints[0];//取集合中的第一个点作为下一个目标点
      waypoints.RemoveAt(0);//获取成为target对象，之后移除
    }
    else {
      StartCoroutine(endComplete());//如果小兵已经到达基地，调用到达基地的协程
    }
  }
  // Update is called once per frame
  void Update() {
    if (isDead) return;//如果小兵已经死亡，返回
    if (healthPoint <= 0) die();//如果生命值小于0，调用死亡的方法
    if (!isDead) { //如果生成的敌人没有死
      if (!isBeAttack) {//如果正在被英雄攻击
        float dis = ReturnDistance(thisT.position, target);      //获取当前点与目标点的距离的方法
        if (dis < 0.15f) GetNextPoint();                //判断是否移动到下个点，如果可以，继续移动
        Enemy_Rotate_Move(target);//转向并且移动到目标点的方法
      }
      else {
        if (hero_cur.GetComponent<Hero>().isDead) {//如果英雄已经死亡
          isBeAttack = false;//不被英雄攻击
          return;
        }
        float dis = ReturnDistance(thisT.position, hero_cur.transform.position);                //获取当前点与英雄的距离
        if (dis >= 2.5f) { isBeAttack = false; hero_cur.GetComponent<Hero>().FoundEnemy = null; return; }//距离不够，小兵不被攻击，要攻击的英雄不存在
        if (dis > 1f) { //判断是否移动到下个点，如果可以，继续移动
          Enemy_Rotate_Move(hero_cur.transform.position);    //敌人转向和行进的方法
        }
        PlayAnimation(3);//播放攻击动画
        m_timer -= Time.deltaTime;//间断性攻击
        if (m_timer > 0) return;
        m_timer = attackSpeed;
        //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
        //AudioManager.PlayAudio(10);
        hero_cur.GetComponent<Hero>().AttackHero(attackDamage);                //每隔一定时间对英雄进行攻击
      }
    }
  }
  void Enemy_Rotate_Move(Vector3 point) {  //敌人转向和行进的方法
    Quaternion wantedRotate = Quaternion.LookRotation(point - thisT.position);//获取转动变量
    thisT.rotation = Quaternion.Slerp(thisT.rotation, wantedRotate, rotateSpeed * Time.deltaTime);// 敌人转身动作的实现
    Vector3 d_range = point - thisT.position;//获取方向距离
    Vector3 dir = d_range.normalized;        //敌人移动到目标点单位化
    PlayAnimation(1);//播放移动动画
    thisT.Translate(dir * moveSpeed * Time.deltaTime, Space.World);//移动
  }
  void OnGUI() {  //绘制血条的方法
    if (!isBeAttack_Other) return;//如果小兵没有受到攻击，不显示血条
    if (thisT == null) return;        //如果当前小兵的位置不存在，则不进行绘制
    if (Camera.main == null || isDead) return;    //如果主摄像机不存在或者小兵已经死亡，不进行绘制
    Vector3 worldPosition = new Vector3(thisT.position.x, thisT.position.y + 1f, thisT.position.z);        //默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
    Vector2 position = Camera.main.WorldToScreenPoint(worldPosition);        //根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
    position = new Vector2(position.x, Screen.height - position.y);        //得到真实NPC头顶的2D坐标
    Vector2 bloodSize = GUI.skin.label.CalcSize(new GUIContent(blood_red));        //计算出血条的宽高
    float blood_width = bloodSize.x * healthPoint / HealthPoint;        //通过血值计算红色血条显示区域
    if (blood_width < 0) {  //如果小兵的生命值低于零
      blood_width = 0;//让其的血条显示为0
    }
    //先绘制黑色血条
    GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 4 * Scale), position.y - bloodSize.y * Scale * 0.3f, bloodSize.x * Scale, bloodSize.y * Scale * 0.3f), blood_black);
    //在绘制红色血条
    GUI.DrawTexture(new Rect(position.x - (bloodSize.x / 4 * Scale), position.y - bloodSize.y * Scale * 0.3f, blood_width * Scale, bloodSize.y * Scale * 0.3f), blood_red);
    StartCoroutine(StopUI());  //停止显示血条
  }
  IEnumerator StopUI() {//停止显示血条
    yield return new WaitForSeconds(15f);//等待十五秒
    isBeAttack_Other = false;//不被攻击
  }
  public void die() {  //敌人死亡，并播放动画
    isDead = true;        //是否已经死亡的标志位
    UIManager.remainMonney += deadPrice;//游戏金钱增加
    UIManager.kill++;//记录杀敌数增加
    PlayAnimation(2);        //播放小兵死亡的动画
    GameManager.game.EnemyList.Remove(this.gameObject);         //将小兵集合中的当前小兵去掉
    StartCoroutine(dieComplete());        //调用彻底死亡的方法
  }
  IEnumerator dieComplete() { //协程用法，敌人死后尸体存在三秒自动消失
    yield return new WaitForSeconds(1.4f);//等待1.4秒
    Destroy(this.gameObject);//敌人消失
  }
  IEnumerator endComplete() {//到达基地
    isDead = true;        //到达终点小兵死亡的标志位
    yield return new WaitForSeconds(0.05f);//等待0.05秒
    GameManager.game.EnemyList.Remove(this.gameObject);        //从列表中移除当前小兵
    Destroy(this.gameObject);        //销毁当前物体
    UIManager.remainLife--;//游戏生命值减少
  }
  float ReturnDistance(Vector3 v1, Vector3 v2) { //返回距离的方法
    return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z));
  }
  public void Be_Attack(int attackDamage) { //受到塔攻击或者技能攻击的方法
    isBeAttack_Other = true;//正在被攻击标志位
    UIManager.damage += attackDamage;//伤害总值增加
    healthPoint -= attackDamage;        //对小兵生命值的递减
  }
  void Be_Attack_Huo() {//受到火攻击的方法
    isBeAttack_Other = true;//正在受到攻击标志位
    healthPoint -= huoDamage;//小兵生命值减少
    UIManager.damage += huoDamage;//伤害总值增加
  }
  public void Be_Attack_Hero(int heroattack, GameObject hero) {//被英雄攻击的方法
    isBeAttack = true;  //小兵受到攻击，不移动
    isBeAttack_Other = true;//正在受到攻击标志位，血条显示
    UIManager.damage += heroattack;//伤害总值增加
    healthPoint -= heroattack;//小兵生命值减少
    hero_cur = hero;//英雄引用
  }
  void OnParticleCollision(GameObject other) {//粒子碰撞检测
    huoDamage = other.GetComponentInParent<ShootObject>().attackDamage;//获取火的伤害值引用
    Be_Attack_Huo();//受到火攻击
  }

  void PlayAnimation(int index) {//播放动画的方法，1跑动；2死亡；3攻击
    switch (index) {
      case 1: GetComponent<Animation>().Play("Run"); break;//跑动
      case 2: GetComponent<Animation>().Play("Dead"); break;//死亡
      case 3: GetComponent<Animation>().Play("Attack"); break;//攻击
    }
  }
}
