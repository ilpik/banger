using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;
using Composition = DarkArtsStudios.SoundGenerator.Composition;

namespace Assets.Scripts.SoundGeneration.Presets
{
    public class BaseSoundConfiguration
    {
        protected Output output;

        public void Configure(Composition composition)
        {
            ClearModules(composition);
            output = AddModule<Output>(composition);
            OnConfigure(composition);
            new SoundModuleLocationConfiguration().Update(composition);
        }

        protected virtual void OnConfigure(Composition composition)
        {

        }

        protected T AddModule<T>(Composition composition) where T : BaseModule
        {
            var gameObject = new GameObject();
            gameObject.name = typeof(T).Name;
            gameObject.transform.SetParent(composition.transform);
            var module = gameObject.AddComponent<T>();
            module.InitializeAttributes();
            module.InitializeName();
            composition.modules.Add(module);
            return module;
        }

        private void ClearModules(Composition composition)
        {
            foreach (Transform child in composition.transform)
            {
                if (child.GetComponent<BaseModule>())
                {
                    Object.DestroyImmediate(child.gameObject);
                }
            }

            composition.modules.Clear();

        }

    }
}