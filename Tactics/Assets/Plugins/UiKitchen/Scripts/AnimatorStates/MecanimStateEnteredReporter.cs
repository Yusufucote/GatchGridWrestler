using System;
using System.Collections.Generic;
using UnityEngine;

namespace UiKitchen
{
	public delegate void MecanimStateEnteredDelegate(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag);

	public class MecanimStateEnteredReporter : StateMachineBehaviour
	{
		[SerializeField]
		private string _stateName;
		public string StateName
		{
			get
			{
				return _stateName;
			}
		}

		[SerializeField]
		private bool _isEntryState = false;
		private bool _hasEnteredState = false;

		[SerializeField]
		StateTransitionTag _onEnteredTag;

		[SerializeField]
		private string _guidString = Guid.NewGuid().ToString();
		private SerializableStateReference _serializableReference = null;

		public string GuidString
		{
			get
			{
				return _guidString;
			}

			set
			{
				_guidString = value;
			}
		}

		public event MecanimStateEnteredDelegate StartedInState;
		public event MecanimStateEnteredDelegate EnteredState;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (_isEntryState && !_hasEnteredState)
			{
				if (StartedInState != null)
				{
					StartedInState(this, ToSerializableReference(), _onEnteredTag);
				}
				_hasEnteredState = true;
			}
			else
			{
				if (EnteredState != null)
				{
					EnteredState(this, ToSerializableReference(), _onEnteredTag);
				}
			}
		}

		public SerializableStateReference ToSerializableReference()
		{
			if (_serializableReference == null)
			{
				_serializableReference = new SerializableStateReference(_stateName, _guidString);
			}
			return _serializableReference;
		}
	}
}


public class AttackBehavior : StateMachineBehaviour
{
	[SerializeField]
	AttackEnum attack;

	public EventHandler<AttackBehaviourEvent> AttackStarted;
	public EventHandler<AttackBehaviourEvent> AttackEnded;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (AttackStarted != null) {
			AttackStarted(this, new AttackBehaviourEvent() { Attack = attack });
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (AttackStarted != null) {
			AttackEnded(this, new AttackBehaviourEvent() { Attack = attack });
		}
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}
}

public class AttackBehaviourEvent: EventArgs
{
	public AttackEnum Attack;
}

public enum AttackEnum
{
	punch
}