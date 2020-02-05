using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;

namespace Assets.Scripts.SoundGeneration.Modules
{
    class SequencerX8 : SequencerBase
    {
        public static string MenuEntry() => MenuEntryProvider.Sequencer("Simple Sequencer (8)");

        protected override int NotesCount => 8;
    }

}
