using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class attribute : MonoBehaviour {
  public Button exit;
  public GameObject aduioPrefab;//声音控制的预制件
  private GameObject a_clone;//无聊引用
  void Start() {
    QualitySettings.shadowDistance = 90;//将影子长度设为90，如果这个值太小手机上不会显示人物影子
    exit.onClick.AddListener(BackScene);//对退出当前界面按钮注册监听
    if (!welcome.isExitAudio) {//如果当前没有声音，实例化一个声音控制器
      a_clone = Instantiate(aduioPrefab) as GameObject;
      DontDestroyOnLoad(a_clone);
      welcome.isExitAudio = true;
    }
  }

  // Update is called once per frame
  void Update() {

  }

  void BackScene() {//退出当前场景
    AudioManager.PlayAudio(0);//按钮点击的声效
    QualitySettings.shadowDistance = 40;//影子距离下调
    GetComponent<Loading>().LoadScene(3);//异步加载切换场景
  }
}
