using Abilities;
using System;

namespace PlayerWrestler{
	[Serializable]
	public class MatchAciton {
		public Keyword acitonType;
		public float value;

		public MatchAciton(Keyword acitonType, float value) {
			this.acitonType = acitonType;
			this.value = value;
		}
	}
}

