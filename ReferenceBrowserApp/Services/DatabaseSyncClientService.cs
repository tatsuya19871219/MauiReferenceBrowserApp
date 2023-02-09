//using Android.Telephony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Services;

public class DatabaseSyncClientService
{
    Socket _socket;

    public DatabaseSyncClientService(IPAddress targetIPAddress, int port=11775) 
    {
        IPEndPoint remoteEP = new IPEndPoint(targetIPAddress, port);

        EvokeClient(remoteEP);
        
    }

    async void EvokeClient(IPEndPoint endPoint)
    {
        _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await _socket.ConnectAsync(endPoint);

            _ = await _socket.SendAsync(Encoding.UTF8.GetBytes("Hello. Hello."), SocketFlags.None);

            var buffer = new byte[1024];
            var recieved = await _socket.ReceiveAsync(buffer, SocketFlags.None);

            var response = Encoding.UTF8.GetString(buffer, 0, recieved);
        }
        catch (Exception ex)
        {
            //
        }
    }

    public void ShutdownClient()
    {

        _socket?.Close();

        _socket = null;
    }
}
