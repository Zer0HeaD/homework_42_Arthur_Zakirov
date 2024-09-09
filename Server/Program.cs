using System.Net;
using System.Net.Sockets;
using System.Text;

const int serverPort = 5000;
const string stopMessage = "Stop";

try
{
    MainWorker();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    throw;
}


void MainWorker()
{
    var ipAddress = IPAddress.Loopback;
    var endPoint = new IPEndPoint(ipAddress, serverPort);

    using var listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    listener.Bind(endPoint);
    listener.Listen(20);

    while (true)
    {
        Console.WriteLine($"Waiting for connection for {listener.LocalEndPoint}");
    
        using var handler = listener.Accept();

        var bytes = new byte[1024];
        var receivedBytes = handler.Receive(bytes);

        string request = Encoding.UTF8.GetString(
            bytes,
            0,
            receivedBytes);
        
        Console.WriteLine($"Request: {request}");

        var response = $"Receive request with length {request.Length}";
        var responseBytes = Encoding.UTF8.GetBytes(response);
        handler.Send((responseBytes));
    
        handler.Shutdown(SocketShutdown.Both);
        handler.Close();
        if (request.Equals(stopMessage))
        {
            break;
        }
    }
}