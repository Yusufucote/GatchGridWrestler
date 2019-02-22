using System;
using System.Collections;
using System.Collections.Generic;
using PlayerWrestler;
using UnityEngine;
using WrestlingMatch;

namespace UI {
	public class CurrentWrestlersTurnAbilityDisplayer : MonoBehaviour {

		[SerializeField]
		MatchTurnOrder matchTurnOrder;


		private void Start() {
			matchTurnOrder.NewWrestlersTurn += HandleNewWrestlersTurn;
		}

		private void HandleNewWrestlersTurn(object sender, MatchWresterGenericEventArgs e) {
			
			
		}

		private void OnDestroy() {
			if (matchTurnOrder != null) {
				matchTurnOrder.NewWrestlersTurn -= HandleNewWrestlersTurn;
			}
		}
	}
}
