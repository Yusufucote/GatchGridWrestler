using System;
using System.Collections;
using System.Collections.Generic;
using Abilities;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;

namespace WrestlingMatch {
	public class SystemSpeedPoolSetter : AbstractSystemMatchActionSetter {

		public void ChangeSpeedPoolByPrecent() {
			MatchAciton matchAciton = new MatchAciton(Keyword.TurnMeter_ChangeByPercentOfMax, float.Parse(inputFeild.text));
			if (target != null) {
				target.HandleMatchAction(matchAciton);
			}
		}

	}
}
