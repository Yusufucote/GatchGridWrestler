using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	public class MatchTurnOrder {

		private List<MatchWrestler> _turnOrder = new List<MatchWrestler>();

		public EventHandler<MatchWresterGenericEventArgs> NewWrestlersTurn;
		public EventHandler<EventArgs> CurrentTurnDone;
		public EventHandler<EventArgs> TurnSequenceDone;

		public void AddWrestlerToTurnQue(MatchWrestler wrestler) {
			_turnOrder.Add(wrestler);
			wrestler.LostSpeed += HandleWrestlerLostSpeed;
		}

		private void HandleWrestlerLostSpeed(object sender, MatchWresterGenericEventArgs e) {
			if (_turnOrder.Count > 1) {
				_turnOrder.Remove(e.wrestler);
			}
			else {
				EndCurrentTurn();
			}
		}

		public void StartTurnSequance() {
			SortTurnList();
			NextTurn();
		}

		private void NextTurn() {
			NewWrestlersTurn?.Invoke(this, new MatchWresterGenericEventArgs() { wrestler = _turnOrder[0] });
			_turnOrder[0].StartTurn();
		}

		public void EndCurrentTurn() {
			if (_turnOrder.Count > 0) {
				_turnOrder[0].EndTurnMyTurn();
				_turnOrder.RemoveAt(0);
				if (CurrentTurnDone != null) {
					CurrentTurnDone(this, EventArgs.Empty);
				}
				if (_turnOrder.Count > 0) {
					NextTurn();
				}
				else {
					if (TurnSequenceDone != null) {
						TurnSequenceDone(this, EventArgs.Empty);
					}
				}
			}
		}

		private void SortTurnList() {
			MatchWrestler_Compairison_AgiStrDefRandom sorter = new MatchWrestler_Compairison_AgiStrDefRandom();
			_turnOrder.Sort(sorter);
		}

	}
}
