using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace VideoServer
{
    public class HttpProcessor
    {
        private TcpClient s;
        private HttpServer srv;
        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();
        private static int MAX_POST_SIZE = 10 * 1024 * 1024;
        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.s = s;
            this.srv = srv;
        }
        private string streamReadLine(Stream inputStream) //Hàm này sẽ chuyển chuỗi byte nhận từ luồng inputStream sang String
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\n') { continue; }
                if(next_char == -1) { Thread.Sleep(1);continue };
                data += Convert.ToChar(next_char);
            }
            return data;
        }
        internal void process()
        {
            inputStream = new BufferedStream(s.GetStream());
            outputStream = new StreamWriter(new BufferedStream(s.GetStream()));
            try
            {
                parseRequest();
                readHeader();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTREQUEST();
                }
            }catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
        }

        private void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        private void handlePOSTREQUEST()
        {
            
        }

        private void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        private void readHeader()
        {
            Console.WriteLine("Read header");
            String line;
            while((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    Console.WriteLine("got headers");
                    return;
                }
                int separator = line.IndexOf(':');
                if(separator == -1)
                {
                    throw new Exception("Invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while((pos < line.Length) && (line[pos]==' '))
                {
                    pos++;
                }
                string value = line.Substring(pos, line.Length - pos);
                Console.WriteLine("header:{0}:{1}", name, value);
                httpHeaders[name] = value;
            }
        }

        private void parseRequest() // Định dạng của request sau khi đc parse GET /myurl HTTP/1.0
        {
            String request = streamReadLine(inputStream); // nhận request từ luồng chuyển byte request sang chữ
            string[] tokens = request.Split(' ');
            if(tokens.Length != 3) // Request sẽ được chia là 3 bao gồm phương: thức truyền như đã thấy ở dòng trên: GET/POST; đường dẫn url(ví dụ: index.html, EmdepLam.mp4); phiên bản của HTTP (ở đây sẽ là phiên bản 1.0)
            {
                throw new Exception("invadod http request line");
            }
            http_method = tokens[0].ToUpper(); // GET/POST
            http_url = tokens[1];   //URL
            http_protocol_versionstring = tokens[2]; //VERSION

            Console.WriteLine("starting: " + request);
        }
    }
}