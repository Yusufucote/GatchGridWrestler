using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PlayerWrestler {
	public class MatchWrestlerActionHandler : MonoBehaviour {

		[SerializeField]
		MatchActionChangeSpeedPoolByPercentOfTotal changeSpeedPoolByPercentOfTotal;

		public void HandleMatchAction(MatchAciton matchAction) {
			switch (matchAction.acitonType) {
				case MatchActionType.ChangeSpeedPoolByPercentOfTotal:
					changeSpeedPoolByPercentOfTotal.HandleAction(matchAction.value);
					break;
				case MatchActionType.SpeedDebuff:

					break;
				default:
					break;
			}

		}

	}
}
