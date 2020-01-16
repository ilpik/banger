using UnityEngine;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	public interface IElementAdderMenuCommand<TContext>
	{
		GUIContent Content
		{
			get;
		}

		bool CanExecute(IElementAdder<TContext> elementAdder);

		void Execute(IElementAdder<TContext> elementAdder);
	}
}
