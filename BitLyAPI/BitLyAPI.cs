using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BitLyAPI
{
    public class BitLyAPI
    {
        //Acess Token BitLy: fbe028c27575adc236f403905ee5882bfd89396d
        private string _bitLyApiURL;
        private string _bitLyApiToken;

        public BitLyAPI()
        {
            _bitLyApiURL = ConfigurationManager.AppSettings["BitLyAPIUrl"];
            _bitLyApiToken = ConfigurationManager.AppSettings["BitLyAPIToken"];
        }

        public async Task<string> ShortenAsync(string long_url)
        {
            return await Task.Run(() => Shorten(long_url));
        }

        private string Shorten(string long_url)
        {
            if (CheckAccessToken())
            {
                using (HttpClient client = new HttpClient())
                {
                    string temp = string.Format(_bitLyApiURL, _bitLyApiToken, WebUtility.UrlEncode(long_url));
                    var response = client.GetAsync(temp).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var message = response.Content.ReadAsStringAsync().Result;
                        dynamic obj = JsonConvert.DeserializeObject(message);
                        return obj.results[long_url].shortUrl;
                    }
                    else
                    {
                        return "Não foi possível encurtar a URL";
                    }
                }
            }
            else
            {
                return "Não foi possível encurtar a URL";
            }
        }

        private bool CheckAccessToken()
        {
            if (string.IsNullOrEmpty(_bitLyApiToken))
                return false;

            string temp = string.Format(_bitLyApiURL, _bitLyApiToken, "google.com");
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(temp).Result;
                return response.IsSuccessStatusCode;
            }
        }
    }
}
