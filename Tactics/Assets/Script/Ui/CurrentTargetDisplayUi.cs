using System;
using System.Collections;
using System.Collections.Generic;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;
using WrestlingMatch;

namespace UI {
	public class CurrentTargetDisplayUi : MonoBehaviour {

		[SerializeField]
		InMatchWrestlingTargetDeterminator targetDeterminator;

		[SerializeField]
		Text nameDisplayText;


		private void Awake() {
			nameDisplayText.text = "";
			targetDeterminator.NewTarget += HandleNewTarget;
			targetDeterminator.RemovedTarget += HandleRemovedTarget;
		}

		private void HandleRemovedTarget(object sender, EventArgs e) {
			nameDisplayText.text = "";
		}

		private void HandleNewTarget(object sender, MatchWresterGenericEventArgs e) {
			nameDisplayText.text = e.wrestler.Name;
		}
	}
}
