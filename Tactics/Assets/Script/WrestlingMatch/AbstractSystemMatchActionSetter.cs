using PlayerWrestler;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace WrestlingMatch {
	public abstract class AbstractSystemMatchActionSetter : MonoBehaviour {

		[SerializeField]
		InMatchWrestlingTargetDeterminator targetDeterminator;

		[SerializeField]
		internal InputField inputFeild;

		[SerializeField]
		CanvasGroup canvasGroup;

		internal MatchWrestler target;

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

	}
}
