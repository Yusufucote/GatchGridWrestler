﻿using System;
using PlayerWrestler;
using UnityEngine;
using UnityEngine.UI;

namespace WrestlingMatch {
	public class SpeedTesterDisplay : MonoBehaviour {

		[SerializeField]
		MatchWrestler matchWrestler;

		[SerializeField]
		MatchWrestlerSpeedPoolHandler speedPoolHandler;

		[SerializeField]
		Text nameText;

		[SerializeField]
		Text speedDisplay;

		[SerializeField]
		Image speedFillBar;

		[SerializeField]
		Gradient fillerColor;

		[SerializeField]
		Color wrestlersTurnColor;

		private void Awake() {			
			matchWrestler.WrestlerInitialized += HandleWrestlerInitialized;
			matchWrestler.TurnStarted += HandleTurnStarted;
			matchWrestler.EndTurn += HandleEndTurn;
			speedPoolHandler.SpeedUpdated += HandleWrestlerSpeedUpdated;
		}

		private void HandleEndTurn(object sender, MatchWresterGenericEventArgs e) {
			speedFillBar.color = fillerColor.Evaluate(0);
		}

		private void HandleTurnStarted(object sender, MatchWresterGenericEventArgs e) {
			speedFillBar.color = wrestlersTurnColor;
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

	}
}
