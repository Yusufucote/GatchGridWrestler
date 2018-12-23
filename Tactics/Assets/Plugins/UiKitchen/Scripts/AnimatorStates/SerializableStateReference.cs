using System;
using System.Collections.Generic;
using UnityEngine;

namespace UiKitchen
{
	[Serializable]
	public class SerializableStateReference
	{
		[SerializeField]
		string _name = "Unassigned";
		public string Name { get { return _name; } }

		[SerializeField]
		string _guid;
		public string Guid { get { return _guid; } }

		public SerializableStateReference(string name, string guidString)
		{
			this._name = name;
			this._guid = guidString;
		}

		public SerializableStateReference(SerializableStateReference original)
		{
			if (original != null)
			{
				_name = original._name;
				_guid = original._guid;
			}
		}
	}
}
