using Robot.CommandModels;
using GLaDOS.Service.SoundSpeaker;
using GLaDOS.VoiceMode.Handler.GPTService.Yandex;
using NAudio.Vorbis;

namespace GLaDOS.VoiceMode.Handler
{
    public class GPTCommandHandler
    {
        public GPTCommandHandler() { }
        public async Task<IGladCommand> Run(string text, Settings settings)
        {
            //GPT обрабатывает текст
            var gptHandler= new YandexGPTHandler();
            string response = await gptHandler.GetResponse(text, settings.GhatGPT);

            if (string.IsNullOrEmpty(response)) { return new PhysCommand(PhysCommand.DeviceType.Animation, 0, 0); }
            string path = new TTSController().GetOggPath(response);

            int ms = 0;
            using (var vr = new VorbisWaveReader(path))
            {
                TimeSpan duration = vr.TotalTime;
                ms = (int)duration.TotalMilliseconds;
            }
            _= Task.Run(()=> { new AudioPlayer().Play(path, true); });
            int ae = ms / General.Configuration.AnimationMsD;
            if(ae >9999) { ae = 9999; }
            return new PhysCommand(PhysCommand.DeviceType.Animation, ae/1000, ae%1000);
        }
    }
}
