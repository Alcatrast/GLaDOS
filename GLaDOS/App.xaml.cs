
using GLaDOS.Service;

namespace GLaDOS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            CommandSenderManager.Init();
            MainPage = new AppShell();
        }
    }
}
