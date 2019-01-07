using PlayerWrestler;
using UnityEngine;

namespace WrestlingMatch {
	public class WrestlerFactory : MonoBehaviour {

		[SerializeField]
		MatchWrestler wrestlerPrefab;

		[SerializeField]
		Transform wrestlerParent;

		public MatchWrestler InstantiateWrester(WrestlerData wrestlerData) {
			MatchWrestler matchWrestler = Instantiate(wrestlerPrefab);
			matchWrestler.transform.SetParent(wrestlerParent);
			matchWrestler.transform.localScale = Vector3.one;
			matchWrestler.transform.localPosition = Vector3.zero;
			return matchWrestler;
		}



	}
}
