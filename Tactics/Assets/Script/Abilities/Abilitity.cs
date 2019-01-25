using System;
using System.Collections.Generic;

namespace Abilities {
	[Serializable]
	public class Abilitity {
		public string Name;
		public AbilitityType AbilitityType;
		public int CoolDown;
		public List<AbilitityEffect> AbilitityEffects;
	}

	//Todo: implment Passive
	public enum AbilitityType {
		Active_Base, Active_Special
	}
}
