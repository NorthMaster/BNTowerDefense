using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIManager : MonoBehaviour {
  public Toggle isBackGroundMusic;//检测背景音乐开闭
  public Toggle isEffectMusic;//检测声效开闭
  //摄像机的引用
  public GameObject camera;
  //当前场景的编号
  public int scene = 1;
  //当前类引用
  public static UIManager uimanager;
  //炮塔管理器的引用
  public TowerManager towermanager;
  public SpawnManager spawnmanager;
  //游戏设置基地生命值
  public int set_remainLife;
  //游戏设置初始金钱
  public int set_remainMoney;
  //设置波数
  public int set_wave;
  //基地剩余生命值
  public static int remainLife;
  public Text remainLife_t;
  //剩余金钱
  public static int remainMonney;
  public Text remainMoney_t;
  //当前波数
  public static int currentwave;
  public Text round_t;
  //变速复选框
  public Toggle changespeed;
  //暂停按钮
  public Button pause;
  //塔的图片
  public Sprite[] towerTextures;
  //禁止买塔的图片覆盖
  public GameObject[] tower_select_not;
  //禁止升级的图片覆盖
  public GameObject tower_up_not;
  bool isStartCheck = false;
  //升级塔的数值
  public Text confim_up_text;
  //卖出当前塔的数值
  public Text sell_current_text;
  public Text sell_top_text;
  //选塔UI
  public GameObject tower_select;
  //升级塔UI
  public GameObject tower_up;
  //顶级塔出售UI
  public GameObject tower_top;
  //几个关于种塔的动画
  Animator anim_select;
  Animator anim_up;
  Animator anim_top;
  //种塔板子的引用
  [HideInInspector]
  public PlantForm palnt;
  [HideInInspector]
  public int tower_index_up;
  [HideInInspector]
  public GameObject tower_exit;

  //确认种塔
  public Button confirm_up_b;
  //确认升级塔
  public Button selltower_up_b;
  //确认出售
  public Button selltower_top_b;

  //几个特效的引用
  public Toggle[] effects;
  public int[] coldtime;

  bool isPause = false;
  bool isChangeSpeed = false;
  //退出关卡
  public Button exit_scece_p;
  public Button exit_scene_v;
  public Button exit_scene_f;
  //重玩
  public Button restart_v;
  public Button restart_p;
  public Button restart_f;
  //继续游戏
  public Button continues;
  //下一关
  public Button nextscene;
  //几个容器的引用,1game，2pause，3vec，4fal
  public GameObject[] plane_;
  //出兵的监听
  public Button startround;
  [HideInInspector]
  public bool isStartRound = false;
  //游戏结束数据统计
  public static int kill = 0;
  public static int damage = 0;
  public static int score = 0;

  public Text kill_v;
  public Text kill_f;
  public Text damage_v;
  public Text damage_f;
  public Text score_v;
  public Text score_f;
  //游戏结束的标志位
  bool isEnd = false;
  [HideInInspector]
  public int effect_i = -1;

  void Awake() {
    if (UIData.game_backmusic_bool) isBackGroundMusic.isOn = true;
    else isBackGroundMusic.isOn = false;
    if (UIData.game_effectmusic_bool) isEffectMusic.isOn = true;
    else isEffectMusic.isOn = false;
  }
  // Use this for initialization
  void Start() {
    isStartCheck = false;//开始检测是否可以买塔的标志位
    //初始只显示GameUI
    SwitchPlane(1);//只显示“Game”界面
    uimanager = this;//获取当前类的引用
    remainLife = set_remainLife;//记录基地生命值
    remainMonney = set_remainMoney;//记录总金钱
    currentwave = set_wave;//记录总回合
    anim_select = tower_select.GetComponent<Animator>();//获取动画控制器引用
    anim_up = tower_up.GetComponent<Animator>();
    anim_top = tower_top.GetComponent<Animator>();
    //如果为第一关或者第二关的话，对开始下一关的按钮注册监听，否则将此按钮设置为不可见
    if (scene == 1 || scene == 2) {
      nextscene.gameObject.SetActive(true);//将开始下一关的按钮设为可见
      nextscene.onClick.AddListener(LoadNextScene);//对开始下一关的按钮注册监听
    }
    else {
      nextscene.gameObject.SetActive(false);//如果为第三关不显示下一关的按钮
    }
    //注册监听
    confirm_up_b.onClick.AddListener(InitTower_Up);
    selltower_up_b.onClick.AddListener(Sell_Tower);
    selltower_top_b.onClick.AddListener(Sell_Tower);
    //暂停
    pause.onClick.AddListener(Pause);
    //退出当前场景的监听
    exit_scece_p.onClick.AddListener(Exit_Scene);
    exit_scene_f.onClick.AddListener(Exit_Scene);
    exit_scene_v.onClick.AddListener(Exit_Scene);
    //重启本关的监听
    restart_f.onClick.AddListener(Restart);
    restart_p.onClick.AddListener(Restart); ;
    restart_v.onClick.AddListener(Restart);
    //继续游戏的监听
    continues.onClick.AddListener(Continue);
    //出兵的监听
    startround.onClick.AddListener(StartRound);
  }

  // Update is called once per frame
  void Update() {
    if (isEnd) return;//如果游戏已经结束，返回
    if (isStartCheck) {//开始检查是否点击购塔或者升级的方法
      CheckIsPurse();//购塔的方法
      CheckIsUp();//升级的方法
    }
    CheckSelectEffect();//技能选定的方法
    ChangeUI();//改变UI的方法
    ChangeSpeed();//是否改变速度的方法
    ChangeBackGroundMusic();//是否开启或者关闭背景音乐的方法
    ChangeEffectMusic();//是否开启或者关闭游戏音效的方法
    JudgeState();//判断暂停或者游戏速度改变的状态
    // 失败
    if (remainLife <= 0 && !plane_[3].activeInHierarchy) {//如果
      isEnd = true;//游戏结束的标志位置真
      ChangeTextEnd();//改变游戏结束时统计文本的值
      SwitchPlane(4);//失败界面显示
    }//成功
    else if (spawnmanager.allEnemyDie() && remainLife > 0 && !plane_[2].activeInHierarchy) {
      isEnd = true;//游戏结束的标志位置真
      ChangeTextEnd();//改变游戏结束时统计文本的值
      SwitchPlane(3);//成功界面显示
    }
  }

  #region 游戏暂停与速度改变的相关功能的实现
  void JudgeState() {//判断暂停或者游戏速度改变的状态
    if (isPause && !isEnd) {  //如果是暂停了，让暂停的panel可见
      SwitchPlane(2);//1game，2pause，3vec，4fal
      Time.timeScale = 0;//游戏暂停
    }
    else {
      SwitchPlane(1);//显示主界面
      if (isChangeSpeed) Time.timeScale = 1.5f;//游戏1.5倍速度
      else Time.timeScale = 1;//游戏正常速度
    }
  }
  //继续游戏
  void Continue() {
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    AudioManager.PlayAudio(0);
    isPause = false;
  }
  //退出当前场景到主界面场景
  void Exit_Scene() {
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    AudioManager.PlayAudio(0);
    camera.GetComponent<Loading>().LoadScene(3);
  }
  //加载下一关场景
  void LoadNextScene() {
    camera.GetComponent<Loading>().LoadScene(scene + 1);
  }
  //重新加载本关
  void Restart() {
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    AudioManager.PlayAudio(0);
    Application.LoadLevel("scene" + scene);
  }
  //游戏暂停
  void Pause() {
    if (this.gameObject.GetComponent<UIFirstHelp>() != null) this.gameObject.GetComponent<UIFirstHelp>().isPauseTip = true;
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    AudioManager.PlayAudio(0);
    isPause = !isPause;
  }
  //出兵
  void StartRound() {
    isStartRound = true;
    if (this.gameObject.GetComponent<UIFirstHelp>() != null) this.gameObject.GetComponent<UIFirstHelp>().isFirstStartWave = true;
    startround.gameObject.SetActive(false);
  }
  //获取是否加速的toggle的状态
  void ChangeSpeed() {
    if (changespeed.isOn && !isChangeSpeed) isChangeSpeed = true;
    else if (!changespeed.isOn && isChangeSpeed) isChangeSpeed = false;
  }

  void ChangeBackGroundMusic() {
    //Debug.Log("检测背景音乐开");
    if (isBackGroundMusic.isOn && !UIData.game_backmusic_bool) UIData.game_backmusic_bool = true;
    else if (!isBackGroundMusic.isOn && UIData.game_backmusic_bool) UIData.game_backmusic_bool = false;
  }

  void ChangeEffectMusic() {
    //Debug.Log("检测声效开");
    if (isEffectMusic.isOn && !UIData.game_effectmusic_bool) UIData.game_effectmusic_bool = true;
    else if (!isEffectMusic.isOn && UIData.game_effectmusic_bool) UIData.game_effectmusic_bool = false;
  }

  //设置几个容器的可见性
  public void SwitchPlane(int index) {
    plane_[index - 1].SetActive(true);
    for (int i = 0;i < 4;i++) {
      if (i == index - 1) continue;
      plane_[i].SetActive(false);
    }
  }

  void ChangeTextEnd() {//改变结束时的数值
    kill_f.text = kill.ToString();//失败界面显示杀敌数
    kill_v.text = kill.ToString();//成功界面显示杀敌数
    damage_f.text = damage.ToString();//失败界面显示伤害值
    damage_v.text = damage.ToString();//成功界面显示伤害值
    score = kill * 100 + damage * 10;//计算伤害值
    score_f.text = score.ToString();//失败界面显示评分
    score_v.text = score.ToString();//成功界面显示评分
    if (PlayerPrefs.GetInt("historykill", 0) < kill) PlayerPrefs.SetInt("historykill", kill);//如果杀敌数超过之前记录值，重新记录
    if (PlayerPrefs.GetInt("historydamage", 0) < damage) PlayerPrefs.SetInt("historydamage", damage);//如果伤害值超过之前记录值，重新记录
    if (PlayerPrefs.GetInt("historyscore", 0) < score) PlayerPrefs.SetInt("historyscore", score);//如果评分超过之前记录值，重新记录
  }


  #endregion
  //改变相关游戏中相关text的值
  void ChangeUI() {//改变显示
    remainLife_t.text = remainLife + "/" + set_remainLife;//改变基地生命值
    remainMoney_t.text = remainMonney + "";//改变购塔金钱
    round_t.text = "回合：" + currentwave + "/" + set_wave;//改变回合数
  }

  #region 其他方法调用，将有关塔种植的每个引用传递出去
  public static GameObject Send_Tower_Select() {//传递选塔UI引用
    uimanager.CheckIsPurse();//第一次检测是否可以买塔
    return uimanager.tower_select;//返回选塔UI引用
  }

  public static GameObject Send_Tower_Up() {//传递升级塔UI引用
    return uimanager.tower_up;//返回升级塔UI引用
  }

  public static GameObject Send_Tower_Top() {//传递顶级塔UI引用
    return uimanager.tower_top;//返回顶级塔UI引用
  }
  bool isC = false;
  //判断是否可以购买塔
  void CheckIsPurse() {//判断是否可以购买塔
    for (int i = 0;i < 4;i++) {//遍历四种塔的购塔金钱
      if (!tower_select_not[i].activeInHierarchy && GameData.pursePrice_Tower[i] <= remainMonney) continue;//如果可以种塔，并且钱够的话，结束当前循环
      if (GameData.pursePrice_Tower[i] > remainMonney) {//如果钱不够，就不可以种塔
        tower_select_not[i].SetActive(true);
      }
      else {//钱够了，可以种塔
        tower_select_not[i].SetActive(false);
      }
    }
    isStartCheck = true;//开始检测种塔的标志位
  }

  void CheckIsUp() {//判断是否可以升级塔
    if (tower_exit == null) return;//如果不存在塔，返回
    if (tower_exit.GetComponent<Tower>().tower_index > 7) return;//如果为顶级塔则返回
    if (!tower_up_not.activeInHierarchy && GameData.pursePrice_Tower[tower_exit.GetComponent<Tower>().tower_index + 4] > remainMonney) {
      tower_up_not.SetActive(true);//上锁图片显示
    }
    else if (tower_up_not.activeInHierarchy && GameData.pursePrice_Tower[tower_exit.GetComponent<Tower>().tower_index + 4] <= remainMonney) {
      tower_up_not.SetActive(false);//上锁图片隐藏
    }
  }

  #endregion

  #region 设置选塔界面的出现与消失的动画
  public void Set_State(int index) {
    switch (index) {
      //第一次选塔出现与消失
      case 1: anim_select.SetInteger("appear", 1);
        anim_up.SetInteger("appear_up", 4);
        anim_top.SetInteger("appear_top", 6);
        break;
      case 2: anim_select.SetInteger("appear", 2); break;
      //升级出现与消失
      case 3: anim_up.SetInteger("appear_up", 3);
        anim_select.SetInteger("appear", 2);
        anim_top.SetInteger("appear_top", 6);
        break;
      case 4: anim_up.SetInteger("appear_up", 4); break;
      //顶级卖塔出现与消失        
      case 5: anim_top.SetInteger("appear_top", 5);
        anim_select.SetInteger("appear", 2);
        anim_up.SetInteger("appear_up", 4);
        break;
      case 6: anim_top.SetInteger("appear_top", 6); break;
    }
  }
  #endregion

  #region 选塔种植的相关方法的实现，通过调用TowerManager中设置实现


  public void InitTower_l() {
    towermanager.SetTowerPrefab(0, palnt);
  }
  public void InitTower_h() {
    towermanager.SetTowerPrefab(1, palnt);
  }

  public void InitTower_j() {
    towermanager.SetTowerPrefab(2, palnt);
  }
  public void InitTower_f() {
    towermanager.SetTowerPrefab(3, palnt);
  }

  public void InitTower_Up() {
    towermanager.SetTowerPrefab(tower_index_up, palnt);
  }

  public void Sell_Tower() {
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    AudioManager.PlayAudio(12);
    tower_exit.GetComponent<Tower>().plantform.gameObject.SetActive(true);
    //Debug.Log(tower_exit.GetComponent<Tower>().tower_index);
    Destroy(tower_exit);
    remainMonney += tower_exit.GetComponent<Tower>().pursePrice / 2;
    Tower_UI_Disappear();
  }


  //改变买塔的和卖塔的数值
  public void ChangeText() {
    int index = tower_exit.GetComponent<Tower>().tower_index;
    //Debug.Log("当前塔的索引值：" + index);
    if (index > 7) {
      sell_top_text.text = (int)(GameData.pursePrice_Tower[index] / 2) + "";//卖出当前塔的金钱
    }
    else {
      confim_up_text.text = GameData.pursePrice_Tower[index + 4] + "";//升级塔的金钱
      sell_current_text.text = (int)(GameData.pursePrice_Tower[index] / 2) + "";//卖出当前塔的金钱
    }
  }

  #endregion

  #region 设置特效的索引值
  //判断是否选中技能图标
  void CheckSelectEffect() {//检测特效的选择状态
    for (int i = 0;i < 4;i++) {//遍历特效toggle数组
      if (effects[i].isOn && effect_i != i) {//如果特效被选中，并且是第一次被选中
        effect_i = i;
        SetEffect(i);
      }
    }
  }

  public void SetIsOn(int index) {//技能重置
    effects[index].isOn = false;//不选中状态
    effect_i = -1;//当前索引为-1
  }

  //释放技能时设定的，如果检测出来当时并没有选中的技能，就不释放
  public bool CheckIsOn() {
    int j = 0;
    for (int i = 0;i < 4;i++) {
      if (!effects[i].isOn) {
        j++;
      }
    }
    if (j == 4) {
      return false;
    }
    else {
      return true;
    }
  }

  public void SetEffect(int index) {//设置特效索引值
    GameManager.game.effect_index = index;//特效索引传递
  }

  #endregion
  public void Tower_UI_Disappear() {//将UI隐藏的方法
    if (tower_index_up > 3) {
      RangeCircle.rangeCircle.DisAppearCircle();//防御塔范围显示的圈消失
    } 
    Set_State(2);//选塔时UI消失
    Set_State(4);//升级塔的UI消失
    Set_State(6);//顶级塔选中时出现的UI消失
  }
}
