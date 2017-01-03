using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SpawnManager : MonoBehaviour {

  [System.Serializable]
  public class EnemyTable {  // 定义敌人标识
    public string enemyName = ""; //敌人的名称
    public GameObject enemyPrefab;//敌人的prefab
  }
  public class SpawnData { // XML数据
    public int wave = 1;// 波数
    public string enemyname = "";//敌人名称
    public int level = 1;  //敌人等级
    public float wait = 1.0f; //等待时间
    public int spawnpoint = 0;  //生成敌人的位置
    public float speed = 0; //敌人的运动速度
    public int spawncount = 1;//每波敌人的数量
  }
  private int remainEnemy;  //剩余敌人数
  //当前敌人生成数量
  private int number = 0;
  //需要生成的所有敌人的数量
  [HideInInspector]
  public int amount = 0;
  //用于时间的计算
  private int count = 0;
  public UIHelp uihelp;
  //敌人
  public EnemyTable[] m_enemies;
  //路径
  public Path[] paths;
  //起始路点
  public GameObject[] spawnPoints;
  //存储敌人的出场顺序的xml
  public TextAsset xmldata;
  //保存所有的从xml文件中读取的数据
  ArrayList m_enemylist;
  // 距离下一个敌人出场的时间
  float m_timer = 0;
  // 出场敌人的序列号
  int m_index = 0;
  ////敌人死亡的特效
  //public GameObject dead_effect;
  // 当前波的敌人数量,只有销毁当前波内所有敌人,才能进入下一波
  private int m_liveEnemy = 0;
  [HideInInspector]
  public int currentwave = 1;
  //红黑色血条的引用
  public Texture2D red_blood;
  public Texture2D black_blood;
  public GameObject hitobject;

  void ReadXML() {//读取xml数据
    string wave = null;//赋初值
    string enemyname = null;
    string level = null;
    string wait = null;
    string spawnpoint = null;
    string speed = null;
    string spawncount = null;
    m_enemylist = new ArrayList();
    XMLParser xmlparse = new XMLParser();
    XMLNode node = xmlparse.Parse(xmldata.text);
    XMLNodeList list = node.GetNodeList("ROOT>0>table");

    for (int i = 0;i < list.Count;i++) {
      wave = node.GetValue("ROOT>0>table>" + i + ">@wave");
      enemyname = node.GetValue("ROOT>0>table>" + i + ">@enemyname");
      level = node.GetValue("ROOT>0>table>" + i + ">@level");
      wait = node.GetValue("ROOT>0>table>" + i + ">@wait");
      spawnpoint = node.GetValue("ROOT>0>table>" + i + ">@spawnpoint");
      speed = node.GetValue("ROOT>0>table>" + i + ">@speed");
      spawncount = node.GetValue("ROOT>0>table>" + i + ">@spawncount");
      // Debug.Log("==="+wave+"=======" + enemyname +"             "+ speed+"             ");
      //定义一个变量放入列表中,用data作载体放入m_enemylist，每一个data中包含每一行xml文件中的所有内容
      SpawnData data = new SpawnData();
      data.wave = int.Parse(wave);
      data.enemyname = enemyname;
      data.level = int.Parse(level);
      data.wait = float.Parse(wait);
      data.spawnpoint = int.Parse(spawnpoint);
      data.speed = float.Parse(speed);
      data.spawncount = int.Parse(spawncount);
      //计算要生成的所有敌人的数量
      amount += data.spawncount;
      //Debug.Log(amount);
      m_enemylist.Add(data);
    }
  }

  void Start() {
    ReadXML();
    remainEnemy = amount;
  }

  void Update() {
    SpawnEnemy();
  }

  //声明赋值标志位，用于每波兵的初始赋值
  bool isFuzhi = false;
  //每隔一定时间生成一个敌人
  void SpawnEnemy() {
    if (!UIManager.uimanager.isStartRound) { //如果没有点击出兵
      return;
    }
    if (m_index >= m_enemylist.Count) {//如果出场敌人的索引大于当前的波数
      return;
    }
    SpawnData data = (SpawnData)m_enemylist[m_index];//获取下一个敌人
    if (!isFuzhi) {//如果还没有对游戏生成数量以及生成时间赋值
      isFuzhi = true;//已经赋值的标志位
      m_liveEnemy = data.spawncount;//读取本回合出兵数
      m_timer = data.wait;//读取出兵等待时间
      UIManager.currentwave = data.wave;//读取当前回合数
      count = 0;//记录总出兵数
      if (UIData.game_tip) uihelp.StartOpenWaveDetial(UIManager.currentwave - 1);//如果游戏提示开启就用进行出兵提示         
      //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
      AudioManager.PlayAudio(13);//播放出兵音效
    }
    if (m_liveEnemy == 0) {//如果本回合已经出完兵
      if (GameManager.game.EnemyList.Count > 0) { return;}//并且敌人集合中还有敌人
      m_index++;//回合数增加
      isFuzhi = false;//下次调用此方法开始为下回合敌人赋初值
    }
    if (count % (20 * ReturnTime(m_timer)) == 0) {//控制本回合敌人出兵时间在一定时间范围内随机
      GameObject enemyprefab = FindEnemy(data.enemyname);      // 查找敌人
      if (enemyprefab != null) {      // 生成敌人
        Transform currentspawnPoint = spawnPoints[data.spawnpoint].transform;//获取当前出生点
        GameObject dis = GameObject.Instantiate(enemyprefab, currentspawnPoint.position, Quaternion.identity) as GameObject;//实例化一个敌人
        GameManager.game.EnemyList.Add(dis);        //将生成的敌人添加到敌人的list中
        Enemy enemy = dis.GetComponent<Enemy>();//获取敌人的脚本
        enemy.blood_black = black_blood;//为模型中的参数赋值，例如红色血条图片， 黑色血槽图片，行进路径，移动速度，生命值，死亡金钱，攻击力，攻击速度等。
        enemy.blood_red = red_blood;
        enemy.path = paths[data.spawnpoint];
        enemy.moveSpeed = data.speed;
        enemy.healthPoint = GameData.healthPoint_Enemy[data.level];
        enemy.deadPrice = GameData.deadPrice_Enemy[data.level];
        enemy.attackDamage = GameData.damage_Enemy[data.level];
        enemy.attackSpeed = GameData.attackSpeed_Enemy[data.level];
        //enemy.dead_effect = dead_effect;
        enemy.init();//调用敌人路径初始化的方法，敌人移动
      }
      number++;//记录敌人数量的参数每次加一
      m_liveEnemy--;//本回合剩余出兵数每次减少一
    }
    count++;//时间记录索引每帧增加一
  }

  float ReturnTime(float time) {
    return time -= Random.Range(0, 2);
  }

  float ReturnRangeNumber() {
    return Random.Range(0, 1f);
  }
  //通过名字找到相应敌人的prefab
  GameObject FindEnemy(string enemyname) {
    foreach (EnemyTable enemy in m_enemies) {
      if (enemy.enemyName.CompareTo(enemyname) == 0) {
        return enemy.enemyPrefab;
      }
    }
    return null;
  }
  //当生成了所有的敌人
  public bool allEnemyDie() {
    return amount == number&&GameManager.game.EnemyList.Count==0;
  }
}
