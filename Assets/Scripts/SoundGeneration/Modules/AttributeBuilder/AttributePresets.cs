using DarkArtsStudios.SoundGenerator.Module;

namespace Assets.Scripts.SoundGeneration.AttributeBuilder
{
    static class AttributePresets
    {
        public static BaseModule.Attribute FloatInput(string name = "Input")
        {
            return AttributeBuilder.Create(name).Build();
        }
    }
}
