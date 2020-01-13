using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.Module;
using DarkArtsStudios.SoundGenerator.Module.Oscillator;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Composition = DarkArtsStudios.SoundGenerator.Composition;
using Random = UnityEngine.Random;

public class SinWaveOscillator
{
    double currentSignalTime;           // the current dsp time
    double currentSignalFrequency;      // the current frequency of the signal
    double currentSignalPhaseOffset;    // the current phase offset of the signal

    double currentSignaOutlValue;       //the current signal output value		

    public SinWaveOscillator()
    {
        currentSignalFrequency = 400.0;
        currentSignalPhaseOffset = 0.0;
    }

    public double calculateSignalValue(double newSignalTime, double newSignalFrequency)
    {

        if (Math.Abs(currentSignalFrequency - newSignalFrequency) > 0.00000001)
        {
            /*This part takes care of what should happen when the signal's frequency changes
			(when the incoming frequency is different from the current frequency)
			 This is VERY IMPORTANT, because if you do not handle this matter then you will hear
			 LOTS of CLICKS when the frequency changes.
			Description: When the frequency changes, it is driven either by a slider, or by an
			external signal. Either way, the change happens in (very small) increments. When such
			a change takes place, then the produced wave suddenly changes phase.
			To make this more clear, suppose that there is a change from 
			Sin(2 * PI * 0.75) = -1 
			to Sin(2 * PI * 0.8) = -0.951056516295
			This sudden change from -1 to  -0.951056516295 causes a discontinuity in the sinusoidal function's graph, and this can be VERY bad.
			The way I handled this is:
			Every time the frequency changes, the phase of the wave is shifted in such a way that the current value of the sinusoidal 
			function remains the same between the two steps. This way, no matter how quickly the frequency changes, no clicks are ever 
			heard (because of that, at least). 
			This is kind of complicated, and very low-level audio stuff, so if you do not understand it, you may just use it. */

            // calculate the signal's current period: period = 1 / frequency
            double currentSignalPeriod = 1.0 / currentSignalFrequency;
            // calculate the current number of cycles:
            // the number of cycles is the number of times that a complete period of the wave has occured.
            double currentNumberOfSignalCycles = (currentSignalTime / currentSignalPeriod) + (currentSignalPhaseOffset / (2.0 * Math.PI));
            double currentSignalCyclePosition = currentNumberOfSignalCycles % 1.0;  //current cycle position

            double newSignalPeriod = 1.0 / newSignalFrequency;
            double newNumberOfSignalCycles = currentSignalTime / newSignalPeriod;
            double newSignalCyclePosition = newNumberOfSignalCycles % 1.0;          //new cycle position

            double cycleDifference = currentSignalCyclePosition - newSignalCyclePosition;
            double phaseDifference = cycleDifference * Math.PI * 2.0;

            currentSignalPhaseOffset = phaseDifference;

            currentSignalFrequency = newSignalFrequency;
            currentSignalTime = newSignalTime;

            currentSignaOutlValue = Math.Sin(currentSignalTime * 2.0 * Math.PI * currentSignalFrequency + currentSignalPhaseOffset);
            return currentSignaOutlValue;
        }
        else
        {
            currentSignalFrequency = newSignalFrequency;
            currentSignalTime = newSignalTime;
            currentSignaOutlValue = Math.Sin(currentSignalTime * 2.0 * Math.PI * currentSignalFrequency + currentSignalPhaseOffset);
            return currentSignaOutlValue;
        }
    }
}

public class Sin : BaseOscillator
{

    public static string MenuEntry()
    {
        return "Oscillator/SinX";
    }

    public override float OnAmplitude(float frequency, float time, float duration, int depth)
    {
        return (float)Math.Sin((float)(Math.PI / 180.0 * (((double)time * (double)frequency + (double)this.attribute("Phase").value) * 360.0)));
        //throw new NotImplementedException();
    }
}

public class Noise2 : BaseOscillator
{
    public static string MenuEntry()
    {
        return "Oscillator/Noise2";
    }

    private System.Random random = new System.Random();

    public override float OnAmplitude(float frequency, float time, float duration, int depth)
    {
        var val = (float)(random.NextDouble() * 2.0 - 1.0);
       // Debug.Log("Generated: " + val);
        return val;
    }
}

//[CustomEditor(typeof(SoundGenerator))]
//public class SoundGeneratorEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        SoundGenerator myTarget = (SoundGenerator)target;

//        if (GUILayout.Button("Test"))
//        {
//            var s = myTarget.GetComponent<DarkArtsStudios.SoundGenerator.Composition>();
//            //s.composition = new DarkArtsStudios.SoundGenerator.Composition();
//            s.modules = new List<BaseModule>();

//            var child = new GameObject();
//            child.transform.SetParent(myTarget.transform);
//            var noise = child.AddComponent<Noise2>();
//            noise.InitializeAttributes();

//            var child2 = new GameObject();
//            child2.transform.SetParent(myTarget.transform);
//            var output = child2.AddComponent<Output>();
//            output.InitializeAttributes();
//            s.modules.Add(noise);
//            s.modules.Add(output);

//            //Debug.Log(string.Join(", ", output.attributes.Select(a => a.name).ToArray()));
//            output.attribute("Input").generator = noise;
//            //s.modules.Add(Noise.);
//            //s.modules.Add(new Noise());
//            //s.modules.Add(new Output());
//        }

//        serializedObject.Update();
//        //EditorGUIUtility.LookLikeInspector();
//        SerializedProperty tps = serializedObject.FindProperty("waveForm");
//        EditorGUI.BeginChangeCheck();
//        EditorGUILayout.PropertyField(tps, true);
//        if (EditorGUI.EndChangeCheck())
//            serializedObject.ApplyModifiedProperties();
//        //EditorGUIUtility.LookLikeControls();

//    }
//}

class BaseSoundConfiguration
{
    protected Output output;

    public virtual void Configure(Composition composition)
    {
        composition.modules.Clear();
        //todo: add dispose logic

        composition.modules = new List<BaseModule>();

        var child2 = new GameObject();
        child2.transform.SetParent(composition.transform);
        output = child2.AddComponent<Output>();
        output.InitializeAttributes();

        composition.modules.Add(output);
    }
}


class NoiseSoundConfiguration : BaseSoundConfiguration
{
    public override void Configure(Composition composition)
    {
        base.Configure(composition);
        
        var child = new GameObject();
        child.transform.SetParent(composition.transform);
        var noise = child.AddComponent<Square>();
        noise.InitializeAttributes();
       
        composition.modules.Add(noise);

        output.attribute("Input").generator = noise;
    }
}

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Composition))]
public class SoundGenerator : MonoBehaviour
{
    private double sampleRate;
    public double mainFrequency = 500;
    public float masterVolume = 1.0f;

    //public float[] waveForm;

    void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;

    }

    private Composition composition;

    private Output output;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.lo
        composition = GetComponent<Composition>();
        var config = new NoiseSoundConfiguration();
        config.Configure(composition);
        output = composition.modules.OfType<Output>().Single();
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
            //Debug.Log("From output: " + fromOutput);
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
public class ADSR : MonoBehaviour
{
    public static ADSR Instance { get; private set; }

    readonly List<Envelope> envelopes = new List<Envelope>();

    void Start()
    {
        if (Instance) throw new InvalidOperationException("Two ADSR objects in scene!");
        Instance = this;
    }
    void OnDestroy()
    {
        if (Instance != this) throw new InvalidOperationException("wat");
        Instance = null;
    }

    void Update()
    {
        //foreach (var e in envelopes)
        //    e.Update();
    }

    public void RegisterEnvelope(Envelope e)
    {
        envelopes.Add(e);
    }
    public void UnregisterEnvelope(Envelope e)
    {
        envelopes.Remove(e);
    }
}

public class FlatEnvelopePhase
{
    public double Duration;

    public FlatEnvelopePhase() { }
    public FlatEnvelopePhase(FlatEnvelopePhase other)
    {
        Duration = other.Duration;
    }
}

public enum EasingType
{
    Linear
}

public class EnvelopePhase
{
    public double Duration;
    public double Target;
    public EasingType In = EasingType.Linear;
    public EasingType Out = EasingType.Linear;

    public EnvelopePhase() { }
    public EnvelopePhase(EnvelopePhase other)
    {
        Duration = other.Duration;
        Target = other.Target;
        In = other.In;
        Out = other.Out;
    }

    //public float Ease(float step)
    //{
    //    if (In == EasingType.Linear) return Easing.EaseOut(step, Out);
    //    if (Out == EasingType.Linear) return Easing.EaseIn(step, In);
    //    return Easing.EaseInOut(step, In, Out);
    //}
}

public class Envelope
{
    float current;
    float? sinceTriggered;
    float? sinceReleased;

    public FlatEnvelopePhase P { get; private set; }
    public EnvelopePhase A { get; private set; }
    public EnvelopePhase D { get; private set; }
    public FlatEnvelopePhase S { get; private set; }

    // R.Target is ignored, assumed to be 0
    public EnvelopePhase R { get; private set; }

    public Envelope()
    {
        P = new FlatEnvelopePhase();
        A = new EnvelopePhase();
        D = new EnvelopePhase();
        S = new FlatEnvelopePhase();
        R = new EnvelopePhase();

        ADSR.Instance.RegisterEnvelope(this);
    }
    public Envelope(Envelope other)
    {
        P = new FlatEnvelopePhase(other.P);
        A = new EnvelopePhase(other.A);
        D = new EnvelopePhase(other.D);
        S = new FlatEnvelopePhase(other.S);
        R = new EnvelopePhase(other.R);

        ADSR.Instance.RegisterEnvelope(this);
    }
    ~Envelope()
    {
        ADSR.Instance.UnregisterEnvelope(this);
    }

    public void Trigger(float? scheduleReleaseIn = null)
    {
        sinceTriggered = 0;
        sinceReleased = scheduleReleaseIn.HasValue ? (float?)-scheduleReleaseIn.Value : null;
    }

    public void Release()
    {
        sinceReleased = 0;
        sinceTriggered = null;
    }

    public static implicit operator float(Envelope d)
    {
        return d.current;
    }

    //internal void Update()
    //{
    //    if (sinceTriggered == null && sinceReleased == null)
    //    {
    //        current = 0;
    //        return;
    //    }

    //    if (sinceReleased != null)
    //    {
    //        bool wasNegative = sinceReleased < 0;
    //        sinceReleased += Time.deltaTime;

    //        if (sinceReleased > 0)
    //        {
    //            if (wasNegative)
    //                Release();

    //            float step = Mathf.Clamp01(sinceReleased.Value / (float)R.Duration);
    //            current = Mathf.Lerp((float)(D.Duration == 0 ? A.Target : D.Target), 0, R.Ease(step));

    //            if (step >= 1)
    //            {
    //                current = 0;
    //                sinceReleased = null;
    //            }
    //        }
    //    }

    //    if (sinceTriggered != null)
    //    {
    //        sinceTriggered += Time.deltaTime;

    //        float t = sinceTriggered.Value;
    //        float aStep = Mathf.Clamp01((t -= (float)P.Duration) / (float)A.Duration);
    //        float dStep = Mathf.Clamp01((t -= (float)A.Duration) / (float)D.Duration);
    //        float sStep = Mathf.Clamp01((t -= (float)D.Duration) / (float)S.Duration);

    //        if (aStep < 1)
    //            current = Mathf.Lerp(0, (float)A.Target, A.Ease(aStep));
    //        else if (dStep < 1)
    //            current = Mathf.Lerp((float)A.Target, (float)D.Target, D.Ease(dStep));
    //        else if (sStep < 1)
    //            current = (float)(D.Duration == 0 ? A.Target : D.Target);
    //        else
    //            Release();
    //    }
    //}
}

