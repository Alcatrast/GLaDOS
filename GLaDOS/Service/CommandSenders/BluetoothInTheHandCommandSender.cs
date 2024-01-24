using System.Text;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;


namespace GLaDOS.Service.CommandSenders
{
    public class BluetoothInTheHandCommandSender: ICommandSender
    {
        private BluetoothClient client = new();
        private string _name;
        public BluetoothInTheHandCommandSender(string name) { _name = name; }
        public async void Restore()
        {
            client.Close();
            client.Dispose();
            client = new();
            BluetoothDeviceInfo device = client.DiscoverDevices().First(d => d.DeviceName == _name);
            if (device == null) { await Shell.Current.DisplayAlert("Device not found", _name, "Ok"); return; }

            BluetoothEndPoint endPoint = new BluetoothEndPoint(device.DeviceAddress, BluetoothService.SerialPort);
            client.Connect(endPoint);

        }
        public async void Send(string text)
        {

            if (client.Connected == false) { Restore(); }
            if (client.Connected == false) { await Shell.Current.DisplayAlert("Device not found", _name, "Ok"); return; }

            if (string.IsNullOrEmpty(text)) { text = "empty_string"; }

            byte[] buffer = Encoding.UTF8.GetBytes(text);
            Stream stream = client.GetStream();
            try
            {
                stream.Write(buffer, 0, buffer.Length);
            }
            catch
            {
                Restore();
                try
                {
                    stream.Write(buffer, 0, buffer.Length);
                }
                catch
                {
                    await Shell.Current.DisplayAlert("Impossible connect to", _name, "Ok");
                    client.Close();
                    client.Dispose();
                }
            }
        }

    }
}
