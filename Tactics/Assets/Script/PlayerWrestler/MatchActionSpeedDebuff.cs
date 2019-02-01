using Abilities;
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
					actionHandler.SendMatchActionComplete(new MatchAciton(Keyword.Debuff_LowerSpeed, 0));
				}
			}
		}

		private void SendSpeedDebuffUpdated (){
			MatchAciton updatedMatchAction = new MatchAciton(Keyword.Debuff_LowerSpeed, debuffCount);
			actionHandler.SendMatchActionUpdatedEvent(updatedMatchAction);
		}

		public override void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			if (e.matchAciton.acitonType == Keyword.Debuff_LowerSpeed) {
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
		public Keyword ActionType;
		public int Count;
	}
}
