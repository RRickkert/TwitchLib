using System;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using TwitchLib.Services.CommandService;
using TwitchLib.Services.CommandService.Commands;
using TwitchLib.Services.CommandService.Modules;
using NetCore.Client.Modules.Utilities;

namespace NetCore.Client
{
    class Program
    {
        private const string _username = "";
        private const string _oauth = "oauth:";
        private const string _clientId = "";
        private const string _secret = "";
        public static string Channel = "prom3theu5";
        public static ITwitchAPI Api;
        private static ITwitchClient _client;
        private static ICommandServiceClient _csc;

        static void Main(string[] args)
        {
            SetupAndConnectBot().GetAwaiter().GetResult();
        }

        private static async Task SetupAndConnectBot()
        {
            #region Api
            Api = new TwitchAPI();
            Api.Settings.Validators.SkipAccessTokenValidation = true;
            Api.Settings.Validators.SkipDynamicScopeValidation = true;
            Api.Settings.ClientId = _clientId;
            Api.Settings.AccessToken = _secret;
            #endregion

            await Task.Run(() =>
            {
                Log.Logger = new LoggerConfiguration()
                             .WriteTo.Console(theme: AnsiConsoleTheme.Grayscale)
                             .CreateLogger();

                #region Client Setup
                var credentials = new ConnectionCredentials(_username, _oauth);
                var logFactory = new TwitchLib.Logging.Providers.SeriLog.SerilogFactory();
                _client = new TwitchClient(credentials, channel: Channel, logging: true, logger: new TwitchLib.Logging.Providers.SeriLog.SerilogLogger(Log.Logger, logFactory));
                _client.OnConnectionError += _client_OnConnectionError;

                _client.ChatThrottler = new TwitchLib.Services.MessageThrottler(_client, 60, TimeSpan.FromSeconds(60));
                _client.WhisperThrottler = new TwitchLib.Services.MessageThrottler(_client, 30, TimeSpan.FromSeconds(30));

                _client.OnConnected += (s, e) => {
                    _client.ChatThrottler.StartQueue();
                    _client.WhisperThrottler.StartQueue();
                };

                _client.OnDisconnected += (s, e) => {
                    _client.ChatThrottler.StopQueue();
                    _client.WhisperThrottler.StopQueue();
                };
                #endregion

                #region CommandService Setup
                var commandService = new CommandService(new CommandServiceConfigBuilder
                {
                    CustomPrefixHandler = m => 0,
                    ErrorHandler = (s, e) =>
                    {
                        if (string.IsNullOrWhiteSpace(e.Exception?.Message))
                            return;
                        try
                        {
                            _client.SendMessage(e.Exception.Message);
                            Log.Error($"Error in Command: {e.Exception.Message}");
                        }
                        catch { }
                    },
                    ExecuteHandler = (s, e) =>
                    {
                        Log.Information("Command Executed: {Command} - Args: {Args} - User: {UserId}:{User}", e.Command.Text, e.Args, e.Message.UserId, e.Message.Username);
                    }
                });

                _csc = new CommandServiceClient(_client);
                _csc.AddService(commandService);
                var modules = _csc.AddService(new ModuleService());
                modules.Add(new UtilitiesModule(_csc), "Utilities");

                #endregion

                _client.Connect();

                bool running = true;
                while (running)
                {
                    var line = Console.ReadLine();
                    switch (line)
                    {
                        case "!exit":
                            running = false;
                            break;
                    }
                }
            });
        }

        private static void _client_OnConnectionError(object sender, TwitchLib.Events.Client.OnConnectionErrorArgs e)
        {
            throw e.Error.Exception.GetBaseException();
        }
    }
}
