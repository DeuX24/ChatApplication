using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatApplication;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Listen_Button_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(portTextBox.Text, out var port) && port is >= 0 and <= 65535)
        {
            Server.ConnectAsync(port);
        }
        else
        {
            MessageBox.Show("Incorrect IP Address or Port");
        }
    }

    private async void Connect_Button_Click(object sender, RoutedEventArgs e)
    {
        Connect_Button.IsEnabled = false;
        if (IPAddress.TryParse(addressTextBox.Text, out var ipAddress) && int.TryParse(portTextBox.Text, out var port) && port is >= 0 and <= 65535)
        {
            MessageBox.Show($"Attempting to connect to {ipAddress}:{port}");
            await Client.ConnectAsync(ipAddress, port);
            Connect_Button.IsEnabled = true;
        }
        else
        {
            MessageBox.Show("Incorrect IP Address or Port");
            Connect_Button.IsEnabled = true;
        }
    }
}