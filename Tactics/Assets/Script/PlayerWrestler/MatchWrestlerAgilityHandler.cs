using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerWrestler {
	public class MatchWrestlerAgilityHandler : AbstractMatchWrestlerStatHandler<float> {

		[Range(0,1)]
		[SerializeField]
		float speedDebuffPercent;

		private float baseAgility;
		public float CurrentAgility {
			get {
				if (statIsDebuffed) {
					float debuffAmount = (baseAgility * speedDebuffPercent);
					return baseAgility - debuffAmount;
				}
				else {
					return baseAgility;
				}
			}
		}

		bool statIsDebuffed;

		public override void Initialize(float baseStatValue) {
			baseAgility = baseStatValue;
		}

		public void SetDebuffed(bool value) {
			statIsDebuffed = value;
		}
	}
}
