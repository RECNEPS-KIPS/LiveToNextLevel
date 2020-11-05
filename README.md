#### LiveToNextLevel
##### 1.随机连通地图:
###### 洗牌算法(Fisher Yates &amp; Knuth - Pursten Feld),
地图与障碍物的随机生成,涉及FIFO,Lerp
1.洗牌原理,遍历目标列表,每次遍历随机一个索引元素与当前遍历索引交换位置
2.获取不重复随机对象,乱序列表转队列,队头出队再入队,保证取随机值的同时不会取到重复值,也保证了列表的完整性
###### 洪水填充算法
1.从地图中心点出发,四领域遍历每一个可以到达的格子,使用提前随机好的障碍物列表来限制不可到达的状态格子,绕过这些不可到达的格子,从而生成连通的地图
##### 2.武器系统
##### 3.Player和Enemy类
##### 4.Bullet类,监听事件与伤害机制
##### 5.部分接口说明
###### <1>Physics.OverlapSphere方法
```csharp
Physics.OverlapSphere(transform.position, 0.1f, collisionMask);
//说明:返回以参数1为原点和参数2为半径的球体内'满足条件'的碰撞体集合,该球体为3D相交球
//形参:position:3D相交球的球心
//radius:3D相交球的球半径
//layerMask 在某个Layer层上进行碰撞体检索,例如当前选中Player层,则只会返回周围半径内 Layer标示为Player的GameObject的碰撞体集合
//返回值:Collider[];//排好序的数据,索引越大说明目标碰撞体距离监测点越远
```    
###### <2>Rigidbody刚体的AddForce方法和AddTorque
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
###### <3>Random.insideUnitSphere 返回单位球内一个随机点(only read)
##### 机制:
###### <1>类挂机检测:玩家长时间在某一处静止不动,或者攻击时持续逗留在某地超过一定时间被检测到,在玩家附近生成敌人,增加紧张感
