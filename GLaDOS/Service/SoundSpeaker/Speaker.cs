using NAudio.Wave;
using NAudio.Vorbis;

namespace GLaDOS.Service.SoundSpeaker
{
    public class Speaker
    {
        private const string _dir = @"C:\TEMP\speech\";
        private const string _ext = @".ogg";
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
            audioPlayer.Play(path, false);
        }

    }
    public class AudioPlayer
    {
        private WaveOutEvent waveOut;
        private string _pathdisp=string.Empty;
        public void Play(string filePath, bool dispose)
        {
            Thread.Sleep(1100);

            waveOut = new WaveOutEvent();
            if (dispose) {
                waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
            }
            VorbisWaveReader OggReader = new VorbisWaveReader(filePath);

            waveOut.Init(OggReader);
            waveOut.Play();

        }

        private void WaveOut_PlaybackStopped(object? sender, StoppedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_pathdisp))
            {
                Thread.Sleep(1000);
                if (File.Exists(_pathdisp))
                    File.Delete(_pathdisp);
                _pathdisp = string.Empty;
            }
            waveOut.PlaybackStopped -= WaveOut_PlaybackStopped;
        }

        public void Stop()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
        }
    }
}
