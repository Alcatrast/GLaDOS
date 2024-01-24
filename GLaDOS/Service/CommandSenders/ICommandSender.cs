
namespace GLaDOS.Service.CommandSenders
{
    public interface ICommandSender
    {
        public void Restore();
        public void Send(string text);
    }
}
