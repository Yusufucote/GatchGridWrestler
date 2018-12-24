using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchActionChangeSpeedPoolByPercentOfTotal : MonoBehaviour {

		[SerializeField]
		MatchWrestlerSpeedPoolHandler speedPoolHandler;

		public void HandleAction(float value) {
			float newSpeedPoolValue = speedPoolHandler.CurrentSpeedPool + value;
			speedPoolHandler.SetSpeedPoolValue(newSpeedPoolValue);
		}

	}
}
