namespace Assets.Scripts.SoundGeneration.Modules
{
    class SequencerX3 : SequencerBase
    {
        public static string MenuEntry() => MenuEntryProvider.Sequencer("Simple Sequencer (3)");

        protected override int NotesCount => 3;
    }
}