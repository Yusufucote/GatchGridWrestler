using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerWrestler {
	public class MatchWrestlerAgilityHandler  {

		private float speedDebuffPercent  = 0.25f;

		private float _baseAgility;
		public float CurrentAgility {
			get {
				if (statIsDebuffed) {
					float debuffAmount = (_baseAgility * speedDebuffPercent);
					return _baseAgility - debuffAmount;
				}
				else {
					return _baseAgility;
				}
			}
		}

		bool statIsDebuffed;

		public MatchWrestlerAgilityHandler(float baseAgility) {
			_baseAgility = baseAgility;
		}


		public void SetDebuffed(bool value) {
			statIsDebuffed = value;
		}
	}
}
