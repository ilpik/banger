using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal static class ElementAdderMenuBuilder
	{
		public static IElementAdderMenuBuilder<TContext> For<TContext>()
		{
			return new GenericElementAdderMenuBuilder<TContext>();
		}

		public static IElementAdderMenuBuilder<TContext> For<TContext>(Type contractType)
		{
			IElementAdderMenuBuilder<TContext> elementAdderMenuBuilder = For<TContext>();
			elementAdderMenuBuilder.SetContractType(contractType);
			return elementAdderMenuBuilder;
		}
	}
}
