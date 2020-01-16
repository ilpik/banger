using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	public interface IElementAdder<TContext>
	{
		TContext Object
		{
			get;
		}

		bool CanAddElement(Type type);

		object AddElement(Type type);
	}
}
