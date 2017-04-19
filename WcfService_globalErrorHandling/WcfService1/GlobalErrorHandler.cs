using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;

namespace WcfService1
{
    public class GlobalErrorHandler : IErrorHandler
    {
        /// <summary>
        /// The method that's get invoked if any unhandled exception raised in service
        /// Here you can do what ever logic you would like to. For example logging the exception details
        /// Here the return value indicates that the exception was handled or not
        /// Return true to stop exception propegation and system considers that the exception was handled properly
        /// else return false to abort the session
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {
            Console.WriteLine("HandleError called.");
            return true;
        }

        /// <summary>
        /// If you want to communicate the exception details to the service client as proper fault message
        /// here is the place to do it
        /// If we want to suppress the communication about the exception, set fault to null
        /// </summary>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
		public void ProvideFault(Exception error,
							  System.ServiceModel.Channels.MessageVersion version,
							  ref System.ServiceModel.Channels.Message fault)
		{
			var ServiceFile = error.TargetSite.DeclaringType.FullName;
			var MethodName = error.TargetSite.Name;
			var ErrorMessage = error.Message;
			var StackTrace = error.StackTrace;
			var ExceptionType = error.GetType().Name;
			var LineNumber = (new System.Diagnostics.StackTrace(error, true)).GetFrame(0).GetFileLineNumber();

			var newEx = new FaultException(
			string.Format("Exception caught at Service Application GlobalErrorHandler ServiceFile: {0}, Method: {1}, LineNumber: {2},ErrorMessage:{3}, ExceptionType: {4}",
			ServiceFile, MethodName, LineNumber, ErrorMessage,  ExceptionType));
			var s = Environment.CurrentDirectory;
			MessageFault msgFault = newEx.CreateMessageFault();
			fault = Message.CreateMessage(version, msgFault, newEx.Action);
		}
	}
}