using System.Net;

namespace GLaDOS;

internal class STTController
{
    public string GetText(string audioWaveFilePath)
    {

        string apiToken = General.Configuration.Tokens.SileroTTS;
        string apiUrl = "https://api.silero.ai/transcribe";

        var bytes = File.ReadAllBytes(audioWaveFilePath);
        var payloade = Convert.ToBase64String(bytes);

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                api_token = apiToken,
                payload = payloade,
                remote_id = "my_tracking_id",
                sample_rate = 16000,
                encrypted = false,
                channels = 1,

            });

            streamWriter.Write(json);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        var responseStream = httpResponse.GetResponseStream();

        string result = "DEFAULT";
        using (var streamReader = new StreamReader(responseStream))
        {
            result = streamReader.ReadToEnd();
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
            result = json.transcriptions[0].transcript;
        }
        // File.WriteAllText(@"C:\TEMP\test_result.txt", result);
        return result;
    }
}

internal class TTSController
{

    public string GetOggPath(string texte)
    {
        string audioFilePath =$@"C:\TEMP\speech\temp\sound-{DateTime.Now.Ticks}.ogg";
        string apiToken = General.Configuration.Tokens.SileroTTS;
        string apiUrl = "https://api.silero.ai/voice";

        var httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
        httpWebRequest.ContentType = "application/json";
        httpWebRequest.Method = "POST";

        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        {
            var json = System.Text.Json.JsonSerializer.Serialize(new
            {
                api_token = apiToken,
                text = texte,
                sample_rate = 48000,
                speaker = "glados",
                remote_id = "test",
                lang = "ru",
                format = "ogg",
                word_ts = false
            });

            streamWriter.Write(json);
        }

        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        var responseStream = httpResponse.GetResponseStream();

        string result = "DEFAULT";
        byte[] oggAudioData = [];
        using (var streamReader = new StreamReader(responseStream))
        {
            result = streamReader.ReadToEnd();
            dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);
            oggAudioData = json.results[0].audio;
        }


        File.WriteAllBytes(audioFilePath, oggAudioData);
        return audioFilePath;
    }

}
