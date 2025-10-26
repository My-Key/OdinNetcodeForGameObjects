using Sirenix.OdinInspector.Editor;
using Unity.Netcode;
using Unity.Netcode.Editor;
using UnityEditor;

[CustomEditor(typeof(NetworkBehaviour), true)]
public class OdinNetworkBehaviourEditor : OdinEditor
{
	private void OnEnable()
	{
		if (target == null)
			return;

		NetworkBehaviourEditor.CheckForNetworkObject(((NetworkBehaviour)target).gameObject);
	}
}