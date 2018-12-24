using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	public class MatchTurnOrder : MonoBehaviour {

		List<MatchWrestler> turnOrder = new List<MatchWrestler>();

		public EventHandler<MatchWresterGenericEventArgs> NewWrestlersTurn;
		public EventHandler<EventArgs> TurnsDone;

		public void AddWrestlerToTurnQue(MatchWrestler wrestler) {
			turnOrder.Add(wrestler);
		}

		public void StartTurnSequance() {
			SortTurnList();
			NextTurn();
		}

		private void NextTurn() {
			if (NewWrestlersTurn != null) {
				NewWrestlersTurn(this, new MatchWresterGenericEventArgs() { wrestler = turnOrder[0] });
			}
			turnOrder[0].StartTurn();
		}

		public void EndCurrentTurn() {
			turnOrder[0].EndTurnMyTurn();
			turnOrder.RemoveAt(0);
			if (turnOrder.Count > 0) {
				NextTurn();
			}
			else {
				if (TurnsDone != null) {
					TurnsDone(this, EventArgs.Empty);
				}
			}
		}

		private void SortTurnList() {
			MatchWrestler_Compairison_AgiStrDefRandom sorter = new MatchWrestler_Compairison_AgiStrDefRandom();
			turnOrder.Sort(sorter);
		}

	}
}
