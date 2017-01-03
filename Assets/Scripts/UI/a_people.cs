using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class a_people : MonoBehaviour {
  public Image experience;//经验值设置
  public Text grade;//等级
  public Text fight;//战斗力
  public GameObject[] grade_texture;//评级
  public Button selectHero;//选择英雄进入游戏选关
  public Button switch_next;                      //切换怪物展示界面按钮
  public GameObject plane_player;             //人物选择界面
  public GameObject plane_guai;               //怪物选择界面
  public Toggle[] select_content;                //选择要展示的属性
  public Toggle[] select_player;                     //选择人物的toggle
  public GameObject[] player;                     //人物模型数组
  public GameObject[] content;         //选择展示属性的子界面
  public GameObject player_z;       //人物汇总
  public GameObject guai_z;
  public Text[] content_1;//详细属性的文本框数组
  public Image content_2_image;//英雄属性的图片框
  public Sprite[] content_wantselect;//
  public Text content_2_text;//英雄技能介绍
  public Text content_3;//英雄背景介绍
  public static int player_select_static = -1;    //记录当前选中的英雄索引的全局数据
  bool player_select = false;                         //记录人物图标是否被选中状态
  bool content_select = false;                            //记录展示图标是否被选中状态
  //控制是否第一次执行
  private int player_i_select = -1;//记录当前选中的英雄索引
  private int content_i_select = -1;//记录当前内容的索引
  private int contentchange = -1;//记录内容是否改变，避免多次赋值
  private int gradeTextureInt = -1;//记录图片改变索引，避免多次对图片进行赋值
  // Use this for initialization
  void Start() {
    switch_next.onClick.AddListener(SwitchNext);//切换怪物属性界面按钮注册监听
    selectHero.onClick.AddListener(SelectHeroMoveTo);//选中英雄按钮注册监听
    player_z.SetActive(true);//英雄模型汇总显示
    guai_z.SetActive(false);//怪物模型汇总消失
    plane_player.SetActive(true);//英雄面板显示
    plane_guai.SetActive(false);//怪物面板消失
    FirstChange();//第一次赋值
  }

  // Update is called once per frame
  void Update() {
    CheakSelectPlayer();//判断当前选中的英雄
    ChangeContent();//判断选中当期英雄下，各项内容显示是否改变
    ChangeValue();//改变文本框的值
  }

  void SwitchNext() {//切换到怪物界面
    AudioManager.PlayAudio(18);//切换上下界面的声效
    plane_player.SetActive(false);//英雄面板消失
    plane_guai.SetActive(true);//怪物面板出现
    player_z.SetActive(false);//英雄模型汇总对象消失
    guai_z.SetActive(true);//怪物模型汇总对象出现
  }

  void CheakSelectPlayer() {//判断人物选定状态
    for (int i = 0;i < select_player.Length;i++)//遍历人物数组
        {
      player_select = select_player[i].isOn;  //获取人物头像被选中的状态
      if (player_select && player_i_select != i)        //如果被选中并且是第一次执行             
            {
        if (player_i_select != -1) AudioManager.PlayAudio(17);//按钮点击的声效
        player_select_static = i;//记录当前全局英雄索引，当点击选择英雄以后会在选关界面会读此索引
        player_i_select = i;//记录当前英雄索引
        ChangeRightContent(i);//改变显示的内容
      }
    }
  }

  void FirstChange() {//第一次赋值，这个方法是用在从选关界面返回时，英雄应该与之前选择的一样，并不是初始状态
    if (player_select_static == -1) return;//如果还没有选中英雄，则不进行赋值
    for (int i = 0;i < select_player.Length;i++) {//遍历英雄头像复选框数组
      if (player_select_static == i) {//将已经选中的英雄，表示选中的状态
        select_player[i].isOn = true;//英雄头像被选中
        player_i_select = i;//记录选中的英雄
        ChangeRightContent(i);//改变游戏的属性值
      }
      else {
        select_player[i].isOn = false;//其他没有被选中的英雄，没有选中状态
      }
    }
  }

  void ChangeContent() {//判断内容选项是否选中的状态
    for (int i = 0;i < select_content.Length;i++) {
      content_select = select_content[i].isOn;//获取是否选中状态
      if (content_select && content_i_select != i)//判断选中并且第一次选中，防止多次赋值
            {
        if (content_i_select != -1) AudioManager.PlayAudio(18);//按钮点击的声效
        content_i_select = i;//更改显示内容选中索引
        content[i].SetActive(true);//将选中内容的内容显示
        for (int j = 0;j < select_content.Length;j++)//遍历内容选项的数组，将不是当前要显示的内容设置为不显示
                {
          if (j == content_i_select) continue;//如果与索引相同，就继续
          content[j].SetActive(false);//将内容置为不可见
        }
      }
    }
  }

  void ChangeRightContent(int index) {//展示右侧英雄的属性
    Debug.Log("index:" + index);
    experience.fillAmount = GameData.exprience[index];//经验值改变
    fight.text = GameData.fight[index] + "";//战斗力改变
    grade.text = GameData.grade[index] + "";//评分改变
    for (int i = 0;i < 5;i++) {//评级改变，将星级显示
      if (i == GameData.gradeTexture[index]) {
        gradeTextureInt = i;
        break;
      }
      grade_texture[i].SetActive(true);
    }
    for (int i = gradeTextureInt + 1;i < 5;i++) {
      grade_texture[i].SetActive(false);
    }
  }


  void ChangeValue()//改变英雄属性
  {
    if (contentchange == player_i_select) return;
    contentchange = player_i_select;//记录当前选中英雄索引，避免多次赋值
    player[player_i_select].SetActive(true);//将选中你的英雄模型显示
    for (int j = 0;j < select_player.Length;j++) {//将未选中的英雄隐藏
      if (j == player_i_select) continue;
      player[j].SetActive(false);
    }
    content_1[0].text = GameData.healthPoint_Hero[player_i_select] + "";//生命值显示
    content_1[1].text = GameData.attackSpeed_Hero[player_i_select] + "";//攻击速度显示
    content_1[2].text = GameData.damage_Hero[player_i_select] + "";//伤害值显示
    content_1[3].text = GameData.attackRange + "";//攻击范围显示
    content_1[4].text = GameData.moveSpeed_Hero[player_i_select] + "";//移动速度显示
    content_2_image.sprite = content_wantselect[player_i_select];//技能图片显示
    content_2_text.text = GameData.skill[player_i_select] + "";//技能介绍显示
    content_3.text = GameData.introduce[player_i_select];//英雄背景介绍显示
  }

  void SelectHeroMoveTo() {//切换当前场景到游戏选关场景
    for (int i = 0;i < select_player.Length;i++) {
      if (select_player[i].isOn) {//如果英雄被选中，将索引传给全局记录参数，并切换到选关界面
        UIData.heroselectindex_int = i;//记录选中的英雄
        AudioManager.PlayAudio(0);//按钮点击的声效
        QualitySettings.shadowDistance = 40;//将影子距离改为40
        this.gameObject.GetComponent<Loading>().LoadScene(5);//异步加载游戏选关场景
        break;
      }
    }
  }
}
