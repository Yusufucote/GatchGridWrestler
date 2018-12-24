using System;
using System.Collections.Generic;
using UnityEngine;
using WrestlingMatch;

namespace PlayerWrestler {
	public class MatchWrestler : MonoBehaviour {

		private float speedBank;
		private Proto_WrestlerData baseWrestlerData;

		public EventHandler<WrestlerSpeedUpdatedEventArgs> SpeedUpdated;
		public EventHandler<MatchWresterGenericEventArgs> ReadyForMyTurn;
		public EventHandler<MatchWresterGenericEventArgs> TurnStarted;
		public EventHandler<MatchWresterGenericEventArgs> EndTurn;
		public EventHandler<MatchWresterGenericEventArgs> WrestlerInitialized;
		public EventHandler<MatchWresterGenericEventArgs> ImSelected;

		public float CurrentAgility {
			get {
				return baseWrestlerData.Agility;
			}
		}
		public float CurrentStrength {
			get {
				return baseWrestlerData.Strength;
			}
		}  
		public float CurrentDefense {
			get {
				return baseWrestlerData.Defense;
			}
		}
		public string Name {
			get {
				return baseWrestlerData.Name;
			}
		}

		public void InitializeWrestler(Match proto_SpeedTester, Proto_WrestlerData wrestlerData) {
			proto_SpeedTester.UpdateSpeed += HandleSpeedUpdated;
			baseWrestlerData = wrestlerData;
			if (WrestlerInitialized != null) {
				WrestlerInitialized(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void EndTurnMyTurn() {
			if (EndTurn != null) {
				EndTurn(this, new MatchWresterGenericEventArgs() {wrestler = this});
			}
			speedBank = 0;
			SendSpeedUpdatedEvent();
		}

		private void HandleSpeedUpdated(object sender, EventArgs e) {
			speedBank += baseWrestlerData.Agility;
			SendSpeedUpdatedEvent();
			if (speedBank >= 100) {
				SendMyTurnEvent();
			}
		}

		private void SendMyTurnEvent() {
			if (ReadyForMyTurn != null) {
				ReadyForMyTurn(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void StartTurn() {
			if (TurnStarted != null) {
				TurnStarted(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		private void SendSpeedUpdatedEvent() {
			if (SpeedUpdated != null) {
				SpeedUpdated(this, new WrestlerSpeedUpdatedEventArgs() { WrestlerSpeed = speedBank });
			}
		}
	}

	public class WrestlerSpeedUpdatedEventArgs : EventArgs {
		public float WrestlerSpeed;
	}

	public class MatchWresterGenericEventArgs: EventArgs {
		public MatchWrestler wrestler;
	}

	public class MatchWrestler_Compairison_AgiStrDefRandom : IComparer<MatchWrestler> {

		public int Compare(MatchWrestler x, MatchWrestler y) {
			//Check Agi
			if (x.CurrentAgility > y.CurrentAgility) {
				return -1;
			}
			else if (x.CurrentAgility < y.CurrentAgility) {
				return 1;
			}

			//check Str
			if (x.CurrentStrength > y.CurrentStrength) {
				return -1;
			}
			else if (x.CurrentStrength < y.CurrentStrength) {
				return 1;
			}

			//check def
			if (x.CurrentDefense > y.CurrentDefense) {
				return -1;
			}
			else if (x.CurrentDefense < y.CurrentDefense) {
				return 1;
			}

			return UnityEngine.Random.value >= 0.5 ? 1 : -1;
		}
	}
}

