using System;
using UnityEngine;

namespace UiKitchen
{
	[Serializable]
	public class UiEventStateAnimatorParameterSetter : AnimatorParameterSetter
	{
		[SerializeField]
		UiElementEventStateEnum _sendOn;
		public UiElementEventStateEnum SendOn
		{
			get
			{
				return _sendOn;
			}

			set
			{
				_sendOn = value;
			}
		}
	}
}
