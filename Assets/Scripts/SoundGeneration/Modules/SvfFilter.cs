using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.SoundGeneration.Modules
{
    class BellFilter : DAAudioFilter
    {
        public static string MenuEntry() => MenuEntryProvider.Get("Bell Filter");

        public Attribute gainDb;

        public Attribute cutoff;

        public Attribute q;

        public Attribute frequency;

        public override void InitializeAttributes()
        {
            base.InitializeAttributes();
            gainDb = AddAttribute("Gain", b=>b.Slider(-100, 100).WithValue(12f));
            cutoff = AddAttribute("Cutoff", b=>b.Slider(-100, 100000).WithValue(1000));
            q = AddAttribute("Q", b=>b.Slider(0.001f, 100).WithValue(0.5f));
            frequency = AddAttribute("Frequency");
        }

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            var v0 = frequency.getAmplitudeOrValue(time, depth, sampleRate);

            if (Math.Abs(v0) < 0.0001)
                return 0;

            var ic1eq = 0.0;
            var ic2eq = 0.0;

            var a = Math.Pow(10, gainDb.getAmplitudeOrValue(time, depth, sampleRate));
            var g = Math.Tan(Math.PI * cutoff.getAmplitudeOrValue(time, depth, sampleRate) / sampleRate);
            var k = 1 / (q.getAmplitudeOrValue(time, depth, sampleRate) * a);
            var a1 = 1 / (1 + g * (g + k));
            var a2 = g * a1;
            var a3 = g * a2;
            var m0 = 1;
            var m1 = k * (a * a - 1);
            var m2 = 0;

            
            var v3 = v0 - ic2eq;
            var v1 = a1 * ic1eq + a2 * v3;
            var v2 = ic2eq + a2 * ic1eq + a3 * v3;
            ic1eq = 2 * v1 - ic1eq;
            ic2eq = 2 * v2 - ic2eq;

            var bell = v0 + m1 * v1;

            return bell;

        }
    }

    class SvfFilter : DAAudioFilter
    {
        public static string MenuEntry() => MenuEntryProvider.Get("SVF Filter");

        public Attribute cutoff;

        public Attribute q;

        public Attribute input;

        public override void InitializeAttributes()
        {
            base.InitializeAttributes();
            input = AddInput();
            cutoff = AddAttribute("Cutoff");
            q = AddAttribute("Q");
        }

        double cutoffFrequency = 0.25;
        double Resonance = 0.5;
        double oversamplingFactor = 4;
        int filterMode = 0;
        double sampleRate = 44100.0;


        double dt_prime = 0;
        double fb_prime = 0;

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            //float cutoffFrequency = GetCutoffFrequency(frequency, time, depth, sampleRate);

            double input = this.input.getAmplitudeOrValue(time, depth + 1, sampleRate);

            // noise term

            // integration rate
            double dt = 0.001 + 2.25 * (cutoffFrequency * cutoffFrequency *
                                        cutoffFrequency * cutoffFrequency);

            // feedback amount
            double fb = 1.0 - Resonance;

            // smooth parameter changes
            dt_prime = (dt * (1.0 - 0.999)) + (dt_prime * 0.999);
            fb_prime = (fb * (1.0 - 0.993)) + (fb_prime * 0.993);

            // update noise terms
            double noise = Random.value;
            noise = 2.0 * (noise - 0.5);

            double bp = 0;
            double lp = 0;

            double result = 0;
            // integrate filter state
            // with oversampling
            for (int nn = 0; nn < oversamplingFactor; nn++)
            {
                var hp = input - (2.0 * fb_prime - 1.0) * bp - lp + 1.0e-6 * noise;
                bp += (dt_prime / oversamplingFactor) * hp;
                bp = Math.Tanh(bp);
                lp += (dt_prime / oversamplingFactor) * bp;
                lp = Math.Tanh(lp);

                switch (filterMode)
                {
                    case 0:
                        result = lp;
                        break;

                    case 1:
                        result = bp;
                        break;

                    case 2:
                        result = hp;
                        break;

                    default:
                        result = 0.0;
                        break;
                }

                // downsampling filter
               // result = fir->FIRfilter(result);
            }

            return (float)result;
        }

        private double GetCutoffFrequency(float frequency, float time, int depth, int sampleRate)
        {
            return Math.Pow(2, 10 * cutoff.getAmplitudeOrValue(time, depth + 1, sampleRate) - 10) * 15000;
        }



    }
}
