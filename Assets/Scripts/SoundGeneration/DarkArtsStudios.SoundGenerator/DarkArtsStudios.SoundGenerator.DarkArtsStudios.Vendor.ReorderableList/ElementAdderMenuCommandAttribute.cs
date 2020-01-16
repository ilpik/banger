using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	internal sealed class ElementAdderMenuCommandAttribute : Attribute
	{
		public Type ContractType
		{
			get;
			private set;
		}

		public ElementAdderMenuCommandAttribute(Type contractType)
		{
			ContractType = contractType;
		}
	}
}
