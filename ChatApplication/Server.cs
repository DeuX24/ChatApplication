using System.Net;
using System.Net.Sockets;
using System.Windows;

namespace ChatApplication;

public static class Server
{
    public static async void ConnectAsync(int port)
    {
        TcpListener listener = null!;
        try
        {
            listener = new TcpListener(IPAddress.Any, port);

            // Start listening for client requests.
            listener.Start();

            MessageBox.Show($"Now listening on Port: {port}");
            
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                // Connection accepted, handle client asynchronously
                _ = HandleClientAsync(client);
            }
        }
        catch (SocketException e)
        {
            MessageBox.Show($"SocketException: {e.Message}");
        }
        finally
        {
            // Stop listening for new connections (if needed)
            listener.Stop();
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            MessageBox.Show($"Successfully connected to {client.Client.RemoteEndPoint}!");

            ClientWindow clientWindow = new ClientWindow(client);
            clientWindow.Show();
        }
        catch (Exception e)
        {
            MessageBox.Show($"Error handling client: {e.Message}");
        }
    }
}

