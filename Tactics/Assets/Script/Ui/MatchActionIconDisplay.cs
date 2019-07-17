using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
	public class MatchActionIconDisplay : MonoBehaviour {

		[SerializeField]
		Image image;

		[SerializeField]
		Text text;

		public void InitilizeIcon(Sprite sprite, int textValue, Color color) {
			image.sprite = sprite;
			if (textValue > 1) {
				text.text = textValue.ToString();
			}
			else {
				text.enabled = false;
			}
			image.color = color;
		}

		public void UpdateIconText(int textValue) {
			if (textValue > 1) {
				text.text = textValue.ToString();
			}
			else {
				text.enabled = false;
			}
		}

	}
}
