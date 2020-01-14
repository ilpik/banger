using System.Collections.Generic;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;
using Composition = DarkArtsStudios.SoundGenerator.Composition;

public class BaseSoundConfiguration
{
    protected Output output;

    public virtual void Configure(Composition composition)
    {
        composition.modules.Clear();
        //todo: add dispose logic
        composition.modules = new List<BaseModule>();

        output = AddModule<Output>(composition);
    }

    protected T AddModule<T>(Composition composition) where T : BaseModule
    {
        var gameObject = new GameObject();
        gameObject.transform.SetParent(composition.transform);
        var module = gameObject.AddComponent<T>();
        module.InitializeAttributes();
        composition.modules.Add(module);
        return module;
    }

}