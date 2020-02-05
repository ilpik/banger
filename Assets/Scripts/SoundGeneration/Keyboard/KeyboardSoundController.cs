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
        public int octave = 4;

        const int minOctave = 1;
        const int maxOctave = 8;

        public bool debug = true;

        private SoundGenerator soundGenerator;

        void Awake()
        {
            foreach (var code in notes.Keys)
                freqs[code] = false;
        }

        void Start()
        {
            soundGenerator = GetComponent<SoundGenerator>();

            var audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            var freqs = ReadFrequencies();

            double time = AudioSettings.dspTime;
            foreach (var freq in freqs)
            {
                PlayFrequency(time, freq, data, channels);
            }
        }

        public void Update()
        {
            foreach (var code in notes.Keys)
            {
                freqs[code] = Input.GetKey(code);
                if (debug && Input.GetKeyDown(code))
                {
                    var note = notes[code];
                    Debug.Log($"Now playing {Music.NoteName[note.note]} (octave {octave + note.octave}) [{GetFrequency(code)}]");
                }
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                octave = Math.Min(octave + 1, maxOctave);
                Debug.Log("Octave set to " + octave);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                octave = Math.Max(octave - 1, minOctave);
                Debug.Log("Octave set to " + octave);
            }
        }

        //readonly KeyCode[] noteKeys =
        //{
        //    KeyCode.A, KeyCode.W, KeyCode.S, KeyCode.E,
        //    KeyCode.D, KeyCode.F, KeyCode.T, KeyCode.G,
        //    KeyCode.Y, KeyCode.H, KeyCode.U, KeyCode.J,
        //    KeyCode.K, KeyCode.O, KeyCode.L
        //};

        //readonly KeyCode[] noteKeys =
        //{
        //    KeyCode.H, //A
        //    KeyCode.U, //A# 
        //    KeyCode.J, //B
        //    KeyCode.A, //C
        //    KeyCode.W, //C#
        //    KeyCode.S, //D
        //    KeyCode.E, //D#
        //    KeyCode.D, //E
        //    KeyCode.F, //F
        //    KeyCode.T, //F#
        //    KeyCode.G, //G
        //    KeyCode.Y  //G#
        //};

        public static Dictionary<KeyCode, (int note, int octave)> notes = new Dictionary<KeyCode, (int note, int octave)>()
        {
            [KeyCode.A] = (3, 0), //C
            [KeyCode.W] = (4, 0), //C#
            [KeyCode.S] = (5, 0), //D
            [KeyCode.E] = (6, 0), //D#
            [KeyCode.D] = (7, 0), //E
            [KeyCode.F] = (8, 0), //F
            [KeyCode.T] = (9, 0), //F#
            [KeyCode.G] = (10, 0), //G
            [KeyCode.Y] = (11, 0), //G#
            [KeyCode.H] = (0, 1), //A
            [KeyCode.U] = (1, 1), //A# 
            [KeyCode.J] = (2, 1), //B
            [KeyCode.K] = (3, 1), //C
            [KeyCode.O] = (4, 1), //C#
            [KeyCode.L] = (5, 1), //D
            [KeyCode.P] = (6, 1), //D#
            [KeyCode.Semicolon] = (7, 1), //E
            [KeyCode.Quote] = (8, 1), //F
        };


        private readonly IDictionary<KeyCode, bool> freqs = new Dictionary<KeyCode, bool>();

        private void PlayFrequency(double startTime, float frequency, float[] data, int channels)
        {
            var noteIn = soundGenerator.input;
            if (noteIn == null)
                return;

            noteIn.frequency.value = frequency;
            noteIn.button.pressed = true;
            soundGenerator.output.ReadData(startTime, data, channels);
        }

        private IEnumerable<float> ReadFrequencies()
        {
            foreach (var note in notes)
            {
                if (freqs[note.Key])
                    yield return GetFrequency(note.Key);
            }
        }

        private float GetFrequency(KeyCode key)
        {
            var note = notes[key];
            return Music.Frequency(octave + note.octave, note.note);
        }

    }
}
