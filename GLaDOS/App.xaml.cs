
using GLaDOS.Service;

namespace GLaDOS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
           // new TTSController().GetMP3("");
            CommandSenderManager.Init();
            MainPage = new AppShell();
        }
    }
}
