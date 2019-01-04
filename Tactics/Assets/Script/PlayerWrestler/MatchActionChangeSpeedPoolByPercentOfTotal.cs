using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchActionChangeSpeedPoolByPercentOfTotal : AbstractMatchActionHandler {

		[SerializeField]
		MatchWrestlerSpeedPoolHandler speedPoolHandler;

		public override void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			float newSpeedPoolValue = speedPoolHandler.CurrentSpeedPool + e.matchAciton.value;
			speedPoolHandler.SetSpeedPoolValue(newSpeedPoolValue);
			actionHandler.SendMatchActionComplete(new MatchAciton(MatchActionType.ChangeSpeedPoolByPercentOfTotal, 0));
		}
	}
}
