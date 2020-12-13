using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Qj.Utility.Helper
{
    public class HttpHelper
    {
        public static TResponse Post<TRequest, TResponse>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null)
                {
                    list.Add(new KeyValuePair<string, string>(p.Name, p.GetValue(request).ToString()));
                }
            }

            var content = new FormUrlEncodedContent(list);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var client = new HttpClient();

            var resp = client.PostAsync(url, content).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResponse>(body);

            return result;
        }

        public static TResponse Put<TRequest, TResponse>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null)
                {
                    list.Add(new KeyValuePair<string, string>(p.Name, p.GetValue(request).ToString()));
                }
            }

            var content = new FormUrlEncodedContent(list);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var client = new HttpClient();

            var resp = client.PutAsync(url, content).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResponse>(body);

            return result;
        }

        public static TResponse Delete<TResponse>(string url)
        {
            var client = new HttpClient();
            var resp = client.DeleteAsync(url).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResponse>(body);

            return result;
        }

        public static TResponse Get<TRequest, TResponse>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            StringBuilder urlSb = new StringBuilder();
            urlSb.Append("?");
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null)
                {
                    urlSb.Append(p.Name).Append("=").Append(p.GetValue(request).ToString()).Append("&");
                }
                else
                {
                    urlSb.Append(p.Name).Append("=").Append("&");
                }
            }

            url = (url + urlSb.ToString()).TrimEnd('&');
            var client = new HttpClient();

            var resp = client.GetAsync(url).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResponse>(body);

            return result;
        }

        public static TResponse Get<TResponse>(string url)
        {
            var list = new List<KeyValuePair<string, string>>();
            var client = new HttpClient();

            var resp = client.GetAsync(url).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResponse>(body);

            return result;
        }

        public static string Get(string url)
        {
            var list = new List<KeyValuePair<string, string>>();
            var client = new HttpClient();

            var resp = client.GetAsync(url).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = body;

            return result;
        }

        public static TResponse Post<TResponse>(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<TResponse>(reader.ReadToEnd());
            }
        }

        /// <summary>
        /// Post请求返回body
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string PostAsync<TRequest>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null)
                {
                    list.Add(new KeyValuePair<string, string>(p.Name, p.GetValue(request).ToString()));
                }
            }

            var content = new FormUrlEncodedContent(list);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var client = new HttpClient();

            var resp = client.PostAsync(url, content).Result;
            var body = resp.Content.ReadAsStringAsync().Result;

            return body;
        }

        /// <summary>
        /// 返回Xml解析类
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <param name="url"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static TResponse PostXMLAsync<TRequest, TResponse>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null)
                {
                    list.Add(new KeyValuePair<string, string>(p.Name, p.GetValue(request).ToString()));
                }
            }

            var content = new FormUrlEncodedContent(list);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            var client = new HttpClient();

            var resp = client.PostAsync(url, content).Result;
            var body = resp.Content.ReadAsStringAsync().Result;

            var result = XMLDeserialize<TResponse>(body);

            return result;
        }

        public static TObject XMLDeserialize<TObject>(string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                try
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(TObject));
                    return (TObject)xmldes.Deserialize(sr);
                }
                catch
                {
                    return default(TObject);
                }
            }
        }

        public static TResponse GetAsync<TRequest, TResponse>(string url, TRequest request)
        {
            var list = new List<KeyValuePair<string, string>>();
            StringBuilder urlSb = new StringBuilder();
            urlSb.Append("?");
            foreach (System.Reflection.PropertyInfo p in request.GetType().GetProperties())
            {
                if (p.GetValue(request) != null)
                {
                    urlSb.Append(p.Name).Append("=").Append(p.GetValue(request).ToString()).Append("&");
                }
                else
                {
                    urlSb.Append(p.Name).Append("=").Append("&");
                }
            }

            url = (url + urlSb.ToString()).TrimEnd('&');
            var client = new HttpClient();
            var resp = client.GetAsync(url).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<TResponse>(body);

            return result;
        }

        public static T HttpPost<T, TRequest>(string url, TRequest model)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            string body = JsonConvert.SerializeObject(model);
            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
            }
            

        }


        /// <summary>
        /// 增加Headers，去SSL证书验证   示例方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        public static T SampleHttpPost<T>(string url, string Authorization)
        {
            var list = new List<KeyValuePair<string, string>>();
        
            var content = new FormUrlEncodedContent(list);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //去SSL证书验证
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, certificate2, arg3, arg4) => true,
            };

            var client = new HttpClient(httpClientHandler);

            //认证token 增加Headers
            if (!string.IsNullOrWhiteSpace(Authorization))
            {
                client.DefaultRequestHeaders.Add("Authorization", Authorization);
            }
            ServicePointManager.ServerCertificateValidationCallback = (message, certificate2, arg3, arg4) => true;
            var resp = client.PostAsync(url, content).Result;
            var body = resp.Content.ReadAsStringAsync().Result;
            Console.WriteLine(body);
            var result = JsonConvert.DeserializeObject<T>(body);
            return result;
        }
    }
}