using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoServer
{
    class MyHttpServer : HttpServer
    {
        public MyHttpServer(int port): base(port)
        {

        }
        public override void handleGETRequest(HttpProcessor p)
        {
            if (p.http_url.Contains(".mp4"))
            {
                string a = p.http_url.Substring(1, p.http_url.Length - 1);
                string link = ".\\videos\\" + a;
                using (FileStream fs = new FileStream(link, FileMode.Open))
                {
                    int startByte = -1;
                    int endByte = -1;
                    if (p.httpHeaders.Contains("Range"))
                    {
                        string rangeHeader = p.httpHeaders["Range"].ToString().Replace("bytes=", "");
                        string[] range = rangeHeader.Split('-');
                        startByte = int.Parse(range[0]);
                        if (range[1].Trim().Length > 0) int.TryParse(range[1], out endByte);
                        if (endByte == -1) endByte = (int)fs.Length;
                    }
                    else
                    {
                        startByte = 0;
                        endByte = (int)fs.Length;
                    }
                    byte[] buffer = new byte[endByte - startByte];
                    fs.Position = startByte;
                    int read = fs.Read(buffer, 0, endByte - startByte);
                    fs.Flush();
                    fs.Close();
                    p.outputStream.AutoFlush = true;
                    p.outputStream.WriteLine("HTTP/1.0 206 Partial Content");
                    p.outputStream.WriteLine("Content-Type: video.mp4");
                    p.outputStream.WriteLine("Accept-Ranges: bytes");
                    int totalCount = startByte + buffer.Length;
                    p.outputStream.WriteLine(string.Format("Content-Range: bytes {0}-{1}/{2}", startByte, totalCount - 1, totalCount));
                    p.outputStream.WriteLine("Content-Length: " + buffer.Length.ToString());
                    p.outputStream.WriteLine("Connection: keep-alive");
                    p.outputStream.WriteLine("");
                    p.outputStream.AutoFlush = false;

                    p.outputStream.BaseStream.Write(buffer, 0, buffer.Length);

                }

            }
            if (p.http_url.Contains(".png"))
            {
                string a = p.http_url.Substring(1, p.http_url.Length - 1);
                string link = @"C:\Users\Admin\Downloads\" + a;
                using (FileStream fs = new FileStream(link, FileMode.Open))
                {
                    
                    int endByte = (int)fs.Length;
                    byte[] buffer = new byte[endByte];
                    
                    int read = fs.Read(buffer, 0, endByte);
                    fs.Flush();
                    fs.Close();
                    p.outputStream.AutoFlush = true;
                    p.outputStream.WriteLine("HTTP/1.0 206 Partial Content");
                    p.outputStream.WriteLine("Content-Type: image.ong");
                    p.outputStream.WriteLine("Accept-Ranges: bytes");
                    p.outputStream.WriteLine("Content-Length: " + buffer.Length.ToString());
                    p.outputStream.WriteLine("Connection: keep-alive");
                    p.outputStream.WriteLine("");
                    p.outputStream.AutoFlush = false;
                    p.outputStream.BaseStream.Write(buffer, 0, buffer.Length);
                    p.outputStream.Flush();
                }

            }
            else
            {
                Console.WriteLine("request: {0}", p.http_url);
                p.writeSuccess();
                p.outputStream.WriteLine("<html><body><h1>Em yêu Lee Shang</h1>");
                p.outputStream.Flush();
            }
            Console.WriteLine("request: {0}", p.http_url);
            p.writeSuccess();
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            
        }
    }
}
