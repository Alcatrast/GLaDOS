using Robot.CommandModels;
using GLaDOS.Service.SoundSpeaker;

namespace GLaDOS.VoiceMode.Handler
{
    public class GPTCommandHandler
    {
        public GPTCommandHandler() { }
        public IGladCommand Run(string text)
        {
            //GPT обрабатывает текст
            string response = "я слишком мала для таких вопросов";
            _ = Task.Run(() => { Tell(response); });
            return new PhysCommand(PhysCommand.DeviceType.Animation, 4, 0);
        }

        private void Tell(string text)
        {
            if (string.IsNullOrEmpty(text)) { return; }
            new Speaker().Play(Speaker.Audio.Beatles);
        }
    }
}
