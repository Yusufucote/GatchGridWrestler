using System;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;

namespace WrestlingMatch {
	public class SpeedTesterDisplay : MonoBehaviour {

		[SerializeField]
		MatchWrestler matchWrestler;

		[SerializeField]
		Text nameText;

		[SerializeField]
		Text speedDisplay;

		[SerializeField]
		Image speedFillBar;

		[SerializeField]
		Gradient fillerColor;

		private void Awake() {			
			matchWrestler.SpeedUpdated += HandleWrestlerSpeedUpdated;
			matchWrestler.WrestlerInitialized += HandleWrestlerInitialized;
		}

		private void HandleWrestlerInitialized(object sender, MatchWresterGenericEventArgs e) {
			nameText.text = e.wrestler.Name;
		}

		private void HandleWrestlerSpeedUpdated(object sender, WrestlerSpeedUpdatedEventArgs e) {
			SetSpeed(e.WrestlerSpeed);
		}

		public void SetName(string name) {
			nameText.text = name;
		}

		public void SetSpeed(float speed) {
			speedDisplay.text = speed.ToString();
			speedFillBar.fillAmount = speed / 100;
			speedFillBar.color = fillerColor.Evaluate(speed / 100);
		}

		private void OnDestroy() {
			if (matchWrestler != null) {
				matchWrestler.SpeedUpdated -= HandleWrestlerSpeedUpdated;
			}
		}
	}
}
