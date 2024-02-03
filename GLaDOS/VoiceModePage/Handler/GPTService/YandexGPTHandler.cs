using System.Text;

namespace GLaDOS.VoiceMode.Handler.GPTService.Yandex
{
    internal class YandexGPTHandler
    {
        public YandexGPTRequestBody Body { get; set; }=new YandexGPTRequestBody();
        public string GetResponse(string text)
        {
           // Task<string> getResponseTask = Task.Run(async () =>  {
            string url = "https://llm.api.cloud.yandex.net/foundationModels/v1/completion";
            string data = System.Text.Json.JsonSerializer.Serialize(Body);
            string result = "Запрос не исполнен.";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {General.Configuration.Tokens.YandexIAM}");

                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    dynamic responseJson = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
                    result = responseJson.alternatives[0].message.text;
                }
                else
                {
                    result = "Ошибка: " + response.StatusCode;
                }
            }
            return result;
            //}); return getResponseTask.Result;
        }
    }
}
