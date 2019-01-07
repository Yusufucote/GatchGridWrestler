using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	public class Match : MonoBehaviour {

		[SerializeField]
		List<WrestlerDataScriptableObject> wrestlers;

		[SerializeField]
		WrestlerFactory wrestlerFactory;

		[SerializeField]
		MatchTurnOrder turnOrder;

		[SerializeField]
		InMatchWrestlingTargetDeterminator targetDeterminator;

		private Dictionary<string, MatchWrestler> matchMrestlers = new Dictionary<string, MatchWrestler>();
		public Dictionary<string, MatchWrestler> MatchMrestlers {
			get {
				return matchMrestlers;
			}
		}

		private bool someonesTurn;
		
		Coroutine turnStarter;

		public EventHandler<EventArgs> UpdateSpeed;
		public EventHandler<MatchWrestlerRosterEventArgs> WrestlerRosterUpated;

		void Start() {
			turnOrder.TurnSequenceDone += HandleTurnsDone;
			foreach (var wrestlerSO in wrestlers) {
				MatchWrestler matchWrestler = wrestlerFactory.InstantiateWrester(wrestlerSO.wrestlerData);
				matchMrestlers.Add(matchWrestler.GetHashCode().ToString(), matchWrestler);
				matchWrestler.InitializeWrestler(this, wrestlerSO.wrestlerData);
				RegisterForWrestlerEvents(matchWrestler);
			}
			SendRosterUpdatedEventArgs();
			targetDeterminator.Initialize(matchMrestlers);
		}

		private void SendRosterUpdatedEventArgs() {
			if (WrestlerRosterUpated != null) {
				WrestlerRosterUpated(this, new MatchWrestlerRosterEventArgs() { Wrestlers = matchMrestlers });
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
			matchWrestler.ReadyForMyTurn += HandleWrestlerTurn;
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

	public class MatchWrestlerRosterEventArgs: EventArgs {
		public Dictionary<string, MatchWrestler> Wrestlers;
	}
}
