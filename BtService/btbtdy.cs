using BtService.HttpHelper;
using BtVideo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BtService
{
    public class btbtdy : HtmlHttp
    {
        public btbtdy(string url)
        {
            base.Url = url;
        }

        //public List<WebLink> GetLinks(out bool Err)
        //{
        //    base.GetHtmlCont();

        //    if (string.IsNullOrEmpty(base.HtmlCont))
        //    {
        //        Err = true;
        //        return new List<WebLink>();
        //    }

        //    List<WebLink> list = new List<WebLink>();

        //    MatchCollection matchCollection = Regex.Matches(base.HtmlCont, "/btdy/dy[\\w\\W]*?.html");

        //    foreach (var item in matchCollection)
        //    {
        //        //list.Add(new WebLink()
        //        //{
        //        //    Dt = DateTime.Now,
        //        //    Guid = Guid.NewGuid().ToString(),
        //        //    IsGeted = false,
        //        //    Url = "http://www.btbtdy.com" + item.ToString()
        //        //});
        //    }

        //    Err = false;
        //    return list;
        //}

        public Movie GetMovie(out bool Err)
        {
            base.GetHtmlCont();

            if (string.IsNullOrEmpty(base.HtmlCont) || base.HtmlCont.Contains("该影片不存在"))
            {
                Err = true;
                return null;
            }

            Movie model = new Movie()
            {
                AuthorID = "admin",
                CommentCount = 0,
                DateCreated = DateTime.Now,
                Grade = 0,
                Director = "",
                AreaID = 0,
                ImdbLink = "",
                ImdbTitle = "",
                IsPublic = true,
                LikeCount = 0,
                Magnet = "",
                MetaDescription = "",
                MetaKeywords = "",
                MovieContent = "",
                MovieTitle = "",
                Stars = "",
                ShowDate = DateTime.Now,
                SortOrder = 0,
                PageVisits = 0,
                DateUpdate = DateTime.Now
            };

            using (SiteDataContext db = new SiteDataContext())
            {
                string value = "";

                //标题
                var title = @"<div class=""vod_intro rt""><h1>(?<title>.*?)<span class=""year"">((?<year>.*?))</span></h1>";
                Regex r = new Regex(title, RegexOptions.None);
                Match match = r.Match(base.HtmlCont);

                if (match.Success)
                {
                    model.MovieTitle = match.Groups["title"].ToString();
                    model.ShowDate = new DateTime(int.Parse(match.Groups["year"].ToString().Trim().Trim('(').Trim(')')), 1, 1, 0, 0, 0);
                    if (db.Movies.Any(m => m.MovieTitle == model.MovieTitle))
                    {
                        Err = true;
                        return null;
                    }

                    model.MovieTags = new List<MovieTag>();

                    model.MovieTags.Add(new MovieTag()
                    {
                        Tag = model.MovieTitle + "720p"
                    });
                    model.MetaKeywords += model.MovieTitle + "720p,";

                    model.MovieTags.Add(new MovieTag()
                    {
                        Tag = model.MovieTitle + "迅雷下载"
                    });
                    model.MetaKeywords += model.MovieTitle + "迅雷下载,";

                    model.MovieTags.Add(new MovieTag()
                    {
                        Tag = model.MovieTitle + "磁力链接"
                    });
                    model.MetaKeywords += model.MovieTitle + "磁力链接,";

                    model.MovieTags.Add(new MovieTag()
                    {
                        Tag = model.MovieTitle + "BT种子下载"
                    });
                    model.MetaKeywords += model.MovieTitle + "BT种子下载,";

                    model.MovieTags.Add(new MovieTag()
                    {
                        Tag = model.MovieTitle + "1080p"
                    });
                    model.MetaKeywords += model.MovieTitle + "1080p";
                }

                //评分
                //string reg = @"<input type=""hidden"" id=""MARK_B2"" name=""MARK_B2"" value=""(?<value>.*?)""[\s\S]*?>";
                //r = new Regex(reg, RegexOptions.None);
                //match = r.Match(base.HtmlCont);
                //if (match.Success)
                //{
                //    value = match.Groups["value"].ToString();
                //    model.Grade = double.Parse(value);
                //}

                //地区
                string area = @"<dt>地区：</dt><dd><a href=""/screen/0---(?<area>.*?)--time-1.html"">(?<area>.*?)</a>";
                r = new Regex(area, RegexOptions.None);
                match = r.Match(base.HtmlCont);

                if (match.Success)
                {
                    value = match.Groups["area"].ToString();
                    var areaS = db.MovieAreas.Where(m => m.AreaName == value).FirstOrDefault();
                    model.AreaID = areaS == null ? 0 : areaS.AreaID;
                }

                //类型
                string type = @"<dt>类型：</dt><dd>(?<type>.*?)</dd><dt>地区";
                r = new Regex(type, RegexOptions.None);
                match = r.Match(base.HtmlCont);
                MatchCollection cols = null;
                if (match.Success)
                {
                    value = match.Groups["type"].ToString();

                    type = @"<a(?<url>.*?)>(?<type1>.*?)</a>";
                    cols = Regex.Matches(value, type);

                    model.MovieCategoryJoins = new List<MovieCategoryJoin>();
                    foreach (Match item in cols)
                    {
                        string ty = item.Groups["type1"].ToString();

                        var category = db.MovieCategories.Where(m => m.CategoryName == ty).FirstOrDefault();
                        if (category != null)
                        {
                            model.MovieCategoryJoins.Add(new MovieCategoryJoin()
                            {
                                CategoryID = category.CategoryID
                            });
                        }
                    }
                }

                //演员
                string action = @"<a href='/search/(?<action>.*?).html'>(?<action>.*?)</a>&nbsp;&nbsp;";

                cols = Regex.Matches(base.HtmlCont, action);

                foreach (Match item in cols)
                {
                    value = item.Groups["action"].ToString();

                    if (value != "T.")
                    {
                        model.Stars += value + "|";
                    }
                }

                model.Stars = model.Stars.Trim('|');

                //图片
                string pic = @"<div class=""vod""><div class=""vod_img lf""><img src=""(?<url>.*?)"" alt=""(?<alt>.*?)""/></div><div class=""vod_intro rt"">";
                r = new Regex(pic, RegexOptions.None);
                match = r.Match(base.HtmlCont);

                if (match.Success)
                {
                    value = match.Groups["url"].ToString();

                    model.PictureFile = value;
                }

                //ImdbLink
                var imdb = @"<dt>imdb：</dt><dd><a href=""(?<url>.*?)"" target=""_blank"" rel=""nofollow"">(?<name>.*?)</a></dd>";
                r = new Regex(imdb, RegexOptions.None);
                match = r.Match(base.HtmlCont);

                if (match.Success)
                {
                    model.ImdbLink = match.Groups["url"].ToString();
                    model.ImdbTitle = match.Groups["name"].ToString();
                }

                //磁力
                var me = @"<li><a title=""(?<name>.*?)"" href=""(?<url>.*?)"" target=""_blank"" class=""ico_1"">(?<name>.*?)<span class=""bt"">种子</span></a><span><a class=""d1"" href=""(?<magnet>.*?)"">磁力</a></span></li>";

                cols = Regex.Matches(base.HtmlCont, me);
                model.MovieLinks = new List<MovieLink>();

                WebClient client = new WebClient();
                client.Encoding = Encoding.GetEncoding("utf-8");

                foreach (Match item in cols)
                {
                    model.Magnet = item.Groups["magnet"].ToString();
                    var strCont = client.DownloadString("http://www.btbtdy.com" + item.Groups["url"].ToString());

                    string zj = @"<a class=""className"" href=""(?<url>.*?)"" target=""_blank"">种子①</a>";
                    r = new Regex(zj, RegexOptions.None);
                    match = r.Match(strCont);

                    if (match.Success)
                    {
                        model.MovieLinks.Add(new MovieLink()
                        {
                            LinkUrl = match.Groups["url"].ToString(),
                            LinkName = item.Groups["name"].ToString(),
                            Magnet = item.Groups["magnet"].ToString(),
                            DownloadCount = 0
                        });
                    }
                }

                //内容
                var content = @"<div class=""c05""><strong>剧情介绍：</strong>(?<content>.*?)</div></div>";
                r = new Regex(content, RegexOptions.None);
                match = r.Match(base.HtmlCont);

                if (match.Success)
                {
                    model.MovieContent = match.Groups["content"].ToString();
                }

            }
            Err = false;
            return model;
        }
    }
}
