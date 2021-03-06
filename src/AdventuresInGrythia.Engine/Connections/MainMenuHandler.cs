using AdventuresInGrythia.Domain.Models;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AdventuresInGrythia.Engine.Objects;
using AdventuresInGrythia.Engine.Actions;
using AdventuresInGrythia.Engine.Managers;

namespace AdventuresInGrythia.Engine.Connections
{
    public class MainMenuHandler : ConnectionHandler
    {
        private enum MainMenuState
        {
            MainMenu = 1,
            ChoosingCharacter,
        }

        private MainMenuState _state;
        private List<AiGEntity> _characters;

        public MainMenuHandler(Connection c, Account a) : base(c, a)
        {
            try
            {
                _script = new Script();
                _script.Globals["Game"] = typeof(Game);
                _script.DoString(ScriptManager.Instance.GetScript(ScriptType.GameFlow, "main-menu"));
            }
            catch (KeyNotFoundException ex)
            {
                //Add logging
                throw ex;
            }
        }

        public override void Enter(params object[] args)
        {
            _state = MainMenuState.MainMenu;
            _characters = Game.Instance.LoadPlayerCharacters(_account.Id);
            _script.Globals["Characters"] = typeof(List<AiGEntity>);
            _script.Call(_script.Globals["enter"], _characters, _account.Id);
        }

        public override void Handle(string command)
        {
            switch (_state)
            {
                case MainMenuState.MainMenu:
                    if (command == "help" || command == "?")
                        Game.Instance.SendMessage(_account.Id, "You asked for <#orange>main help<#>.");
                    else if (command == "money")
                        Game.Instance.SendMessage(_account.Id, "You asked for <#lightgreen>main $MONEY<#>.");
                    else if (command == "c")
                    {
                        _connection.RemoveHandler();
                        _connection.AddHandler<NewCharacterHandler>();
                        _connection.Handler.Enter();
                    }
                    else if (command == "p")
                    {
                        _state = MainMenuState.ChoosingCharacter;
                        _script.Call(_script.Globals["printCharacters"]);
                    }
                    break;

                case MainMenuState.ChoosingCharacter:
                    int idx;
                    if (!int.TryParse(command, out idx))
                    {
                        if (command == "q")
                        {
                            _state = MainMenuState.MainMenu;
                            _connection.Handler.Enter();
                        }
                        else
                            Game.Instance.SendMessage(_account.Id, "Please only enter a number or press \"<#white>q<#>\" to go back.");
                        break;
                    }
                    AiGEntity chosenCharacter = null;
                    try
                    {
                        chosenCharacter = _characters[idx - 1];
                        if (chosenCharacter != null)
                        {
                            Game.Instance.SendMessage(_account.Id, $"You chose {chosenCharacter.Name}");
                            _connection.RemoveHandler();
                            _connection.AddHandler<GameplayHandler>();
                            _connection.Handler.Enter(chosenCharacter.Id);
                        }
                    }
                    catch(ArgumentOutOfRangeException)
                    {
                        Game.Instance.SendMessage(_account.Id, "Invalid selection.  Please try again.");
                    }
                    break;
            }
        }

        public override void Leave()
        {
            throw new NotImplementedException();
        }
    }
}
