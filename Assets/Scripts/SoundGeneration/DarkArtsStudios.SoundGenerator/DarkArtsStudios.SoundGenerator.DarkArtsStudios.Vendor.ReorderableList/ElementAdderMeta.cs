using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.ReorderableList
{
	internal static class ElementAdderMeta
	{
		private static Dictionary<Type, Dictionary<Type, List<Type>>> s_ContextMap = new Dictionary<Type, Dictionary<Type, List<Type>>>();

		private static Dictionary<Type, Type[]> s_ConcreteElementTypes = new Dictionary<Type, Type[]>();

		private static IEnumerable<Type> GetMenuCommandTypes<TContext>()
		{
			return from a in AppDomain.CurrentDomain.GetAssemblies()
				from t in a.GetTypes()
				where t.IsClass && !t.IsAbstract && t.IsDefined(typeof(ElementAdderMenuCommandAttribute), inherit: false)
				where typeof(IElementAdderMenuCommand<TContext>).IsAssignableFrom(t)
				select t;
		}

		public static Type[] GetMenuCommandTypes<TContext>(Type contractType)
		{
			if (contractType == null)
			{
				throw new ArgumentNullException("contractType");
			}
			List<Type> value2;
			if (s_ContextMap.TryGetValue(typeof(TContext), out Dictionary<Type, List<Type>> value))
			{
				if (value.TryGetValue(contractType, out value2))
				{
					return value2.ToArray();
				}
			}
			else
			{
				value = new Dictionary<Type, List<Type>>();
				s_ContextMap[typeof(TContext)] = value;
			}
			value2 = new List<Type>();
			foreach (Type menuCommandType in GetMenuCommandTypes<TContext>())
			{
				if (((ElementAdderMenuCommandAttribute[])Attribute.GetCustomAttributes(menuCommandType, typeof(ElementAdderMenuCommandAttribute))).Any((ElementAdderMenuCommandAttribute a) => a.ContractType == contractType))
				{
					value2.Add(menuCommandType);
				}
			}
			value[contractType] = value2;
			return value2.ToArray();
		}

		public static IElementAdderMenuCommand<TContext>[] GetMenuCommands<TContext>(Type contractType)
		{
			Type[] menuCommandTypes = GetMenuCommandTypes<TContext>(contractType);
			IElementAdderMenuCommand<TContext>[] array = new IElementAdderMenuCommand<TContext>[menuCommandTypes.Length];
			for (int i = 0; i < menuCommandTypes.Length; i++)
			{
				array[i] = (IElementAdderMenuCommand<TContext>)Activator.CreateInstance(menuCommandTypes[i]);
			}
			return array;
		}

		private static IEnumerable<Type> GetConcreteElementTypesHelper(Type contractType)
		{
			if (contractType == null)
			{
				throw new ArgumentNullException("contractType");
			}
			if (!s_ConcreteElementTypes.TryGetValue(contractType, out Type[] value))
			{
				value = (from a in AppDomain.CurrentDomain.GetAssemblies()
					from t in a.GetTypes()
					where t.IsClass && !t.IsAbstract && contractType.IsAssignableFrom(t)
					orderby t.Name
					select t).ToArray();
				s_ConcreteElementTypes[contractType] = value;
			}
			return value;
		}

		public static Type[] GetConcreteElementTypes(Type contractType, Func<Type, bool>[] filters)
		{
			return (from t in GetConcreteElementTypesHelper(contractType)
				where IsTypeIncluded(t, filters)
				select t).ToArray();
		}

		public static Type[] GetConcreteElementTypes(Type contractType)
		{
			return GetConcreteElementTypesHelper(contractType).ToArray();
		}

		private static bool IsTypeIncluded(Type concreteType, Func<Type, bool>[] filters)
		{
			if (filters != null)
			{
				for (int i = 0; i < filters.Length; i++)
				{
					if (!filters[i](concreteType))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
