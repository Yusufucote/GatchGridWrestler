using PlayerWrestler;
using UnityEngine;

namespace WrestlingMatch {
	public class WrestlerFactory : MonoBehaviour {

		[SerializeField]
		MatchWrestler wrestlerPrefab;

		[SerializeField]
		Transform team1Transform;

		[SerializeField]
		Transform team2Transform;

		public MatchWrestler InstantiateWrester(WrestlerData wrestlerData, int teamPosition) {
			MatchWrestler matchWrestler = Instantiate(wrestlerPrefab);
			if (teamPosition == 1) {
				matchWrestler.transform.SetParent(team1Transform);
			}
			else {
				matchWrestler.transform.SetParent(team2Transform);
			}
			matchWrestler.transform.localScale = Vector3.one;
			matchWrestler.transform.localPosition = Vector3.zero;
			return matchWrestler;
		}

	}
}
