using System;
using UnityEngine;

namespace UiKitchen
{
	[Serializable]
	public class SliderUiEventStateAnimatorParameterSetterData : AnimatorParameterSetter
	{
		[SerializeField]
		ValuedUiElementEventStateEnum _sendOn;
		public ValuedUiElementEventStateEnum SendOn
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


		[SerializeField]
		float _compairsonValue;
		public float CompairsonValue
		{
			get
			{
				return _compairsonValue;
			}

			set
			{
				_compairsonValue = value;
			}
		}

		[SerializeField]
		RelationalOperatorsEnum _relationalOperator;
		public RelationalOperatorsEnum RelationalOperator
		{
			get
			{
				return _relationalOperator;
			}

			set
			{
				_relationalOperator = value;
			}
		}

	}
}
