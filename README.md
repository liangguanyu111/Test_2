# Test_2
第二次笔试


第一次修改 1.删减了数据状态的多余字段，增加任务状态/签到状态枚举表示任务的状态， DataStatus包括NotFinish，Finished，Get三种状态更加符合UI状态，签到包括NotSign，CanSign,CanResign，Signed 2.修改了ConfigRoot中的LoadData，返回类型改为字典 3.修改了TimeMgr中的时间检测，直接改为协程触发时间检测 4.删除SignInData中的SignInTime字段，添加SignInData数据类用来记录每月累计的数据，在SignInDataRoot中加载。 5.修改Save逻辑，在ApplicationQuit中保存，也可以主动保存。