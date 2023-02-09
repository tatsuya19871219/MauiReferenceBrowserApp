using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
//using Java.Net;
//using Java.Net;

namespace ReferenceBrowserApp.Services;

public class DatabaseSyncServerService
{
    Socket _listener = null;
    Socket _handler = null;

    public DatabaseSyncServerService(NetworkInfoService networkInfo, int port = 11775)
    {
        IPAddress ipAddress = IPAddress.Parse(networkInfo.MyIPAddress);

        IPEndPoint localEndPoint = new(ipAddress, port);

        EvokeServer(localEndPoint);
    }

    async void EvokeServer(IPEndPoint endPoint)
    {
        _listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        _listener.Bind(endPoint);
        _listener.Listen(1000);

        // Listening...
        try
        {
            _handler = await _listener.AcceptAsync();

           
            var buffer = new byte[1024];
            var recieved = await _handler.ReceiveAsync(buffer, SocketFlags.None);

            var response = Encoding.UTF8.GetString(buffer, 0, recieved);

            _ = await _handler.SendAsync(Encoding.UTF8.GetBytes($"You said {response}"), SocketFlags.None);
        }
        catch (Exception ex)
        {

        }
    }

    public void ShutdownServer()
    {

        //_listener?.Shutdown(SocketShutdown.Both);
        _listener?.Close();

        //_handler?.Shutdown(SocketShutdown.Both);
        //_handler?.Close();

        _listener = null;
        //_handler = null;
    }
}
