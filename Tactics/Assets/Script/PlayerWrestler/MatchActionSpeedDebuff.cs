using System;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchActionSpeedDebuff : MonoBehaviour {

		[SerializeField]
		MatchWrestlerAgilityHandler agilityHandler;

		int debuffCount;

		public EventHandler<IncermentalMatchActionEventArgs> speedDebuffUpdated;

		public void HandleAction(float value) {
			if (value > 0) {
				if (debuffCount < value) {
					debuffCount = (int)value;
					SendSpeedDebuffUpdated();
				}
				agilityHandler.SetDebuffed(true);
			}
		}

		//TODO: Need to determin when debuff should fade.

		private void SendSpeedDebuffUpdated (){
			if (speedDebuffUpdated != null) {
				speedDebuffUpdated(this, new IncermentalMatchActionEventArgs() {
					ActionType = MatchActionType.SpeedDebuff, 
					Count = debuffCount
				});
			}
		}
	}

	public class IncermentalMatchActionEventArgs: EventArgs {
		public MatchActionType ActionType;
		public int Count;
	}
}
