using System;

namespace PlayerWrestler{
	[Serializable]
	public class MatchAciton {
		public MatchActionType acitonType;
		public float value;

		public MatchAciton(MatchActionType acitonType, float value) {
			this.acitonType = acitonType;
			this.value = value;
		}
	}

	public enum MatchActionType  {
		ChangeSpeedPoolByPercentOfTotal 
	}
}

