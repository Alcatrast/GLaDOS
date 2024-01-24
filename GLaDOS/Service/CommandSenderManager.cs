using Robot.CommandModels;


using GLaDOS.Service.CommandSenders;

namespace GLaDOS.Service
{
    public static class CommandSenderManager
    {

        private static int SEND_DELAY=1200;

        private static ICommandSender sender = new BluetoothInTheHandCommandSender("GLA");
        private static Queue<IGladCommand> queue = new();
        public static void Init() { sender.Restore();
            _requestQueueHandler = new(HandleRequestQueue);
            _requestQueueHandler.Start(); } 
        public static void Send(IGladCommand command)
        {
            queue.Enqueue(command);
        }

        private static Thread _requestQueueHandler;
        private static void HandleRequestQueue()
        {
            while (true)
            {
                if(queue.Count>0)
                {
                    queue.TryDequeue(out var command);
                    string res= command.Render();
                    sender.Send(res);
                    Thread.Sleep(SEND_DELAY);
                }
            }
        }
    }
}
