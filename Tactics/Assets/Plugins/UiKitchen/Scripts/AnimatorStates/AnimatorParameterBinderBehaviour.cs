using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UiKitchen
{
	[RequireComponent(typeof(AnimatorStateBroadcasterBehaviour))]
	public class AnimatorParameterBinderBehaviour : MonoBehaviour
	{

		[SerializeField, FormerlySerializedAs("_target")]
		AnimatorStateBroadcasterBehaviour _broadcaster;
		public AnimatorStateBroadcasterBehaviour Broadcaster
		{
			get
			{
				return _broadcaster;
			}
		}


		private AnimatorStateBroadcasterBehaviour _receiver;
		public AnimatorStateBroadcasterBehaviour Receiver
		{
			get
			{
				if (_receiver == null)
				{
					_receiver = GetComponent<AnimatorStateBroadcasterBehaviour>();
				}
				return _receiver;
			}
		}

		[HideInInspector]
		[SerializeField]
		List<ParameterBinding> _parameterBindings;
		public List<ParameterBinding> ParameterBindings
		{
			get
			{
				return _parameterBindings;
			}

#if UNITY_EDITOR
			// Changing the bindings during runtime is dangerous due to event subscription
			set
			{
				_parameterBindings = value;
			}
#endif
		}

		void Start()
		{
			SubscribeBindings();
		}

		void OnDestroy()
		{
			UnsubscribeBindings();
		}

		private void SubscribeBindings()
		{
			if (Broadcaster != null)
			{
				for (int i = 0; i < _parameterBindings.Count; i++)
				{
					_parameterBindings[i].Subscribe(Broadcaster);
				}
			}
		}

		void UnsubscribeBindings()
		{
			if (Broadcaster != null)
			{
				for (int i = 0; i < _parameterBindings.Count; i++)
				{
					_parameterBindings[i].Unsubscribe(Broadcaster);
				}
			}
		}
	}
}
