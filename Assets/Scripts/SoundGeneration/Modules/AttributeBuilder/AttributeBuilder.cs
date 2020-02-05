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

        public AttributeBuilder WithType(Attribute.AttributeType type)
        {
            _item.type = type;
            return this;
        }

        public AttributeBuilder WithValue(float value)
        {
            _item.value = value;
            return this;
        }

        public AttributeBuilder WithHiddenValue(bool isHidden)
        {
            _item.hiddenValue = isHidden;
            return this;
        }

        public AttributeBuilder WithInput(bool allowInput)
        {
            _item.allowInput = allowInput;
            return this;
        }

        public AttributeBuilder WithMinValue(float minValue)
        {
            _item.clampMinimum = true;
            _item.clampMinimumValue = minValue;
            return this;
        }

        public AttributeBuilder WithMaxValue(float maxValue)
        {
            _item.clampMaximum = true;
            _item.clampMaximumValue = maxValue;
            return this;
        }


        public AttributeBuilder Slider(float min, float max)
        {
            return this.WithMinValue(min).WithMaxValue(max).WithType(Attribute.AttributeType.SLIDER);
        }
        //public AttributeBuilder
        //{

        //}

    }
}
