using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Unity.Netcode;

public class NetworkVariablePropertyResolver<T> : OdinPropertyResolver<NetworkVariable<T>>
{
	public override InspectorPropertyInfo GetChildInfo(int childIndex)
	{
		var getterSetter = Activator.CreateInstance(typeof(NetworkVariableValueGetterSetter)) as IValueGetterSetter;

		return InspectorPropertyInfo.CreateValue("m_InternalValue", 0, SerializationBackend.None, getterSetter,
			Property.Attributes);
	}

	public override int ChildNameToIndex(string name) => 0;

	public override int ChildNameToIndex(ref StringSlice name) => 0;

	protected override int GetChildCount(NetworkVariable<T> value) => 1;

	private class NetworkVariableValueGetterSetter : IValueGetterSetter<NetworkVariable<T>, T>
	{
		public bool IsReadonly => false;

		public Type OwnerType => typeof(NetworkVariable<T>);

		public Type ValueType => typeof(T);
		
		public void SetValue(ref NetworkVariable<T> owner, T value) => owner.Value = value;

		public T GetValue(ref NetworkVariable<T> owner) => owner.Value;

		public void SetValue(object owner, object value) => ((NetworkVariable<T>)owner).Value = (T)value;

		public object GetValue(object owner) => ((NetworkVariable<T>)owner).Value;
	}
}