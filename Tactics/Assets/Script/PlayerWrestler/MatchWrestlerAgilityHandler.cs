using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerWrestler {
	public class MatchWrestlerAgilityHandler : AbstractMatchWrestlerStatHandler<float> {

		private float baseAgility;
		public float CurrentAgility {
			get {
				return baseAgility;
			}
		}

		public override void Initialize(float baseStatValue) {
			baseAgility = baseStatValue;
		}
	}
}
