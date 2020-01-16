using UnityEditor;
using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.Module
{
	[CustomEditor(typeof(Envelope), true)]
	internal class EnvelopeEditor : BaseModuleEditor
	{
		public override Rect OnModuleGUI(Rect innerRect)
		{
			Rect rect = base.OnModuleGUI(innerRect);
			Envelope envelope = base.target as Envelope;
			Rect position = new Rect(innerRect.x, rect.y + rect.height, innerRect.width, BaseModuleEditor.AttributeHeight);
			envelope.envelopeType = (Envelope.EnvelopeType)(object)EditorGUI.EnumPopup(position, envelope.envelopeType);
			Rect position2 = new Rect(innerRect.x, position.y + position.height, innerRect.width, 32f);
			try
			{
				envelope.envelope = EditorGUI.CurveField(position2, new GUIContent(""), envelope.envelope);
			}
			catch (ExitGUIException)
			{
			}
			innerRect.height = 32f + BaseModuleEditor.AttributeHeight;
			return innerRect;
		}
	}
}
