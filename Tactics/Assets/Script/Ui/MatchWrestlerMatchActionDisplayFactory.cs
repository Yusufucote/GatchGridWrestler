using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
	public class MatchWrestlerMatchActionDisplayFactory : AbstractMatchActionHandler {

		[SerializeField]
		Transform iconParent;

		[SerializeField]
		Color buffColor = Color.green;

		[SerializeField]
		Color debuffColor = Color.magenta;

		[SerializeField]
		MatchActionIconDisplay iconDisplay;

		[SerializeField]
		Sprite speedStatIcon;

		Dictionary<MatchActionType, MatchActionIconDisplay> displays = new Dictionary<MatchActionType, MatchActionIconDisplay>();

		private void Start() {
			actionHandler.ActionUpdated += HandleActionUpdated;
			actionHandler.MatchActionComplete += HandleActionComplete;
		}



		public override void HandleActionRecieved(object sender, MatchWrestlerActionRecievedEventArgs e) {
			switch (e.matchAciton.acitonType) {
				case MatchActionType.ChangeSpeedPoolByPercentOfTotal:
					//TODO: somethign to handle text woudl be ideal
					break;
				case MatchActionType.SpeedDebuff:
					HandleDebuffIcon(e.matchAciton);
					break;
				default:
					break;
			}
		}

		private void HandleDebuffIcon(MatchAciton action) {
			if (!displays.ContainsKey(action.acitonType)) {
				MatchActionIconDisplay icon = Instantiate(iconDisplay);
				icon.transform.SetParent(iconParent);
				icon.transform.localPosition = Vector3.zero;
				icon.transform.localScale = Vector3.one;

				//TODO: have a better way to manage debuff sprites
				icon.InitilizeIcon(speedStatIcon, (int)action.value, debuffColor);
				displays.Add(action.acitonType, icon);
			}
		}

		private void HandleActionComplete(object sender, MatchActionCompleteEventArgs e) {
			if (displays.ContainsKey(e.matchAction.acitonType)) {
				MatchActionIconDisplay iconDisplay = displays[e.matchAction.acitonType];
				displays.Remove(e.matchAction.acitonType);
				Destroy(iconDisplay.gameObject);
			}
		}

		private void HandleActionUpdated(object sender, MatchWrestlerActionRecievedEventArgs e) {
			if (displays.ContainsKey(e.matchAciton.acitonType)) {
				MatchActionIconDisplay iconDisplay = displays[e.matchAciton.acitonType];
				iconDisplay.UpdateIconText((int)e.matchAciton.value);
			}
		}

	}
}
