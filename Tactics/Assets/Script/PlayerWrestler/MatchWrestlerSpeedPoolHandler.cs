using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchWrestlerSpeedPoolHandler : MonoBehaviour {

		[SerializeField]
		MatchWrestlerAgilityHandler agilityHandler;

		[SerializeField]
		MatchWrestler wrestler;

		private float speedPool = 0f;
		public float CurrentSpeedPool {
			get {
				return speedPool;
			}
		}

		public EventHandler<WrestlerSpeedUpdatedEventArgs> SpeedUpdated;

		private void Awake() {
			wrestler.UpdateSpeed += HandleUpdateSpeed;
			wrestler.EndTurn += HandleEndTurn;
		}

		public void SetSpeedPoolValue(float value) {
			if (value < 0) {
				value = 0;
			}
			else if (value > 100) {
				value = 100;
			}
			if (value < speedPool) {
				wrestler.SendLostSpeedEvent();
			}
			
			speedPool = value;
			SendSpeedUpdatedEvent();
		}

		private void HandleUpdateSpeed(object sender, EventArgs e) {
			speedPool += agilityHandler.CurrentAgility;
			SendSpeedUpdatedEvent();
			if (speedPool >= 100) {
				wrestler.SendReadyForMyTurnEvent();
			}
		}

		private void HandleEndTurn(object sender, MatchWresterGenericEventArgs e) {
			speedPool = 0;
			SendSpeedUpdatedEvent();
		}

		private void SendSpeedUpdatedEvent() {
			if (SpeedUpdated != null) {
				SpeedUpdated(this, new WrestlerSpeedUpdatedEventArgs() { WrestlerSpeed = speedPool });
			}
		}
	}
}
