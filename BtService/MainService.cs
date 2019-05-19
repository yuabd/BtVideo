using BtVideo.Models;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using BtVideo.Models.Site;
using System.Diagnostics;

namespace BtService
{
    partial class MainService : ServiceBase
    {
        private Timer weiboTimer;

        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
			btbtdyService.WritLog("开启服务");
            // TODO: 在此处添加代码以启动服务。
            weiboTimer = new Timer();
            weiboTimer.Interval = 60000.00;
            weiboTimer.Elapsed += new ElapsedEventHandler(btbtdyService.btbtdy_Elapsed);
            weiboTimer.Enabled = true;
        }

        protected override void OnStop()
        {
			btbtdyService.WritLog("停止服务");
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            weiboTimer.Enabled = false;
            weiboTimer.Stop();
        }
    }
}
