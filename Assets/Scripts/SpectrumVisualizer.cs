using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumVisualizer : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip clip;
    public int spectrumSize = 512;
    private float[] spectrum;
    public float scale = 1.0f;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spectrum = new float[spectrumSize];
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = spectrumSize;
        lineRenderer.SetPositions(GetInitialLine());
        StartCoroutine(DisplaySpectrum());
    }

    Vector3[] GetInitialLine()
    {
        var start = lineRenderer.GetPosition(0);
        var end = lineRenderer.GetPosition(1);

        var line = new Vector3[spectrumSize];
        for (int i = 0; i < spectrumSize; i++)
        {
            var x = Mathf.Lerp(start.x, end.x, i / (float) (spectrumSize - 1));
            var z = Mathf.Lerp(start.z, end.z, i / (float) (spectrumSize - 1));
            line[i] = new Vector3(x, 0, z);
        }

        return line;
    }

    IEnumerator DisplaySpectrum()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1F);
            if (audioSource.clip?.loadState != AudioDataLoadState.Loaded)
                continue;

            audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
            for (int i = 0; i < spectrum.Length; i++)
            {
                var old = lineRenderer.GetPosition(i);
                lineRenderer.SetPosition(i, new Vector3(old.x, spectrum[i] * scale, old.z));
            }
         
        }
    }

}
