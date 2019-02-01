using Abilities;
using PlayerWrestler;

namespace WrestlingMatch {
	public class SystemSpeedDebuffSetter : AbstractSystemMatchActionSetter {

		public void GiveTargetSpeedDebuff() {
			MatchAciton matchAciton = new MatchAciton(Keyword.Debuff_LowerSpeed, float.Parse(inputFeild.text));
			if (target != null) {
				target.HandleMatchAction(matchAciton);
			}
		}
	}
}
