namespace GLaDOS.VoiceMode.Handler.GPTService.Yandex
{
    public class YandexGPTRequestBody

    {
        public string modelUri { get; set; } = "gpt://b1gjrufr9r3iqldoao59/yandexgpt-lite";
        public YandexGPTCompletionOptions completionOptions { get; set; } = new();
        public List<YandexGPTMessage> messages { get; set; } = new()
        {
        new(){ role="system",text="Ты умный ассистент, отвечай ОЧЕНЬ кратко."},
        new(){role="user",text="Привет! Как мне подготовиться к экзаменам?"},
        new(){role="assistant",text="Привет! По каким предметам?"},
        new(){role="user",text="Математике и физике"}
        };

    }
    public class YandexGPTCompletionOptions
    {
        public bool stream {  get; set; }=false;
        public double temperature { get; set; } = 0.6;
        public int maxTokens { get; set; } = 2000;

    }
    public class YandexGPTMessage
    {
        public string role { get; set; } = string.Empty;
        public string text { get; set; } = string.Empty;
    
    }
}
