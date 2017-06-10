namespace AdventuresInGrythia.Engine.Objects
{
    public class AiGTrait
    {
        public string Name { get; }
        public string Value { get; set; }

        public AiGTrait(string name)
        {
            Name = name;
        }

        public AiGTrait(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}