using System;
using System.Collections;
using System.Collections.Generic;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;

namespace WrestlingMatch {
	public class SystemSpeedPoolSetter : MonoBehaviour {

		[SerializeField]
		InMatchWrestlingTargetDeterminator targetDeterminator;

		[SerializeField]
		InputField inputFeild;

		[SerializeField]
		CanvasGroup canvasGroup;

		private MatchWrestler target;

		private void Awake() {
			targetDeterminator.NewTarget += HandleNewTarget;
			targetDeterminator.RemovedTarget += HandleRemovedTarget;
			canvasGroup.alpha = 0.3f;
			canvasGroup.interactable = false;
		}

		private void HandleRemovedTarget(object sender, EventArgs e) {
			target = null;
			canvasGroup.alpha = 0.3f;
			canvasGroup.interactable = false;
		}

		private void HandleNewTarget(object sender, MatchWresterGenericEventArgs e) {
			target = e.wrestler;
			canvasGroup.alpha = 1f;
			canvasGroup.interactable = true;
		}

		public void ChangeSpeedPoolByPrecent() {
			MatchAciton matchAciton = new MatchAciton(MatchActionType.ChangeSpeedPoolByPercentOfTotal, float.Parse(inputFeild.text));
			if (target != null) {
				target.HandleMatchAction(matchAciton);
			}
		}


	}
}
