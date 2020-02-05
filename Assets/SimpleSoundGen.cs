using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSoundGen : MonoBehaviour
{
    public double frequency = 400.0;

    private double increment;
    private double phase;
    private double sampleRate = 48000.0;

    public float gain;

    void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2.0 * Mathf.PI / sampleRate;
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;
            data[i] = (float) (gain * Mathf.Sin((float) phase));

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            if (phase > Mathf.PI)
            {
                phase = 0.0;
            }
        }
    }
}
