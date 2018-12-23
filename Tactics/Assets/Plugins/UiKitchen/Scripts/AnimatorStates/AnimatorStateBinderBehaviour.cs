using System;
using System.Collections;
using System.Collections.Generic;
using UiKitchen.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace UiKitchen
{
	[RequireComponent(typeof(AnimatorStateBroadcasterBehaviour))]
	public class AnimatorStateBinderBehaviour : MonoBehaviour
	{
		[SerializeField, FormerlySerializedAs("_target")]
		private AnimatorStateBroadcasterBehaviour _broadcaster;
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

		[SerializeField]
		private List<StateBinding> _stateBindings = new List<StateBinding>();
		public List<StateBinding> StateBindings
		{
			get { return _stateBindings; }
			set { _stateBindings = value; }
		}

		private List<Coroutine> _delayedParameterSets = new List<Coroutine>();

		void Start()
		{
			Broadcaster.AnimatorEnteredState -= Broadcaster_AnimatorStateEntered;
			Broadcaster.AnimatorEnteredState += Broadcaster_AnimatorStateEntered;

			Broadcaster.AnimatorStartedInState -= Broadcaster_AnimatorStartedInState;
			Broadcaster.AnimatorStartedInState += Broadcaster_AnimatorStartedInState;
		}

		private void Broadcaster_AnimatorStartedInState(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			EnteredState(stateReporter, state, tag);
		}

		void Broadcaster_AnimatorStateEntered(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			EnteredState(stateReporter, state, tag);
		}

		private void EnteredState(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			StopDelayedParameterChanges();
			foreach (var binding in _stateBindings)
			{
				if (binding.StateEntered.Guid == state.Guid)
				{
					if (!binding.IsDelayed)
					{
						Receiver.SetParameter(binding.Parameter);
					}
					else if (gameObject.activeInHierarchy)
					{
						_delayedParameterSets.Add(StartCoroutine(SetParameterAfterDelay(binding.DelayTime, binding.Parameter)));
					}
				}
			}
		}

		private IEnumerator SetParameterAfterDelay(float delayTime, SerializableAnimatorControllerParameter parameter)
		{
			yield return new WaitForSeconds(delayTime);
			Receiver.SetParameter(parameter);
		}

		private void StopDelayedParameterChanges()
		{
			for (int i = 0; i < _delayedParameterSets.Count; i++)
			{
				StopCoroutine(_delayedParameterSets[i]);
			}
			_delayedParameterSets.Clear();
		}

		void OnDestroy()
		{
			StopDelayedParameterChanges();
			if (Broadcaster != null)
			{
				Broadcaster.AnimatorStartedInState -= Broadcaster_AnimatorStartedInState;
				Broadcaster.AnimatorEnteredState -= Broadcaster_AnimatorStateEntered;
			}
		}

#if UNITY_EDITOR
		/// <summary>
		/// Scans all bindings for states that have Guids that no longer map to a valid state and attempts to remap on name
		///	<para>Returns the number of remaps performed</para>
		/// </summary>
		public int RecoverUnassignedStates()
		{
			int remapCount = 0;
			if (Broadcaster != null)
			{
				var validStates = Broadcaster.Animator.GetStateReferences();

				foreach (var binding in StateBindings)
				{
					if (!string.IsNullOrEmpty(binding.StateEntered.Guid))
					{
						bool foundState = false;
						foreach (var state in validStates)
						{
							if (state.Guid == binding.StateEntered.Guid)
							{
								foundState = true;
								break;
							}
						}
						if (!foundState)
						{
							// Doesn't map to a state, attempt map on name
							foreach (var state in validStates)
							{
								if (state.Name == binding.StateEntered.Name)
								{
									// Found a remap
									binding.StateEntered = new SerializableStateReference(state);
									remapCount++;
									break;
								}
							}
						}
					}
				}
			}
			return remapCount;
		}
#endif
	}
}
