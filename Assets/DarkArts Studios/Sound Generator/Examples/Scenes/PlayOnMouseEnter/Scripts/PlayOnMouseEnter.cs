using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DarkArtsStudios.SoundGenerator.Examples
{
    public class PlayOnMouseEnter : MonoBehaviour
    {
        private AudioClip audioClip = null;

        IEnumerator Start()
        {
            Dictionary<string,AudioClip> audioClips = GetComponent<Composition>().audioClips;
            while (audioClips.Count < 1)
            {
                yield return null;
            }

            audioClip = audioClips["Output"];
        }

        public void OnMouseEnter()
        {
            GetComponent<AudioSource>().PlayOneShot(audioClip);
        }

        public void OnGUI()
        {
            if (audioClip == null)
            {
                GUI.Label(new Rect(10, 10, Screen.width - 20, 20), "Generating Sounds...");
            }
        }
    }
}