using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Core
{
	internal class EditorZoomArea
	{
		private const float _EditorWindowTabHeight = 21f;

		private static Matrix4x4 _prevGuiMatrix;

		public static Rect Begin(float zoomScale, Rect screenCoordsArea)
		{
			GUI.EndGroup();
			Rect rect = screenCoordsArea.ScaleSizeBy(1f / zoomScale, screenCoordsArea.TopLeft());
			rect.y += 21f;
			GUI.BeginGroup(rect);
			_prevGuiMatrix = GUI.matrix;
			Matrix4x4 lhs = Matrix4x4.TRS(rect.TopLeft(), Quaternion.identity, Vector3.one);
			Matrix4x4 rhs = Matrix4x4.Scale(new Vector3(zoomScale, zoomScale, 1f));
			GUI.matrix = lhs * rhs * lhs.inverse * GUI.matrix;
			return rect;
		}

		public static void End()
		{
			GUI.matrix = _prevGuiMatrix;
			GUI.EndGroup();
			GUI.BeginGroup(new Rect(0f, 21f, Screen.width, Screen.height));
		}
	}
}
