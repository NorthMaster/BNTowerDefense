using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
  private Transform thisT;//当前脚本所在对象的位置引用
  public float space_move = 0.001f;//摄像机移动步距
  public float space_zoom = 0.003f;//摄像机缩放步距
  public float limit_x_left;    //摄像机移动左限制
  public float limit_x_right;//摄像机移动右限制
  public float limit_z_down;//摄像机移动上限制
  public float limit_z_up;//摄像机移动下限制
  public float near; //摄像机近限制
  public float far; //摄像机远限制
  float vx = 0f;    //速度x轴分量
  float vy = 0f; //速度y轴分量
  float vz = 0f; //速度z轴分量
  bool start_zoom = false;  //摄像机缩放标志位
  bool start_move = false;//摄像机移动的标志位
  float zoom_depth = 0f;    //摄像机缩放深度
  private float temp1; //x轴临时存储变量
  private float temp2;//z轴临时存储变量
  private float length = 0;
  void Start() {
    thisT = this.transform;//获取脚本所在对象位置的引用
    //Application.targetFrameRate = 40;
  }
  void Update() {//每帧调用的方法
    CameraMove();//摄像机缩放或者移动的方法
  }
  void CameraMove() {//摄像机缩放或者移动的方法
    if (Input.touchCount == 1) {          //如果触控点的个数为1
      start_move = false;//置未开始移动标志位
      Touch[] touch = Input.touches;//获取触控点移动信息
      if (touch[0].phase == TouchPhase.Moved) {//如果与屏幕接触的第一个触控点开始移动
        UIManager.uimanager.Tower_UI_Disappear();     //添加的ui消失的方法
        start_move = true;                //设置开始移动的标志量
        vx = touch[0].deltaPosition.x * 300;//移动后x轴的冲量
        vz = touch[0].deltaPosition.y * 300;//移动后z轴的冲量
        temp1 = thisT.position.x - touch[0].deltaPosition.x * space_move; //x轴移动(后边的移动对应的是手指在屏幕上滑动)
        temp2 = thisT.position.z - touch[0].deltaPosition.y * space_move;//z轴移动
        ChangePosition(temp1, temp2);//改变摄像机位置的方法
      }
    }
    else if (Input.touchCount > 1) {//如果触控点的个数大于1
      start_zoom = false;//置未开始缩放标志位
      Touch[] touch = Input.touches;//获取触控点移动信息
      float length_temp = 0f;            //临时记录最先接触手指间距离
      if (touch[0].phase == TouchPhase.Began || touch[1].phase == TouchPhase.Began) { //最开始触碰到屏幕的两个触控点未开始移动        
        zoom_depth = Distance(touch[0].position.x, touch[0].position.y, touch[1].position.x, touch[1].position.y);//记录最先接触屏幕的两个手指的距离
      }
      if (touch[0].phase == TouchPhase.Moved || touch[1].phase == TouchPhase.Moved) {//最开始触碰到屏幕的两个触控点开始移动    
        UIManager.uimanager.Tower_UI_Disappear();               //添加的ui消失的方法
        length_temp = Distance(touch[0].position.x, touch[0].position.y, touch[1].position.x, touch[1].position.y);//记录最先接触屏幕的两个手指的当前距离
        ChangePosition((zoom_depth - length_temp) * space_zoom);//改变摄像机位置的方法，缩放
        start_zoom = true;//开始缩放
      }
    }
    SlowSlide();//平滑移动的方法
  }
  void ChangePosition(float limit_x_temp, float limit_z_temp) {//改变摄像机位置的方法，移动
    if (limit_x_temp < limit_x_left) {//超出滑动范围，左边界锁死
      limit_x_temp = limit_x_left;//对x轴的临时存储变量给出固定值
    }
    else if (limit_x_temp > limit_x_right) {//超出滑动范围，右边界锁死
      limit_x_temp = limit_x_right;//对x轴的临时存储变量给出固定值
    }
    if (limit_z_temp < limit_z_down) { //超出滑动范围，下边界锁死
      limit_z_temp = limit_z_down;//对y轴的临时存储变量给出固定值
    }
    else if (limit_z_temp > limit_z_up) { //超出滑动范围，上边界锁死
      limit_z_temp = limit_z_up;//对y轴的临时存储变量给出固定值
    }
    thisT.position = new Vector3(limit_x_temp, thisT.position.y, limit_z_temp);//改变摄像机的位置
  }

  float Distance(float x1, float y1, float x2, float y2) { //计算两点间距离的方法
    return Mathf.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));//x的平方-y的平方，再开根号
  }
  void ChangePosition(float zoom_length) {//改变摄像机位置的方法，缩放
    if (thisT.position.y + zoom_length > far) {//超出缩放范围，最远距离锁死
      vy = 0;//y轴速度冲量为0
      length = far;//当前位置为最远位置
    }
    else if (thisT.position.y + zoom_length < near) {//超出缩放范围，最近距离锁死
      vy = 0;//y轴速度冲量为0
      length = near;//当前位置为最近位置
    }
    else {//未超出缩放范围
      length = thisT.position.y + zoom_length;//时时缩放滑动长度
      vy = zoom_length;//获取y轴速度冲量
    }
    limit_x_left += (length - thisT.position.y) * 0.9f;        //摄像机缩进以后要改变滑动的时锁死x轴的边界
    limit_x_right -= (length - thisT.position.y) * 0.9f;        //摄像机缩进以后要改变滑动的时锁死y轴的边界
    limit_z_down += (length - thisT.position.y) * 0.0001f;        //摄像机缩进以后要改变滑动的时锁死z轴的边界
    limit_z_up -= (length - thisT.position.y) * 0.9f;        //摄像机缩进以后要改变滑动的时锁死z轴的边界
    temp1 = thisT.position.x;//缓存x轴的位置信息
    temp2 = thisT.position.z;//缓存y轴的位置信息
    if (temp1 < limit_x_left) { //x轴左边界锁死
      temp1 = limit_x_left;//记录x轴最左滑动位置
    }
    else if (temp1 > limit_x_right) {//x轴右边界锁死
      temp1 = limit_x_right;//记录x轴最右滑动位置
    }
    if (temp2 < limit_z_down) {//z轴下边界锁死
      temp2 = limit_z_down;//记录z轴最下滑动位置
    }
    else if (temp2 > limit_z_up) {//z轴上边界锁死
      temp2 = limit_z_up;//记录z轴最上滑动位置
    }
    thisT.localPosition = new Vector3(temp1, length, temp2);        //更新摄像机位置
  }
  void SlowSlide() { //实现摄像机缓慢滑动的方法
    if (start_zoom == false && vy != 0) {//如果没有开始缩放，y轴存在速度分量
      vy *= 0.1f;//y轴速度分量递减
      ChangePosition(vy);//改变摄像机位置，实现缓动
      if (Mathf.Abs(vy) < 0.01) {//如果y轴速度分量小于阈值
        vy = 0;//速度分量为0
      }
    }
    if (start_move == false && (vz != 0 || vx != 0)) {//如果没有开始滑动，x轴存在速度分量或者z轴存在速度分量
      vx = vx * 0.06f;//x轴速度分量递减
      vz = vz * 0.06f;//z轴速度分量递减
      temp1 = thisT.position.x - (vx * space_move * 0.01f);//记录摄像机x轴位置
      temp2 = thisT.position.z - (vz * space_move * 0.01f);//记录摄像机z轴位置
      ChangePosition(temp1, temp2);//改变摄像机的位置
      if (Mathf.Abs(vx) < 0.1f) {//如果x轴速度分量小于阈值
        vx = 0;//x轴速度分量为0
      }
      if (Mathf.Abs(vz) < 0.1f) {//如果z轴速度分量小于阈值
        vz = 0;//z轴速度分量为0
      }
    }
  }
}
