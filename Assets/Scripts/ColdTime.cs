using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColdTime : MonoBehaviour {
    public Image filledimage;    //冷却出现的灰色图片
    public Text timeleft;    //显示冷却时间
    private float cd = 2;//缓存cd时间
    private float timer;//时间记录器
    private bool isStartTimer = false;//是否开始冷却的标志位

    void Start()
    {
        timer = 0;                              //时间归零
        filledimage.fillAmount = 0;             //黑色填充图归零
        //timeleft.gameObject.SetActive(false);   //隐藏倒计时文本框 
        isStartTimer = false;                   //开始cd标志位置为true
        filledimage.gameObject.SetActive(false);                //灰色图片初始状态不会出现
    }
    // Update is called once per frame
    void Update()
    {
        if (isStartTimer)        //进入开始冷却状态
        {
            timer += Time.deltaTime;    //计算时间
            filledimage.fillAmount = (cd - timer) / cd;                 //改变填充图比例
            timeleft.text = ((int)(cd - timer) + 1).ToString();//+ "s";   显示剩余cd时间
            if (timer >= cd)            //冷却结束
            {
                timer = 0;              //时间归零
                filledimage.fillAmount = 0;             //黑色填充图归零
                isStartTimer = false;                   //开始cd标志位置为true
                filledimage.gameObject.SetActive(false);                //冷却状态不见
            }
        }
    }
    public void StartCold(int cd)    //开始技能冷却，传入的是cd时间
    {
        this.cd = cd;                           //接收传入cd时间
        filledimage.fillAmount = 1;         //初始全覆盖
        //timeleft.gameObject.SetActive(true);    //显示倒计时文本框 
        isStartTimer = true;                    //开始cd标志位置为true
        filledimage.gameObject.SetActive(true);                //灰色图片出现       
    }
}
