namespace OOAdvantech.Remoting.RestApi
{
    /// <MetaDataID>{100afa59-cf7c-408f-9456-47f429e4153c}</MetaDataID>
    public class RestApiExceptionData
    {

        public RestApiExceptionData()
        {
        }

        public RestApiExceptionData(ExceptionCode exceptionCode,System.Exception exception)
        {
            ExceptionCode = exceptionCode;
            if (exception != null)
            {
                ExceptionMessage = exception.Message;
                ServerStackTrace = exception.StackTrace;
            }
            else
            {

            }
        }

        /// <MetaDataID>{75190e5e-7800-4e4d-bfbd-fdfe6cb4f1a4}</MetaDataID>
        public string ExceptionMessage;

        /// <MetaDataID>{1527262a-2f06-4bbd-97ff-cb5a7c682fc7}</MetaDataID>
        public ExceptionCode ExceptionCode;

        public string ServerStackTrace;
    }

    /// <MetaDataID>{8f1c7bdf-99d4-4d98-a423-55a77f116904}</MetaDataID>
    public enum ExceptionCode
    {
        ServerError = 1,
        AccessTokenExpired = 12,
        ConnectionError = 2
    }

    /// <MetaDataID>{15b03d23-dc51-4586-883e-060fdb44c9b8}</MetaDataID>
    public class EndpointNotFoundException : System.Exception
    {


        public EndpointNotFoundException()
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message.
        //
        // Parameters:
        //   message:
        //     The message that describes the error.
        public EndpointNotFoundException(string message) : base(message)
        {

        }
        //
        // Summary:
        //     Initializes a new instance of the System.Exception class with a specified error
        //     message and a reference to the inner exception that is the cause of this exception.
        //
        // Parameters:
        //   message:
        //     The error message that explains the reason for the exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public EndpointNotFoundException(string message, System.Exception innerException) : base(message, innerException)
        {

        }
    }
}