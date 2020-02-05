using System;
using System.Text;
using Assets.Scripts.SoundGeneration;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using UnityEngine;

public class SecondOscillator : MonoBehaviour
{
    public double frequency = 400.0;

    private double sampleRate;

    public float gain;

    public void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
    }

    private StringBuilder sb = new StringBuilder();

    private int samplesTakenStart = 5;
    private int samplesTakenEnd = 5;
    
    private int loggedFrames = 5;
    private int loggedFrameIndex = 0;


    public void OnAudioFilterRead(float[] data, int channels)
    {
   

        //if (loggedFrameIndex++ == loggedFrames)
        //    Debug.Log(sb.ToString());
    }

    private double Amplitude(double freq, double time)
    {
        return Math.Sin(time * 2.0 * Math.PI * freq);

        //return Mathf.Sin((float)(freq * time * 2 * Mathf.PI));
    }
}