using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
  public static AudioManager audiomanger; //当前类的引用
  AudioSource audio;  //声明游戏音效AudioSource组件，用于更换Clip
  AudioSource backgoundaudio; //声明背景音乐AudioSource组件，用于更换Clip
  public AudioSource backgroundmusic;//当前场景的背景音乐
  public AudioSource effectmusic;//当前场景的游戏音效
  //游戏关卡的背景音乐
  //游戏选关界面音乐
  //游戏主界面音乐
  public AudioClip selectHero_aduio;   //选中英雄头像的声音
  public AudioClip switch_aduio;    //各种切换的音效
  public AudioClip switccNextLast_audio;//切换至下一面板的音效
  public AudioClip buttonClick_audio;    //按钮音效
  public AudioClip select_audio;    //选中音效
  public AudioClip shaizi_audio;    //摇骰子
  public AudioClip wave_audio; //出兵的声效，锣声
  public AudioClip purse_audio;    //买塔的声效
  public AudioClip sell_audio;//卖塔的声效
  public AudioClip jTower_audio;  //箭塔的攻击声效
  public AudioClip hTower_audio; //红色塔的攻击声效
  public AudioClip lTower_audio; //绿色塔的攻击声效
  public AudioClip fTower_audio; //范围的攻击声效
  public AudioClip[] hero_attack_audio;    //英雄攻击的声效
  public AudioClip[] enemy_attack_audio;    //小兵攻击的声效

  public AudioClip effect_1_audio;    //技能1攻击声效
  public AudioClip effect_2_audio;//技能2攻击声效
  public AudioClip effect_3_audio;//技能3攻击声效
  public AudioClip effect_4_audio;//技能4攻击声效
  void Start() {
    audiomanger = this;    //获取当前类的引用
    audio = effectmusic.GetComponent<AudioSource>();//获取游戏音效AudioSource组件的引用
    backgoundaudio = backgroundmusic.GetComponent<AudioSource>();//获取背景音乐AudioSource组件的引用
  }
  void Update() {
    JudgeBackGroundMusic();//时时检测背景音乐是否需要播放
  }
  public static void PlayAudio(int index) {//播放游戏音效的方法
    if (!UIData.game_effectmusic_bool) return;//如果背景音效不允许播放，则返回
    audiomanger.audio.volume = 0.8f;//控制音效音量为0.8
    //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
    //为AudioSource指定clip
    switch (index) {
      case 0://按钮点击音效
        audiomanger.audio.clip = audiomanger.buttonClick_audio; break;
      case 1://绿色炮塔攻击音效
        audiomanger.audio.clip = audiomanger.lTower_audio; audiomanger.audio.volume = 0.5f; break;
      case 2: //红色炮塔攻击音效
        audiomanger.audio.clip = audiomanger.hTower_audio; audiomanger.audio.volume = 0.5f; break;
      case 3://箭塔攻击音效
        audiomanger.audio.clip = audiomanger.jTower_audio; audiomanger.audio.volume = 0.5f; break;
      case 4: //范围塔攻击音效
        audiomanger.audio.clip = audiomanger.fTower_audio; audiomanger.audio.volume = 0.5f; break;
      case 5://技能1攻击音效 
        audiomanger.audio.clip = audiomanger.effect_1_audio; break;
      case 6://技能2攻击音效
        audiomanger.audio.clip = audiomanger.effect_2_audio; break;
      case 7://技能3攻击音效
        audiomanger.audio.clip = audiomanger.effect_3_audio; break;
      case 8://技能3攻击音效
        audiomanger.audio.clip = audiomanger.effect_4_audio; break;
      case 9://英雄攻击声效,随机传入数据 
        audiomanger.audio.clip = audiomanger.hero_attack_audio[audiomanger.RangeHero()]; break;
      case 10: //小兵攻击声效,随机传入数据 
        audiomanger.audio.clip = audiomanger.enemy_attack_audio[audiomanger.RangeEnemy()]; break;
      case 11: //种塔声效
        audiomanger.audio.clip = audiomanger.purse_audio; break;
      case 12: //卖塔声效
        audiomanger.audio.clip = audiomanger.sell_audio; break;
      case 13: //出兵声效
        audiomanger.audio.clip = audiomanger.wave_audio; break;
      case 14: //切换声效
        audiomanger.audio.clip = audiomanger.switch_aduio; break;
      case 15: //选中英雄的声效
        audiomanger.audio.clip = audiomanger.selectHero_aduio; break;
      case 16://摇色子
        audiomanger.audio.clip = audiomanger.shaizi_audio; break;
      case 17://选中音效
        audiomanger.audio.clip = audiomanger.select_audio; break;
      case 18://切换上下界面的声效
        audiomanger.audio.clip = audiomanger.switccNextLast_audio; break;
    }
    audiomanger.audio.Play();               //播放声音，默认播放一次
  }
  void JudgeBackGroundMusic() {//判断是否进行背景音乐的播放
    if (UIData.game_backmusic_bool) {//如果可以进行背景音乐播放
      if (backgoundaudio.isPlaying) return;//正在播放呢，则返回
      else backgoundaudio.Play();//否则开始播放
    }
    else {//如果不可以播放背景音乐
      if (backgoundaudio.isPlaying) backgoundaudio.Pause();//如果正在播放，就停止播放
    }
  }
  int RangeHero() { //随机返回英雄声效索引值
    return (int)(Random.Range(0, hero_attack_audio.Length - 1));
  }
  int RangeEnemy() {//随机返回小兵声效索引值
    return (int)(Random.Range(0, enemy_attack_audio.Length - 1));
  }
}
