using System;
using System.Collections;
using System.Collections.Generic;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;

namespace WrestlingMatch {
	public class InMatchWrestlingTargetDeterminator : MonoBehaviour {

		[SerializeField]
		MatchTurnOrder turnOrder;

		private Dictionary<string, MatchWrestler> matchRoster;
		private bool isSomeOnesTurn;
		private MatchWrestler whosTurn;
		private MatchWrestler isTargeted;

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
			if (whosTurn != null) {
				if (isTargeted != null) {
					isTargeted.BeUnTargeted();
				}
				isTargeted = e.wrestler;
				isTargeted.BeTargeted();
				if (NewTarget != null) {
					NewTarget(this, e);
				}
			}
		}

		private void HandleNewWrestlersTurn(object sender, MatchWresterGenericEventArgs e) {
			whosTurn = e.wrestler;
		}

		private void HandleCurrentTurnDone(object sender, EventArgs e) {
			if (RemovedTarget != null) {
				RemovedTarget(this, EventArgs.Empty);
			}
			whosTurn = null;
			if (isTargeted != null) {
				isTargeted.BeUnTargeted();
				isTargeted = null;
			}
		}

		private void HandleTurnsDone(object sender, EventArgs e) {
			if (RemovedTarget != null) {
				RemovedTarget(this, EventArgs.Empty);
			}
			whosTurn = null;
			if (isTargeted != null) {
				isTargeted.BeUnTargeted();
				isTargeted = null;
			}

		}
	}
}
