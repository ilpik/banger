using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Assets.Scripts.SoundGeneration.Keyboard;
using Assets.Scripts.SoundGeneration.Util;
using DarkArtsStudios.SoundGenerator;
using DarkArtsStudios.SoundGenerator.DarkArtsStudios.Audio;
using DarkArtsStudios.SoundGenerator.Module;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.SoundGeneration.Modules
{
    class MonoKeyboardInput : DAAudioFilter
    {
        public static string MenuEntry() => MenuEntryProvider.Get("Mono Keyboard Input");

        public Attribute active;

        public Attribute octave;

        public int? note;

        public int? octaveDelta;

        public override void InitializeAttributes()
        {
            active = AddAttribute("Active", b => b.WithType(Attribute.AttributeType.BUTTON).WithInput(false));
            active.pressed = true;

            octave = AddAttribute("Octave", b => b.Slider(1, 8).WithValue(4));
        }

        public override double OnAmplitude(double time, int depth, int sampleRate)
        {
            if (!active.pressed)
                return 0f;

            if (note == null || octaveDelta == null)
                return 0;

            var currentOctave = MathUtil.RoundToInt(octave.getAmplitudeOrValue(time, depth + 1, sampleRate)) +
                                octaveDelta.Value;

            float freq;
            try
            {
                freq = Music.Frequency(currentOctave, note.Value);
            }
            catch (InvalidOperationException ioe)
            {
                freq = 0;
            }
            

            return freq;
        }
    }

    [CustomEditor(typeof(MonoKeyboardInput), true)]
    class KeyboardInputEditor : BaseModuleEditor
    {
        private MonoKeyboardInput kb => (MonoKeyboardInput) target;

        private KeyCode? currentKeyPressed;

        public override Rect OnModuleGUI(Rect innerRect)
        {
            bool isActive = kb.active.pressed;
         
            var texture = GetTexture(isActive);
            innerRect.width = texture.width;

            var rect = base.OnModuleGUI(innerRect);

            innerRect.height = rect.height + texture.height;

            var texturePos = new Rect(innerRect.x, rect.y + rect.height, texture.width, texture.height);


            var e = Event.current;
            if (isActive && EditorWindow.focusedWindow.GetType() == typeof(CompositionEditor) && 
                e.type == EventType.KeyDown && 
                KeyboardSoundController.notes.TryGetValue(e.keyCode, out var noteInfo))
            {
                currentKeyPressed = e.keyCode;
                kb.note = noteInfo.note;
                kb.octaveDelta = noteInfo.octave;
            }
            else if (isActive && e.type == EventType.MouseDown && texturePos.Contains(e.mousePosition))
            {
                var mp = e.mousePosition;
                var wr = blackRects.Concat(whiteRects).FirstOrDefault(r =>
                {
                    var realPos = new Rect(texturePos.x + r.rect.x, texturePos.y, r.rect.width, r.rect.height);
                    return realPos.Contains(mp);
                });

                if (wr != default)
                {
                    currentKeyPressed = wr.key;
                    var n = KeyboardSoundController.notes.TryGetValue(wr.key, out var ni);
                    kb.note = ni.note;
                    kb.octaveDelta = ni.octave;
                }
            }
            else if (e.type == EventType.KeyUp || e.type == EventType.MouseUp)
            {
               currentKeyPressed = null;
               kb.note = null;
               kb.octaveDelta = null;
            }


            GUI.DrawTexture(texturePos, texture);

            return innerRect;
        }

        private List<(Rect rect, KeyCode key)> whiteRects = new List<(Rect, KeyCode)>();
        private List<(Rect rect, KeyCode key)> blackRects = new List<(Rect, KeyCode)>();

        private Texture2D GetTexture(bool isActive)
        {
            var notes = KeyboardSoundController.notes;
            var whiteKeyWidth = 15;
            var whiteSize = new Vector2(whiteKeyWidth, whiteKeyWidth * 6);

            var blackKeyWidth = 8;
            var blackSize = new Vector2(blackKeyWidth, blackKeyWidth * 6);

            bool IsBlack(int note) => Music.NoteName[note].EndsWith("#");

            var texture = new Texture2D((notes.Count(n => !IsBlack(n.Value.note)) + 1) * whiteKeyWidth, (int) whiteSize.y + 5);
            
            Vector2 prevWhitePosition = new Vector2(-whiteSize.x + 1, 0);

            bool firstPass = whiteRects.Count == 0;

            var pressedWhiteRects = new List<Rect>();
            var pressedBlackRects = new List<Rect>();

            foreach (var note in notes)
            {
                var noteValue = note.Value.note;
                bool black = IsBlack(noteValue);
                Rect rect;

                if (!black)
                {
                    rect = new Rect(prevWhitePosition.x + whiteSize.x, texture.height - whiteSize.y - 1, whiteSize.x, whiteSize.y);
                    if (currentKeyPressed == note.Key)
                        pressedWhiteRects.Add(rect);
                    
                    if (firstPass)
                        whiteRects.Add((rect, note.Key));

                    prevWhitePosition = rect.position;
                }
                else
                {
                    rect = new Rect(prevWhitePosition.x + whiteSize.x - (blackSize.x / 2), texture.height - blackSize.y - 1, blackSize.x, blackSize.y);
                    if (currentKeyPressed == note.Key)
                        pressedBlackRects.Add(rect);
                    
                    if (firstPass)
                        blackRects.Add((rect, note.Key));
                }
            }

            Color pressColor = Color.magenta;

            foreach (var rect in whiteRects)
                PaintRect(texture, rect.rect, isActive ? Color.white : new Color(0.6f, 0.6f, 0.6f));

            foreach (var rect in pressedWhiteRects)
                PaintRect(texture, rect, pressColor);

            foreach (var rect in blackRects)
                PaintRect(texture, rect.rect, isActive ? Color.black : new Color(0.2f, 0.2f, 0.2f));

            foreach (var rect in pressedBlackRects)
                PaintRect(texture, rect, pressColor);

            texture.Apply(false);

            return texture;
        }

        private void PaintRect(Texture2D texture, Rect rect, Color fillColor)
        {
            FillRect(texture, rect, fillColor);
            DrawRect(texture, rect, Color.black);
        }

        private void DrawRect(Texture2D texture, Rect rect, Color color)
        {
            var x = Mathf.RoundToInt(rect.x);
            var y = Mathf.RoundToInt(rect.y);
            var width = Mathf.RoundToInt(rect.width);
            var height = Mathf.RoundToInt(rect.height);

            var colors = Enumerable.Repeat(color, width).ToArray();
            texture.SetPixels(x, y, width, 1, colors);
            texture.SetPixels(x, y + height, width, 1, colors);

            colors = Enumerable.Repeat(color, height).ToArray();
            texture.SetPixels(x, y, 1, height, colors);
            texture.SetPixels(x + width, y, 1, height, colors);
        }

        private void FillRect(Texture2D texture, Rect rect, Color color)
        {
            var x = Mathf.RoundToInt(rect.x);
            var y = Mathf.RoundToInt(rect.y);
            var width = Mathf.RoundToInt(rect.width);
            var height = Mathf.RoundToInt(rect.height);

            var colors = Enumerable.Repeat(color, width * height).ToArray();
            texture.SetPixels(x, y, width, height, colors);
        }
    }
}