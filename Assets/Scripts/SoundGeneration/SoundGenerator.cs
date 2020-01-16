using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SoundGeneration.Oscillators;
using Assets.Scripts.SoundGeneration.Presets;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Composition = DarkArtsStudios.SoundGenerator.Composition;

namespace Assets.Scripts.SoundGeneration
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Composition))]
    [RequireComponent(typeof(SoundGeneratorControls))]
    public class SoundGenerator : MonoBehaviour
    {
        private double sampleRate;
        public float mainFrequency = 500;
        public float masterVolume = 1.0f;

        //public float[] waveForm;

        void Awake()
        {
            sampleRate = AudioSettings.outputSampleRate;

        }

        private Composition composition;

        private Output output;

        public BaseSoundConfiguration configuration;

        public void UpdateConfiguration()
        {
            if (configuration == null)
                configuration = new TestSoundConfiguraiton();

            if (composition == null)
                composition = GetComponent<Composition>();

            configuration.Configure(composition);
        }

        //[ExecuteAlways]
        // Start is called before the first frame update
        void Start()
        {
            composition = GetComponent<Composition>();
            UpdateConfiguration();
            output = composition.GetOutput();
        }


        // Update is called once per frame
        void Update()
        {

        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            var currentDspTime = AudioSettings.dspTime;
            var dataLen = data.Length / channels;   // the actual data length for each channel
            var chunkTime = dataLen / sampleRate;   // the time that each chunk of data lasts
            double currentFreq = mainFrequency;

            var dspTimeStep = chunkTime / dataLen;	// the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)
        
            var sin = new SinWaveOscillator();
            for (int i = 0; i < dataLen; i++)
            {
                var preciseDspTime = currentDspTime + i * dspTimeStep;

                double signalValue = 0.0;
                var fromOutput = output.OnAmplitude((float) currentFreq, (float) preciseDspTime, (float) chunkTime, 0);
                signalValue += fromOutput;
                //signalValue += sin.calculateSignalValue(preciseDspTime, currentFreq);
                float x = masterVolume * 0.5f * (float) signalValue;

                for (int j = 0; j < channels; j++)
                {
                    data[i * channels + j] = x;
                    //Debug.Log("Put: " + x);
                }

            }

            //waveForm = data;
        }
    }
}