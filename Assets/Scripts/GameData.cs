using UnityEngine;
using System.Collections;

public class GameData {

    #region 小兵的相关参数
    public static float rotateSpeed_Enemy = 7f;//小兵转体速度
    //小兵生命值
    public static int[] healthPoint_Enemy =
    {              
        300,400,500,600,           
        700,800,800
    };
    //小兵死亡金钱
    public static int[] deadPrice_Enemy =
    {
        30,40,50,50,
        60,60,70
    };
    //小兵的攻击力
    public static int[] damage_Enemy =
    {
        6,7,8,9,
        10,11,12
    };
    //小兵的攻击速度
    public static float[] attackSpeed_Enemy =
    {
        0.5f,0.6f,0.7f,0.8f,
        0.9f,0.9f,0.9f
    };
    //小兵移动速度
    public static float[] moveSpeed_Enemy =
    {
        0.8f,1f,1.2f,1.6f,
        1.8f,2.0f,2.2f
    };
    //小兵提示
    public static string[] enemyDetial =
    {
        "来自一层地狱，移动速度不高，伤害不高，较易消灭",
        "来自二层地狱，伤害不高，略难消灭",
        "来自三层地狱，伤害较之前有所提高，较难消灭",
        "来自四层地狱，伤害提升比较大，消灭比较困难",
        "来自五层地狱，伤害比较高，很难消灭",
        "来自六层地狱，移动速度比较快，伤害较高，非常难消灭",
        "来自七层地狱，伤害特别高，特别难消灭"
    };
    #endregion
  
    #region 英雄的相关参数
    public static float rotateSpeed_Hero =8f;//英雄转体速度
    //英雄生命值
    public static int[]healthPoint_Hero=
    {                      
        3000,3500,4000,5000,
        5000,5000
    };
    //英雄的攻击速度
    public static float[] attackSpeed_Hero =
    {
        1.5f,1.5f,1.5f,1.5f,
        2f,2f
    };
    //英雄攻击力
    public static int[] damage_Hero =
    {
        50,60,70,80,
        90,100
    };
    //英雄移动速度
    public static float []moveSpeed_Hero =
    {              
        3f,3f,3f,3f,
        3f,3f                                                                                                    
    };
    //英雄的攻击范围
    public static float attackRange = 4f;
    //大招简介
    public static string[] skill =
    {
        "绝招名称：      无尽之力\n\n绝招范围：     7\n\n绝招杀伤力：       300",
        "绝招名称：      黄沙漫天\n\n绝招范围：     8\n\n绝招杀伤力：       400",
        "绝招名称：      毁天灭地世界\n\n绝招范围：     9\n\n绝招杀伤力：       500",
        "绝招名称：      奔走沙场\n\n绝招范围：     10\n\n绝招杀伤力：       600",
        "绝招名称：      龙战九天\n\n绝招范围：     11\n\n绝招杀伤力：       700",
        "绝招名称：      天使之翼\n\n绝招范围：     12\n\n绝招杀伤力：       800",
    };
    //英雄介绍
    public static string[] introduce =
    {
        "地狱战神是目前战争学院最多产的联赛战士。在加入联盟之前，地狱战神只是个平凡的雇佣士兵。地狱战神被放在了联盟裁决候选人名单的首位。",
        "在高耸入云的巨神峰上，有一个叫做拉阔尔的民族自古就在此生息繁衍，他们是一群拥有战士天赋的民族。族群中能力超群的人可以登上嘉岗坦山的顶峰，响应“神圣”召唤。这群人又叫做烈阳族，他们退出战争，将生命献给了崇敬的太阳。",
        "怨灵是萦绕于暗影岛上的一个最为可怖和可憎的灵体。被埋葬在古代铠甲中的冥魂之主据说是第一个不死灵体，甚至还是一个在暗影岛被改造前就存在的怨魂。他扭曲的灵魂，因他的苦痛和他对其他人施加的痛苦而增长着。",
        "羊灵的箭矢是为那些接受命运的人们所提供的一次迅速释放。狼灵则会捕猎那些想要从自身寿命终点逃离的人们，并在他的血盆大口内为他们献上暴力的结局。",
        "符文之地不乏英勇之人，但很少有人能和水火一样坚毅。带着自己神圣的武器，这个决绝的地狱法王已经花费了数不清的岁月来寻找那个难得的光明。",
        "这个英勇好战的种族在这块冻土之上已经经历了上千年的洗礼。他们的首领是一位暴怒的敌手，他可以召唤闪电的力量击打敌人，以使其畏惧。",
    };
    //英雄攻击力
    public static int[] fight =
    {
        10000,20000,30000,20000,
        7000,60000
    };
    //英雄评分
    public static int[] grade =
    {
        10,15,20,30,15,25      
    };
    //英雄星级
    public static int[] gradeTexture =
    {
        2,3,4,3,5,2
    };
    //英雄经验值
    public static  float[] exprience =
    {
        0.5f,0.2f,0.3f,0.6f,
        0.4f,0.5f
    };
    #endregion

    #region 塔的相关设置
    //塔的旋转速度
    public static float rotateSpeed_Tower=4f;
    //塔的攻击范围
    public static float[] attackRange_Tower =
    {
        //一级
        4f,4f,4f,4f,
        //中级
        5f,5f,5f,5f,
        //顶级
        6f,6f,6f,6f
    };
    //塔的攻击速度
    public static float[] attackSpeed_Tower =
    {
        1f,0.8f,1f,0.5f,
        1.2f,1f,1.5f,0.8f,
       1.4f,1.4f,1.4f,1f
    };
    //塔的伤害值
    public static int[] attackDamage_Tower =
    {
        40,50,40,40,
        50,60,50,50,
         60,70,60,60
    };
    //塔的价值
    public static int[] pursePrice_Tower =
    {
        70,70,80,90,
        120,130,140,150,
        180,180,190,200
    };  
    #endregion
}