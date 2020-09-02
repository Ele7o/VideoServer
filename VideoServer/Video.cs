using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoServer
{
    class Video
    {
        public int Id { get; set; }
        public string TenVideo { get; set; }
        public string UrlVideo { get; set; }

        public Video() { }

        public Video(int Id, string TenVideo, string UrlVideo)
        {
            this.Id = Id;
            this.TenVideo = TenVideo;
            this.UrlVideo = UrlVideo;
        }
        public Video(DataRow row)
        {
            this.Id = (int)row["id"];
            this.TenVideo = row["tenVideo"].ToString();
            this.UrlVideo = row["tenVideo"].ToString();
        }
    }
}
