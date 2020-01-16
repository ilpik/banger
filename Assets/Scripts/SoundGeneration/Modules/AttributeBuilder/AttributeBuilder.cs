using Attribute = DarkArtsStudios.SoundGenerator.Module.BaseModule.Attribute;

namespace Assets.Scripts.SoundGeneration.AttributeBuilder
{
    public class AttributeBuilder
    {
        private Attribute _item;

        private AttributeBuilder(string name)
        {
            _item = new Attribute(name);
        }

        public static AttributeBuilder Create(string name) => new AttributeBuilder(name);

        public Attribute Build() => _item;

        //public AttributeBuilder
        //{

        //}

    }
}
