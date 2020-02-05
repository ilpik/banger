using System.Collections.Generic;
using Assets.Scripts.SoundGeneration.Util;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration.Modules
{
    public abstract class SequencerBase : DAAudioFilter
    {
        protected List<Attribute> notes = new List<Attribute>();

        private Attribute length;

        protected abstract int NotesCount { get; }
        //private Attribute input;

        public override void InitializeAttributes()
        {
            base.InitializeAttributes();

            for (int i = 1; i <= NotesCount; i++)
            {
                int octave = 4;
                var note = i;
                notes.Add(AddAttribute($"Note {i}", b => b.WithType(Attribute.AttributeType.FREQUENCY)/*.WithValue(Music.Frequency(octave, note))*/));
            }

            length = AddAttribute("Note Length", b => b.WithValue(1));
            //input = AddInput();
        }

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            if (notes.Count == 0)
                return 0.0f;

            int note = MathUtil.FloorToInt(time / length.value) % notes.Count;

            //this means some time trick is happening
            if (note < 0 || note > notes.Count - 1)
                return 0.0f;
            
            return notes[note].getAmplitudeOrValue(time, depth + 1, sampleRate);
            //return gen.amplitude(notes[note].value, time, depth + 1, sampleRate);
        }
    }
}