using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIHelp : MonoBehaviour {
    public static UIHelp uihelp;
    //关于敌人提示的功能
    public Button openWaveDetial;//打开敌人提示
    public GameObject planeWaveDetial;//敌人提示框
    public Button exitWaveDetial;//关闭敌人提示框
    public Sprite[] enemyTextures;//敌人的图片
    public Image openButtonImage;//提示当前出那波兵的图
    public Image waveDetialImage;//出兵详情的图
    public Text waveDetialText;//出兵的介绍

    public HeroManager heroman;
    public Button hero;//英雄图片
    public Text heroname;//英雄名字
    public Image heroblood;//英雄血量
    public Sprite[] herotexture;//待选图片
    [HideInInspector]
    public int heroCurrentBlood;
    [HideInInspector]
    public int heroYBlood;
    private int currentGrade = 1;
    private int isUpGrade = 0;
    float currentTimescale = -1;//记录当前的timescale
	// Use this for initialization
	void Start () {
        planeWaveDetial.SetActive(false);//提示框初始关闭状态
        openWaveDetial.gameObject.SetActive(false);//打开提示框按钮初始不可见
        openWaveDetial.onClick.AddListener(OpenWaveDetial);//对打开提示框注册监听
        exitWaveDetial.onClick.AddListener(CloseWaveDetial);//对关闭提示框注册监听
        hero.GetComponent<Image>().sprite = herotexture[UIData.heroselectindex_int];//英雄头像
        heroname.text = UIData.player_name;//玩家昵称显示
        heroCurrentBlood = GameData.healthPoint_Hero[UIData.heroselectindex_int];
        heroYBlood = GameData.healthPoint_Hero[UIData.heroselectindex_int];
        hero.onClick.AddListener(HeroSelect);
	}
	
	// Update is called once per frame
	void Update () {
        ChangeHeroBlood();
	}
    void ChangeHeroBlood()
    {
        if(heroman.currentHero != null)
        {
            if (heroman.currentHero.healthPoint>0)
            {
                heroblood.fillAmount =(float)heroman.currentHero.healthPoint/ heroYBlood;//英雄血量显示
            }
        }      
    }

    void HeroSelect()
    {
        GameManager.game.hero = heroman.currentHero.gameObject;
        GameManager.game.isSelectHero = true;
        heroman.currentHero.BeSelected();
    }
    
    //打开关卡敌人提示
    void OpenWaveDetial()
    {
        //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
        AudioManager.PlayAudio(0);
        currentTimescale = Time.timeScale;
        Time.timeScale = 0;
        openWaveDetial.gameObject.SetActive(false);
        planeWaveDetial.SetActive(true);
    }
    //关闭敌人提示框
    void CloseWaveDetial()
    {
        //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
        AudioManager.PlayAudio(0);
        Time.timeScale = currentTimescale;
        planeWaveDetial.SetActive(false);
    }

    //更改信息，确保更新
    public void StartOpenWaveDetial(int index)
    {
        openWaveDetial.gameObject.SetActive(true);
        planeWaveDetial.SetActive(false);
        //Debug.Log("开始对敌人详情进行复制");
        openButtonImage.sprite = enemyTextures[index];
        waveDetialImage.sprite = enemyTextures[index];
        waveDetialText.text = GameData.enemyDetial[index];
    }
    

    
  
}
