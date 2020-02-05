namespace Assets.Scripts.SoundGeneration.Modules
{
    class SequencerX2 : SequencerBase
    {
        public static string MenuEntry() => MenuEntryProvider.Sequencer("Simple Sequencer (2)");

        protected override int NotesCount => 2;
    }
}