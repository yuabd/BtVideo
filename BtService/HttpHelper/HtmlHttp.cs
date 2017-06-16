using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BtService.HttpHelper
{
    public class HtmlHttp
    {
        private string strCont;
        private string url;
        private WebClient client;

        protected string SetEncoding
        {
            set
            {
                this.client.Encoding = Encoding.GetEncoding(value);
            }
        }

        protected string Url
        {
            set
            {
                this.url = value;
            }
        }

        protected string HtmlCont
        {
            get
            {
                return this.strCont;
            }
        }

        public HtmlHttp()
        {
            this.strCont = string.Empty;
            this.client = new WebClient();
            this.client.Encoding = Encoding.GetEncoding("utf-8");
        }

        protected void GetHtmlCont()
        {
            try
            {
                this.strCont = this.client.DownloadString(this.url);
            }
            catch(Exception e)
            {

            }
        }

        //public virtual List<PK8Data> GetPK8Data(out bool Err)
        //{
        //    Err = false;
        //    return new List<PK8Data>();
        //}

        ~HtmlHttp()
        {
            this.client.Dispose();
        }
    }
}
