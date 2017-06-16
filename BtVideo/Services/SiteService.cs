using BtVideo.Models;
using BtVideo.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BtVideo.Services
{
    public class SiteService : DbAccess
    {
        public BaseObject InsertLink(Links links)
        {
            BaseObject obj = new BaseObject();
            try
            {
                links.DateCreated = DateTime.Now;
                db.Links.Add(links);
                db.SaveChanges();

                obj.Tag = 1;
                obj.Message = "添加成功!";
            }
            catch (Exception e)
            {
                obj.Tag = -1;
                obj.Message = e.Message;
            }

            return obj;
        }

        public Links GetLink(int id)
        {
            var link = (from l in db.Links
                        where l.ID == id
                        select l).FirstOrDefault();

            return link;
        }

        public BaseObject UpdateLink(Links link)
        {
            BaseObject obj = new BaseObject();
            var l = db.Links.Find(link.ID);

            if (l == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录不存在！";
                return obj;
            }

            try
            {
                l.Contact = link.Contact;
                l.Description = link.Description;
                l.Email = link.Email;
                l.Name = link.Name;
                l.LinkUrl = link.LinkUrl;
                l.PictureFile = link.PictureFile;
                l.SortOrder = link.SortOrder;
                //l.DateCreated = DateTime.Now;

                db.SaveChanges();

                obj.Tag = 1;
            }
            catch (Exception)
            {
                obj.Tag = -1;
                obj.Message = "修改失败！";
            }

            return obj;
        }

        public BaseObject DeleteLink(int id)
        {
            BaseObject obj = new BaseObject();
            var link = db.Links.Find(id);
            if (link == null)
            {
                obj.Tag = -1;
                obj.Message = "该记录不存在!";
                return obj;
            }

            db.Links.Remove(link);
            db.SaveChanges();

            obj.Tag = 1;

            return obj;
        }

        public List<Links> GetLinks()
        {
            var list = (from l in db.Links
                        orderby l.SortOrder descending
                        select l).ToList();

            return list;
        }

        public BaseObject SaveKeyword(string keyword)
        {
            BaseObject obj = new BaseObject(1);
            var k = db.HotKeywords.FirstOrDefault(m => m.Keyword == keyword);

            if (k != null)
            {
                k.Count += 1;
                k.UpdateDate = DateTime.Now;
            }
            else
            {
                db.HotKeywords.Add(new HotKeyword()
                {
                    Count = 0,
                    Keyword = keyword,
                    UpdateDate = DateTime.Now
                });
            }

            db.SaveChanges();

            return obj;
        }

        public IQueryable<HotKeyword> GetKeywords()
        {
            return db.HotKeywords;
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}