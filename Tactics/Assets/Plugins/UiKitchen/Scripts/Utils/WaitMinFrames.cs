using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiKitchen.Utilities
{
	/// <summary>
	/// Waits for a minimum of the specified frame count or the seconds specified, whichever is longer.
	/// <para>Most useful for delayed coroutines that start in OnEnable and are cancelled during OnDisable</para>
	/// </summary>
	public class WaitMinFrames : CustomYieldInstruction
	{
		private int _minFramesToWait = 2; // Must skip 2 frames for this to solve for Enable/Disable framing
		private int _framesWaited = 0;
		private float _timeLeft = 0f;
		private bool _realtime = false;

		/// <summary>
		/// keepWaiting is called once per frame to see if the coroutine should continue waiting.
		/// </summary>
		/// <returns>
		/// True to indicate the coroutine should keep waiting.
		/// </returns>
		public override bool keepWaiting
		{
			get
			{
				_framesWaited++;
				if (_realtime)
					_timeLeft -= Time.fixedDeltaTime;
				else
					_timeLeft -= Time.deltaTime;

				return (_framesWaited < _minFramesToWait || 0f < _timeLeft);
			}
		}

		public WaitMinFrames(float minSecondsToWait = 0f, int minFramesToWait = 2, bool realtime = false)
		{
			_timeLeft = minSecondsToWait;
			_realtime = realtime;
		}

	}
}
