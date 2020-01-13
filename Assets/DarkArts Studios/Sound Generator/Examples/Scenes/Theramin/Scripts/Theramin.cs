using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DarkArtsStudios.SoundGenerator.Examples
{
    public class Theramin : MonoBehaviour
    {
        private AudioSource audioSource = null;

        IEnumerator Start()
        {
            audioSource = GetComponent<AudioSource>();
            Dictionary<string, AudioClip> audioClips = GetComponent<Composition>().audioClips;
            while ( audioClips.Count < 1)
            {
                yield return null;
            }
            audioSource.clip = audioClips["Output"];
            audioSource.loop = true;
            audioSource.volume = 0;
            audioSource.Play();
        }

        void Update()
        {
            float xThreshold = Mathf.Clamp(Input.mousePosition.x / Screen.width, 0, 1);
            float yThreshold = Mathf.Clamp(Input.mousePosition.y / Screen.height, 0, 1);
            float x = xThreshold - 0.5f;
            float y = yThreshold - 0.5f;
            transform.position = new Vector3(x * 10, y * 10, 0);
            audioSource.volume = 1 - xThreshold;
            audioSource.pitch = 0.5f + yThreshold * 2;
        }

    }
}