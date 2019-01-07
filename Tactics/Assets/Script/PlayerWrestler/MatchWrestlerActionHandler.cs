using System;
using UnityEngine;
namespace PlayerWrestler {
	public class MatchWrestlerActionHandler : MonoBehaviour {

		[SerializeField]
		MatchWrestler wrestler;

		public EventHandler<MatchWresterGenericEventArgs> TurnStarted;
		public EventHandler<MatchWresterGenericEventArgs> EndTurn;

		public EventHandler<MatchWrestlerActionRecievedEventArgs> ActionRecieved;
		public EventHandler<MatchWrestlerActionRecievedEventArgs> ActionUpdated;
		public EventHandler<MatchActionCompleteEventArgs> MatchActionComplete;

		private void Start() {
			wrestler.ActionRecieved += HandleActionRecieved;
			wrestler.TurnStarted += HandleTurnStarted;
			wrestler.EndTurn += HandleEndTurn;
		}

		private void HandleTurnStarted(object sender, MatchWresterGenericEventArgs e) {
			TurnStarted(this, e);
		}

		private void HandleEndTurn(object sender, MatchWresterGenericEventArgs e) {
			if (EndTurn != null) {
				EndTurn(this, e);
			}
		}

		private void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			if (ActionRecieved != null) {
				ActionRecieved(this, e);
			}
		}

		public void SendMatchActionUpdatedEvent(MatchAciton updatedMatchAciton) {
			if (ActionUpdated != null) {
				ActionUpdated(this, new MatchWrestlerActionRecievedEventArgs() { matchAciton = updatedMatchAciton });
			}
		}

		public void SendMatchActionComplete(MatchAciton completeAction) {
			if (MatchActionComplete != null) {
				MatchActionComplete(this, new MatchActionCompleteEventArgs() { matchAction = completeAction });
			}
		}

	}

	public enum IncermentalMatchActionIncermentalState {
		StartOfTurn, EndOfTurn
	}

	public class MatchActionCompleteEventArgs: EventArgs {
		public MatchAciton matchAction;
	}
}
