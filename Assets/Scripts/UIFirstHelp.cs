using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIFirstHelp : MonoBehaviour {
    /// <summary>
    /// 此脚本用来控制游戏帮助功能
    /// </summary>
    public GameObject UIHelpGameObject;//帮助物体
    public GameObject tip_first_plane;//开启帮助后，进入场景第一次见到的界面
    public GameObject plane_1;//第一界面界面的内容
    public GameObject plane_2;//下一个界面的内容
    public Button next_tip;//切换到下一个提示
    public Button last_tip;//切换到上一个提示
    public Button confim_tip;//确认看到了提示
    public GameObject stopUIClick_plantform;//透明的图片，挡住ui，避免点击
    public GameObject stopUIClick_startWave;//透明的图片，挡住ui，避免点击
    public GameObject stopUIClick_effect;//透明的图片，挡住ui，避免点击
    public GameObject stopUIClick_changespeed;//透明的图片，挡住ui，避免点击
    public GameObject false_plantform;//假的板子，对其添加动画，用来吸引玩家注意
    public GameObject mainTip;//提示关卡目标
    public Text tip;//提示
    public Button slipButton;//提示滑动的按钮，点击屏幕取消
    public Image startwaveTip;//开始出兵的提示箭头
    public Image effectTip;//施放技能的提示箭头
    public Image changeSpeedTip;//改变速度的提示箭头
    public Image pauseTip;//暂停的提示箭头
    public Toggle changeSpeed;//切换速度复选框引用
    float alpha = 1;//声明透明度值
    [HideInInspector]
    public bool isFirstUI = false;//判断是否第一次出现选塔UI
    [HideInInspector]
    public bool isFirstStartWave = false;//是否是第一次出兵
    [HideInInspector]
    public bool isEffectTip = false;//是否是第一次施放技能
    [HideInInspector]
    public bool isChangeSpeedTip = false;//是否是第一次改变速度
    [HideInInspector]
    public bool isPauseTip = false;//是否是第一次暂停
    [HideInInspector]
    public  int countTower = 0;//是否是第一次种塔

	void Start () {
        if (!UIData.game_help) return;//如果游戏帮助没有开启，就不进行以下步骤
        UIHelpGameObject.SetActive(true);
        countTower = 0;//当前种塔数，如果没有种塔，那个提示种塔的板子也不会消失
        slipButton.gameObject.SetActive(false);//提示滑动和缩放的按钮初始不可见
        tip_first_plane.SetActive(true);//第一个物体盛放游戏最初的提示
        plane_1.SetActive(true);//第一个提示panel的第一个子界面可见
        plane_2.SetActive(false);//第一个提示panel的第二子界面不可见
        last_tip.gameObject.SetActive(false);//切换到上一个界面的按钮消失
        next_tip.gameObject.SetActive(true);//切换到下一个界面的按钮打开
        confim_tip.gameObject.SetActive(false);//确定按钮不出现
        next_tip.onClick.AddListener(SwitchNext);//切换下一面板按钮添加监听
        last_tip.onClick.AddListener(SwitchLast);//切换到上一面板按钮添加监听
        confim_tip.onClick.AddListener(ConfirmTip);//确认按钮添加监听
        slipButton.onClick.AddListener(ExitSlipTip);//介绍滑动按钮添加监听
	}
	
	void Update () {
        CheckFirstSelect();//检测第一次种塔的方法
        CheckStartWave();//检测第一次出兵的方法
        CheckEffectTip();//检测第一次施放技能的方法
        CheckChangeSpeedTip();//检测第一次改变速度的方法
        CheckPauseTip();//检测第一次暂停的方法
	}

    void SwitchNext()//切换到下一界面
    {
        AudioManager.PlayAudio(14);//播放切换声效
        plane_1.SetActive(false);//第一内容消失
        plane_2.SetActive(true);//第二内容出现
        confim_tip.gameObject.SetActive(true);//确定按钮出现
        last_tip.gameObject.SetActive(true);//切换到上一个界面的按钮出现
        next_tip.gameObject.SetActive(false);//切换到下一界面的按钮消失
    }

    void SwitchLast()//切换到上一界面
    {
        AudioManager.PlayAudio(14); //播放切换声效
        plane_1.SetActive(true);//第一内容消失
        plane_2.SetActive(false);//第二内容出现
        confim_tip.gameObject.SetActive(false);//确定按钮出现
        last_tip.gameObject.SetActive(false);//切换到上一个界面的按钮出现
        next_tip.gameObject.SetActive(true);//切换到下一界面的按钮消失
    }

    void ConfirmTip()//确认后消失提示界面
    {
        AudioManager.PlayAudio(0);//播放点击按钮声效
        tip_first_plane.SetActive(false);//第一提示界面消失
        stopUIClick_plantform.SetActive(true);//激活阻挡，避免UI被点击
        ChangeTipMessage("保护自己的基地，避免怪物入侵");//文字提示
        mainTip.SetActive(true);//激活提示板子
        StartCoroutine(mainTips());//开启协程，闪动三秒      
    }
    IEnumerator mainTips()//提示守卫基地的板子闪动三秒，继续执行的方法
    {
        yield return new WaitForSeconds(3f);//等待三秒
        mainTip.SetActive(false);//提示消失
        false_plantform.SetActive(true);//提示的板子出现
        ChangeTipMessage("点击闪烁的板子，选塔种塔");//文字提示
    }
    void CheckFirstSelect()//提示第一次种塔
    {
        if(countTower==1&&false_plantform.activeInHierarchy)//如果已经种塔，并且提示种塔的板子已经被激活
        {
            false_plantform.SetActive(false);   //假板子消失
            stopUIClick_plantform.SetActive(false);//阻挡点击的UI出现
            StartCoroutine(SlipTip());//滑动提示出现
        }
    }


    IEnumerator SlipTip()//滑动提示
    {
        yield return new WaitForSeconds(1f);//等待一秒
        ChangeTipMessage("通过手指操控实现移动与缩放");//文字提示
        slipButton.gameObject.SetActive(true);//提示滑动信息的按钮
    }
    #region 控制提示信息
    void ChangeTipMessage(string mess)
    {
        //alpha = 1;
        //tip.gameObject.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        tip.gameObject.SetActive(true);//设为可见
        tip.text = mess;
        StartCoroutine(DisAppear());
    }
    IEnumerator DisAppear()
    {
        yield return new WaitForSeconds(7f);
        tip.gameObject.SetActive(false);//过五秒设为不可见
    }
#endregion

    void ExitSlipTip()//退出滑动提示操作
    {
        slipButton.gameObject.SetActive(false);//滑动提示按钮消失
        StartCoroutine(AppearStartWaveTip());//调用点击出兵按钮提示的功能
    }
    IEnumerator AppearStartWaveTip()//点击出兵按钮提示
    {
        yield return new WaitForSeconds(1f);//等待一秒
        ChangeTipMessage("点击右下角闪烁的出兵按钮，敌人开始攻击");//文字提示
        stopUIClick_startWave.SetActive(true);//阻挡ui被点击的透明图 出现
        startwaveTip.gameObject.SetActive(true);//指向开始出兵的按钮出现
    }

    void CheckStartWave()//检测第一次出兵的方法
    {
       if(isFirstStartWave&&startwaveTip.gameObject.activeInHierarchy)//如果第一次点击出兵，并且指向出兵按钮的提示箭头已经激活
       {
           startwaveTip.gameObject.SetActive(false);//出兵提示取消，技能提示出现
           stopUIClick_startWave.SetActive(false);//阻挡点击的UI图片消失
           stopUIClick_effect.SetActive(true);//阻挡除技能外其他UI的图片出现
           effectTip.gameObject.SetActive(true);//指向技能的箭头出现
           ChangeTipMessage("自由选择技能进行施放");//文字提示
       }
    }
    void CheckEffectTip()//检测第一次施放技能的方法
   {
        if(isEffectTip&&effectTip.gameObject.activeInHierarchy)//如果已经释放了技能，并且提示的箭头被激活
        {
            effectTip.gameObject.SetActive(false);//技能提示箭头消失
            stopUIClick_effect.SetActive(false);//阻止技能点击的UI消失
            ChangeTipMessage("点击加速游戏按钮，自由控制游戏速度");//文字提示
            stopUIClick_changespeed.SetActive(true);//阻挡UI出现
            changeSpeedTip.gameObject.SetActive(true);//提示改变速度的箭头出现
        }
   }

    void CheckChangeSpeedTip()//检测第一次改变速度的方法
    {
        if(changeSpeed.isOn&&!isChangeSpeedTip)//如果点击了加速按钮
        {
            isChangeSpeedTip = true;//加速的标志位
        }
        if(isChangeSpeedTip&&changeSpeedTip.gameObject.activeInHierarchy)//如果已经加速了，并且正在提示要加速
        {
            changeSpeedTip.gameObject.SetActive(false);//加速提示箭头消失
            stopUIClick_changespeed.SetActive(false);//阻挡UI消火
            ChangeTipMessage("点击可以暂停游戏");//文字提示
            pauseTip.gameObject.SetActive(true);//暂停提示
        }
    }

    void CheckPauseTip()//检测第一次暂停的方法
    {
        if(isPauseTip&&pauseTip.gameObject.activeInHierarchy)//如果第一次点暂停，并且正在提示点击暂停
        {
            pauseTip.gameObject.SetActive(false);//提示箭头消失
            UIData.game_help = false;//游戏帮助关闭，并在下一次进入第一关时不再打开
        }
    }

}
