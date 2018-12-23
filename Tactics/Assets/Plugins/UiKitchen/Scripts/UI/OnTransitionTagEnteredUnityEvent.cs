using System;
using System.Collections;
using System.Collections.Generic;
using UiKitchen.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace UiKitchen
{
	public class OnTransitionTagEnteredUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private AnimatorStateBroadcasterBehaviour _broadcaster;

		[SerializeField]
		private float _secondsDelay = 0f;

		[SerializeField]
		private StateTransitionTag _tag;

		[SerializeField]
		StartingStateOrStateEntry _eventTrigger = StartingStateOrStateEntry.OnlyStateEntry;

		[SerializeField]
		UnityEvent _onStateEntered;
		private Coroutine _eventCR = null;

		void Awake()
		{
			_broadcaster.AnimatorStartedInState -= AnimatorStartedInState;
			_broadcaster.AnimatorStartedInState += AnimatorStartedInState;

			_broadcaster.AnimatorEnteredState -= AnimatorEnteredState;
			_broadcaster.AnimatorEnteredState += AnimatorEnteredState;
		}

		void OnDestroy()
		{
			_broadcaster.AnimatorStartedInState -= AnimatorStartedInState;

			_broadcaster.AnimatorEnteredState -= AnimatorEnteredState;
		}

		private void AnimatorStartedInState(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			if (_eventTrigger != StartingStateOrStateEntry.OnlyStateEntry)
				TagTriggered(stateReporter, state, tag);
		}

		private void AnimatorEnteredState(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			if (_eventTrigger != StartingStateOrStateEntry.OnlyStartingState)
				TagTriggered(stateReporter, state, tag);
		}

		private void TagTriggered(MecanimStateEnteredReporter stateReporter, SerializableStateReference state, StateTransitionTag tag)
		{
			if (tag == _tag)
			{
				_eventCR = StartCoroutine(InvokeEvent());
			}
			else if (_eventCR != null)
			{
				StopCoroutine(_eventCR);
				_eventCR = null;
			}
		}

		private IEnumerator InvokeEvent()
		{
			float timeStamp = Time.realtimeSinceStartup;

			yield return new WaitMinFrames();
			var waitTime = _secondsDelay - (Time.realtimeSinceStartup - timeStamp);
			if (waitTime > 0)
				yield return new WaitForSeconds(waitTime);

			_onStateEntered.Invoke();
		}
	}
}
