using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ReferenceBrowserApp.Data;
//using Windows.Storage.Streams;
using ReferenceBrowserApp.Models;

namespace ReferenceBrowserApp.Services;

public class DatabaseSyncServerService
{
    ReferenceSearchItemDatabase _database;

    IPEndPoint _clientEndPoint;

    Socket _listener = null;
    Socket _handler = null;

    public DatabaseSyncServerService(ReferenceSearchItemDatabase database, NetworkInfoService networkInfo, int port = 11775)
    {
        _database = database;

        IPAddress ipAddress = IPAddress.Parse(networkInfo.MyIPAddress);

        _clientEndPoint = new(ipAddress, port);
    }

    async public Task Invoke()
    {
        _listener = new Socket(_clientEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        _listener.Bind(_clientEndPoint);
        _listener.Listen(1000);

        // Listening...
        try
        {
            _handler = await _listener.AcceptAsync();

            // preparation for sync 
            await _database.PrepareToSync();

            // update: client -> server
            string response;

            response = await AwaitClientResponse(_handler);

            int n = Int32.Parse(response);

            await ReplyToClient(_handler);

            for (int i=0; i<n; i++)
            {

                response = await AwaitClientResponse(_handler);

                string uriString = response;

                await ReplyToClient(_handler);


                response = await AwaitClientResponse(_handler);

                int minorCounts = Int32.Parse(response);

                await ReplyToClient(_handler);

                // store 
                var uri = new SearchUri(uriString);

                await _database.TryPushSearchUri(uri, minorCounts);

            }

            // update: server -> client
            var list = _database.GetSearchItemInfos();

            int m = list.Count;

            _ = await _handler.SendAsync(Encoding.UTF8.GetBytes($"{m}"), SocketFlags.None);

            await AwaitClientResponse(_handler);

            for (int i = 0; i < m; i++)
            {
                _ = await _handler.SendAsync(Encoding.UTF8.GetBytes(list[i].UriString), SocketFlags.None);

                await AwaitClientResponse(_handler);

                _ = await _handler.SendAsync(Encoding.UTF8.GetBytes($"{list[i].MinorCount}"), SocketFlags.None);

                await AwaitClientResponse(_handler);
            }
            
        }
        catch (Exception ex)
        {

        }

        _handler.Close();
        _listener.Close();

        Invoke();
    }

    async private Task ReplyToClient(Socket handler)
    {
        await handler.SendAsync(Encoding.UTF8.GetBytes("OK"), SocketFlags.None);
    }

    async private Task<string> AwaitClientResponse(Socket handler)
    {
        var buffer = new byte[1024];
        var recieved = await handler.ReceiveAsync(buffer, SocketFlags.None);
        var response = Encoding.UTF8.GetString(buffer, 0, recieved);

        return response;
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
