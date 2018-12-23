using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UiKitchen
{
	public class InvokeUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private float _secondsDelay = 0.0f;
		[SerializeField]
		private bool _delayIsRealtime = true;

		[SerializeField]
		private UnityEvent _event;

		public void InvokeEvent()
		{
			StartCoroutine(InvokeAfterDelay());
		}

		private IEnumerator InvokeAfterDelay()
		{
			if (_secondsDelay > 0f)
			{
				if (_delayIsRealtime)
					yield return new WaitForSecondsRealtime(_secondsDelay);
				else
					yield return new WaitForSeconds(_secondsDelay);
			}
			_event.Invoke();
		}
	}
}
