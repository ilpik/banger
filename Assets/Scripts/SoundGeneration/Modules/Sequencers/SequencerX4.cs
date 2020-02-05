namespace Assets.Scripts.SoundGeneration.Modules
{
    class SequencerX4 : SequencerBase
    {
        public static string MenuEntry() => MenuEntryProvider.Sequencer("Simple Sequencer (4)");

        protected override int NotesCount => 4;
    }
}