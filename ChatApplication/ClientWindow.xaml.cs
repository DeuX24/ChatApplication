using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChatApplication;

public partial class ClientWindow : Window
{
    private readonly TcpClient _client;
    private readonly NetworkStream _stream;
    private string _username = "user";

    public ClientWindow(TcpClient tcpClient)
    {
        InitializeComponent();
        _client = tcpClient;
        _stream = _client.GetStream();
        Task.Run(async () => await ReceiveMessages());
        SendUsername(_username);
    }
    
    private async Task ReceiveMessages()
    {
        // Pattern to match and capture username.
        const string usernamePattern = @"\|username:(.*?)\|";
        
        byte[] buffer = new byte[1024];
        while (true)
        {
            try
            {
                var bytesRead = await _stream.ReadAsync(buffer);
                if (bytesRead == 0)
                {
                    break; // connection closed
                }
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                
                Match match = Regex.Match(message, usernamePattern);

                if (match.Success)
                {
                    // Extract username from captured group.
                    Dispatcher.Invoke(() => recipientUsernameTextBlock.Text = match.Groups[1].Value);
                }
                else
                {
                    Dispatcher.Invoke(() => lastMessageTextBlock.Text = message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error receiving message: {ex.Message}");
                break;
            }
        }
        _stream.Close();
        _client.Close();
        Close();
    }

    private async void SendUsername(string username)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes($"|username:{username}|");
            await _stream.WriteAsync(data, 0, data.Length);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error sending username: {ex.Message}");
        }

        _username = username;
    }
    
    private async void SendButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string message = inputTextBox.Text;
            byte[] data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);
            inputTextBox.Text = ""; // clear the input box after sending
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error sending message: {ex.Message}");
        }
    }

    private void yourUsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var username = yourUsernameTextBox.Text;
        if (username == _username) return;
        
        SendUsername(username);
    }

    private void yourUsernameTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;
        
        SendUsername(yourUsernameTextBox.Text);

        Keyboard.ClearFocus();
    }
}