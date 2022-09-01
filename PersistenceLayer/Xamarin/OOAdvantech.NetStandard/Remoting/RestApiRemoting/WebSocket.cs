using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket4Net
{
    public class WebSocket
    {
        private string Uri;
        private readonly ClientWebSocket client;
        private CancellationTokenSource cts;

        public WebSocket(string uri)
        {
            Uri = uri;
            client = new ClientWebSocket();
            cts = new CancellationTokenSource();

        }
        public async Task Send(string message)
        {
            var byteMessage = Encoding.UTF8.GetBytes(message);
            var segmnet = new ArraySegment<byte>(byteMessage);

            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);

        }
        public event EventHandler Opened;

        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler Closed;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public void Close()
        {
            //Task.Run(async () =>
            //{
            client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token).Wait();
            this.Closed?.Invoke(this, EventArgs.Empty);

            //});

        }
        public void Open()
        {
            Task.Run(async () =>
            {
                try
                {
#if __IOS__
            await client.ConnectAsync(new Uri("ws://localhost:5000"), cts.Token);
#else
                    await client.ConnectAsync(new Uri(Uri), cts.Token);
#endif
                    await Task.Factory.StartNew(async () =>
                    {
                        while (true)
                        {
                            WebSocketReceiveResult result;
                            var message = new ArraySegment<byte>(new byte[4096]);
                            do
                            {

                                try
                                {
                                    result = await client.ReceiveAsync(message, cts.Token);
                                    //while (!taskResult.Wait(1000))
                                    //{
                                    //    if (client.State != WebSocketState.Open)
                                    //    {

                                    //    }
                                    //    else
                                    //    {

                                    //    }

                                    //}
                                }
                                catch (Exception error)
                                {
                                    this.Error?.Invoke(this, new ErrorEventArgs(error));
                                    throw;
                                }
                                //result = taskResult.Result;
                                var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                                string serialisedMessae = Encoding.UTF8.GetString(messageBytes);

                                try
                                {
                                    this.MessageReceived?.Invoke(this, new MessageReceivedEventArgs(serialisedMessae));
                                    //var msg = JsonConvert.DeserializeObject<Message>(serialisedMessae);
                                    //Messages.Add(msg);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Invalide message format. {ex.Message}");
                                }

                            } while (!result.EndOfMessage);
                        }
                    }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                }
                catch (Exception error)
                {

                    this.Error?.Invoke(this, new ErrorEventArgs(error));
                }

            }).Wait();
        }

        public WebSocketState State { get => client.State; }

    }
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
        public Exception Exception { get; private set; }
    }
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
