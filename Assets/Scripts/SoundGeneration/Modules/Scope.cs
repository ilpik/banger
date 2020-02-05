using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration.Modules
{
    class Scope : DAAudioFilter
    {
        //
        public static string MenuEntry() => MenuEntryProvider.Get("Scope");

        public Attribute input;

        public Attribute speed;

        public Attribute zoom;

        public Attribute showAxis;

        public override void InitializeAttributes()
        {
            input = AddInput();
            speed = AddAttribute("Speed", b => b.Slider(0, 1).WithValue(0.003f));
            zoom = AddAttribute("Zoom", b => b.Slider(0, 2000f).WithValue(2000f));
            showAxis = AddAttribute("Show Axis", b => b.WithType(Attribute.AttributeType.BUTTON));
        }

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            return input.getAmplitudeOrValue(time, depth + 1, sampleRate);
        }
    }
}