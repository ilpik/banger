using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class SampleRateTest : MonoBehaviour
    {
        public int sampleRate;

        public void Awake()
        {
            sampleRate = AudioSettings.outputSampleRate;
        }

        private StringBuilder sb = new StringBuilder();

        private int samplesTakenStart = 5;
        private int samplesTakenEnd = 5;

        private int loggedFrames = 5;
        private int loggedFrameIndex = 0;

        public void OnAudioFilterRead(float[] data, int channels)
        {
            var time = AudioSettings.dspTime;
            for (var index = 0; index < data.Length; index++)
            {
                if (index < samplesTakenStart || index > data.Length - samplesTakenEnd)
                    sb.AppendLine($"Sample {index} time is {time}");
                else if (index == samplesTakenStart)
                    sb.AppendLine("...");

                time += 1.0 / sampleRate / channels;
            }

            sb.AppendLine("End of frame " + loggedFrameIndex);

            if (loggedFrameIndex++ == loggedFrames)
                Debug.Log(sb.ToString());
        }

    }
}
