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
            WritLog("开启服务");
            // TODO: 在此处添加代码以启动服务。
            weiboTimer = new Timer();
            weiboTimer.Interval = 60000.00;
            weiboTimer.Elapsed += new ElapsedEventHandler(btbtdy_Elapsed);
            weiboTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            WritLog("停止服务");
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            weiboTimer.Enabled = false;
            weiboTimer.Stop();
        }

        private void btbtdy_Elapsed(object sender, ElapsedEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            using (SiteDataContext db = new SiteDataContext())
            {
                for (int i = 0; i < 10; i++)
                {
                    var task = db.SpiderTasks.FirstOrDefault(m => m.SpiderTaskID == 1);

                    try
                    {
                        var url = "http://www.btbtdy.com/btdy/dy" + task.CurrentID + ".html";
                        btbtdy b = new btbtdy(url);

                        bool err = false;

                        var blog = b.GetMovie(out err);

                        if (err)
                        {
                            task.CurrentID += 1;
                            task.UpdateDate = DateTime.Now;

                            db.SaveChanges();

                            return;
                        }

                        if (string.IsNullOrEmpty(blog.PageTitle))
                        {
                            blog.PageTitle = blog.MovieTitle;
                        }

                        try
                        {
                            WebClient client = new WebClient();
                            byte[] bytes = client.DownloadData(new Uri("http://www.btbtdy.com/include/ajax.php?id=" + task.CurrentID + "&action=videoscore&timestamp=1497498303965"));
                            StringBuilder sb = new StringBuilder();
                            using (MemoryStream ms = new MemoryStream(bytes))
                            {
                                ms.Seek(0, SeekOrigin.Begin);

                                var s = System.Text.Encoding.Default.GetString(ms.ToArray()).Split(',');

                                var point = (double.Parse(s[2]) / double.Parse(s[3]));
                                blog.Grade = point > 0 ? point : 0;
                            }
                        }
                        catch (Exception)
                        {
                        }

                        db.Movies.Add(blog);

                        db.SaveChanges();

                        try
                        {
                            if (!string.IsNullOrWhiteSpace(blog.PictureFile))
                            {
                                WebClient client = new WebClient();
                                byte[] bytes = client.DownloadData(new Uri(blog.PictureFile));
                                blog.PictureFile = DateTime.Now.ToString("yyyyMM") + "/" + blog.MovieID + ".jpg";
                                using (MemoryStream ms = new MemoryStream(bytes))
                                {
                                    ms.Seek(0, SeekOrigin.Begin);
                                    ms.WriteTo(new FileStream("D:/btmovie/Content/Pictures/Blog/" + blog.PictureFile, FileMode.OpenOrCreate));
                                }

                                db.SaveChanges();
                            }
                        }
                        catch (Exception)
                        {
                        }

                        //加入明星
                        MovieStar star = null;
                        MovieStarJoin starJoin = null;
                        foreach (var item in blog.Stars.Split('|'))
                        {
                            if (!string.IsNullOrWhiteSpace(item))
                            {
                                star = db.MovieStars.Where(m => m.StarName == item).FirstOrDefault();

                                if (star == null)
                                {
                                    star = new MovieStar()
                                    {
                                        StarName = item
                                    };

                                    db.MovieStars.Add(star);

                                    db.SaveChanges();
                                }

                                starJoin = new MovieStarJoin()
                                {
                                    MovieID = blog.MovieID,
                                    StarID = star.StarID,
                                    Type = StarType.actor
                                };

                                db.MovieStarJoins.Add(starJoin);

                                db.SaveChanges();
                            }
                        }

                        // add slug after (depends on ID)
                        blog.Slug = blog.MovieID.ToString();
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        WritLog(ex.Message);
                    }
                    finally
                    {
                        task.CurrentID += 1;
                        task.UpdateDate = DateTime.Now;

                        db.SaveChanges();
                    }
                }

                sw.Stop();
                WritLog("本次抓取总共花费{0}ms." + sw.Elapsed.TotalMilliseconds);
            }
        }

        private static void WritLog(string cont)
        {
            try
            {
                string text = AppDomain.CurrentDomain.BaseDirectory + "log";
                if (!Directory.Exists(text))
                {
                    Directory.CreateDirectory(text);
                }
                using (StreamWriter streamWriter = new StreamWriter(text + "\\" + DateTime.Today.ToString("yyMMdd") + ".txt", true))
                {
                    streamWriter.WriteLine(DateTime.Now.ToString("yyyy-M-d HH:mm:ss") + ": " + cont);
                    streamWriter.Close();
                }
            }
            catch
            {
            }
        }
    }
}
