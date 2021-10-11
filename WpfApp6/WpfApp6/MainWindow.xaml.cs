using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp6
{
    public partial class MainWindow : Window
    {
        string address = "127.0.0.1"; 
        int port = 8005;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void DoWork()
        {
            int count = 0;
            while (true)
            {
                WebRequest request = WebRequest.Create(@"https://2452238.com");
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    count = 0;
                }
                catch (Exception)
                {
                    count++;
                    if (count == 5) // Должно быть 60 но это слишком ДОЛГО
                    {
                        
                        IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);
                        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(ipPoint);

                        string message = " 2452238.com не отвечает уже минуту";
                        byte[] data = Encoding.Unicode.GetBytes(message);
                        socket.Send(data);
                    }
                }
                Thread.Sleep(1000); // 1 сек
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(DoWork));
            thread1.Start();
        }
    }
}
