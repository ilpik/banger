using System;
using Assets.Scripts.SoundGeneration;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;


public abstract class DAAudioFilter : BaseModule, IAudioFilter
{
    //[SerializeField]
    public double inputFrequency = 400.0;

    private double inputSampleRate = 48000.0;

    public float gain;

    public Attribute modulation1;

    public Attribute modulation2;

    public override void InitializeAttributes()
    {
        inputSampleRate = AudioSettings.outputSampleRate;

        modulation1 = AddAttribute("Modulation 1");
        modulation2 = AddAttribute("Modulation 2");
    }

    public void ReadData(double currentDspTime, float[] data, int channels)
    {
        int dataLen = data.Length / channels;   // the actual data length for each channel
        double chunkTime = dataLen / inputSampleRate;   // the time that each chunk of data lasts
        double dspTimeStep = chunkTime / dataLen;	// the time of each dsp step. (the time that each individual audio sample (actually a float value) lasts)

        for (int i = 0; i < dataLen; i++)
        {
            double preciseDspTime = currentDspTime + i * dspTimeStep;

            float x = 0.5f * (float)(0.5 * amplitude(preciseDspTime, 0, (int)inputSampleRate));

            for (int j = 0; j < channels; j++)
            {
                data[i * channels + j] += x;
            }

        }
    }

    public void OnAudioFilterRead(float[] data, int channels)
    {
        double currentDspTime = AudioSettings.dspTime;
        ReadData(currentDspTime, data, channels);
    }
}
