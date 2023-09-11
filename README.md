# 埃尔迪亚 游戏DEMO
## 个人独立开发
_ 灵感来源 浮岛物语、星露谷物语 _

## 按键说明
**【W】【S】【A】【D】**：角色上下左右移动；  
**【Shift】+【W】【S】【A】【D】**：角色慢走；  
**【E】**：与场景中箱子、床的交互；  
**【Q】**：跳过游戏开场动画；  
**【F】**：游戏开场动画加速；  
**【数字1-5】**：选择角色物品栏具体物品；  
**【空格】**：与场景中NPC的交互、对话栏的交互；  
**【鼠标左键】**：与场景、物品交互；  
**【鼠标右键】**：物品栏卖出物品；  

> 另外测试用：  
**【T】**：时间加速；  
**【G】**：时间增加一天；  
**【I】**：存档；  
**【O】**：读取当前存档；  

## 现有功能

**开场动画**：玩家开启新游戏时会播放游戏背景介绍和相关动画，此处可快进或跳过；  
**场景互动**：玩家使用具体工具与场景中的树木、树桩和石头进行交互、获得材料；  
**种植系统**：使用工具可在地上进行庄稼的种植和采摘；  
**背包系统**：玩家可利用鼠标对于角色背包和场景中储物箱进行容量交换。  
**商城系统**：玩家点击商城中具体物品，选择或自定义数量进行购买。玩家右键选择背包中物品进行出售；  
**建造系统**：玩家在商城购买对应图纸，根据所需原材料数量进行对应物品的建造；  
**NPC系统**：在设置NPC的自动寻路、对应时间的事件触发、与人物的交互；  
**灯光系统**：设置全局光和对应点光源，特定时间进行变化；夜晚玩家可建造床，进行休息，进入下一天；  
**音量系统**：设置游戏不同场景的背景音乐和音效，可分别设置特定音乐和音效的大小；  
**进度保存读取**：退出游戏时自动保存游戏进度；游戏可新建三个游戏进度；  

## 项目亮点

1.利用_对象池_管理音效、拾取的物品，避免频繁地创建和销毁对象带来内存泄漏的风险；  
2.NPC的自动寻路采用了_A星_算法，实现场景之间定点的最短路径查找；  
3.游戏开发中大量使用事件的控制和管理，通过_Eventhandler_单例类管理所有事件的呼叫和实现。  
4.游戏使用多个协程，实现特定功能的同步；  
5.使用_Unity Timeline_实现游戏开启时的动画录制，以及对应人物的聊天clip；  
6.使用_Audio Mixer_实现管理不同音轨的播放和切换；  
7.使用_URP2D_全局光和对应光源设置游戏内灯光；  
8.设计模式采用单例模式、观察者模式、MVC设计模式；  
9.开发中也使用了Unity的_UGUI_对于各种UI界面进行设计和布局，此外使用_Dotween_实现简易的动画效果，实现对应信息随着游戏进度的实时更新。  
10.在存储数据的模块中，使用Unity插件_Newtonsoft-Json_进行数据的序列化和保存，在读取时再进行反序列化交给各个模块的管理类进行初始游戏数据。  

## 项目收获

1.了解一个功能从提出到完成发布的完整过程，对于Unity的一些插件和组件有了更深入的了解，包括但不限于Events、UGUI;  
2.在反复的试错中，实现了游戏一些常见系统，比如交易系统、背包系统、建造系统、与NPC的互动等，提高个人的代码水平和debug能力；  

## 演示视频

**B站**:https://www.bilibili.com/video/BV1fh4y1P77C  

## 演示图片
1.![开始界面](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/%E5%BC%80%E5%A7%8B%E7%95%8C%E9%9D%A2.png)  
2.![图片1](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/1.png)  
3.![图片2](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/2.png)  
4.![图片3](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/4.png)
5.![图片4](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/5.png)
6.![图片5](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/6.png)
7.![图片6](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/7.png)
8.![图片7](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/9.png)
9.![图片8](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/10.png)
10.![图片9](https://github.com/zainlt/Eldia_UnityDemo/blob/main/Image_Introduce/11.png)
