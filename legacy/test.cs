using System;
using System.Runtime.InteropServices;
using System.Net.Http;

namespace HttpHelper
{
    [ComVisible(true)]
    [Guid("D7691E76-BAD6-4578-A107-6AEFBFF0F458")]
    [ClassInterface(ClassInterfaceType.None)]
    public class HttpHelper
    {
        public static string HttpGet(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = client.GetStringAsync(url).Result;
                    return (response);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public static string HttpPost(string url, string data)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var content = new StringContent(data, System.Text.Encoding.UTF8, "application/json");
                    var response = client.PostAsync(url, content).Result.Content.ReadAsStringAsync().Result;
                    return (response);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }
    }
}
