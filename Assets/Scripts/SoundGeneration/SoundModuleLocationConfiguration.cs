using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEngine;
using Composition = DarkArtsStudios.SoundGenerator.Composition;

namespace Assets.Scripts.SoundGeneration
{
    internal class SoundModuleLocationConfiguration
    {
        private Vector2 defaultSize = new Vector2(300, 600);
        private Vector2 deltaPosition = new Vector2(20, 10);

        public void Update(Composition composition)
        {
            List<BaseModule> modules = new List<BaseModule>();
            UpdateRect(composition.GetOutput(), modules, new Vector2(800, 500));

            Vector2 position = Vector2.zero;
            foreach (var notUsedModule in composition.modules.Where(m => !modules.Contains(m)))
            {
                MoveTo(notUsedModule, position);
                position += new Vector2(defaultSize.x + deltaPosition.x, 0);
            }
        }

        private void UpdateRect(BaseModule module, List<BaseModule> passedModules, Vector2 point)
        {
            Debug.Log(module.name);
            passedModules.Add(module);
            MoveTo(module, point);
            float dy = 0;
            foreach (var attribute in module.attributes)
            {
                if (attribute.generator != null && !passedModules.Contains(attribute.generator))
                {
                    var newPoint = new Vector2(point.x - defaultSize.x - deltaPosition.x, point.y + dy);

                    UpdateRect(attribute.generator, passedModules, newPoint);
                    dy += defaultSize.y + deltaPosition.y;
                }
            }
        }

        private void MoveTo(BaseModule module, Vector2 point)
        {
            module.visualPlacementRect = new Rect(point.x, point.y, module.visualPlacementRect.width, module.visualPlacementRect.height);
            Debug.Log("Module: " + module.name + "; rect: " + module.visualPlacementRect);
        }
    }
}