using System;
using System.Collections.Generic;
using UnityEngine;
using WrestlingMatch;

namespace PlayerWrestler {
	public class MatchWrestler : MonoBehaviour {


		private MatchWrestlerAgilityHandler _agilityHandler;
		private MatchWrestlerSpeedPoolHandler _speedPoolHandler;
		private InMatchWrestlingTargetDeterminator _targetDeterminator;
		private PlayerMovementAction _playerMovementAction;
		private WrestlerData _baseWrestlerData;

		private float _speedBank;

		public EventHandler<EventArgs> UpdateSpeed;
		public EventHandler<MatchWresterGenericEventArgs> ReadyForMyTurn;
		public EventHandler<MatchWresterGenericEventArgs> LostSpeed;
		public EventHandler<MatchWresterGenericEventArgs> TurnStarted;
		public EventHandler<MatchWresterGenericEventArgs> EndTurn;
		public EventHandler<MatchWresterGenericEventArgs> WrestlerInitialized;
		public EventHandler<MatchWresterGenericEventArgs> IsSelected;
		public EventHandler<MatchWresterGenericEventArgs> IsTargeted;
		public EventHandler<MatchWresterGenericEventArgs> UnTargeted;

		public float CurrentAgility => _agilityHandler.CurrentAgility;
		public float CurrentStrength => _baseWrestlerData.Strength;
		public float CurrentDefense => _baseWrestlerData.Defense;
		public string Name => _baseWrestlerData.Name;
		public float CurrentSpeedPool => _speedPoolHandler.CurrentSpeedPool;


		public List<MatchWrestler> Team { get; internal set; }

		public void InitializeWrestler(Match matchManager, WrestlerData wrestlerData, InMatchWrestlingTargetDeterminator targetDeterminator, Ring ring, RingPosition startingPosition) {
			matchManager.UpdateSpeed += HandleSpeedUpdated;

			_baseWrestlerData = wrestlerData;
			_targetDeterminator = targetDeterminator;
			//actions need to be handled by an action handler that gets init here.
			_playerMovementAction = new PlayerMovementAction(transform, ring);
			_agilityHandler = new MatchWrestlerAgilityHandler(wrestlerData.Agility);
			_speedPoolHandler = new MatchWrestlerSpeedPoolHandler(_agilityHandler, this);


			WrestlerInitialized?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}


		public void EndTurnMyTurn() {
			EndTurn?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}

		public void BeTargeted() {
			IsTargeted?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}

		public void BeUnTargeted() {
			UnTargeted?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}

		public void SendIsSelectedEventArgs() {
			IsSelected?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}


		private void HandleNewTarget(object sender, MatchWresterGenericEventArgs e) {
			if (_targetDeterminator != null) {
				_targetDeterminator.NewTarget -= HandleNewTarget;
			}

			//annouce Using Ability
			//TryingToUseAbility?.Invoke(this, new MatchWrestlerMatchAbilityEventArgs() { matchAbility = currentMatchAbility });
		}

		private void HandleSpeedUpdated(object sender, EventArgs e) {
			UpdateSpeed?.Invoke(this, EventArgs.Empty);
		}

		public void SendReadyForMyTurnEvent() {
			ReadyForMyTurn?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}

		public void SendLostSpeedEvent() {
			LostSpeed?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
		}

		public void StartTurn() {
			TurnStarted?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = this });
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

