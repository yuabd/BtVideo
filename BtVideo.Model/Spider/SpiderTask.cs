using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtVideo.Model.Spider
{
    public class SpiderTask
    {
        public int SpiderTaskID { get; set; }

        public int CurrentID { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
