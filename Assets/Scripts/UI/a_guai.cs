using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class a_guai : MonoBehaviour {
    public Button switchlast;//切换到上一界面的按钮
    public GameObject plane_player;//英雄属性的界面
    public GameObject plane_guai;//怪物属性的界面
    public Toggle[] guai_select;//怪物的复选框组
    public GameObject[] guai;//怪物模型数组
    public Text[] guai_text;//显示怪物属性的文本框数组
    public GameObject player_z;//英雄汇总
    public GameObject guai_z;//怪物汇总
    private bool isselect = false;//图片是否被选中
    private int guai_i_select = -1;//被选中的图片索引
    private int text_i = -1;//避免多次赋值的索引

    

	// Use this for initialization
	void Start () {
        switchlast.onClick.AddListener(SwitchLast);//为切换界面按钮添加监听
	}
	
	// Update is called once per frame
	void Update () {
        CheakSelectGuai();//判断怪物选定状态
        ChangeValue();//改变怪物属性值
	}

    void CheakSelectGuai()//判断怪物选定状态
    {
        for(int i=0;i<guai_select.Length;i++)//遍历怪物头像复选框数组
        {
            isselect = guai_select[i].isOn;//当前复选框状态，是否被选中
            if(isselect&&guai_i_select!=i)//如果被选中，并且第一次被选中
            {
                if (guai_i_select != -1) AudioManager.PlayAudio(17);//按钮点击的声效
                guai_i_select = i;//记录当前怪物索引
                guai[i].SetActive(true);//怪物模型出现
                for(int j=0;j<guai_select.Length;j++)//遍历怪物模型，将其他怪物模型置为不可见
                {
                    if (j == i) continue;//如果索引相同，跳过当前值
                    guai[j].SetActive(false);//将怪物模型置为不可见状态
                }
            }
        }
    }

    void ChangeValue()//改变属性值的方法
    {
        if (text_i == guai_i_select) return;//如果已经赋值，则返回
        text_i = guai_i_select;//记录当前怪物索引赋值索引
        guai_text[0].text = GameData.healthPoint_Enemy[guai_i_select] + "";//显示生命值
        guai_text[1].text = GameData.deadPrice_Enemy[guai_i_select] + "";//显示死亡金钱
        guai_text[2].text=GameData.damage_Enemy[guai_i_select]+"";//显示伤害值
        guai_text[3].text = GameData.attackSpeed_Enemy[guai_i_select] + "";//显示攻击速度
        guai_text[4].text = GameData.moveSpeed_Enemy[guai_i_select] + "";//显示移动速度
    }

    void SwitchLast()//切换到英雄选择界面
    {
        AudioManager.PlayAudio(18);//切换上下界面的声效
        plane_player.SetActive(true);//英雄属性面板被激活
        plane_guai.SetActive(false);//怪物属性面板被关闭
        player_z.SetActive(true);//开始显示英雄
        guai_z.SetActive(false);//关闭怪物显示
    }
}
