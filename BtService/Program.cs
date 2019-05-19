using BtService;
using BtVideo.Models;
using BtVideo.Models.Site;
using BtVideo.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BtService
{
    class Program
    {
        //private BlogService blogService = new BlogService();
        static void Main(string[] args)
		{
			//btbtdyService.btbtdy_Elapsed(null, null);
			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new MainService()
			};
			ServiceBase.Run(ServicesToRun);
		}
    }
}
