using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UiKitchen
{
	[System.Serializable]
	public class StateBinding
	{
		[SerializeField, FormerlySerializedAs("_triggerState")]
		private SerializableStateReference _stateEntered;
		public SerializableStateReference StateEntered
		{
			get { return _stateEntered; }
			set { _stateEntered = value; }
		}

		[SerializeField]
		private SerializableAnimatorControllerParameter _parameter;
		public SerializableAnimatorControllerParameter Parameter
		{
			get { return _parameter; }
			set { _parameter = value; }
		}

		[SerializeField]
		private bool _isDelayed = false;
		public bool IsDelayed
		{
			get { return _isDelayed; }
			set { _isDelayed = value; }
		}

		[SerializeField]
		private float _delayTime = 0f;
		public float DelayTime
		{
			get { return _delayTime; }
			set { _delayTime = value; }
		}

		public StateBinding()
		{ }

		public StateBinding(StateBinding original)
		{
			if (original != null)
			{
				_stateEntered = new SerializableStateReference(original._stateEntered);
				_parameter = new SerializableAnimatorControllerParameter(original._parameter);
				_isDelayed = original._isDelayed;
				_delayTime = original._delayTime;
			}
		}
	}
}
