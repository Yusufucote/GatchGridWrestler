using System;
using System.Collections;
using System.Collections.Generic;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;
using WrestlingMatch;

namespace UI {
	public class CurrentMatchTurnDisplayUi : MonoBehaviour {

		[SerializeField]
		Text nameTextFeild;

		[SerializeField]
		MatchTurnOrder matchTurnOrder;

		MatchWrestler currentTurn;

		private void Start() {
			matchTurnOrder.NewWrestlersTurn += HandleNewWrestlersTurn;
		}

		private void HandleNewWrestlersTurn(object sender, MatchWresterGenericEventArgs e) {
			currentTurn = e.wrestler;
			nameTextFeild.text = e.wrestler.Name;
		}

		public void EndCurrentTurn() {
			if (currentTurn != null) {
				nameTextFeild.text = "";
				currentTurn = null;
				matchTurnOrder.EndCurrentTurn();
			}
		}

		private void OnDestroy() {
			if (matchTurnOrder != null) {
				matchTurnOrder.NewWrestlersTurn -= HandleNewWrestlersTurn;
			}
		}
	}
}

