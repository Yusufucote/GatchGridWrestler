using System;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchActionSpeedDebuff : AbstractMatchActionHandler {

		[SerializeField]
		MatchWrestlerAgilityHandler agilityHandler;

		int debuffCount;

		public EventHandler<MatchWrestlerActionRecievedEventArgs> speedDebuffUpdated;

		private void Start() {
			actionHandler.TurnStarted += HandleTurnStarted;
		}

		private void HandleTurnStarted(object sender, MatchWresterGenericEventArgs e) {
			if (debuffCount > 0) {
				debuffCount--;
				SendSpeedDebuffUpdated();
				if (debuffCount == 0) {
					agilityHandler.SetDebuffed(false);
					actionHandler.SendMatchActionComplete(new MatchAciton(MatchActionType.SpeedDebuff, 0));
				}
			}
		}

		private void SendSpeedDebuffUpdated (){
			MatchAciton updatedMatchAction = new MatchAciton(MatchActionType.SpeedDebuff, debuffCount);
			actionHandler.SendMatchActionUpdatedEvent(updatedMatchAction);
		}

		public override void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			if (e.matchAciton.acitonType == MatchActionType.SpeedDebuff) {
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
	}

	public class IncermentalMatchActionEventArgs: EventArgs {
		public MatchActionType ActionType;
		public int Count;
	}
}
