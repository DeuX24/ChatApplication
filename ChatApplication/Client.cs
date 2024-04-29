using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace ChatApplication;

public static class Client
{
    public static async Task ConnectAsync(IPAddress server, int port)
    {
        // Create a TcpClient.
        TcpClient client = new TcpClient();
        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            
            var connectTask = client.ConnectAsync(server, port);

            var completedTask = await Task.WhenAny(connectTask, Task.Delay(5000, cancellationTokenSource.Token));

            if (completedTask != connectTask)
            {
                MessageBox.Show("Connection Attempt Timed out");
                return;
            }
            MessageBox.Show($"Successfully connected to: {server}:{port}");
        }
        catch (Exception e)
        {
            MessageBox.Show($"SocketException: {e.Message}");
        }
        
        try
        {
            ClientWindow clientWindow = new ClientWindow(client);
            clientWindow.Show();
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error handling client: {e.Message}");
        }
    }
}