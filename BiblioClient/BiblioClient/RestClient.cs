using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BiblioClient
{
    public enum HttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    class RestClient
    {
        public string EndPoint { get; set; }
        public HttpVerb httpVerb { get; set; }

        public RestClient()
        {
            EndPoint = String.Empty;
            httpVerb = HttpVerb.GET;
        }

        public string makeHttpRequest(string postData)
        {
            string response = String.Empty;

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(EndPoint.ToString());
            httpWebRequest.Method = httpVerb.ToString();

            if (httpVerb == HttpVerb.POST)
            {
                httpWebRequest.ContentType = "application/json";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            }

            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                if (httpWebResponse.StatusCode != HttpStatusCode.OK & httpWebResponse.StatusCode != HttpStatusCode.Created)
                {
                    throw new ApplicationException("Erreur : " + httpWebResponse.StatusCode.ToString());
                }
                else
                {
                    using (Stream stream = httpWebResponse.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (StreamReader streamReader = new StreamReader(stream))
                            {
                                response = streamReader.ReadToEnd().ToString();
                            }
                        }
                    }
                }
            }

            return response;
        }


    }
}
