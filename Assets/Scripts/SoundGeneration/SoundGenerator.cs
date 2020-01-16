using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SoundGeneration.Oscillators;
using Assets.Scripts.SoundGeneration.Presets;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Composition = DarkArtsStudios.SoundGenerator.Composition;
using EditorUtility = UnityEditor.EditorUtility;

namespace Assets.Scripts.SoundGeneration
{
    class MonoMelody
    {
        public struct PlayNote
        {
            public float Frequency;
            public float Duration;

            public PlayNote(float frequency, float duration)
            {
                Frequency = frequency;
                Duration = duration;
            }
        }

        public List<PlayNote> notes = new List<PlayNote>();

        public void AddNote(float frequency, float duration)
        {
            notes.Add(new PlayNote(frequency, duration));
        }
    }

    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Composition))]
    [RequireComponent(typeof(SoundGeneratorControls))]
    public class SoundGenerator : MonoBehaviour
    {
        private int sampleRate;
        public float mainFrequency = 500;
        public float masterVolume = 1.0f;

        //public float[] waveForm;

        void Awake()
        {
            sampleRate = AudioSettings.outputSampleRate;
            composition = GetComponent<Composition>();
            //UpdateConfiguration();
            output = composition.GetOutput();
        }

        public Composition composition;

        [NonSerialized]
        public Output output;

        public BaseSoundConfiguration configuration;

        public void UpdateConfiguration()
        {
            if (configuration == null)
                configuration = new TestSoundConfiguraiton();

            if (composition == null)
                composition = GetComponent<Composition>();
            
            Undo.RecordObject(this, "Set composition");
            configuration.Configure(composition);
        }

        private MonoMelody GetMelody()
        {
            var melody = new MonoMelody();
            melody.AddNote(Music.Frequency(4, 1), 1);
            melody.AddNote(Music.Frequency(4, 3), 1);
            melody.AddNote(Music.Frequency(4, 5), 1);
            melody.AddNote(Music.Frequency(4, 3), 1);
            return melody;
        }

        private MonoMelody melody;

        private void PlayNote(float[] data, int channels, float frequency, float duration)
        {
            var currentDspTime = AudioSettings.dspTime;
            var dataLen = data.Length / channels;   // the actual data length for each channel
            var chunkTime = (double)dataLen / sampleRate;   // the time that each chunk of data lasts
            double currentFreq = mainFrequency;

            var dspTimeStep = chunkTime / dataLen;	// the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

            var sin = new SinWaveOscillator();
            for (int i = 0; i < dataLen; i++)
            {
                var preciseDspTime = currentDspTime + i * dspTimeStep;

                double signalValue = 0.0;
                var fromOutput = output.OnAmplitude((float)currentFreq, (float)preciseDspTime, (float)chunkTime, 0, sampleRate);
                signalValue += fromOutput;
                //signalValue += sin.calculateSignalValue(preciseDspTime, currentFreq);
                float x = masterVolume * 0.5f * (float)signalValue;

                for (int j = 0; j < channels; j++)
                {
                    data[i * channels + j] = x;
                    //Debug.Log("Put: " + x);
                }

            }
        }


        void OnAudioFilterRead(float[] data, int channels)
        {
            //foreach (var note in melody.notes)
            //{
            //    PlayNote(data, channels, note.Frequency, note.Duration);
            //}

            //waveForm = data;
        }
    }
}