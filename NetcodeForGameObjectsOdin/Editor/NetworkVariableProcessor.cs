using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Unity.Netcode;
using UnityEngine;

public class NetworkVariableProcessor<T> : OdinAttributeProcessor<NetworkVariable<T>>
{
	private List<Attribute> m_attributes = new();
	
	public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
	{
		base.ProcessSelfAttributes(property, attributes);
		
		attributes.Add(new SuppressInvalidAttributeErrorAttribute());
	}
}