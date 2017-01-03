using UnityEngine;
using System.Collections;
using UnityEngine.UI;//引用系统包

public class Loading : MonoBehaviour {//声明类名
  public Sprite[] loadingTextures;//异步加载时实现帧动画的图片组
  public Image loadingImage;//切换场景时图片载体
  public Image curtain;//黑色背景图
  public Text progess;//显示加载进度的文本框
  AsyncOperation asyn;//跟踪异步操作的生存期变量
  bool isStart = true;//是否开始显示
  [HideInInspector]//公有变量在Inspector面板中不显示
  public bool isAsyn = false;//是否开始异步加载
  [HideInInspector]
  public int scene_index = -1;//当前场景索引
  string[] sceneName = { "scene1", "scene2", "scene3", "main", "attribute", "selectscene", "welcome" };//场景名称数组
  float alpha = 1;//背景图片透明度
  float curTexture = 0;//用于帧动画的图片
  int curIndex = 0;//当前索引值
  void Start() {//进入游戏是调用的方法
    scene_index = -1;//场景索引值为-1
    isStart = true;//开始场景
    alpha = 1;//设置背景图片为不透明
    isAsyn = false;//不开始异步加载
    loadingImage.gameObject.SetActive(false);//场景切换时的帧动画图片不显示
  }
  void Update() {//每帧调用的方法
    if (isStart) {//进入场景
      StartScene();//开始场景的方法
    }
    ChangeTexture();//改变帧动画图片的方法
    if (isAsyn) {//开始异步加载
      ChangeScene();//切换场景的方法
    }
    if (alpha >= 1) {//如果背景图片的透明度大于1
      if (asyn == null) return;//如果跟踪异步操作的生存期不存在，返回
      loadingImage.gameObject.SetActive(true);//显示帧动画
      progess.text = (int)(asyn.progress * 100) + "" + "%";//显示加载进度
    }
  }
  public void LoadScene(int index) {//外部调用的方法，传进来要切换场景的索引值
    isAsyn = true;//开始异步加载的标志
    scene_index = index;//要切换到的场景的索引
  }
  void StartScene() {//开始场景的方法
    alpha -= 0.08f;//透明度依次递减，黑色背景图片，由有到无
    curtain.gameObject.GetComponent<CanvasRenderer>().SetAlpha(alpha);//改变背景图片的透明度
    if (alpha <= 0) {//如果透明度的值小于0
      isStart = false;//进入场景结束
    }
  }
  void ChangeScene() {//切换场景的方法
    alpha += 0.08f;//索引值由0开始增加
    curtain.gameObject.GetComponent<CanvasRenderer>().SetAlpha(alpha);//改变背景图片的透明度
    if (alpha >= 1) {//如果透明度的值大于1
      if (scene_index == -1) return;// 如果没有进行场景的切换，则返回
      StartCoroutine(LoadingScene());//开启协程方法，实现场景的异步加载
      isAsyn = false;//协程方法执行完毕，异步加载结束
    }
  }
  IEnumerator LoadingScene() {//协程方法，用来实现场景的异步加载
    asyn = Application.LoadLevelAsync(sceneName[scene_index]);//对跟踪异步操作的生存期变量赋值
    yield return asyn;//返回此变量到方法中
  }
  void ChangeTexture() {//切换场景帧动画的实现
    if (!loadingImage.gameObject.activeInHierarchy) return;//如果当前的图片载体没有被禁用
    curTexture += Time.deltaTime * 8;//当前图片浮点型索引值增加
    curIndex = (int)curTexture;//当前图片索引值
    loadingImage.sprite = loadingTextures[curIndex % 5];//切换图片
  }
}
