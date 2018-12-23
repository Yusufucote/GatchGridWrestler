using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace UiKitchen
{
	public class KitchenSliderAnimatorParameterSetterBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		[SerializeField]
		Slider _slider;

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
		List<SliderUiEventStateAnimatorParameterSetterData> _events;
		public List<SliderUiEventStateAnimatorParameterSetterData> Events
		{
			get
			{
				return _events;
			}

			set
			{
				_events = value;
			}
		}


		List<SliderUiEventStateAnimatorParameterSetterData> _downEvents;

		List<SliderUiEventStateAnimatorParameterSetterData> _upEvents;

		List<SliderUiEventStateAnimatorParameterSetterData> _onValueChangedEvents;

		void Start()
		{
			_slider.onValueChanged.AddListener(OnValueChanged);
			_downEvents = CheckForEvent(_events, ValuedUiElementEventStateEnum.OnPointerDown);
			_upEvents = CheckForEvent(_events, ValuedUiElementEventStateEnum.OnPointerUp);
			_onValueChangedEvents = CheckForEvent(_events, ValuedUiElementEventStateEnum.OnValueChanged);
		}

		void OnValueChanged(float newValue)
		{
			foreach (var uiEvent in _onValueChangedEvents)
			{
				CheckRelationalOperator(newValue, uiEvent);
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			foreach (var uiEvent in _downEvents)
			{
				SendEvents(uiEvent);
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			foreach (var uiEvent in _upEvents)
			{
				SendEvents(uiEvent);
			}
		}

		List<SliderUiEventStateAnimatorParameterSetterData> CheckForEvent(List<SliderUiEventStateAnimatorParameterSetterData> eventList, ValuedUiElementEventStateEnum sendOnEvent)
		{
			var matchedEvents = new List<SliderUiEventStateAnimatorParameterSetterData>();
			foreach (var uiEvent in eventList)
			{
				if (uiEvent.SendOn == sendOnEvent)
				{
					matchedEvents.Add(uiEvent);
				}
			}
			return matchedEvents;
		}

		void CheckRelationalOperator(float newValue, SliderUiEventStateAnimatorParameterSetterData uiEvent)
		{
			switch (uiEvent.RelationalOperator)
			{
				case RelationalOperatorsEnum.EqualTo:
					if (newValue == uiEvent.CompairsonValue)
					{
						SendEvents(uiEvent);
					}
					break;
				case RelationalOperatorsEnum.GreaterThan:
					if (newValue > uiEvent.CompairsonValue)
					{
						SendEvents(uiEvent);
					}
					break;
				case RelationalOperatorsEnum.GreaterThanOrEqualTo:
					if (newValue >= uiEvent.CompairsonValue)
					{
						SendEvents(uiEvent);
					}
					break;
				case RelationalOperatorsEnum.LessThan:
					if (newValue < uiEvent.CompairsonValue)
					{
						SendEvents(uiEvent);
					}
					break;
				case RelationalOperatorsEnum.LessThanOrEqualTo:
					if (newValue <= uiEvent.CompairsonValue)
					{
						SendEvents(uiEvent);
					}
					break;
				case RelationalOperatorsEnum.NotEqualTo:
					if (newValue != uiEvent.CompairsonValue)
					{
						SendEvents(uiEvent);
					}
					break;
				default:
					break;
			}
		}

		void SendEvents(SliderUiEventStateAnimatorParameterSetterData uiEvent)
		{
			switch (uiEvent.Type)
			{
				case AnimatorControllerParameterType.Bool:
					var boolValue = _setter.GetBool(uiEvent.Name);
					_setter.SetBool(uiEvent.Name, !boolValue);
					break;
				case AnimatorControllerParameterType.Float:
					_setter.SetFloat(uiEvent.Name, uiEvent.FloatVlaue);
					break;
				case AnimatorControllerParameterType.Int:
					_setter.SetInt(uiEvent.Name, uiEvent.IntVlaue);
					break;
				case AnimatorControllerParameterType.Trigger:
					_setter.SetTrigger(uiEvent.Name);
					break;
				default:
					break;
			}
		}

		void OnDestory()
		{
			_slider.onValueChanged.RemoveListener(OnValueChanged);
		}
	}
}
