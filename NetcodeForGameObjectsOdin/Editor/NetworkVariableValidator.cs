using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector.Editor.Validation;
using Sirenix.Utilities;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[assembly: RegisterValidator(typeof(NetworkVariableValidator<>))]
public class NetworkVariableValidator<T> : ValueValidator<NetworkVariable<T>>
{
	protected override void Validate(ValidationResult result)
	{
		if (Property.SerializationRoot.ValueEntry.WeakSmartValue is not NetworkBehaviour) 
			result.AddError(nameof(NetworkVariable<T>) + " is not in " + nameof(NetworkBehaviour) + " script");

		var type = typeof(T);

		if (IsValidType(type))
			return;

		if (type.ImplementsOpenGenericInterface(typeof(ICollection<>)))
		{
			var types = type.GetArgumentsOfInheritedOpenGenericInterface(typeof(ICollection<>));

			foreach (var type1 in types)
			{
				if (IsValidType(type1))
					return;
			}
		}
		
		result.AddError(typeof(T) + " is not supported");
	}

    // Types based on documentation
    // https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.6/manual/basics/networkvariable.html#supported-types
	private static bool IsValidType(Type type)
	{
		if (type == typeof(bool) || 
		    type == typeof(byte) || type == typeof(sbyte) ||
		    type == typeof(char) ||
		    type == typeof(decimal) || type == typeof(double) || type == typeof(float) || 
		    type == typeof(int) || type == typeof(uint) ||
		    type == typeof(long) || type == typeof(ulong) || 
		    type == typeof(short) || type == typeof(ushort) || 
		    type.IsEnum ||
		    type == typeof(Vector2) || type == typeof(Vector3) || 
		    type == typeof(Vector2Int) || type == typeof(Vector3Int) || 
		    type == typeof(Vector4) || type == typeof(Quaternion) ||
		    type == typeof(Color) || type == typeof(Color32) || 
		    type == typeof(Ray) || type == typeof(Ray2D) || 
		    type == typeof(FixedString32Bytes) || type == typeof(FixedString64Bytes) || 
		    type == typeof(FixedString128Bytes) || type == typeof(FixedString512Bytes) || 
		    type == typeof(FixedString4096Bytes) ||
		    type.ImplementsOrInherits(typeof(INetworkSerializable)) ||
		    type.ImplementsOrInherits(typeof(INetworkSerializeByMemcpy)))
		{
			return true;
		}

		var serialization = typeof(UserNetworkVariableSerialization<>).MakeGenericType(type);
		var writeValue = serialization.GetField("WriteValue", BindingFlags.Public | BindingFlags.Static);

		return writeValue != null && writeValue.GetValue(null) != null;
	}
}