## LiveToNextLevel

### How to play:

##### · 移动控制: WASD 或者 ↑ ↓ ← →
##### · 开火: 鼠标左键
##### · 切换开火模式: X
##### · 使用道具: 暂无
##### · 子弹装填: R

#### 1.随机连通地图:
##### 洗牌算法(`Fisher Yates &amp`; `Knuth - Pursten Feld`),
地图与障碍物的随机生成,涉及FIFO,Lerp
1.洗牌原理,遍历目标列表,每次遍历随机一个索引元素与当前遍历索引交换位置
2.获取不重复随机对象,乱序列表转队列,队头出队再入队,保证取随机值的同时不会取到重复值,也保证了列表的完整性
##### 洪水填充算法
1.从地图中心点出发,四领域遍历每一个可以到达的格子,使用提前随机好的障碍物列表来限制不可到达的状态格子,绕过这些不可到达的格子,从而生成连通的地图

#### 2.武器系统,开火模式[自动,点射,单点],子弹装填功能,子弹后座力实现

#### 3.Player和Enemy类

#### 4.Bullet类,监听事件与伤害机制

#### 5.PopCanvasManager类,处理popCanvas层的UI逻辑,目前包含弹出浮动Tips的接口

#### 6.音频模块的添加,音频管理

#### 7.部分接口说明
##### <1>Physics.OverlapSphere方法
```csharp
Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
//说明:返回以参数1为原点和参数2为半径的球体内'满足条件'的碰撞体集合,该球体为3D相交球
//形参:position:3D相交球的球心
//radius:3D相交球的球半径
//layerMask 在某个Layer层上进行碰撞体检索,例如当前选中Player层,则只会返回周围半径内 Layer标示为Player的GameObject的碰撞体集合
//返回值:Collider[];//排好序的数据,索引越大说明目标碰撞体距离监测点越远
```    
##### <2>Rigidbody刚体的AddForce方法和AddTorque
```csharp
public void AddForce(Vector3 force, ForceMode mode = ForceMode.Force);
// 功能:对刚体施加一个直线方向的力 
// 参数介绍:
// force 决定力的方向和大小
// mode 决定作用力的模式,缺省方式为ForceMode.Force

public void AddTorque(Vector3 torque, ForceMode mode = ForceMode.Force);
// 功能:对刚体施加一个旋转力
// 参数介绍:
// torque 决定旋转力的大小和旋转轴的方向,旋转方向参照左手定则
// mode 决定作用力的模式,缺省方式为ForceMode.Force

// 参数ForceMode枚举类型
// 功能:力的作用方式 枚举类型,有四个枚举成员
// 计算公式:    Ft = mv(t) 即 v(t) = Ft/m
// (1)ForceMode.Force : 持续施加一个力,与重力mass有关,t = 每帧间隔时间,m = mass
// (2)ForceMode.Impulse : 瞬间施加一个力,与重力mass有关,t = 1.0f,m = mass
// (3)ForceMode.Acceleration:持续施加一个力,与重力mass无关,t = 每帧间隔时间,m = 1.0f
// (4)ForceMode.VelocityChange:瞬间施加一个力,与重力mass无关,t = 1.0f,m = 1.0f

```    
##### <3>Random.insideUnitSphere 返回单位球内一个随机点(only read)
##### <4>Cursor.visible = true; 设置光标是否可见
##### <5> 颜色的转换:
```csharp
ColorUtility.ToHtmlStringRGB(color);//将color对象转换成十六进制
Color nowColor;
ColorUtility.TryParseHtmlString("#FECEE1", out nowColor);//将十六进制字符串转换为color对象

```    

#### 8.组件
##### <1>Trail Renderer(轨迹渲染器)组件 用来做轨迹跟踪,渲染轨迹弹道

#### 9.机制:
##### <1>类挂机检测:玩家长时间在某一处静止不动,或者攻击时持续逗留在某地超过一定时间被检测到,在玩家附近生成敌人,增加紧张感
##### <2>武器系统:增加开火模式,点射,自动,单点三种模式
##### <3>双开火点,两个枪口都可以发射子弹

#### 10.问题:
##### <1>:获取物体的Renderer组件的material.color属性的Alpha通道值来改变预制体的颜色透明度,必须将material的(Renderring Mode)渲染模式修改为Fade模式
##### <2>:VsCode无法自动引用脚本命名空间导致编译器报错的解决方案,工程中删掉`Assembly-CSharp.csproj`文件,Unity中首选项中将编译器设置为Visual Studio,重新生成所有的csproj文件(Regenerate all csproj files),再将默认编译器切换为VsCode
##### <3>:Trail Renderer颜色的设置,Color属性调节渐变和颜色,startColor和endColor属性可以设置渐变色值
##### <4>:在敌人开始生成的位置,地图瓦片闪烁期间,在瓦片坐标击杀其他敌人,导致瓦片颜色为红色消除不掉的问题:在相同位置连续生成敌人,瓦片颜色置为oriColor,但是oriColor赋值为tile.color,此颜色不一定为真正的初始色,需要修改为Color.white
##### <5>:UI等响应鼠标事件,UI Canvas底下必须要有EventSystem
##### <6>:暂停游戏的TimeScale,需要解决连同界面UI动画一并暂停的问题
##### <7>:DEVELOP MODE的接入问题
##### <8>:射线:(相机指向鼠标的屏幕坐标)与虚拟底板的交点判定为枪口开火指向的射线,该射线不一定为水平的,若方向与地板夹角过大,则会造成子弹射程过短,无法有效命中敌人的问题,解决方案:调整相机视角为第一人称,计算枪口的开火方向时忽略竖直方向上的影响,使子弹轨迹水平,保证有效射击,个人更偏向第一种解决方案
### 11.DEVELOP MODE 开发者模式键位说明

##### · Enter 跳过当前波的进度

### 12.UIPanel的生命周期流程图
![image](https://github.com/RECNEPS-KIPS/LiveToNextLevel/blob/main/Assets/UIFramework/Pic/UIPanel_Life_Cycle_Flowchart.png)

### 13.UIFramework UI框架的说明
##### 主要功能:负责处理UI界面的相关模块,管理场景中的UI面板,控制面板之间的跳转逻辑,将UI的管理从GameManager中剥离出来,实现模块之间的解耦合以及界面之间的数据通信更加简洁明了便于管理
#### UIManager的相关说明:
##### 功能说明:
##### <1>加载和显示隐藏关闭界面,根据面板唯一ID标识获得界面实例
##### <2>提供界面显隐的必要接口以及一些界面缓动动画
##### <3>界面层级,背景管理
##### <4>根据存储的导航信息完成界面导航,跳转功能
##### <5>界面通用对话框处理
##### <6>便于对需求的实现和功能的扩展

#### UI框架实现流程:
##### <1>:界面Build:将UI预制体制作好后放入Resources文件夹,使用`BuildPanelToJsonData`脚本将UI预制体的相关数据存储到`UIPanelData.json`中
##### <2>:通过UIManager的`SavePanelInfoInDictByID()`方法将界面的信息存储到字典中,便于后续加载和访问界面数据
##### <3>:构建存储当前显示界面的栈,将当前需要显示的界面入栈,关闭界面时进行出栈,即栈顶元素始终为当前显示的界面
##### <4>:BasePanel:所有面板的基类,提供界面生命周期的流程接口,由业务界面进行重写
##### <5>:UILauncher:负责启动UI框架