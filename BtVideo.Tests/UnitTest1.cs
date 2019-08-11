using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BtService;
using System.Text.RegularExpressions;

namespace BtVideo.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
			btbtdyService.btbtdy_Elapsed(null, null);
		}

		[TestMethod]
		public void TestMethod2()
		{
			var content = "<li><a title=\"2017引爆者HD1080P中字\" href=\"/down/11880-0-0.html\" target=\"_blank\" class=\"ico_1\">2017引爆者HD1080P中字<span class=\"bt\">详情</span></a><span><a class=\"d1\" href=\"magnet:?xt=urn:btih:2DECUA32YEWYP2A6IBST34VKXVILWQVS\">磁力</a></span></li>";

			var me = @"<li><a title=""(?<name>.*?)"" href=""(?<url>.*?)"" target=""_blank"" class=""ico_1"">(?<name>.*?)<span class=""bt"">详情</span></a><span><a class=""d1"" href=""(?<magnet>.*?)"">磁力</a></span></li>";

			var cols = Regex.Matches(content, me);

			foreach (Match item in cols)
			{
				Console.WriteLine(item.Groups["name"].ToString());
			}
		}
    }
}
