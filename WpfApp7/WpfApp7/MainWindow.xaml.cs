using Notifications.Wpf;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace WpfApp7
{
    public partial class MainWindow : Window
    {
        static int port = 8005; 
        public MainWindow()
        {
            InitializeComponent();
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                MessageBox.Show("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; 
                    byte[] data = new byte[256];
                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    var notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "Sample notification",
                        Message = DateTime.Now.ToShortTimeString() + ": " + builder.ToString(),
                        Type = NotificationType.Error
                    });

                    MessageBox.Show(DateTime.Now.ToShortTimeString() + ": " + builder.ToString());
                    
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
