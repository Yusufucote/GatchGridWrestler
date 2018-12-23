using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	public class Match : MonoBehaviour {

		[SerializeField]
		List<Proto_WrestlerData> wrestlers;

		[SerializeField]
		WrestlerFactory wrestlerFactory;

		[SerializeField]
		MatchTurnOrder turnOrder;

		private Dictionary<string, MatchWrestler> matchMrestlers = new Dictionary<string, MatchWrestler>();
		private bool someonesTurn;
		
		Coroutine turnStarter;

		public EventHandler<EventArgs> UpdateSpeed;

		void Start() {
			turnOrder.TurnsDone += HandleTurnsDone;
			foreach (var wrestler in wrestlers) {
				MatchWrestler matchWrestler = wrestlerFactory.InstantiateWrester(wrestler);
				matchMrestlers.Add(matchWrestler.GetHashCode().ToString(), matchWrestler);
				matchWrestler.InitializeWrestler(this, wrestler);
				RegisterForWrestlerEvents(matchWrestler);
			}
		}

		private void Update() {
			if (!someonesTurn) {
				if (UpdateSpeed != null) {
					UpdateSpeed(this, EventArgs.Empty);
				}
			}
		}

		private void RegisterForWrestlerEvents(MatchWrestler matchWrestler) {
			matchWrestler.MyTurn += HandleWrestlerTurn;
			matchWrestler.EndTurn += HandleWrestlerEndTurn;
		}

		private void HandleWrestlerEndTurn(object sender, MatchWresterGenericEventArgs e) {
			
		}

		private void HandleWrestlerTurn(object sender, MatchWresterGenericEventArgs e) {
			turnOrder.AddWrestlerToTurnQue(e.wrestler);
			if (turnStarter == null) {
				turnStarter = StartCoroutine(StartTurnSequanceOnNextFrame());
			}
			someonesTurn = true;
		}

		private IEnumerator StartTurnSequanceOnNextFrame() {
			yield return new WaitForEndOfFrame();
			turnOrder.StartTurnSequance();
			turnStarter = null;
		}


		private void HandleTurnsDone(object sender, EventArgs e) {
			someonesTurn = false;
		}
	}
}
