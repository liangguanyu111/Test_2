# Test_2
第二次笔试

![image](https://github.com/liangguanyu111/Test_2/blob/master/ff5f4c9e54539a4c084ab5c70cdeca0.png)
第二次修改:
1.简单的任务系统UI演示，UI显示通过回调和OnStatusChange配合
2.将Data类中的event全部修改为OnStatusChange，当数据状态发生变化就Invoke，DataRoot类根据具体的状态判断逻辑。
3.移除了多余的的监听，主要是已经完成的任务，刷新时把监听添加回来
4.新增saveMgr管理数据保存，可以定时保存，也可以主动保存，主动保存发生在数据状态变化时。

第一次修改: 1.删减了数据状态的多余字段，增加任务状态/签到状态枚举表示任务的状态， DataStatus包括NotFinish，Finished，Get三种状态更加符合UI状态，签到包括NotSign，CanSign,CanResign，Signed 2.修改了ConfigRoot中的LoadData，返回类型改为字典 3.修改了TimeMgr中的时间检测，直接改为协程触发时间检测 4.删除SignInData中的SignInTime字段，添加SignInData数据类用来记录每月累计的数据，在SignInDataRoot中加载。 5.修改Save逻辑，在ApplicationQuit中保存，也可以主动保存。
