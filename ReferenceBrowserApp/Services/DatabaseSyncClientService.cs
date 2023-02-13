using ReferenceBrowserApp.Data;
using ReferenceBrowserApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceBrowserApp.Services;

public class DatabaseSyncClientService
{
    ReferenceSearchItemDatabase _database;

    IPEndPoint _serverEndPoint;

    Socket _socket;

    public DatabaseSyncClientService(ReferenceSearchItemDatabase database, IPAddress targetIPAddress, int port=11775) 
    {
        _database = database;

        _serverEndPoint = new IPEndPoint(targetIPAddress, port);
    }
    
    async public Task Invoke()
    {
        _socket = new Socket(_serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            await _socket.ConnectAsync(_serverEndPoint);

            // Preparation
            var list = _database.GetSearchItemInfos();

            // Send number of items in database
            int n = list.Count;

            _ = await _socket.SendAsync(Encoding.UTF8.GetBytes($"{n}"), SocketFlags.None);

            await AwaitServerResponse(_socket);

            for (int i = 0; i < n; i++)
            {

                //_ = await _socket.SendAsync(Encoding.UTF8.GetBytes("Hello. Hello."), SocketFlags.None);
                
                _ = await _socket.SendAsync(Encoding.UTF8.GetBytes(list[i].UriString), SocketFlags.None);

                await AwaitServerResponse(_socket);

                _ = await _socket.SendAsync(Encoding.UTF8.GetBytes($"{list[i].MinorCount}"), SocketFlags.None);

                await AwaitServerResponse(_socket);

            }

            // Recieve number of items in detabase in server
            string response = await AwaitServerResponse(_socket);

            int m = Int32.Parse(response);

            await ReplyToServer(_socket);

            await _database.ClearDatabaseAsync();

            for (int i=0; i<m; i++)
            {
                response = await AwaitServerResponse(_socket);
                string uriString = response;

                await ReplyToServer(_socket);


                response = await AwaitServerResponse(_socket);

                int minorCounts = Int32.Parse(response);

                await ReplyToServer(_socket);

                // store 
                var uri = new SearchUri(uriString);

                await _database.TryPushSearchUri(uri, minorCounts);
            }

        }
        catch (Exception ex)
        {
            //
        }

        ShutdownClient();
    }

    async private Task ReplyToServer(Socket socket)
    {
        await socket.SendAsync(Encoding.UTF8.GetBytes("OK"), SocketFlags.None);
    }

    async private Task<string> AwaitServerResponse(Socket socket)
    {
        var buffer = new byte[1024];
        var recieved = await socket.ReceiveAsync(buffer, SocketFlags.None);

        var response = Encoding.UTF8.GetString(buffer, 0, recieved);

        return response;
    }

    public void ShutdownClient()
    {

        _socket?.Close();

        _socket = null;
    }
}
