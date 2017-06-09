using System.Collections.Generic;
using MoonSharp.Interpreter;

namespace AdventuresInGrythia.Engine.Managers
{
    public class CommandManager
    {
        Dictionary<int, List<string>> _entityCommands;
        Dictionary<string, Script> _commandSet;

        public CommandManager()
        {
            _commandSet = new Dictionary<string, Script>();
            _entityCommands = new Dictionary<int, List<string>>();
        }
        
    }
}