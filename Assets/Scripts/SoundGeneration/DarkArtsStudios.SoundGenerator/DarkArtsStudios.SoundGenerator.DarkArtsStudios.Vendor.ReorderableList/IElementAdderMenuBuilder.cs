using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	public interface IElementAdderMenuBuilder<TContext>
	{
		void SetContractType(Type contractType);

		void SetElementAdder(IElementAdder<TContext> elementAdder);

		void SetTypeDisplayNameFormatter(Func<Type, string> formatter);

		void AddTypeFilter(Func<Type, bool> typeFilter);

		void AddCustomCommand(IElementAdderMenuCommand<TContext> command);

		IElementAdderMenu GetMenu();
	}
}
