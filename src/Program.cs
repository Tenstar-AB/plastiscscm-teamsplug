using log4net;
using System.Net;
using System.Reflection;
using TeamsPlug;

ILog mLog = LogManager.GetLogger("teamsplug");

//TeamsAPI debugTeamsAPI = new TeamsAPI("<flow url>"); // Since the TeamsAPI class works without any other parts we can create one here to send debug messages during the setup of the plug
//await debugTeamsAPI.PostMessage($"[TeamsPlug] Starting up...", "<user.name>@<email.com>"); // Send a message when the program starts to verify that the flow works

try
{
    PlugArguments plugArgs = new PlugArguments(args);

    bool bValidArgs = plugArgs.Parse();

    ConfigureLogging(plugArgs.BotName);

    mLog.InfoFormat("SlackPlug [{0}] started. Version [{1}]",
        plugArgs.BotName,
        System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

    string argsStr = args == null ? string.Empty : string.Join(" ", args);
    mLog.DebugFormat("Args: [{0}]. Are valid args?: [{1}]", argsStr, bValidArgs);

    if (!bValidArgs || plugArgs.ShowUsage)
    {
        PrintUsage();
        return 0;
    }

    CheckArguments(plugArgs);
    Config config = ParseConfig.Parse(File.ReadAllText(plugArgs.ConfigFilePath));
    TeamsNotification.email = config.EmailDomain;

    mLog.DebugFormat("Starting ws...");

    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

    WebSocketRequest request = new WebSocketRequest(config.FlowURL, config.EmailDomain);

    WebSocketClient ws = new WebSocketClient(
        plugArgs.WebSocketUrl,
        "notifierPlug",
        plugArgs.BotName,
        plugArgs.ApiKey,
        plugArgs.Organization,
        request.ProcessMessage);

    ws.ConnectWithRetries();

    Task.Delay(-1).Wait();

    return 0;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    mLog.ErrorFormat("Error: {0}", ex.Message);
    mLog.DebugFormat("StackTrace: {0}", ex.StackTrace);
    return 1;
}

static void CheckArguments(PlugArguments plugArgs)
{
    CheckAgumentIsNotEmpty(
        "Plastic web socket url endpoint",
        plugArgs.WebSocketUrl,
        "web socket url",
        "--server wss://blackmore:7111/plug");

    CheckAgumentIsNotEmpty("name for this bot", plugArgs.BotName, "name", "--name teams");
    CheckAgumentIsNotEmpty("connection API key", plugArgs.ApiKey, "api key",
        "--apikey 014B6147A6391E9F4F9AE67501ED690DC2D814FECBA0C1687D016575D4673EE3");
    CheckAgumentIsNotEmpty("JSON config file", plugArgs.ConfigFilePath, "file path",
        "--config teams-config.conf");
}
static void ConfigureLogging(string plugName)
{
    if (string.IsNullOrEmpty(plugName))
        plugName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm");

    try
    {
        string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string logOutputPath = Path.Join(basePath, "\\logs", "teamsplug." + plugName + ".log.txt");
        Console.WriteLine($"{logOutputPath}");
        string log4netpath = LogConfig.GetLogConfigFile(basePath);
        log4net.GlobalContext.Properties["LogOutputPath"] = logOutputPath;
        log4net.Config.XmlConfigurator.Configure(new FileInfo(Path.Join(basePath, "teamsplug.log.conf")));
    }
    catch
    {
        //it failed configuring the logging info; nothing to do.
    }

}
static void CheckAgumentIsNotEmpty(string fielName, string fieldValue, string type, string example)
{
    if (!string.IsNullOrEmpty(fieldValue))
        return;
    string message = string.Format("teamsplug can't start without specifying a {0}.{1}" +
        "Please type a valid {2}. Example:  \"{3}\"",
        fielName, Environment.NewLine, type, example);
    throw new Exception(message);
}

static void PrintUsage()
{
    Console.WriteLine("Usage:");
    Console.WriteLine("\tteamsplug.exe --server <WEB_SOCKET_URL> --config <JSON_CONFIG_FILE_PATH>");
    Console.WriteLine("\t              --apikey <WEB_SOCKET_CONN_KEY> --name <PLUG_NAME>");
    Console.WriteLine();
    Console.WriteLine("Example:");
    Console.WriteLine("\tteamsplug.exe --server wss://blackmore:7111/plug --config teams-config.conf ");
    Console.WriteLine("\t              --apikey x2fjk28fda --name teams");
    Console.WriteLine();
}

