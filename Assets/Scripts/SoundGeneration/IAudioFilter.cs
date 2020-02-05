using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SoundGeneration
{
    interface IAudioFilter
    {
        void OnAudioFilterRead(float[] data, int channels);
    }
}
