using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets._Game.Script
{
    public enum UpdaterState
    {
        CheckUpdate,            //检测更新
        GetServerVersionFail,   //获取服务器版本失败
        DownLoading,            //下载中
        DownLoadFail,           //下载失败
        Complete                //更新完成
    }
    public class Updater
    {
        
        
    }
}
