using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UiKitchen
{
	public class AndroidBackButton : MonoBehaviour
	{
		[SerializeField]
		private Button _button;

		[SerializeField]
		private AnimatorStateBroadcasterBehaviour _logicAnimator;

		[SerializeField]
		private bool _exitAppInstead = false;

		[SerializeField]
		private List<string> _requiredStates = new List<string>();

#if UNITY_ANDROID && !UNITY_EDITOR
		public void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				bool inState = false;
				var currentStateName = _logicAnimator.LastStateEntered.Name;
				for (int i = 0; i < _requiredStates.Count; i++)
				{
					if (_requiredStates[i] == currentStateName)
					{
						inState = true;
						break;
					}
				}
				if (inState)
				{
					if (_exitAppInstead)
					{
						Application.Quit();
					}
					else if (_button.isActiveAndEnabled && _button.IsInteractable())
					{
						_button.onClick.Invoke();
					}
				}
			}
		}
#endif
	}
}
