using System.Net;
using System.Net.Sockets;
using System.Text;

const int port = 5000;
const string stopMessage = "Stop";

RunClient();

void RunClient()
{
    try
    {
        while (ClientWorker()) {}
       
        Console.WriteLine("Client Finished.");
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}

bool ClientWorker()
{
    var ipAddress = IPAddress.Loopback;
    var endpoint = new IPEndPoint(ipAddress, port);

    using var sender = new Socket(
        ipAddress.AddressFamily, 
        SocketType.Stream, 
        ProtocolType.Tcp);

    sender.Connect(endpoint);
    Console.WriteLine($"Client Connected: {sender.RemoteEndPoint}");

    Console.WriteLine("Read message: ");
    var msg = Console.ReadLine();

    var bytes = Encoding.UTF8.GetBytes(msg);
    sender.Send(bytes);

    var buffer = new byte[1024];
    var receivedBytes = sender.Receive(buffer);
    
    Console.WriteLine($"Received: {receivedBytes}");
    var response = Encoding.UTF8.GetString(
        buffer,
        0,
        receivedBytes);

    Console.WriteLine($"Server responded: {response}");
    sender.Shutdown(SocketShutdown.Both);
    sender.Close();

    return !response.Equals(stopMessage);
}