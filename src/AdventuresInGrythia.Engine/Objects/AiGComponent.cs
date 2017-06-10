using AdventuresInGrythia.Engine.Actions;
using MoonSharp.Interpreter;

namespace AdventuresInGrythia.Engine.Objects
{
    public class AiGComponent
    {
        private readonly Script _script;
        public string Name {get;}
        public AiGEntity Owner { get; private set; }
        public bool IsActive {get; set;}
        public AiGComponent(AiGEntity owner, string name, Script script)
        {
            Owner = owner;
            Name = name;
            _script = script;
        }

        public void OnAttach()
        {
          //_script.Call(_script.Globals["onAttach"]);
            //TODO: notify observers of attachment?
        }

        // public bool Query(GameAction action)
        // {
        //     if (!IsActive) return true;
        //     return _script.Call(_script.Globals["query"], action).Boolean;
        // }

        // public bool Consider(GameAction action)
        // {
        //     if (!IsActive) return true;
        //     return _script.Call(_script.Globals["consider"], action).Boolean;
        // }

        // public void Notify(GameAction action)
        // {
        //     if (!IsActive) return;
        //     _script.Call(_script.Globals["notify"], action);
        // }
        public virtual bool DoAction(AiGAction action)
        {
            return _script.Call(_script.Globals["do"], action).Boolean;
        }
        
        public virtual void Tick(long elapsed)
        {
            if (!IsActive) return;
           _script.Call(_script.Globals["tick"], elapsed);
        }
    }
}