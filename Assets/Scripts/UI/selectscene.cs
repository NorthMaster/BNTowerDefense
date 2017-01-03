using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class selectscene : MonoBehaviour {
  public Text history_kill;//历史最大杀敌数
  public Text history_damage;//历史最大伤害值
  public Text history_score;//历史最高评分
  AudioSource backgoundaudio;//声音源
  public GameObject effectmusic;
  public AudioClip[] music;
  public Button switchselecthero;
  //三个关卡的panel
  public GameObject[] plane_select;
  //确认进入游戏
  public Button startscene;
  // 下一个界面
  public Button nextselect;
  //上一个界面
  public Button lastselect;
  public Button exit;
  public Image heroselect;
  public Sprite[] herotexture;
  [SerializeField]
  private int heroindex;
  [SerializeField]
  private int sceneindex = 0;

  // Use this for initialization
  void Start() {
    history_kill.text = PlayerPrefs.GetInt("historykill", 0) + "";//读取并显示游戏存档中最高杀敌数
    history_damage.text = PlayerPrefs.GetInt("historydamage", 0) + "";//读取并显示游戏存档中最高伤害值
    history_score.text = PlayerPrefs.GetInt("historyscore", 0) + "";//读取并显示游戏存档中最高评分

    if (GameObject.Find("AudioManager(Clone)") != null) {//如果存在音频对象就将其销毁
      welcome.isExitAudio = false;//全局变量赋值
      Destroy(GameObject.Find("AudioManager(Clone)"));//存在音频对象就销毁
    }
    backgoundaudio = GetComponent<AudioSource>();//获取自己当前场景的音频对象
    heroselect.sprite = herotexture[UIData.heroselectindex_int];//英雄头像显示
    lastselect.gameObject.SetActive(false);   //初始状态将选上衣界面的按钮设为不可见
    heroindex = UIData.heroselectindex_int;    //获取被选中的英雄的索引值
    Time.timeScale = 1;//设置游戏的时间分度值
    //初始状态第一关被选中
    plane_select[0].SetActive(true);
    plane_select[1].SetActive(false);
    plane_select[2].SetActive(false);
    // 默认为第一关
    sceneindex = 0;
    //对几个按钮注册监听
    lastselect.onClick.AddListener(LastScene);
    nextselect.onClick.AddListener(NextSelect);
    startscene.onClick.AddListener(LoadingScene);
    exit.onClick.AddListener(ExitCurrentScene);
    switchselecthero.onClick.AddListener(SelectHero);
  }

  // Update is called once per frame
  void Update() {
    JudgeBackGroundMusic();//判断背景音乐是否要播放
  }

  void ExitCurrentScene() {
    PlayAudio(0);//播放点击按钮声效
    GetComponent<Loading>().LoadScene(3);   //调用当前场景的异步加载方法
  }

  void LoadingScene() {
    PlayAudio(0);//播放点击按钮声效
    GetComponent<Loading>().LoadScene(sceneindex);    //调用当前场景的异步加载方法
  }

  void NextSelect() {  //实现切换到下一页的功能
    //如果当前界面是进入第一关的界面 
    if (sceneindex == 0) {
      Debug.Log("到达第二个");
      SetActive(1);
      lastselect.gameObject.SetActive(true);
    }
    else if (sceneindex == 1) {
      Debug.Log("到达第三个");
      SetActive(2);
      nextselect.gameObject.SetActive(false);
    }
  }

  void LastScene() {
    if (sceneindex == 1) {   //如果当前界面是进入第二关的界面 
      lastselect.gameObject.SetActive(false);
      nextselect.gameObject.SetActive(true);
      SetActive(0);
    }
    else if (sceneindex == 2) {
      lastselect.gameObject.SetActive(true); nextselect.gameObject.SetActive(true);
      SetActive(1);
    }
  }
  void SetActive(int index) {//设计几个界面的可见性
    PlayAudio(1);//播放切换声效
    sceneindex = index;//当前选中的场景索引
    plane_select[index].SetActive(true);//将选中的场景内容显示
    for (int i = 0;i < 3;i++) {//遍历场景信息数组
      if (i == index) continue;//如果是当前场景就跳过当前循环
      plane_select[i].SetActive(false);//将不是当前场景的内容隐藏
    }
  }

  void SelectHero() {
    PlayAudio(0);
    GetComponent<Loading>().LoadScene(4);
  }


  void JudgeBackGroundMusic() {  //判断是否进行背景音乐的播放
    if (UIData.game_backmusic_bool) {//如果可以进行背景音乐播放
      if (backgoundaudio.isPlaying) return;//正在播放呢，则返回
      else backgoundaudio.Play();//否则开始播放
    }
    else {//如果不可以播放音乐
      if (backgoundaudio.isPlaying) backgoundaudio.Pause();//如果正在播放，就停止播放
    }
  }

  void PlayAudio(int index) {//播放切换或者按钮点击的声效
    if (!UIData.game_effectmusic_bool) return;//如果全局游戏音效被关闭
    if (index == 0) {//切换声效
      effectmusic.GetComponent<AudioSource>().clip = music[0];//将clip对象传给声音源
    }
    else if (index == 1) {//按钮
      effectmusic.GetComponent<AudioSource>().clip = music[1];//将clip对象传给声音源
    }
    effectmusic.GetComponent<AudioSource>().Play();//播放声效
  }
}
