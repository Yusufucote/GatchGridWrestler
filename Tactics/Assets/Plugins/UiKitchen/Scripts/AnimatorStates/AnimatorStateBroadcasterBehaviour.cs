using System;
using System.Collections.Generic;
using UiKitchen.Utilities;
using UnityEngine;

namespace UiKitchen
{
	/// <summary>
	/// Broadcasts state and parameter changes on the Animator.
	/// <para>NOTE: Must use the Set calls on this class instead of Animator to broadcast parameter sets</para>
	/// </summary>
	[RequireComponent(typeof(Animator))]
	public class AnimatorStateBroadcasterBehaviour : MonoBehaviour
	{
		Animator _animator;
		public Animator Animator
		{
			get
			{
				if (_animator == null)
				{
					_animator = GetComponent<Animator>();
				}
				return _animator;
			}
		}

		List<MecanimStateEnteredReporter> _animatorReporters;

		public SerializableStateReference LastStateEntered { get; private set; }

		public class AnimatorBoolSetEventArgs : EventArgs
		{
			public string BoolName;
			public bool BoolValue;
		}

		public delegate void TriggerSetDelegate(string name);
		public event TriggerSetDelegate TriggerSet;

		public delegate void IntSetDelegate(string name, int value);
		public event IntSetDelegate IntSet;

		public delegate void FloatSetDelegate(string name, float value);
		public event FloatSetDelegate FloatSet;

		public delegate void BoolSetDelegate(string name, bool value);
		public event BoolSetDelegate BoolSet;

		public event MecanimStateEnteredDelegate AnimatorStartedInState;
		public event MecanimStateEnteredDelegate AnimatorEnteredState;

		void Start()
		{
			_animatorReporters = Animator.GetBehaviourList<MecanimStateEnteredReporter>();
			foreach (var reporter in _animatorReporters)
			{
				reporter.StartedInState += Reporter_StartedInState;
				reporter.EnteredState += Reporter_EnteredState;
			}
		}

		private void Reporter_StartedInState(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			LastStateEntered = state;
			if (AnimatorStartedInState != null)
			{
				AnimatorStartedInState(stateReporter, state, tag);
			}
		}

		void Reporter_EnteredState(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			LastStateEntered = state;
			if (AnimatorEnteredState != null)
			{
				AnimatorEnteredState(stateReporter, state, tag);
			}
		}

		public void SetParameter(SerializableAnimatorControllerParameter param)
		{
			if (param != null)
			{
				switch (param.Type)
				{
					case AnimatorControllerParameterType.Int:
						SetInt(param.Name, param.IntValue);
						break;
					case AnimatorControllerParameterType.Float:
						SetFloat(param.Name, param.FloatValue);
						break;
					case AnimatorControllerParameterType.Bool:
						SetBool(param.Name, param.BoolValue);
						break;
					case AnimatorControllerParameterType.Trigger:
						SetTrigger(param.Name);
						break;
				}
			}
		}

		public void SetTrigger(string trigger)
		{
			if (TriggerSet != null)
			{
				TriggerSet(trigger);
			}
			Animator.SetTrigger(trigger);
		}

		public void SetInt(string name, int value)
		{
			if (IntSet != null)
			{
				IntSet(name, value);
			}
			Animator.SetInteger(name, value);
		}

		public void SetFloat(string name, float value)
		{
			if (FloatSet != null)
			{
				FloatSet(name, value);
			}
			Animator.SetFloat(name, value);
		}

		public void SetBool(string name, bool value)
		{
			if (BoolSet != null)
			{
				BoolSet(name, value);
			}
			Animator.SetBool(name, value);
		}

		public bool GetBool(string name)
		{
			var value = Animator.GetBool(name);
			return value;
		}

		public void SetBoolTrue(string name)
		{

			SetBool(name, true);
		}

		public void SetBoolFalse(string name)
		{
			SetBool(name, false);
		}

		public void ToggleBool(string name)
		{
			var value = GetBool(name);
			SetBool(name, !value);
		}

		void OnDestory()
		{
			foreach (var reporter in _animatorReporters)
			{
				reporter.EnteredState -= Reporter_EnteredState;
			}
		}
	}
}
