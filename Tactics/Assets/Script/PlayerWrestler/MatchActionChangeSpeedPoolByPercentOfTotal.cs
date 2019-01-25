using Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchActionChangeSpeedPoolByPercentOfTotal : AbstractMatchActionHandler {

		[SerializeField]
		MatchWrestlerSpeedPoolHandler speedPoolHandler;

		public override void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			if (e.matchAciton.acitonType == Keyword.TurnMeter_ChangeByPercentOfMax) {
				float newSpeedPoolValue = speedPoolHandler.CurrentSpeedPool + e.matchAciton.value;
				speedPoolHandler.SetSpeedPoolValue(newSpeedPoolValue);
				actionHandler.SendMatchActionComplete(new MatchAciton(Keyword.TurnMeter_ChangeByPercentOfMax, 0));
			}
		}
	}
}
