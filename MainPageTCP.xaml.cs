using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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

namespace TCPClient_WPF
{
    /// <summary>
    /// Interaction logic for MainPageTCP.xaml
    /// </summary>
    public partial class MainPageTCP : Page
    {
        private TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private string _serverIP = "";
        private int _serverPort = 0;
        public MainPageTCP()
        {
            InitializeComponent();
        }
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(IPTextBox.Text) && !String.IsNullOrEmpty(PortTextBox.Text))
            {
                try
                {
                    _serverIP = IPTextBox.Text;
                    _serverPort = Int32.Parse(PortTextBox.Text);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Found error: " + ex.Message);
                    Dispatcher.Invoke(() =>
                    {
                        ConnectionStatus.Content = "Something went wrong.";
                        IPTextBox.Text = null;
                        PortTextBox.Text = null;
                    });
                    return;
                }
                try
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(_serverIP, _serverPort);
                    _networkStream = _tcpClient.GetStream();
                    ConnectionStatus.Content = "Connected!";
                    IPTextBox.IsEnabled = false;
                    PortTextBox.IsEnabled = false;
                    ConnectButton.IsEnabled = false;
                    DisconnectButton.IsEnabled = true;

                    Task.Run(() =>
                    {
                        byte[] data = new byte[256];
                        int bytesRead = 0;
                        try
                        {
                            while (_tcpClient != null && _tcpClient.Connected)
                            {
                                bytesRead = _networkStream.Read(data,0, data.Length);
                                if (bytesRead > 0)
                                {
                                    string message = Encoding.ASCII.GetString(data, 0, bytesRead);
                                    Debug.WriteLine(message);
                                    Dispatcher.Invoke(() =>
                                    {
                                        MessagesTextBox.AppendText(message + "\n");
                                        MessagesTextBox.ScrollToEnd();
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ConnectionStatus.Content = "Connection lost.";
                            Console.WriteLine(ex.Message);
                        }
                    });
                }
                catch (Exception ex)
                {
                    ConnectionStatus.Content = "Connection Failed.";
                    ConnectButton.IsEnabled = true;
                    Debug.WriteLine("Failed to connect: " + ex.Message);
                }
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    ConnectionStatus.Content = "Cannot proceed with null values.";
                    IPTextBox.Text = null;
                    PortTextBox.Text = null;
                });
            }
        }
        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _networkStream.Close();
                _tcpClient.Close();

                ConnectionStatus.Content = "Disconnected from the server.";
                Dispatcher.Invoke(() =>
                {
                    DisconnectButton.IsEnabled = false;
                    ConnectButton.IsEnabled = true;
                    IPTextBox.IsEnabled = true;
                    PortTextBox.IsEnabled = true;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to disconnect: " + ex.Message);
            }
        }
    }
}
