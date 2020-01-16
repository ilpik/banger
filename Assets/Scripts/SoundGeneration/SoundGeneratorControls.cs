using UnityEngine;

namespace Assets.Scripts.SoundGeneration
{
    public class SoundGeneratorControls : MonoBehaviour
    {
        public void UpdateComposition()
        {
            var soundGenerator = GetComponent<SoundGenerator>();
            soundGenerator.UpdateConfiguration();
        }
    }
}