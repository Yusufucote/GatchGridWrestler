using System;
using System.Collections.Generic;
using UnityEngine;
using WrestlingMatch;

namespace PlayerWrestler {
	public class MatchWrestler : MonoBehaviour {

		[SerializeField]
		MatchWrestlerAgilityHandler agilityHandler;

		[SerializeField]
		MatchWrestlerSpeedPoolHandler speedPoolHandler;

		private float speedBank;
		private WrestlerData baseWrestlerData;

		public EventHandler<EventArgs> UpdateSpeed;
		public EventHandler<MatchWresterGenericEventArgs> ReadyForMyTurn;
		public EventHandler<MatchWresterGenericEventArgs> LostSpeed;
		public EventHandler<MatchWresterGenericEventArgs> TurnStarted;
		public EventHandler<MatchWresterGenericEventArgs> EndTurn;

		public EventHandler<MatchWresterGenericEventArgs> WrestlerInitialized;

		public EventHandler<MatchWresterGenericEventArgs> IsSelected;
		public EventHandler<MatchWresterGenericEventArgs> IsTargeted;
		public EventHandler<MatchWresterGenericEventArgs> UnTargeted;

		public EventHandler<MatchWrestlerActionRecievedEventArgs> ActionRecieved;

		public float CurrentAgility {
			get {
				return agilityHandler.CurrentAgility;
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
		public float CurrentSpeedPool {
			get {
				return speedPoolHandler.CurrentSpeedPool;
			}
		}

		public void InitializeWrestler(Match proto_SpeedTester, WrestlerData wrestlerData) {
			proto_SpeedTester.UpdateSpeed += HandleSpeedUpdated;
			baseWrestlerData = wrestlerData;
			agilityHandler.Initialize(wrestlerData.Agility);
			if (WrestlerInitialized != null) {
				WrestlerInitialized(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void EndTurnMyTurn() {
			if (EndTurn != null) {
				EndTurn(this, new MatchWresterGenericEventArgs() {wrestler = this});
			}
		}

		public void BeTargeted() {
			if (IsTargeted != null) {
				IsTargeted(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void BeUnTargeted() {
			if (UnTargeted != null) {
				UnTargeted(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void SendIsSelectedEventArgs() {
			if (IsSelected != null) {
				IsSelected(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void HandleMatchAction(MatchAciton newMatchAction) {
			if (ActionRecieved != null) {
				ActionRecieved(this, new MatchWrestlerActionRecievedEventArgs() { matchAciton = newMatchAction });
			}
		}

		private void HandleSpeedUpdated(object sender, EventArgs e) {
			if (UpdateSpeed != null) {
				UpdateSpeed(this, EventArgs.Empty);
			}
		}

		public void SendReadyForMyTurnEvent() {
			if (ReadyForMyTurn != null) {
				ReadyForMyTurn(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void SendLostSpeedEvent() {
			if (LostSpeed != null) {
				LostSpeed(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}

		public void StartTurn() {
			if (TurnStarted != null) {
				TurnStarted(this, new MatchWresterGenericEventArgs() { wrestler = this });
			}
		}
	}

	public class WrestlerSpeedUpdatedEventArgs : EventArgs {
		public float WrestlerSpeed;
	}

	public class MatchWresterGenericEventArgs: EventArgs {
		public MatchWrestler wrestler;
	}

	public class MatchWrestlerActionRecievedEventArgs : EventArgs {
		public MatchAciton matchAciton;
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

