using System;
using System.Collections.Generic;
using System.Linq;
using AdventuresInGrythia.Domain.Contracts;
using AdventuresInGrythia.Domain.Models;
using AdventuresInGrythia.Engine.Actions;
using MoonSharp.Interpreter;

namespace AdventuresInGrythia.Engine.Managers
{
    public interface ICommandManager
    {
        void Init(Game game);
        void LoadCommandsForEntity(int entityId);
        void LoadCommandsSet();
    }
    public class CommandManager : ICommandManager
    {
        readonly IRepository<EntityCommand> _commandsRepo;
        Dictionary<int, List<string>> _entityCommands;
        Dictionary<string, Script> _commandsSet;
        Game _game;

        public bool IsInitialized { get; private set; }
        public CommandManager(IRepository<EntityCommand> commandsRepo)
        {
            _commandsSet = new Dictionary<string, Script>();
            _entityCommands = new Dictionary<int, List<string>>();
            _commandsRepo = commandsRepo;
        }

        public void Init(Game game)
        {
            _game = game;
            IsInitialized = true;
        }
        public void LoadCommandsForEntity(int entityId)
        {
            var dbCmds = _commandsRepo.Find(x => x.EntityId == entityId).Select(c => c.Command).ToList();
            if (_entityCommands.ContainsKey(entityId))
                _entityCommands[entityId] = dbCmds;
            else
                _entityCommands.Add(entityId, dbCmds);  
        }

        public void LoadCommandsSet()
        {
            _commandsSet.Clear();
            var allScripts = ScriptManager.Instance.GetCommandScripts();

            foreach (var kvp in allScripts)
            {
                var script = new Script();
                //script.Globals["Command"] = typeof(MudCommand);
                script.Globals["Game"] = typeof(Game);
                script.Globals["Action"] = typeof(AiGAction);

                try
                {
                    script.DoString(kvp.Value);

                }
                catch (Exception ex)
                {

                }
                _commandsSet.Add(kvp.Key, script);
            }
        }

        public void AssignCommand(int entityId, string cmdName)
        {
            if (!_entityCommands.ContainsKey(entityId))
            {
                _entityCommands.Add(entityId, new List<string> { cmdName });
                _commandsRepo.Add(new EntityCommand { EntityId = entityId, Command = cmdName });
            }
            else if (!_entityCommands[entityId].Contains(cmdName))
            {
                _entityCommands[entityId].Add(cmdName);
                _commandsRepo.Add(new EntityCommand { EntityId = entityId, Command = cmdName });
            }            
        }

        public void RevokeCommand(int entityId, string command)
        {
            if (!_entityCommands.ContainsKey(entityId)) return;
            if (_entityCommands[entityId].Contains(command))
            {
                _entityCommands[entityId].Remove(command);
                var dbCmd = _commandsRepo.Find(x => x.EntityId == entityId && x.Command == command).ToList();
                if (dbCmd.Any())
                    _commandsRepo.Remove(dbCmd[0].Id);
            }
        }
        public List<string> GetCommands(int entityId)
        {
            if (!_entityCommands.ContainsKey(entityId))
                return new List<string>();
            return new List<string>(_entityCommands[entityId]);
        }

        
        public void Process(int entityId, string input)
        {
            var parts = input.Trim().Split(' ').ToList();
            var verb = parts[0].ToLower();
            parts.RemoveAt(0);

            if (!Parse(entityId, ref verb))
                return;

            Execute(entityId, verb, string.Join(" ", parts));
        }

        private bool Parse(int entityId, ref string verb)
        {
            //this method should really return a custom object that has the command name and
            //typed and named arguments

            if (!_entityCommands.ContainsKey(entityId))
                return false;

            if (verb == "commands")
            {
                //need to communicate available commands to player.
                Game.Instance.DoAction(new AiGAction("infotoplayer", entityId, 0, string.Join(", ", GetCommands(entityId))));
                return false;
            }
            if (!_commandsSet.ContainsKey(verb))
                //Send error to client -- command doesn't exist
                return false;
            if (!_entityCommands[entityId].Contains(verb))
                //entity doesn't know this command
                return false;
            return true;
        }

        private void Execute(int entityId, string cmdName, params string[] args)
        {
            var script = _commandsSet[cmdName];
            try
            {
                script.Call(script.Globals["execute"], entityId, args);
            }
            catch (ScriptRuntimeException ex)
            {

            }
        }
    }
}