using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class WebSocketServer
{
    private readonly ConcurrentDictionary<Guid, WebSocket> _webSockets = new ConcurrentDictionary<Guid, WebSocket>();
    public String Message;

    public async Task RunServerAsync(string uri)
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(uri);
        listener.Start();
        Console.WriteLine($"WebSocket server running at {uri}");
        Thread notifyThread = new Thread(

            async delegate ()
        {
            while (true)
            {
                HttpListenerContext context = await listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    ProcessWebSocketRequestAsync(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
        }
            );
        notifyThread.Start();
    }


    private async Task ProcessWebSocketRequestAsync(HttpListenerContext context)
    {
        WebSocketContext webSocketContext = null;
        try
        {
            webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
            Guid webSocketId = Guid.NewGuid();
            _webSockets.TryAdd(webSocketId, webSocketContext.WebSocket);
            Console.WriteLine($"WebSocket connection established from {context.Request.RemoteEndPoint}, ID: {webSocketId}");

            await SendStringAsync(webSocketContext.WebSocket, "Welcome to the WebSocket server!");

            byte[] buffer = new byte[1024];
            while (webSocketContext.WebSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = await webSocketContext.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocketContext.WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                else
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Received message from {context.Request.RemoteEndPoint}, ID: {webSocketId}: {message}");

                    
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket error from {context.Request.RemoteEndPoint}: {ex.Message}");
        }
        finally
        {
            if (webSocketContext != null)
            {
                Guid webSocketId = _webSockets.FirstOrDefault(x => x.Value == webSocketContext.WebSocket).Key;
                if (_webSockets.TryRemove(webSocketId, out WebSocket _))
                {
                    Console.WriteLine($"WebSocket connection closed for ID: {webSocketId}");
                }
                webSocketContext.WebSocket.Dispose();
            }
        }
    }

    public async Task BroadcastMessageAsync(string message)
    {
        foreach (WebSocket webSocket in _webSockets.Values)
        {
            await SendStringAsync(webSocket, message);
            Console.WriteLine(message);
        }
    }

    private async Task SendStringAsync(WebSocket webSocket, string message)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public  void EndWebServer()
    {
        foreach (WebSocket webSocket in _webSockets.Values)
        {
            webSocket.Abort();
        }
    }
}