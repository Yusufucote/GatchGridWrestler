using System;
using UnityEngine;
using UnityEngine.UI;

namespace UiKitchen
{
	public class KitchenToggleAnimatorParameterSetterBehaviour : MonoBehaviour
	{
		[SerializeField]
		AnimatorStateBroadcasterBehaviour _setter;
		public AnimatorStateBroadcasterBehaviour Setter
		{
			get
			{
				return _setter;
			}
		}

		[SerializeField]
		Toggle _toggle;
		public Toggle Toggle
		{
			get
			{
				return _toggle;
			}
			set
			{
				_toggle = value;
			}
		}

		[SerializeField]
		bool _sendIf;

		[SerializeField]
		AnimatorControllerParameterType _parameterType;
		public AnimatorControllerParameterType ParameterType
		{
			get
			{
				return _parameterType;
			}
		}

		[SerializeField]
		string _parameterName;

		[SerializeField]
		bool _boolValue;

		[SerializeField]
		int _intValue;

		[SerializeField]
		float _floatValue;

		void Start()
		{
			_toggle.onValueChanged.AddListener(ToggleUpdated);
		}

		void ToggleUpdated(bool arg0)
		{
			if (_toggle.isOn == _sendIf)
			{
				SetParameter();
			}
		}

		void SetParameter()
		{
			switch (_parameterType)
			{
				case AnimatorControllerParameterType.Bool:
					_setter.SetBool(_parameterName, _boolValue);
					break;

				case AnimatorControllerParameterType.Float:
					_setter.SetFloat(_parameterName, _floatValue);
					break;

				case AnimatorControllerParameterType.Int:
					_setter.SetInt(_parameterName, _intValue);
					break;

				case AnimatorControllerParameterType.Trigger:
					_setter.SetTrigger(_parameterName);
					break;
				default:
					break;
			}
		}

		void OnDestory()
		{
			_toggle.onValueChanged.RemoveAllListeners();
		}
	}
}
