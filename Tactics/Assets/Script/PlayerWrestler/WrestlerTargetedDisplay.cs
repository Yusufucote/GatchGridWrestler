using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	public class WrestlerTargetedDisplay : MonoBehaviour {

		[SerializeField]
		MatchWrestler wrestler;

		[SerializeField]
		GameObject isSelectedGameObject;

		private void Awake() {
			wrestler.IsTargeted += HandleIsTargeted;
			wrestler.UnTargeted += HandleUnTargeted;
			isSelectedGameObject.SetActive(false);
		}

		private void HandleIsTargeted(object sender, MatchWresterGenericEventArgs e) {
			isSelectedGameObject.SetActive(true);
		}
		private void HandleUnTargeted(object sender, MatchWresterGenericEventArgs e) {
			isSelectedGameObject.SetActive(false);
		}

	}
}
