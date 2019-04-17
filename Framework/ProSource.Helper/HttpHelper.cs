using System;
using System.Collections.Generic;
using System.Text;

namespace ProSource.Helper
{
    public static class HttpHelper
    {
        public static TResult HttpPost<TResult, TInput>(string url, TInput model)
        {
            try
            {
                using (var httpClient = new System.Net.Http.HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var _postDataJson = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                    var _postDataContent = new System.Net.Http.StringContent(_postDataJson, Encoding.UTF8, "application/json");

                    var response = httpClient.PostAsync(url, _postDataContent).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(response.ReasonPhrase);
                    }

                    string jsonResult = response.Content.ReadAsStringAsync().Result;

                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TResult>(jsonResult);

                    return result;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
