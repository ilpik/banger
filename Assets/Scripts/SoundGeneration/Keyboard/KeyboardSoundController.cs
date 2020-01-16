using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.SoundGeneration.Oscillators;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration.Keyboard
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(SoundGenerator))]
    class KeyboardSoundController : MonoBehaviour
    {
        private int sampleRate;

        private Output output;

        public float masterVolume;

        public int octave = 4;

        const int minOctave = 1;
        const int maxOctave = 8;

        public bool debug = true;

        void Awake()
        {
            sampleRate = AudioSettings.outputSampleRate;
            
            foreach (var code in noteKeys)
                freqs[code] = false;
        }

        void Start()
        {
            output = GetComponent<SoundGenerator>().output;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            var freqs = ReadFrequencies();

            foreach (var freq in freqs)
            {
                PlayFrequency(freq, data, channels);
            }

        }

        public void Update()
        {
            foreach (var code in noteKeys)
            {
                freqs[code] = Input.GetKey(code);
                if (debug && Input.GetKeyDown(code))
                    Debug.Log($"Now playing {Music.NoteName[Array.IndexOf(noteKeys, code)]} (octave {octave})");
            }
            
            if (Input.GetKeyDown(KeyCode.X))
                octave = Math.Min(octave + 1, maxOctave);

            if (Input.GetKeyDown(KeyCode.Z))
                octave = Math.Max(octave - 1, minOctave);
        }

        //readonly KeyCode[] noteKeys =
        //{
        //    KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E,
        //    KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G,
        //    KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J,
        //    KeyCode.K, KeyCode.O, KeyCode.L
        //};

        readonly KeyCode[] noteKeys =
        {
            KeyCode.H, //A
            KeyCode.U, //A# 
            KeyCode.J, //B
            KeyCode.A, //C
            KeyCode.W, //C#
            KeyCode.S, //D
            KeyCode.E, //D#
            KeyCode.D, //E
            KeyCode.F, //F
            KeyCode.T, //F#
            KeyCode.G, //G
            KeyCode.Y  //G#
        };


        private IDictionary<KeyCode, bool> freqs = new Dictionary<KeyCode, bool>();

        private void PlayFrequency(float frequency, float[] data, int channels)
        {
            var currentDspTime = AudioSettings.dspTime;
            var dataLen = data.Length / channels;   // the actual data length for each channel
            var chunkTime = (double)dataLen / sampleRate;   // the time that each chunk of data lasts
            var dspTimeStep = chunkTime / dataLen;  // the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

            for (int i = 0; i < dataLen; i++)
            {
                var preciseDspTime = currentDspTime + i * dspTimeStep;

                var signalValue = output.OnAmplitude(frequency, (float)preciseDspTime, (float)chunkTime, 0, sampleRate);
                float x = masterVolume * 0.5f * signalValue;

                for (int j = 0; j < channels; j++)
                {
                    data[i * channels + j] += x;
                }

            }

        }

        private IEnumerable<float> ReadFrequencies()
        {
            for (int i = 0; i < noteKeys.Length; i++)
            {
                if (freqs[noteKeys[i]])
                    yield return GetFrequency(noteKeys[i], i);
            }
        }

        private float GetFrequency(KeyCode key, int index)
        {
            index = index % 12 + 1;
            int oct = octave;
            //if (key == KeyCode.J || key== KeyCode.I || key == KeyCode.K || key == KeyCode.O || key == KeyCode.L || key == )
            return Music.Frequency(oct, index);
        }

        

    }
}
