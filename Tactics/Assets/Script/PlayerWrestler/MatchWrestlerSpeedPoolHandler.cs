using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public class MatchWrestlerSpeedPoolHandler {

		private MatchWrestlerAgilityHandler _agilityHandler;
		private MatchWrestler _wrestler;

		private float speedPool = 0f;
		public float CurrentSpeedPool {
			get {
				return speedPool;
			}
		}

		public EventHandler<WrestlerSpeedUpdatedEventArgs> SpeedUpdated;

		public MatchWrestlerSpeedPoolHandler(MatchWrestlerAgilityHandler agilityHandler, MatchWrestler wrestler) {
			_agilityHandler = agilityHandler;
			_wrestler = wrestler;
			_wrestler.UpdateSpeed += HandleUpdateSpeed;
			_wrestler.EndTurn += HandleEndTurn;
		}

		public void SetSpeedPoolValue(float value) {
			if (value < 0) {
				value = 0;
			}
			else if (value > 100) {
				value = 100;
			}
			if (value < speedPool) {
				_wrestler.SendLostSpeedEvent();
			}
			
			speedPool = value;
			SendSpeedUpdatedEvent();
		}

		private void HandleUpdateSpeed(object sender, EventArgs e) {
			speedPool += _agilityHandler.CurrentAgility;
			SendSpeedUpdatedEvent();
			if (speedPool >= 100) {
				_wrestler.SendReadyForMyTurnEvent();
			}
		}

		private void HandleEndTurn(object sender, MatchWresterGenericEventArgs e) {
			speedPool = 0;
			SendSpeedUpdatedEvent();
		}

		private void SendSpeedUpdatedEvent() {
			SpeedUpdated?.Invoke(this, new WrestlerSpeedUpdatedEventArgs() { WrestlerSpeed = speedPool });
		}

		~MatchWrestlerSpeedPoolHandler() {
			if (_wrestler != null) {
				_wrestler.UpdateSpeed -= HandleUpdateSpeed;
				_wrestler.EndTurn -= HandleEndTurn;
			}
		}
	}
}
