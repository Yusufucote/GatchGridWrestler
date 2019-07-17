using System;
using System.Collections.Generic;
using PlayerWrestler;
using UnityEngine;

namespace WrestlingMatch {
	public class InMatchWrestlingTargetDeterminator : MonoBehaviour {

		[SerializeField]
		MatchTurnOrder turnOrder;

		private Dictionary<string, MatchWrestler> _matchRoster;
		private bool _isSomeOnesTurn;
		private MatchWrestler _whosTurn;
		private MatchWrestler _isTargeted;
		private Team _team1;
		private Team _team2;

		public EventHandler<EventArgs> RemovedTarget;
		public EventHandler<MatchWresterGenericEventArgs> NewTarget;

		private void Awake() {
			turnOrder.NewWrestlersTurn += HandleNewWrestlersTurn;
			turnOrder.TurnSequenceDone += HandleTurnsDone;
			turnOrder.CurrentTurnDone += HandleCurrentTurnDone;
		}

		public void Initialize(Dictionary<string, MatchWrestler> matchRoster) {

			foreach (var wrestler in matchRoster.Values) {
				wrestler.IsSelected += HandleWrestlerIsSelected;
			}
		}



		private void HandleWrestlerIsSelected(object sender, MatchWresterGenericEventArgs e) {
			if (_whosTurn != null) {
				UnTarget();

				SetNewTarget(e);
			}
		}

		private void SetNewTarget(MatchWresterGenericEventArgs e) {
			_isTargeted = e.wrestler;
			_isTargeted.BeTargeted();
			NewTarget?.Invoke(this, e);
		}
		private void SetNewTarget(MatchWrestler wrestler) {
			_isTargeted = wrestler;
			_isTargeted.BeTargeted();
			NewTarget?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = wrestler });
		}

		private void HandleNewWrestlersTurn(object sender, MatchWresterGenericEventArgs e) {
			_whosTurn = e.wrestler;
		}

		private void HandleCurrentTurnDone(object sender, EventArgs e) {
			RemovedTarget?.Invoke(this, EventArgs.Empty);

			_whosTurn = null;
			UnTarget();
		}

		private void HandleTurnsDone(object sender, EventArgs e) {
			RemovedTarget?.Invoke(this, EventArgs.Empty);

			_whosTurn = null;
			UnTarget();

		}

		private void UnTarget() {
			if (_isTargeted != null) {
				_isTargeted.BeUnTargeted();
				_isTargeted = null;
			}
		}

	}
}
