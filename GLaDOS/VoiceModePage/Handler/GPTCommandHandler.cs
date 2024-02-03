using Robot.CommandModels;
using GLaDOS.Service.SoundSpeaker;
using GLaDOS.VoiceMode.Handler.GPTService.Yandex;

namespace GLaDOS.VoiceMode.Handler
{
    public class GPTCommandHandler
    {
        public GPTCommandHandler() { }
        public IGladCommand Run(string text)
        {
            //GPT обрабатывает текст
            var gptHandler= new YandexGPTHandler();
            string response = gptHandler.GetResponse(text);
            _ = Task.Run(() => { Tell(response); });
            return new PhysCommand(PhysCommand.DeviceType.Animation, 1, 0);
        }

        private void Tell(string text)
        {
            if (string.IsNullOrEmpty(text)) { return; }
            string path= new TTSController().GetOggPath(text);
            new AudioPlayer().Play(path,true);
        }

    }
}
