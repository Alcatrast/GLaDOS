using GLaDOS.Service;
using GLaDOS.VoiceMode.Handler;
using NAudio.Wave;

namespace GLaDOS;

public partial class VoiceModePage : ContentPage
{
    private bool _voiceActive = false;
    private bool _isCancelled = false;

    private WaveInEvent waveIn;
    private MemoryStream recordedStream;

    private const string outAudioPath = @"C:\TEMP\sound.wav";

    public VoiceModePage()
    {
        InitializeComponent();

        waveIn = new WaveInEvent();
        waveIn.DataAvailable += WaveIn_DataAvailable;
        waveIn.RecordingStopped += WaveIn_RecordingStopped;
    }
    private void OnCounterClicked(object sender, EventArgs e)
    {
        if (_voiceActive)
        {
            Stop(false);
        }
        else
        {
            Start();
        }
        ChangeIntState(!_voiceActive);
    }

    public void Start()
    {
        recordedStream = new MemoryStream();
        waveIn.WaveFormat = new WaveFormat(16000, 1);

        waveIn.StartRecording();
    }
    public void Stop(bool isCanceled)
    {
        _isCancelled = isCanceled;
        waveIn.StopRecording();
    }

    private void ChangeIntState(bool toActiveState)
    {
        _voiceActive = toActiveState;
        SpeakLb.IsVisible = toActiveState;
        UnspeakBtn.IsVisible = toActiveState;
        SpeakBtn.IsVisible = !toActiveState;
        PressLb.IsVisible = !toActiveState;
        cancelBtn.IsVisible = toActiveState;
    }

    private void Cancel_Clicked(object sender, EventArgs e)
    {
        ChangeIntState(!_voiceActive);
        Stop(true);
    }

    private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        recordedStream.Write(e.Buffer, 0, e.BytesRecorded);
    }
    private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
    {
        recordedStream.Position = 0;

        using (var reader = new RawSourceWaveStream(recordedStream, waveIn.WaveFormat))
        {
            WaveFileWriter.CreateWaveFile(outAudioPath, reader);
        }

        recordedStream.Dispose();

        if (_isCancelled == false) PrepareVoiceCommand();


    }
    async void PrepareVoiceCommand()
    {
        string result = new STTController().GetText(outAudioPath);
        lab.Text = result;

        if (((new CommandDefiner()).Define(result, out var command)) == false) CommandSenderManager.Send(await new GPTCommandHandler().Run(result, General.User.Settings));
        else CommandSenderManager.Send(command);
    }
}