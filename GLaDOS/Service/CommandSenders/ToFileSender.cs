namespace GLaDOS.Service.CommandSenders
{
    class ToFileSender : ICommandSender
    {
        public void Restore()
        {
            Thread.Sleep(5000);
        }

        public void Send(string text)
        {
            string path = @"C:\TEMP\commands.txt";
            File.AppendAllLines(path,new[] { text });
        }
    }
}
