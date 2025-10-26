using System;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;

[DrawerPriority(DrawerPriorityLevel.ValuePriority)]
public class NetworkVariableDrawer<T> : OdinValueDrawer<NetworkVariable<T>>
{
	private static readonly GUIContent NETWORK_VARIABLE_LABEL_GUI_CONTENT = new(string.Empty,
		"This variable is a NetworkVariable. It can not be serialized and can only be changed during runtime.");

	private static MethodInfo m_isBehaviourEditable;

	protected override void Initialize()
	{
		base.Initialize();
		
		m_isBehaviourEditable = typeof(NetworkBehaviour).GetMethod("IsBehaviourEditable", BindingFlags.Instance | BindingFlags.NonPublic);
	}

	private bool IsBehaviourEditable(NetworkBehaviour networkBehaviour) =>
		(bool)m_isBehaviourEditable.Invoke(networkBehaviour, null);

	protected override void DrawPropertyLayout(GUIContent label)
	{
		SirenixEditorGUI.BeginIndentedHorizontal();

		if (Property.SerializationRoot.ValueEntry.WeakSmartValue is NetworkBehaviour behaviour)
			GUIHelper.PushGUIEnabled(IsBehaviourEditable(behaviour));
		else
			GUIHelper.PushGUIEnabled(false);

		SirenixEditorGUI.BeginIndentedVertical();
		Property.Children[0].Draw(label);
		SirenixEditorGUI.EndIndentedVertical();
		
		GUIHelper.PopGUIEnabled();

		var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight,
			GUILayoutOptions.Width(EditorGUIUtility.singleLineHeight));
		
		EditorGUI.LabelField(rect, NETWORK_VARIABLE_LABEL_GUI_CONTENT);
		SdfIcons.DrawIcon(rect, SdfIconType.Link45deg, Color.cyan);
		
		SirenixEditorGUI.EndIndentedHorizontal();
	}
}