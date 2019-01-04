using System;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchActionSpeedDebuff : AbstractMatchActionHandler {

		[SerializeField]
		MatchWrestlerAgilityHandler agilityHandler;

		int debuffCount;

		public EventHandler<IncermentalMatchActionEventArgs> speedDebuffUpdated;

		private void Start() {
			actionHandler.TurnStarted += HandleTurnStarted;
		}

		private void HandleTurnStarted(object sender, MatchWresterGenericEventArgs e) {
			if (debuffCount > 0) {
				debuffCount -= debuffCount;
				SendSpeedDebuffUpdated();
				if (debuffCount == 0) {
					agilityHandler.SetDebuffed(false);
					actionHandler.SendMatchActionComplete(new MatchAciton(MatchActionType.SpeedDebuff, 0));
				}
			}
		}

		private void SendSpeedDebuffUpdated (){
			if (speedDebuffUpdated != null) {
				speedDebuffUpdated(this, new IncermentalMatchActionEventArgs() {
					ActionType = MatchActionType.SpeedDebuff, 
					Count = debuffCount
				});
			}
		}

		public override void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			int value = (int) e.matchAciton.value;
			if (value > 0) {
				if (debuffCount < value) {
					debuffCount = value;
					SendSpeedDebuffUpdated();
				}
				agilityHandler.SetDebuffed(true);
			}
		}
	}

	public class IncermentalMatchActionEventArgs: EventArgs {
		public MatchActionType ActionType;
		public int Count;
	}
}
