using NAudio.Wave;

namespace GLaDOS.Service.SoundSpeaker
{
    public class Speaker
    {
        private const string _dir = @"C:\TEMP\speech\";
        private const string _ext = @".mp3";
        public enum Audio {WakeUp, ShowItSelf, Joke1, Fact1, LightOn, LightOff, UndefinedCommand, EyeOn, EyeOff, Beatles }

        private IReadOnlyDictionary<Audio, string> _audio = new Dictionary<Audio, string>()
        {
            { Audio.WakeUp,"wakeup" },
            { Audio.UndefinedCommand,"undefined_command" },
            { Audio.LightOn,"light_on" },
            { Audio.LightOff,"light_off" },
            { Audio.EyeOn,"activate" },
            { Audio.EyeOff,"deactivate" },
            { Audio.ShowItSelf,"show_itself" },
            { Audio.Joke1,"joke" },
            { Audio.Fact1,"fact" },
            { Audio.Beatles,"beatles" },

        };
        public void Play(Audio audio)
        {
            if ((_audio.TryGetValue(audio, out var file)) == false) return;
            string path = _dir + file + _ext; if (File.Exists(path)==false) { throw new Exception(); }
            AudioPlayer audioPlayer = new();
            audioPlayer.Play(path);
        }

    }
    internal class AudioPlayer
    {
        private WaveOutEvent waveOut;

        public void Play(string filePath)
        {
            Thread.Sleep(1100);
            waveOut = new WaveOutEvent();
            Mp3FileReader mp3Reader = new Mp3FileReader(filePath);
            waveOut.Init(mp3Reader);
            waveOut.Play();
        }

        public void Stop()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
        }
    }
}
