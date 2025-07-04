using log4net;

namespace TeamsPlug
{
    internal class WebSocketRequest
    {
        string flowUrl;
        string emailDomain;
        internal WebSocketRequest(string flowUrl, string emailDomain)
        {
            this.flowUrl = flowUrl;
            this.emailDomain = emailDomain;
        }

        internal string ProcessMessage(string rawMessage)
        {
            string requestId = Messages.GetRequestId(rawMessage);
            try
            {
                NotificationMessage message = Messages.ReadNotificationMessage(rawMessage);

                TeamsNotification.Notify(
                    message.Message, message.Recipients, flowUrl).Wait();

                return Messages.BuildSuccessfulResponse(requestId);
            }
            catch (Exception ex)
            {
                mLog.ErrorFormat("Error processing message:\n{0}. Error: {1}",
                    rawMessage, ex.Message);
                mLog.Debug(ex.StackTrace);
                return Messages.BuildErrorResponse(requestId, ex.Message);
            }
        }


        static readonly ILog mLog = LogManager.GetLogger("teamsplug");
    }
}
