
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WebSocket4Net
{
    /// <MetaDataID>{87534211-148c-432e-90a9-a29d9004a192}</MetaDataID>
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
            //binary webSocket
            //await client.SendAsync(segmnet, WebSocketMessageType.Binary, true, cts.Token);
            await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);

        }

        public async Task Send(byte[] buffer)
        {

            var segmnet = new ArraySegment<byte>(buffer);
            //binary webSocket
            await client.SendAsync(segmnet, WebSocketMessageType.Binary, true, cts.Token);
            //await client.SendAsync(segmnet, WebSocketMessageType.Text, true, cts.Token);

        }
        public event EventHandler Opened;

        public event EventHandler<ErrorEventArgs> Error;
        public event EventHandler Closed;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public void Close()
        {
            //Task.Run(async () =>
            //{
            try
            {
                client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", cts.Token).Wait();
            }
            catch (Exception error)
            {
            }


            //});

        }

        Task OpenTask;
        public void Open()
        {
            Task openTask = null;
            lock (this)
            {
                if (OpenTask != null && (OpenTask.Status == TaskStatus.Running || OpenTask.Status == TaskStatus.WaitingForActivation))
                    openTask = OpenTask;
                else OpenTask = null;

                if (openTask == null)
                {
                    openTask = Task.Run(async () =>
                    {
                        try
                        {
#if __IOS__
            await client.ConnectAsync(new Uri("ws://localhost:5000"), cts.Token);
#else

                            await client.ConnectAsync(new Uri(Uri), cts.Token);
                            if (client.State == WebSocketState.Open)
                                Opened?.Invoke(this, EventArgs.Empty);
                            else
                            {

                            }
#endif
                            await Task.Factory.StartNew(async () =>
                            {
#if DeviceDotNet
                                OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                                //if (device.StatusBarColor == Color.DarkViolet)
                                //    device.StatusBarColor = Color.LightSalmon;
#endif

                                while (true)
                                {
                                    WebSocketReceiveResult result;
                                    var message = new ArraySegment<byte>(new byte[4096]);
                                    System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

                                    var previousState = client.State;
                                    do
                                    {

                                        try
                                        {
                                            result = await client.ReceiveAsync(message, cts.Token);
                                        }
                                        catch (Exception error)
                                        {

                                            device.StatusBarColor = Color.DarkViolet;
#if DeviceDotNet
                                            OOAdvantech.DeviceApplication.Current.Log(new List<string> { "Native WebSocket Receive error : "+error.Message });
#endif

                                            this.Error?.Invoke(this, new ErrorEventArgs(error));

                                            if (previousState == WebSocketState.Open && client.State != WebSocketState.Open)
                                                this.Closed?.Invoke(this, EventArgs.Empty);
                                            throw;
                                        }
                                        //result = taskResult.Result;
                                        var messageBytes = message.Skip(message.Offset).Take(result.Count).ToArray();
                                        memoryStream.Write(messageBytes, 0, messageBytes.Length);
                                        if (result.CloseStatus == WebSocketCloseStatus.NormalClosure)
                                        {
                                            this.Closed?.Invoke(this, EventArgs.Empty);
                                            return;
                                        }
                                        if (result.CloseStatus != null)
                                        {

                                        }
                                        if (result.EndOfMessage)
                                        {
                                            if (result.MessageType==WebSocketMessageType.Text)
                                            {
                                                messageBytes = memoryStream.GetBuffer();
                                                memoryStream.Dispose();
                                                memoryStream = new System.IO.MemoryStream();

                                                try
                                                {
                                                    string serialisedMessae = Encoding.UTF8.GetString(messageBytes);
                                                    this.MessageReceived?.Invoke(this, new MessageReceivedEventArgs(serialisedMessae));
                                                    //var msg = JsonConvert.DeserializeObject<Message>(serialisedMessae);
                                                    //Messages.Add(msg);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Invalide message format. {ex.Message}");
                                                }
                                            }
                                            if (result.MessageType==WebSocketMessageType.Binary)
                                            {
                                                messageBytes = memoryStream.GetBuffer();
                                                memoryStream.Dispose();
                                                memoryStream = new System.IO.MemoryStream();

                                                try
                                                {
                                                    //string serialisedMessae = Encoding.UTF8.GetString(messageBytes);
                                                    this.DataReceived?.Invoke(this, new DataReceivedEventArgs(messageBytes));
                                                    //var msg = JsonConvert.DeserializeObject<Message>(serialisedMessae);
                                                    //Messages.Add(msg);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Invalide message format. {ex.Message}");
                                                }
                                            }
                                        }

                                    } while (!result.EndOfMessage);
                                }
                            }, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                        }
                        catch (Exception error)
                        {



                            this.Error?.Invoke(this, new ErrorEventArgs(error));
                        }

                    });
                }

                if (openTask != null && (openTask.Status == TaskStatus.Running || openTask.Status == TaskStatus.WaitingForActivation))
                    OpenTask = openTask;
                else OpenTask = null;
            }

        }

        public WebSocketState State { get => client.State; }

    }
    /// <MetaDataID>{b47cc5b7-8b74-48cd-9f73-6eb8b81bc1af}</MetaDataID>
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Exception exception)
        {
            Exception = exception;
        }
        public Exception Exception { get; private set; }
    }
    /// <MetaDataID>{81df97d6-9db2-4e82-9a15-db7aef1a2b0c}</MetaDataID>
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    /// <MetaDataID>{3d43cf74-6e52-4c8f-9266-686ba1ce229f}</MetaDataID>
    public class DataReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; private set; }

        public DataReceivedEventArgs(byte[] data)
        {
            Data = data;
        }
    }
}
