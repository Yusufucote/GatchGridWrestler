using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	public class Match : MonoBehaviour {

		[SerializeField]
		Team[] _teams;

		[SerializeField]
		WrestlerFactory _wrestlerFactory;


		[SerializeField]
		InMatchWrestlingTargetDeterminator _targetDeterminator;

		[SerializeField]
		Ring _ring;

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

		private MatchTurnOrder _turnOrder;

		private void Awake() {
			_turnOrder = new MatchTurnOrder();
		}

		void Start() {
			_turnOrder.TurnSequenceDone += HandleTurnsDone;

			for (int i = 0; i < _teams.Length; i++) {
				CreateTeam(_teams[i].WrestlingTeam, i);
			}
			SendRosterUpdatedEventArgs();
			_targetDeterminator.Initialize(matchMrestlers, _turnOrder);
		}

		private List<MatchWrestler> CreateTeam(List<WrestlerDataScriptableObject> wrestlers, int teamPosition) {
			List<MatchWrestler> teamList = new List<MatchWrestler>();

			for (int i = 0; i < wrestlers.Count; i++) {
				MatchWrestler matchWrestler = _wrestlerFactory.InstantiateWrester(wrestlers[i].wrestlerData, teamPosition, i, this, _targetDeterminator, _ring);
				matchMrestlers.Add(matchWrestler.GetHashCode().ToString(), matchWrestler);
				RegisterForWrestlerEvents(matchWrestler);
				teamList.Add(matchWrestler);
			}
			return teamList;
		}

		private void SendRosterUpdatedEventArgs() {
			WrestlerRosterUpated?.Invoke(this, new MatchWrestlerRosterEventArgs() { Wrestlers = matchMrestlers });
		}

		//make an ienumerator;
		private void Update() {
			if (!someonesTurn) {
				UpdateSpeed?.Invoke(this, EventArgs.Empty);
			}
		}

		private void RegisterForWrestlerEvents(MatchWrestler matchWrestler) {
			matchWrestler.ReadyForMyTurn += HandleReadyFoMyTurn;
			matchWrestler.EndTurn += HandleWrestlerEndTurn;
		}

		private void HandleWrestlerEndTurn(object sender, MatchWresterGenericEventArgs e) {
			
		}

		private void HandleReadyFoMyTurn(object sender, MatchWresterGenericEventArgs e) {
			_turnOrder.AddWrestlerToTurnQue(e.wrestler);
			if (turnStarter == null) {
				turnStarter = StartCoroutine(StartTurnSequanceOnNextFrame());
			}
			someonesTurn = true;
		}

		private IEnumerator StartTurnSequanceOnNextFrame() {
			yield return new WaitForEndOfFrame();
			_turnOrder.StartTurnSequance();
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
