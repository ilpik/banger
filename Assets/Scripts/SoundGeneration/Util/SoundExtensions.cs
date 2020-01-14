using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;
using Composition = DarkArtsStudios.SoundGenerator.Composition;

namespace Assets.Scripts.SoundGeneration
{
    internal static class SoundExtensions
    {
        public static Output GetOutput(this Composition composition)
        {
            return composition.modules.OfType<Output>().Single();
        }

        public static BaseModule GetInput(this BaseModule module)
        {
            return module.attribute("Input").generator;
        }

        public static void GetInputFrom(this BaseModule module, BaseModule input)
        {
            var inp = module.attribute("Input");
            if (inp == null)
            {
                Debug.LogError("no input set in module: " + module.GetType());
                return;
            }

            inp.generator = input;
        }
    }
}
