
using Robot.CommandModels;
using GLaDOS.Service;

namespace GLaDOS;

public partial class DirectModePage : ContentPage
{
    private const int SLIDER_DELAY = 800;

    public DirectModePage()
    {
        InitializeComponent();
        SlidersServoThreadsInit();
        SlidersLumenThreadsInit();
    }
    private void RunServo(int number, int value)
    {
        PhysCommand command = new(PhysCommand.DeviceType.Servo, number, value);
        CommandSenderManager.Send(command);
    }
    private void RunLumen(int number, int value)
    {
        PhysCommand command = new(PhysCommand.DeviceType.Light, number, value);
        CommandSenderManager.Send(command);
    }


}