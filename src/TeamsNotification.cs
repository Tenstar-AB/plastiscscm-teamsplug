namespace TeamsPlug
{
    static class TeamsNotification
    {
        internal static string email = "@email.com";
        private static TeamsAPI teamsAPI;
        async internal static Task Notify(string message, List<string> recipients, string flowUrl)
        {
            if(teamsAPI == null)
            {
                teamsAPI = new TeamsAPI(flowUrl);
            }
            for (int i = 0; i < recipients.Count; i++)
            {
                string recipient = recipients[i];
                // The plastic bot only knows about the user name so if its missing email part add it
                if(!recipient.Contains(email))
                {
                    recipient = $"{recipient}{email}";
                }
                teamsAPI.PostMessage(message, recipient);
            }
        }
    }
}
