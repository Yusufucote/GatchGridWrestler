using PlayerWrestler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	public class SystemSpeedDebuffSetter : AbstractSystemMatchActionSetter {

		public void GiveTargetSpeedDebuff() {
			MatchAciton matchAciton = new MatchAciton(MatchActionType.SpeedDebuff, float.Parse(inputFeild.text));
			if (target != null) {
				target.HandleMatchAction(matchAciton);
			}
		}
	}
}
