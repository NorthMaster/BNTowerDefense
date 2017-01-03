using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class welcome : MonoBehaviour {
  public static bool isExitAudio = false;//静态变量避免多次克隆声音控制预制件
  public Sprite[] doing_texture;//摇骰子的模糊过程图
  public Sprite[] done_texture;//摇骰子的结束图
  public GameObject aduioPrefab;//声音控制的预制件
  private GameObject a_clone;//声音缓存
  public Toggle[] head;//头像管理
  public Toggle[] hard;//游戏难度
  public Toggle[] select;    //项目选择
  public GameObject[] select_appear;
  public InputField inputname;    //玩家昵称显示区域，可自行输入
  public string[] Names;//索引值用来控制人物昵称
  public Button random;    //玩家昵称自选按钮
  public Button startgame;//开始游戏按钮
  public Button exit;//退出游戏按钮
  int i_select = -1;//以下整数值，均是为防止多次赋值
  int headindex = 0;//头像索引
  int hardindex = 0;//难度索引
  bool b_select = false;//内容选中状态标志位
  bool head_select = false;//头像选中状态标志位
  int playername = -1;//玩家昵称改变次数
  bool isEndShake = true;//是否开始摇骰子的标志位
  int frameindex = 0;//色子摇动时索引，控制换图
  int len = 0;//色子摇动图片数组的长度
  int gamebackgroundmusic_i = -1;//游戏背景音乐开启或者关闭索引值
  int gameeffectmusic_i = -1;//游戏音效开启或者关闭索引值
  int gamehelp_i = -1;//游戏帮助开启或者关闭索引值
  int gametip_i = -1;//游戏提示开启或者关闭索引值
  void Awake() {
    CheckExit();//检测初始状态，读取之前游戏存档信息
  }
  void Start() {
    SetPlayer();//设置当前场景信息
    if (!isExitAudio)//如果不存在声音源文件，就创建一个，并且切换场景不销毁此对象
    {
      a_clone = Instantiate(aduioPrefab) as GameObject;//实例化一个声音控制器
      DontDestroyOnLoad(a_clone);//切换场景不予销毁
      isExitAudio = true;//存在声音源的标志位置真
    }
    isEndShake = true;//初始状态色子不摇动
    len = doing_texture.Length;//获取执行模糊切换骰子图长度
    playername = -1;//避免第一次色子发出声
    headindex = UIData.player_texture_int;//头像索引
    hardindex = UIData.gamehard_int;//难度索引
    random.GetComponent<Image>().sprite = done_texture[UIData.player_name_int];//图片初始化
    inputname.text = UIData.player_name;//昵称初始化
    startgame.onClick.AddListener(StartMain);//监听进入游戏按钮
    exit.onClick.AddListener(ExitGame);//监听退出游戏按钮
    random.onClick.AddListener(ChangeName);//监听摇色子按钮
  }

  void CheckExit() {//初始检测状态，读取之前游戏存档信息
    UIData.player_texture_int = PlayerPrefs.GetInt("playerTextureIntOut", 0);//图片索引
    UIData.player_name_int = PlayerPrefs.GetInt("playerNameIntOut", 0);//将外部昵称索引赋予游戏静态昵称索引，如果不存在就赋值为1
    UIData.gamehard_int = PlayerPrefs.GetInt("gameHardIntOut", 0);//游戏难度索引  
    UIData.player_name = PlayerPrefs.GetString("playerNameOut", Names[UIData.player_name_int]);
    //获取外部信息
    gamebackgroundmusic_i = PlayerPrefs.GetInt("gameBackGroudMusic", 1);//获取背景音乐开闭索引值
    gameeffectmusic_i = PlayerPrefs.GetInt("gameEffectMusic", 1);//获取游戏音效开闭索引值
    gamehelp_i = PlayerPrefs.GetInt("gamehelp", 1);//获取游戏帮助索引值
    gametip_i = PlayerPrefs.GetInt("gametip", 1);//获取游戏提示索引值
    if (gamebackgroundmusic_i == 1) UIData.game_backmusic_bool = true;//如果背景音乐索引值为1，游戏音乐布尔值为真
    else UIData.game_backmusic_bool = false;//否则为假
    if (gameeffectmusic_i == 1) UIData.game_effectmusic_bool = true;//如果游戏音效索引值为1，游戏音乐布尔值为真
    else UIData.game_effectmusic_bool = false;//否则为假
    if (gamehelp_i == 1) UIData.game_help = true;//如果游戏帮助索引值为1，游戏音乐布尔值为真
    else UIData.game_help = false;//否则为假
    if (gametip_i == 1) UIData.game_tip = true;//如果游戏提示索引值为1，游戏音乐布尔值为真
    else UIData.game_tip = false;//否则为假
  }
  // Update is called once per frame
  void Update() {
    CheakSelect();//检测选中项目，每有切换直接更换内容，包括玩家昵称，玩家头像，游戏难度
    JudgeHead();//判断选中的头像
    JudgeHard();//判断选中的游戏难度
    StartShake();//摇色子的效果
  }
  void StartMain() {//进入游戏按钮
    AudioManager.PlayAudio(0);//播放按钮声效
    GetComponent<Loading>().LoadScene(3);//切换到游戏功能场景
  }
  void ExitGame() {//退出游戏，记录游戏状态
    AudioManager.PlayAudio(0);// 播放按钮音效
    UIData.player_name = inputname.text;//全局玩家昵称赋值
    //退出之前记录当前游戏设置
    PlayerPrefs.SetString("playerNameOut", UIData.player_name);//记录昵称
    PlayerPrefs.SetInt("playerTextureIntOut", UIData.player_texture_int);//记录头像索引
    PlayerPrefs.SetInt("playerNameIntOut", UIData.player_name_int);//记录昵称索引
    PlayerPrefs.SetInt("gameHardIntOut", UIData.gamehard_int);//记录游戏难度索引
    if (UIData.game_backmusic_bool) gamebackgroundmusic_i = 1;//如果游戏背景音乐开，相应索引值为1
    else gamebackgroundmusic_i = 0;//如果游戏背景音乐关，相应索引值为0
    if (UIData.game_effectmusic_bool) gameeffectmusic_i = 1;//如果游戏音效开，相应索引值为1
    else gameeffectmusic_i = 0;//如果游戏音效关，相应索引值为0
    if (UIData.game_help) gamehelp_i = 1;//如果游戏帮助开，相应索引值为1
    else gamehelp_i = 0;//如果游戏帮助关，相应索引值为0
    if (UIData.game_tip) gametip_i = 1;//如果游戏提示开，相应索引值为1
    else gametip_i = 0;//如果游戏提示开，相应索引值为0
    PlayerPrefs.SetInt("gameBackGroudMusic", gamebackgroundmusic_i);//记录背景音乐开闭索引
    PlayerPrefs.SetInt("gameEffectMusic", gameeffectmusic_i);//记录游戏音效开闭索引
    PlayerPrefs.SetInt("gamehelp", gamehelp_i);//记录游戏帮助开闭索引
    PlayerPrefs.SetInt("gametip", gametip_i);//记录游戏提示开闭索引
    Application.Quit();//退出游戏
  }

  void CheakSelect() {//改变内容选中状态
    for (int i = 0;i < select.Length;i++) {//遍历内容复选框组
      b_select = select[i].isOn;//获取复选框状态
      if (b_select && i_select != i) {//复选框被选中，并且不是第一次被选中
        if (i_select != -1) {//如果不是第一次赋值
          AudioManager.PlayAudio(14);//播放选中声效
        }
        i_select = i;//对内容复选框索引赋值
        select_appear[i].SetActive(true);//相应界面出现
        for (int j = 0;j < select.Length;j++) {//遍历所有页面
          if (j == i) continue;//如果与复选框索引值相等，跳过当前循环
          select_appear[j].SetActive(false);//页面被关闭
        }
      }
    }
  }

  void ChangeName() {//改变玩家昵称
    //控制播放声音
    if (playername != -1) {
      AudioManager.PlayAudio(16);//播放摇骰子音效
    }
    playername++;//游戏昵称索引增加
    isEndShake = false;//开始摇动色子
    StartCoroutine(EndShake());//开启结束摇动协程
    StopCoroutine(EndShake());//关闭结束摇动协程
  }

  IEnumerator EndShake() {//结束摇动协程
    yield return new WaitForSeconds(1f);//摇动时间为1秒
    int index = (int)(Random.Range(0, 5));//随机获取0-5的整数
    isEndShake = true;//停止摇动
    random.GetComponent<Image>().sprite = done_texture[index];//切换最后的色子图片
    UIData.player_name_int = index;//全局玩家昵称索引赋值
    inputname.text = Names[index];//显示名字
  }
  void JudgeHead() {//切换头像播放改变全局值的方法
    if (head[0].isOn && headindex != 0) {//头像1被选中，并且不是第一次被选中
      AudioManager.PlayAudio(17);//播放选中声效
      headindex = 0;//头像索引赋值
      UIData.player_texture_int = headindex;//全局头像索引赋值
    }
    else if (head[1].isOn && headindex != 1)//头像1被选中，并且不是第一次被选中
    {
      AudioManager.PlayAudio(17);//播放选中声效
      headindex = 1;//头像索引赋值
      UIData.player_texture_int = headindex;//全局头像索引赋值
    }
  }

  void JudgeHard() {//切换完成直接改变游戏难度索引值的方法
    if (hard[0].isOn && hardindex != 0)//普通难度被选中，并且不是初始状态赋值
    {
      AudioManager.PlayAudio(17);//播放选中声效
      hardindex = 0;//难度索引赋值
      UIData.gamehard_int = hardindex;//全局难度索引赋值
    }
    else if (hard[1].isOn && hardindex != 1)//地狱难度被选中，并且不是初始状态赋值
    {
      AudioManager.PlayAudio(17);//播放选中声效
      hardindex = 1;//难度索引赋值
      UIData.gamehard_int = hardindex;//全局难度索引赋值
    }
  }

  void StartShake() {//实现摇骰子功能
    if (isEndShake) return;//如果停止切换就返回
    if (Time.frameCount % 8 == 0) {//开始切换
      Sprite cur_texture = doing_texture[frameindex];//获取骰子摇动中的图
      random.GetComponent<Image>().sprite = cur_texture;//换图
      frameindex++;//帧率增加
      frameindex %= len;//获取骰子摇动索引
    }
  }

  void SetPlayer() {//当前场景初始状态设置
    if (UIData.player_texture_int == 0) {//如果玩家头像索引为0
      head[0].isOn = true;//头像1被选中
      head[1].isOn = false;//头像2未选中
    }
    else {
      head[0].isOn = false;//头像1未选中
      head[1].isOn = true;//头像2被选中
    }
    inputname.text = UIData.player_name;//玩家昵称初始设定
    if (UIData.gamehard_int == 0) {//如果游戏难度索引为0
      hard[0].isOn = true;//普通难度被选中
      hard[1].isOn = false;//地狱难度未选中
    }
    else {
      hard[0].isOn = false;//普通难度未选中
      hard[1].isOn = true;//地狱难度被选中
    }
  }
}
