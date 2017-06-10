using System.Collections.Generic;
using System.Linq;
using AdventuresInGrythia.Engine.Actions;
using AdventuresInGrythia.Engine.Objects;

namespace AdventuresInGrythia.Engine.Containers
{
    public class ComponentSet
    {
        private List<AiGComponent> _components;
        private List<TimedAiGAction> _actionHooks;

        public int EntityId { get; }
        public int Count => _components.Count;


        public ComponentSet(int entity)
        {
            EntityId = entity;
            _components = new List<AiGComponent>();
            _actionHooks = new List<TimedAiGAction>();
        }

        public bool Has(string name)
        {
            return _components.Any(x => x.Name == name);
        }

        public AiGComponent Add(AiGComponent component)
        {
            if (Has(component.Name)) return null;

            _components.Add(component);
            //TODO: listeners notify that trait was added
            return component;
        }

        public AiGComponent Get(string name)
        {
            return _components.SingleOrDefault(x => x.Name == name);
        }

        public void Remove(string name)
        {
            var existing = _components.SingleOrDefault(x => x.Name == name);
            //TODO notify listners of trait removal
            _components.Remove(existing);
        }

        public AiGComponent[] GetAll()
        {
            return _components.ToArray();
        }

        public bool DoAction(AiGAction action)
        {
            foreach (var c in _components)
            {
                if (c.IsActive && !c.DoAction(action))
                    return false;
            }
            return true;
        }

        //Timed logic

        public void AddHook(TimedAiGAction hook)
        {
            _actionHooks.Add(hook);
        }

        public void RemoveHook(TimedAiGAction hook)
        {
            _actionHooks.Remove(hook);
        }

        public void ClearHooks()
        {
            for (int i = _actionHooks.Count - 1; i >= 0; i--)
            {
                _actionHooks[i].Unhook();
                _actionHooks.Remove(_actionHooks[i]);
            }
        }

        public void ClearComponentHooks(string componentName)
        {
            for (int i = _actionHooks.Count - 1; i >= 0; i--)
            {
                if (_actionHooks[i].Type == "messagecomponent" || _actionHooks[i].Type == "delcomponent")
                {
                    if (_actionHooks[i].Args[0] == componentName)
                    {
                        _actionHooks[i].Unhook();
                        _actionHooks.Remove(_actionHooks[i]);
                    }
                }
            }
        }

        public void KillHook(string actionType, string componentName)
        {
            for (int i = _actionHooks.Count - 1; i >= 0; i--)
            {
                if (_actionHooks[i].Type == actionType && _actionHooks[i].Args[0] == componentName)
                {
                    _actionHooks[i].Unhook();
                    _actionHooks.Remove(_actionHooks[i]);
                }
            }
        }
    }
}