using System;


namespace Abilities {
	[Serializable]
	public class AbilitityEffect {

		public UseOn useOn;
		//Todo: Implment AbilitityEffect conditions
		public Keyword keyword;
		public float effectValue;

	}

	//Todo: Team, OposingTeam
	public enum UseOn { Self, Target};
	//ToDo:TurnMeter_ChangeByPercentOfMax should be LowerTurnMeter and RaiseTurnMeter
	//needs new handlers OR one to handle both.
	public enum Keyword {
		Damage_Phyical, Damnage_Technical, TurnMeter_ChangeByPercentOfMax, Debuff_LowerSpeed
	}
}
