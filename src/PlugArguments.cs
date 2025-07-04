﻿namespace TeamsPlug
{
    internal class PlugArguments
    {
        internal bool ShowUsage
        {
            get { return mShowUsage; }
        }

        internal string WebSocketUrl
        {
            get { return mWebSocketUrl; }
        }

        internal string BotName
        {
            get { return mBotName; }
        }

        internal string ApiKey
        {
            get { return mApiKey; }
        }

        internal string Organization
        {
            get { return mOrganization; }
        }

        internal string ConfigFilePath
        {
            get { return mConfigFilePath; }
        }

        internal string BasePath
        {
            get { return mBasePath; }
        }

        internal PlugArguments(string[] args)
        {
            mArgs = args;
        }

        internal bool Parse()
        {
            return LoadArguments(mArgs);
        }

        bool LoadArguments(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                mShowUsage = true;
                return false;
            }

            bool bValidArgs = true;
            for (int i = 0; i < args.Length; i++)
            {
                if (!bValidArgs)
                    return false;

                if (args[i] == null)
                {
                    continue;
                }

                if (IsUsageArgument(args[i]))
                {
                    mShowUsage = true;
                    return true;
                }

                if (args[i] == WEB_SOCKET_URL_ARG)
                {
                    bValidArgs = ReadArgumentValue(args, ++i, out mWebSocketUrl);
                    continue;
                }

                if (args[i] == BOT_NAME_ARG)
                {
                    bValidArgs = ReadArgumentValue(args, ++i, out mBotName);
                    continue;
                }

                if (args[i] == API_KEY_ARG)
                {
                    bValidArgs = ReadArgumentValue(args, ++i, out mApiKey);
                    continue;
                }

                if (args[i] == ORGANIZATION_KEY_ARG)
                {
                    bValidArgs = ReadArgumentValue(args, ++i, out mOrganization);
                    continue;
                }

                if (args[i] == CONFIG_FILE_ARG)
                {
                    bValidArgs = ReadArgumentValue(args, ++i, out mConfigFilePath);
                    continue;
                }
            }

            return bValidArgs;
        }

        bool ReadArgumentValue(string[] args, int argIndex, out string value)
        {
            value = string.Empty;
            if (argIndex >= args.Length)
                return false;

            value = args[argIndex].Trim();

            foreach (string validArgName in VALID_ARGS_NAMES)
                if (value.Equals(validArgName))
                    return false;

            return true;
        }

        static bool IsUsageArgument(string argument)
        {
            foreach (string validHelpArg in VALID_HELP_ARGS)
                if (argument == validHelpArg)
                    return true;

            return false;
        }

        string[] mArgs;

        string mBotName;
        string mApiKey;
        string mOrganization;
        string mConfigFilePath;
        string mWebSocketUrl;

        bool mShowUsage = false;

        static string[] VALID_HELP_ARGS = new string[] { "--help", "-h", "--?", "-?" };

        const string WEB_SOCKET_URL_ARG = "--server";
        const string BOT_NAME_ARG = "--name";
        const string API_KEY_ARG = "--apikey";
        const string ORGANIZATION_KEY_ARG = "--organization";
        const string CONFIG_FILE_ARG = "--config";

        static string[] VALID_ARGS_NAMES = new string[] {
            WEB_SOCKET_URL_ARG,
            BOT_NAME_ARG,
            API_KEY_ARG,
            CONFIG_FILE_ARG};
    }
}
