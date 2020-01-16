using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration
{
    public class SoundGeneratorControls : MonoBehaviour
    {
        public void UpdateComposition()
        {
            ClearComposition();
            var soundGenerator = GetComponent<SoundGenerator>();
            soundGenerator.UpdateConfiguration();
        }

        public void ClearComposition()
        {
            var soundGenerator = GetComponent<SoundGenerator>();
            soundGenerator.composition?.modules.Clear();

            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject != this.gameObject)
                    DestroyImmediate(child.gameObject);
            }
        }
    }
}