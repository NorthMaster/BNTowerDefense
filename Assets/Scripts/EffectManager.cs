using UnityEngine;
using System.Collections;

public class EffectManager : MonoBehaviour {
    public static EffectManager effectmanager;//当前类的引用
    public GameObject[] effects;//四种技能的预制件
    public int[] damage;//四种技能伤害值
    public float[] attackRange;//四种技能的攻击范围
    public float[] waitDamage;//四种技能伤害值出现的等待时间
    public UIFirstHelp uifirsthelp;//实现游戏帮助功能的类的引用

    void Update() {

    }

    public void DamageAllEnemy(int index, Vector3 hitPoint) {//技能对怪物进行攻击，是怪物掉血
        //0按钮点击 1绿塔攻击；2红塔攻击；3箭塔攻击；4范围塔攻击 5-8技能攻击 9英雄攻击 10小兵攻击 11种塔 12卖塔 13出兵 14各种切换 15选中英雄 16摇色子 17选中 18切换上下
        AudioManager.PlayAudio(index + 5);//播放攻击声效
        GameObject go = effects[index];        //获取技能特效的引用
        Instantiate(go, hitPoint, Quaternion.identity);        //实现技能特效
        if (uifirsthelp != null) uifirsthelp.isEffectTip = true;//技能施放完成
        UIManager.uimanager.SetIsOn(index);        //放完技能将技能UI表示未选中状态
        for (int i = GameManager.game.EnemyList.Count - 1;i >= 0;i--) { //搜寻敌人，遍历敌人的集合
            if (CheckRange(hitPoint, GameManager.game.EnemyList[i].transform.position, attackRange[index])) {   //在攻击范围内的敌人受到攻击
                Enemy e = GameManager.game.EnemyList[i].GetComponent<Enemy>();                //获取受到攻击敌人的引用
                e.Be_Attack(damage[index]);//敌人受到攻击并掉血
            }
        }
    }

    bool CheckRange(Vector3 v1, Vector3 v2, float attackRange_) {//范围判定的方法
        if ((v1.x - v2.x) * (v1.x - v2.x) + (v1.z - v2.z) * (v1.z - v2.z) < attackRange_ * attackRange_) {//如果在范围内
            return true;//返回在范围内
        }
        return false;//返回不在范围内
    }
}
