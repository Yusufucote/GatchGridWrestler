using PlayerWrestler;
using System.Collections.Generic;

namespace Abilities {
	public class MatchAbility: Abilitity {

		Abilitity baseAbility;

		public new string Name {
			get { return baseAbility.Name; }
		}

		public new AbilitityType AbilitityType {
			get { return baseAbility.AbilitityType; }
		}

		public new List<AbilitityEffect> AbilitityEffects {
			get { return baseAbility.AbilitityEffects; }
		}

		private int currentCoolDown;
		public int CurrentCoolDown {
			get {
				return currentCoolDown;
			}
		}

		private bool usedLastTurn;

		public MatchAbility(Abilitity abilitity, MatchWrestler matchWrestler) {
			baseAbility = abilitity;
			matchWrestler.TurnStarted += HandleTurnStarted;
		}

		public void AbilityUsed() {
			currentCoolDown = baseAbility.CoolDown;
		}

		private void HandleTurnStarted(object sender, MatchWresterGenericEventArgs e) {
			if (!usedLastTurn) {
				currentCoolDown--;
			}
			else {
				usedLastTurn = false;
			}
		}
		//TODO: Figure out how to unregister from events in just C# 
	}
}
