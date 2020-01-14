﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator.Module;
using NAudio.Dsp;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Assets.Scripts.SoundGeneration.Adsr
{
    class AdsrEnvelope : BaseModule
    {
        public static string MenuEntry()
        {
            return "Banger/ADSR";
        }

        public Vector2 attack;

        private Attribute attackAtt;
        private Attribute attackStartAtt;

        public Vector2 decay;

        private Attribute decayAtt;

        public Vector2 sustain;

        private Attribute sustainAtt;

        public Vector2 release;

        private Attribute releaseAtt;
        private Attribute releaseStartAtt;

        private BaseModule Input => this.GetInput();

        public override void InitializeAttributes()
        {
            attributes.Add(new Attribute("Input", _hiddenValue: true));
            attackAtt = AddAttribute("Attack");
            attackStartAtt = AddAttribute("Attack Start");
            decayAtt = AddAttribute("Decay");
            sustainAtt = AddAttribute("Sustain");
            releaseAtt = AddAttribute("Release");
            releaseStartAtt = AddAttribute("Release Start");
        }

        private Attribute AddAttribute(string attName)
        {
            Attribute att = new Attribute(attName, 1f)
            {
                clampMinimum = true,
                clampMinimumValue = 0f,
                //clampMaximum = true,
                clampMaximumValue = 1f,
                allowInput = true,
                type = Attribute.AttributeType.SLIDER
            };
            attributes.Add(att);
            return att;
        }


        private enum Stage
        {
            Off,
            Attack,
            Decay,
            Sustain,
            Release
        }

        private Stage CurrentStage = Stage.Off;
        EnvelopeGenerator g = new EnvelopeGenerator();

        public override float OnAmplitude(float frequency, float time, float duration, int depth)
        {
            g.AttackRate = attackAtt.value;
            g.DecayRate = decayAtt.value;
            g.ReleaseRate = releaseAtt.value;
            g.SustainLevel = sustainAtt.value;
            return g.Process();
            //float currentLevel = 1.0f;
            //if (CurrentStage != Stage.Off &&
            //    CurrentStage != Stage.Sustain)
            //{
            //    if (currentSampleIndex == nextStageSampleIndex)
            //    {
            //        Stage newStage = (Stage) (((int)CurrentStage + 1) % Enum.GetNames(typeof(Stage)).Length);
            //        enterStage(newStage);
            //    }
            //    currentLevel *= multiplier;
            //    currentSampleIndex++;
            //}
            //return currentLevel;
        }

        float CalculateMultiplier(float startLevel, float endLevel, long lengthInSamples)
        {
            return 1.0f + (Mathf.Log(endLevel) - Mathf.Log(startLevel)) / (lengthInSamples);
        }

        //void EnvelopeGenerator::enterStage(EnvelopeStage newStage)
        //{
        //    currentStage = newStage;
        //    currentSampleIndex = 0;
        //    if (currentStage == ENVELOPE_STAGE_OFF ||
        //        currentStage == ENVELOPE_STAGE_SUSTAIN)
        //    {
        //        nextStageSampleIndex = 0;
        //    }
        //    else
        //    {
        //        nextStageSampleIndex = stageValue[currentStage] * sampleRate;
        //    }
        //    switch (newStage)
        //    {
        //        case ENVELOPE_STAGE_OFF:
        //            currentLevel = 0.0;
        //            multiplier = 1.0;
        //            break;
        //        case ENVELOPE_STAGE_ATTACK:
        //            currentLevel = minimumLevel;
        //            calculateMultiplier(currentLevel,
        //                1.0,
        //                nextStageSampleIndex);
        //            break;
        //        case ENVELOPE_STAGE_DECAY:
        //            currentLevel = 1.0;
        //            calculateMultiplier(currentLevel,
        //                fmax(stageValue[ENVELOPE_STAGE_SUSTAIN], minimumLevel),
        //                nextStageSampleIndex);
        //            break;
        //        case ENVELOPE_STAGE_SUSTAIN:
        //            currentLevel = stageValue[ENVELOPE_STAGE_SUSTAIN];
        //            multiplier = 1.0;
        //            break;
        //        case ENVELOPE_STAGE_RELEASE:
        //            // We could go from ATTACK/DECAY to RELEASE,
        //            // so we're not changing currentLevel here.
        //            calculateMultiplier(currentLevel,
        //                minimumLevel,
        //                nextStageSampleIndex);
        //            break;
        //        default:
        //            break;
        //    }
        //}



        private float Attack(float frequency, float time, float duration, int depth)
        {
            var len = 10000;
            int step = (int)(time * len) % len;

            float atkStart = attackStartAtt.value;
            int atkEndsAt = (int)(atkStart * len);

            float v = Mathf.Min(step / (float)atkEndsAt, 1.0f);
            var atk = attackAtt.value;
            //return Mathf.Pow(v, -3);
            return v * atk;
        }

        private float Sustain(float time)
        {
            return 1.0f;
        }

        private float Release(float time)
        {
            var len = 10000;
            int step = (int)(time * len) % len;

            float relStart = releaseStartAtt.value;

            if (step < relStart * len)
                return 1.0f; //no effect

            
            float v = Mathf.Max((len - step)/(float)len, 0.0f);
            var rel = releaseAtt.value;
            //return Mathf.Pow(v, -3);
            return v * rel;

        }

        private float Envelope(float time)
        {
            if (time > attack.X && time <= decay.X)
                return time * attack.Y;

            if (time > decay.X && time <= sustain.X)
                return time * decay.Y;

            if (time > sustain.X && time <= release.X)
                return time * sustain.Y;

            
            return time * release.Y;


        }
    }
}
