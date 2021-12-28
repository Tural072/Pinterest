using Pinterest.Commands;
using Pinterest.Extencions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Pinterest.ViewModel
{
    public class MainWindowViewModel:BaseViewModel
    {
        public RelayCommand SelectBtnCommand { get; set; }
        public RelayCommand SendBtnCommand { get; set; }

        byte[] b;

        public MainWindowViewModel(MainWindow mainWindow)
        {
            SelectBtnCommand = new RelayCommand((sender) =>
            {
                try
                {
                    var open = new Microsoft.Win32.OpenFileDialog();
                    open.Multiselect = false;
                    open.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    open.ShowDialog();
                    b = ImageHelper.GetBytesOfImage(path: open.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            SendBtnCommand = new RelayCommand((sender) =>
            {
                var client = new TcpClient();
                var ip = IPAddress.Parse("10.1.18.29");
                var port = 27001;
                var ep = new IPEndPoint(ip, port);
                try
                {
                    client.Connect(ep);
                    if (client.Connected)
                    {
                        var writer = Task.Run(() =>
                        {
                            while (true)
                            {
                                var stream = client.GetStream();
                                var bw = new BinaryWriter(stream);
                                bw.Write(b);
                            };
                        });
                    }
                    else
                    {
                        MessageBox.Show("CLinet doesnt connected");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }
    }
}
