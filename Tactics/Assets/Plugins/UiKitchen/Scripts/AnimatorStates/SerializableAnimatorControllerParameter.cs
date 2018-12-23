using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UiKitchen
{
	/// <summary>
	/// Serializable version of Unity's AnimatorControllerParameter
	/// </summary>
	[Serializable]
	public class SerializableAnimatorControllerParameter
	{
		[SerializeField]
		private string _name = "Unassigned";
		public string Name { get { return _name; } }

		[SerializeField]
		private AnimatorControllerParameterType _type = AnimatorControllerParameterType.Int;
		public AnimatorControllerParameterType Type { get { return _type; } }

		[SerializeField]
		private int _intValue;
		public int IntValue
		{
			get { return _intValue; }
			set { _intValue = value; }
		}
		[SerializeField]
		private float _floatValue;
		public float FloatValue
		{
			get { return _floatValue; }
			set { _floatValue = value; }
		}
		[SerializeField]
		private bool _boolValue;
		public bool BoolValue
		{
			get { return _boolValue; }
			set { _boolValue = value; }
		}

		public SerializableAnimatorControllerParameter(string name = "Unassigned", AnimatorControllerParameterType type = AnimatorControllerParameterType.Bool)
		{
			_name = name;
			_type = type;
		}

		public SerializableAnimatorControllerParameter(AnimatorControllerParameter parameter)
		{
			CopyFrom(parameter);
		}

		public SerializableAnimatorControllerParameter(SerializableAnimatorControllerParameter original)
		{
			if (original != null)
			{
				_name = original._name;
				_type = original._type;
				_intValue = original._intValue;
				_floatValue = original._floatValue;
				_boolValue = original._boolValue;
			}
		}

		public void CopyFrom(AnimatorControllerParameter parameter)
		{
			_name = parameter.name;
			_type = parameter.type;
			_intValue = parameter.defaultInt;
			_floatValue = parameter.defaultFloat;
			_boolValue = parameter.defaultBool;
		}
	}
}
