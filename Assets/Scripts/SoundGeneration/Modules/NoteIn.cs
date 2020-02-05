namespace Assets.Scripts.SoundGeneration.Modules
{
    public class NoteIn : DAAudioFilter
    {

        public static string MenuEntry() => MenuEntryProvider.Get("Note In");

        public Attribute frequency;

        public Attribute button;

        public override void InitializeAttributes()
        {
            frequency = AddAttribute("Frequency", b => b.WithType(Attribute.AttributeType.FREQUENCY));
            button = AddAttribute("Trigger", b => b.WithType(Attribute.AttributeType.BUTTON));
        }

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            if (!button.pressed)
                return 0f;

            return frequency.value;
        }
    }
}