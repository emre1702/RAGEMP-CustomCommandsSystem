using CustomCommandSystem.Common;
using GTANetworkAPI;
using CustomCommandSystem.Common.Extensions;
using CustomCommandSystem.Common.Interfaces.Services;
using CustomCommandSystem.Common.Interfaces.Models;
using System.Diagnostics.CodeAnalysis;
using CustomCommandSystem.Common.Datas;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using CustomCommandSystem.Services.Utils;
using CustomCommandSystem.Common.Models;

namespace CustomCommandSystem.Services
{
    internal class CommandsHandler : ICommandsHandler
    {
        public static CommandsHandler? Instance { get; private set; }

        private readonly ICommandMessageCleaner _cleaner;
        private readonly ICommandParser _commandParser;
        private readonly ICommandArgumentsParser _argumentsParser;
        private readonly ICommandMethodExecuter _methodExecuter;
        private readonly ICommandMethodParser _methodParser;
        private readonly ICommandsConfiguration _configuration;
        private readonly ICommandsLoader _commandsLoader;

        public CommandsHandler(ICommandMessageCleaner cleaner, ICommandParser commandParser, ICommandArgumentsParser argumentsParser, ICommandsLoader commandsLoader,
             ICommandMethodExecuter methodExecuter, ICommandMethodParser methodParser, ICommandsConfiguration configuration)
        {
            _cleaner = cleaner;
            _commandParser = commandParser;
            _argumentsParser = argumentsParser;
            _methodExecuter = methodExecuter;
            _methodParser = methodParser;
            _configuration = configuration;
            _commandsLoader = commandsLoader;

            Instance = this;
        }

        public async void ExecuteCommand(Player player, string message)
        {
            message = _cleaner.Clean(message);
            var (command, remainingMessage) = _commandParser.Parse(message);
            if (!TryGetCommandData(player, command, out var _))
                return;

            var usedParameters = _argumentsParser.ParseUserArguments(remainingMessage);
            if (!TryGetPossibleMethods(player, command, usedParameters, out var possibleMethods))
                return;

            await _methodExecuter.TryExecuteSuitable(player, command, possibleMethods, usedParameters);
        }

        private bool TryGetCommandData(Player player, string cmd, [NotNullWhen(true)] out CommandData? commandData)
        {
            commandData = _commandsLoader.GetCommandData(cmd);

            if (commandData is null && _configuration.CommandDoesNotExistError is { } text)
                NAPI.Task.Run(() => player.SendChatMessage(text));

            return !(commandData is null);
        }

        private bool TryGetPossibleMethods(Player player, string command, string[] usedParameters, [NotNullWhen(true)] out List<CommandMethodData>? possibleMethods)
        {
            var methodCandidates = _methodParser.GetPossibleMethods(command);

            if (methodCandidates != null)
            {
                possibleMethods = _methodParser.FilterByArgsAmount(methodCandidates, usedParameters.Length).ToList();

                if (!possibleMethods.Any())
                {
                    if (methodCandidates.Any())
                    {
                        foreach (var method in methodCandidates)
                        {
                            string usageText = $"USAGE: /{command}";
                            foreach (var param in method.UserParameters)
                            {
                                usageText += $" [{param.Name}]";
                            }
                            NAPI.Task.Run(() => player.SendChatMessage(usageText));
                        }
                    }
                    else if (_configuration.CommandDoesNotExistError is { } text)
                    {
                        NAPI.Task.Run(() => player.SendChatMessage(text));
                    }
                    return false;
                }

                return true;
            }

            possibleMethods = null;
            return false;
        }
    }
}
