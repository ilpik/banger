using System.Collections;
using System.Linq;
using Assets.Scripts.SoundGeneration;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using Composition = DarkArtsStudios.SoundGenerator.Composition;
using Random = UnityEngine.Random;

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

    public BaseSoundConfiguration configuration;

    // Start is called before the first frame update
    void Start()
    {
        composition = GetComponent<Composition>();

        if (configuration == null)
            configuration = new TestSoundConfiguraiton();

        
        configuration.Configure(composition);
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