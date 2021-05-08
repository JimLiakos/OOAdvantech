using System;
using System.Collections.Generic;
using System.Text;

namespace OOAdvantech
{
    /// <MetaDataID>{fad9ea6d-57eb-4c4d-8477-659391aec215}</MetaDataID>
    public class EventUnderProtection
    {
        /// <summary>
        /// The options of this enumerator define how invoke method will handle the exception from an event handler. 
        /// </summary>
        public enum ExceptionHandling
        { 
            /// <summary>
            /// By default invoke method propagate the exception to the caller.  
            /// </summary>
            Default = 0x0, 
            /// <summary>
            /// The method EventUnderProtection.Invoke<> ignore the exceptions and 
            /// continue with the next event handler in invocation list. 
            /// Else continue with the next event handler in invocation list 
            /// but store the exception in exceptions stack and 
            /// when finish with the event handler’s invocation throw a Multicast exception 
            /// with all catched exceptions. 
            /// </summary>
            IgnoreExceptions = 0x1,
            /// <summary>
            /// The method EventUnderProtection.Invoke<> removes from the invocation list the event handler which throws exception.
            /// </summary>
            RemoveEventHandlersWithException = 0x2,
            /// <summary>
            /// The method EventUnderProtection.Invoke<> removes from the invocation list the event handler which is out of process 
            /// and the system has lost the connection.
            /// </summary>
            RemoveDisconnectedEventHandlers = 0x4,

            /// <summary>
            /// The method EventUnderProtection.Invoke<> catch the exceptions and throws the masively at the end as MulticastException 
            /// </summary>
            /// <see cref="MulticastExeption"/>
            CatchExceptions=0x8
        }
        /// <summary>
        /// This structure defines an exception - event handler pair.
        /// </summary>
        public struct EventHandlerException
        {
            public readonly Exception Exception;
            public readonly Delegate EventHandler;
            public EventHandlerException(Delegate eventHandler, Exception exception)
            {
                EventHandler = eventHandler;
                Exception = exception;
            }


        }
        /// <summary>
        /// This class defines an exception which contains all exceptions where cached through an event call. 
        /// </summary>
        public class MulticastExeption : System.Exception
        {
            /// <summary>
            /// This field defenes a collection with  exception - event handler pairs.
            /// </summary>
            public System.Collections.Generic.List<EventHandlerException> Exceptions;
            public MulticastExeption(string message, System.Collections.Generic.List<EventHandlerException> exceptions)
            {
                Exceptions = exceptions;
            }
        }
        /// <MetaDataID>{118e98fe-cde0-4453-afa7-a92e3bcc2679}</MetaDataID>
        static void Test(int? las)
        {
            

        }
        /// <summary>
        /// This method raise event smartly. 
        /// With this method a caller code ensure that 
        /// all event handlers where assigned to the event will be called. 
        /// Throw a multicast exception with the cached exceptions if needed. 
        /// Remove the event handlers for invocation list if needed.   
        /// </summary>
        /// <typeparam name="T">Defines the type of event </typeparam>
        /// <param name="_event">Defines the event</param>
        /// <param name="exceptionHandling">Defines the way where the method hadle the exception </param>
        /// <param name="eventParams">Defines the parameters of event</param>
        /// <MetaDataID>{866e07af-0f22-4f7c-9fbd-5b25d788a8ba}</MetaDataID>
        public static void Invoke<T>(ref T _event, ExceptionHandling exceptionHandling, params object[] eventParams) 
        {

            if (_event == null)
                return;
            Test(1);
            System.Delegate multicastEvent = (System.Delegate)(object)_event;

            if (multicastEvent != null)
            {
                System.Collections.Generic.List<EventHandlerException> exceptions = null;

                foreach (System.Delegate eventHandler in multicastEvent.GetInvocationList())
                {
                    try
                    {

                        eventHandler.DynamicInvoke(eventParams);


                    }
                    catch (System.Exception error)
                    {
                        if (exceptionHandling == ExceptionHandling.Default)
                            throw error;
                        if (((int)(exceptionHandling & ExceptionHandling.IgnoreExceptions)) != 0)
                        {

                            if (((int)(exceptionHandling & ExceptionHandling.RemoveEventHandlersWithException)) != 0)
                                _event = (T)((object)System.Delegate.Remove(_event as System.Delegate, eventHandler));
                            #region Romove event handler when disconnectonected option
#if !DeviceDotNet 
                            if (((int)(exceptionHandling & ExceptionHandling.RemoveDisconnectedEventHandlers)) != 0)
                            {
                                if (error is System.Runtime.Remoting.RemotingException)
                                    _event = (T)((object)System.Delegate.Remove(_event as System.Delegate, eventHandler));
                                System.Exception remotingException = error;
                                while (remotingException != null && !(remotingException is System.Runtime.Remoting.RemotingException))
                                {
                                    remotingException = remotingException.InnerException;
                                    if (remotingException is System.Runtime.Remoting.RemotingException||remotingException is System.Net.Sockets.SocketException)
                                    {
                                        _event = (T)((object)System.Delegate.Remove(_event as System.Delegate, eventHandler));
                                        break;
                                    }
                                }
                            }
#endif
                            #endregion

                        }
                        else
                        { 

                            if (exceptions == null)
                                exceptions = new List<EventHandlerException>();
                            exceptions.Add(new EventHandlerException(eventHandler, error));

                            if (((int)(exceptionHandling & ExceptionHandling.RemoveEventHandlersWithException)) != 0)
                                _event = (T)((object)System.Delegate.Remove(_event as System.Delegate, eventHandler));
                            #region Romove event handler when disconnectonected option
#if !DeviceDotNet 
                            if (((int)(exceptionHandling & ExceptionHandling.RemoveDisconnectedEventHandlers)) != 0)
                            {
                                if (error is System.Runtime.Remoting.RemotingException)
                                    _event = (T)((object)System.Delegate.Remove(_event as System.Delegate, eventHandler));
                                System.Exception remotingException = error;
                                while (remotingException != null && !(remotingException is System.Runtime.Remoting.RemotingException))
                                {
                                    remotingException = remotingException.InnerException;
                                    if (remotingException is System.Runtime.Remoting.RemotingException)
                                    {
                                        _event = (T)((object)System.Delegate.Remove(_event as System.Delegate, eventHandler));
                                        break;
                                    }
                                }
                            }
#endif
                            #endregion
                        }
                    }


                    if (exceptions != null)
                        throw new MulticastExeption("Exceptions on event handlers invocation", exceptions);
                }
            }

        }
    }
}
