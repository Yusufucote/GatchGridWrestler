using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UiKitchen.Utilities
{
	public class OnEnabledUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private float _onEnableDelaySeconds = 0f;
		[SerializeField]
		private UnityEvent _onEnabled;

		Coroutine _delayCR;

		private void OnEnable()
		{
			_delayCR = StartCoroutine(DelayedOnEnable(_onEnableDelaySeconds));
		}

		private IEnumerator DelayedOnEnable(float onEnableDelaySeconds)
		{
			yield return new WaitForSeconds(onEnableDelaySeconds);
			_onEnabled.Invoke();
		}

		private void OnDisable()
		{
			if (_delayCR != null)
				StopCoroutine(_delayCR);
			_delayCR = null;
		}
	}
}
