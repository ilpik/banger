using System.Collections.Generic;
using Assets.Scripts.SoundGeneration.Modules;
using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
    [CustomEditor(typeof(Scope), true)]
    internal class ScopeEditor : BaseModuleEditor
    {
        private Scope sc => (Scope) target;

        private float speed = 1f;

        private Rect? texturePosition;

        public override Rect OnModuleGUI(Rect innerRect)
        {
            innerRect.width = 200f;
            Rect rect = base.OnModuleGUI(innerRect);

            texturePosition = new Rect(innerRect.x, rect.y + rect.height, innerRect.width, 200);
            UpdateTexture();
           
            innerRect.height = texturePosition.Value.height;
            return innerRect;
        }

        private void UpdateTexture()
        {
            if (texturePosition == null)
                return;

            var texture = new Texture2D((int)texturePosition.Value.width, (int)texturePosition.Value.height, TextureFormat.ARGB32, mipChain: false);

            double time = AudioSettings.dspTime;
            Render(texture, time);

            GUI.DrawTexture(texturePosition.Value, texture);

        }


        private void Render(Texture2D texture, double time)
        {
            if (sc.showAxis.pressed)
            {
                DrawLine(texture, new Vector2(0, texture.height / 2f), new Vector2(texture.width, texture.height / 2f),
                Color.magenta);
            }

            int size = Mathf.FloorToInt(texturePosition.Value.width);
            float zoom = sc.zoom.value; 

            float scaleX = texturePosition.Value.width / size;
            float scaleY = texturePosition.Value.height / 2f;
            
            int sampleRate = 44100;

            Vector2? oldPoint = null;

            for (int i = 0; i < size - 1; i++)
            {
                time += zoom / (float)sampleRate;
                var t= time * sc.speed.value;

                var value = (float)sc.amplitude((float) t, 0, sampleRate);
                i++;

                Vector2? newPoint = new Vector2(i * scaleX , texture.height / 2f - value * scaleY);

                if (oldPoint != null)
                {
                    DrawLine(texture, oldPoint.Value, newPoint.Value, Color.black);
                }

                oldPoint = newPoint;
            }

            texture.Apply();
            
        }

        public static void DrawLine(Texture2D tex, Vector2 p1, Vector2 p2, Color col)
        {
            Vector2 t = p1;
            float frac = 1 / Mathf.Sqrt(Mathf.Pow(p2.x - p1.x, 2) + Mathf.Pow(p2.y - p1.y, 2));
            float ctr = 0;

            while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y)
            {
                t = Vector2.Lerp(p1, p2, ctr);
                ctr += frac;
                tex.SetPixel((int)t.x, (int)t.y, col);
            }
        }

    }
}