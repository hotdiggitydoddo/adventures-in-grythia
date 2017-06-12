using System.Collections.Generic;
using AdventuresInGrythia.Engine.Containers;
using AdventuresInGrythia.Engine.Objects;
using MoonSharp.Interpreter;

namespace AdventuresInGrythia.Engine.Managers
{
    public class ComponentManager
    {
        private static readonly ComponentManager _instance = new ComponentManager();
        public static ComponentManager Instance => _instance;

        private Dictionary<string, string> _components;

        public ComponentManager()
        {
            _components = new Dictionary<string, string>();
        }

        public void RefreshAllComponents()
        {
            _components.Clear();
            _components = ScriptManager.Instance.GetComponentScripts();
        }

        public void AssignComponent(AiGEntity entity, string componentName, params AiGTrait[] defaults)
        {
            var script = new Script();
            script.Globals["Component"] = typeof(AiGComponent);
            script.Globals["Trait"] = typeof(AiGTrait);
            script.Globals["TraitSet"] = typeof(TraitSet);
            script.Globals["Entity"] = typeof(AiGEntity);
            script.DoString(_components[componentName]);

            var args = new Dictionary<string, string>();
            foreach (var trait in defaults)
                args.Add(trait.Name, trait.Value);

            var cmp = (AiGComponent)script.Call(script.Globals["init"], entity, script, args).UserData.Object;
            entity.Components.Add(cmp);
        }
    }
}