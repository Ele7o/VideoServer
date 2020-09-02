using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoServer
{
    class VideoDAO
    {
        public String ListVideos()
        {
            int i = 0;
            String ListDrink = "[";
            string query = "SELECT * FROM dbo.Video";
            DataTable data = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                Video video = new Video(item);
                ListDrink += JsonConvert.SerializeObject(video);
                ListDrink += ",";
                
            }
            ListDrink = ListDrink.Substring(0,ListDrink.Length-1);
            ListDrink += "]";
            //List<Video> a = JsonConvert.DeserializeObject<List<Video>>(ListDrink);
            return ListDrink;
        }
    }

}
