using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class main : MonoBehaviour {
  public GameObject[] icon;//各个图标对象
  public Toggle isBackGroundMusic;//检测背景音乐开闭
  public Toggle isEffectMusic;//检测声效开闭
  public Toggle isHelp;//检测帮助是否开始
  public Toggle isTip;//游戏提示
  public GameObject aduioPrefab;//声音控制的预制件
  private GameObject a_clone;//声音控制器的缓存
  public Image playerTexture;//玩家头像
  public Button selectScene;//选关按钮
  public Button setb;//设置按钮
  public Button attributeb;//属性介绍按钮
  public Button helpb;//游戏简介按钮
  public Button relatedb;//关于按钮
  public Button exitb;//退出游戏按钮
  public Button exit_setb;//退出设置界面按钮
  public Button exit_relatedb;//退出关于界面按钮
  public Button exit_helpb;//退出游戏简介按钮
  public GameObject plane_set;//设置界面的引用
  public GameObject plane_related;//关于界面的引用
  public GameObject plane_help;//游戏简介界面的引用
  public Sprite[] player_texture;//存储玩家头像的数组
  int gamebackgroundmusic_i = -1;//记录当期背景音乐是否播放的索引值
  int gameeffectmusic_i = -1;//记录当前背景音效是否播放的索引值
  int gamehelp_i = -1;//记录游戏帮助是否开启的索引值
  int gametip_i = -1;//记录游戏提示是否开启的索引值
  // Use this for initialization
  void Awake() {//游戏进入是调用的方法啊，在Start()方法前调用
    if (UIData.game_backmusic_bool) isBackGroundMusic.isOn = true;//如果背景音乐开启，背景音乐复选框为选中状态
    else isBackGroundMusic.isOn = false;//如果背景音乐关闭，背景音乐复选框为未选中状态
    if (UIData.game_effectmusic_bool) isEffectMusic.isOn = true;//如果背景音效开启，背景音效复选框为选中状态
    else isEffectMusic.isOn = false;//如果背景音效关闭，背景音效复选框为未选中状态
    if (UIData.game_help) isHelp.isOn = true;//如果游戏帮助开启，游戏帮助复选框为选中状态
    else isHelp.isOn = false;//如果游戏帮助关闭，游戏帮助复选框为未选中状态
    if (UIData.game_tip) isTip.isOn = true;//如果游戏提示开启，游戏提示复选框为选中状态
    else isTip.isOn = false;//如果游戏提示关闭，游戏提示复选框为未选中状态
  }
  void Start() {//游戏开始时调用
    if (!welcome.isExitAudio) {//如果当前场景背景音乐不存在
      a_clone = Instantiate(aduioPrefab) as GameObject;//实例化一个音乐播放器
      DontDestroyOnLoad(a_clone);//切换场景是不销毁该对象
      welcome.isExitAudio = true;//记录音乐播放器是否存在全局变量设为真
    }
    playerTexture.sprite = player_texture[UIData.player_texture_int];//显示玩家头像
    selectScene.onClick.AddListener(LoadSelectScene);//对选关按钮注册监听
    setb.onClick.AddListener(Set);//对设置按钮注册监听
    attributeb.onClick.AddListener(Attribute);//对属性介绍按钮注册监听
    helpb.onClick.AddListener(Help);//对游戏关于按钮注册简体
    relatedb.onClick.AddListener(Related);//对游戏帮助按钮注册监听
    exitb.onClick.AddListener(BackScene);//对退出当期场景按钮注册监听
    exit_setb.onClick.AddListener(ExitSet);//对退出设置界面按钮注册监听
    exit_relatedb.onClick.AddListener(ExitRelated);//对退出关于界面按钮注册监听
    exit_helpb.onClick.AddListener(ExitHelp);//对退出游戏简介按钮注册监听
  }

  // Update is called once per frame
  void Update() {//每帧调用的方法
    ChangeBackGroundMusic();//检测是否关闭或者打开背景音乐的方法
    ChangeEffectMusic();//检测是否关闭或者打开游戏音效的方法
    ChangeHelp();//检测是否关闭或者开启游戏帮助的方法
    ChaneTip();//简则是否关闭或者开启游戏提示的方法
  }

  void HideIcon() {//隐藏图标的方法
    for (int i = 0;i < icon.Length;i++) {//遍历图标对象数组
      icon[i].SetActive(false);//禁用图标对象
    }
  }

  void DisplayIcon() {//显示图标的方法
    for (int i = 0;i < icon.Length;i++) {//遍历图标对象数组
      icon[i].SetActive(true);//显示图标对象
    }
  }

  void LoadSelectScene() {//切换到选关场景的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    GetComponent<Loading>().LoadScene(5);//切换场景
  }


  void Set() {//进行游戏设置的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    plane_set.SetActive(true);//显示设置界面
    HideIcon();//隐藏原界面图标
  }

  void ExitSet() { //退出设置界面的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    plane_set.SetActive(false);//隐藏设置界面
    DisplayIcon();//还原界面图标
  }

  void Attribute() {//进入属性介绍场景的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    GetComponent<Loading>().LoadScene(4);//切换娼妓
  }


  void Help() {//进入游戏简介的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    plane_help.SetActive(true);//游戏简介界面出现
    HideIcon();//隐藏图标
  }


  void Related() {//进入关于界面的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    plane_related.SetActive(true);//游戏关于界面出现
    HideIcon();//隐藏图标
  }

  void ExitRelated() {//退出关于界面的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    plane_related.SetActive(false);//游戏关于界面隐藏
    DisplayIcon();//显示图标
  }

  void ExitHelp() {//退出游戏简介界面的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    plane_help.SetActive(false);//游戏简介界面隐藏
    DisplayIcon();//隐藏图标
  }


  void BackScene() { //回到欢迎场景的方法
    AudioManager.PlayAudio(0);//播放点击按钮声效
    ////保存信息
    //if (UIData.game_backmusic_bool) gamebackgroundmusic_i = 1;//如果已经对游戏背景音乐进行了修改，开启为1，关闭为0
    //else gamebackgroundmusic_i = 0;
    //if (UIData.game_effectmusic_bool) gameeffectmusic_i = 1;//如果已经对游戏音效进行了修改，开启为1，关闭为0
    //else gameeffectmusic_i = 0;
    //if (UIData.game_help) gamehelp_i = 1;//如果已经对游戏帮助进行了修改，开启为1，关闭为0
    //else gamehelp_i = 0;
    //if (UIData.game_tip) gametip_i = 1;//如果已经对游戏提示进行了修改，开启为1，关闭为0
    //else gametip_i = 0;
    //PlayerPrefs.SetInt("gameBackGroudMusic", gamebackgroundmusic_i);//记录背景音乐是否开闭的数据，下次进入游戏按照此设置执行
    //PlayerPrefs.SetInt("gameEffectMusic", gameeffectmusic_i);//记录游戏音效是否开闭的数据，下次进入游戏按照此设置执行
    //PlayerPrefs.SetInt("gamehelp", gamehelp_i);//记录游戏帮助是否开闭的数据，下次进入游戏按照此设置执行
    //PlayerPrefs.SetInt("gametip", gametip_i);//记录游戏提示是否开闭的数据，下次进入游戏按照此设置执行
    GetComponent<Loading>().LoadScene(6);//切换到玩家设置场景
  }

  void ChangeBackGroundMusic() {//检测是否关闭或者打开背景音乐的方法
    if (isBackGroundMusic.isOn && !UIData.game_backmusic_bool) {
      UIData.game_backmusic_bool = true;
    }
    else if (!isBackGroundMusic.isOn && UIData.game_backmusic_bool) {
      UIData.game_backmusic_bool = false;
    }
  }

  void ChangeEffectMusic() {//检测是否关闭或者打开游戏音效的方法
    if (isEffectMusic.isOn && !UIData.game_effectmusic_bool) UIData.game_effectmusic_bool = true;
    else if (!isEffectMusic.isOn && UIData.game_effectmusic_bool) UIData.game_effectmusic_bool = false;
  }

  void ChangeHelp() {//检测是否关闭或者开启游戏帮助的方法
    if (isHelp.isOn && !UIData.game_help) UIData.game_help = true;
    else if (!isHelp.isOn && UIData.game_help) UIData.game_help = false;
  }

  void ChaneTip() {//简则是否关闭或者开启游戏提示的方法
    if (isTip.isOn && !UIData.game_tip) UIData.game_tip = true;
    else if (!isTip.isOn && UIData.game_tip) UIData.game_tip = false;
  }
}
