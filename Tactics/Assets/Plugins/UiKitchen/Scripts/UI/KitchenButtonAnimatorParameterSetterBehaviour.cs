using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UiKitchen
{
	public class KitchenButtonAnimatorParameterSetterBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
		List<UiEventStateAnimatorParameterSetter> _events;
		public List<UiEventStateAnimatorParameterSetter> Events
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


		List<UiEventStateAnimatorParameterSetter> _downEvents;

		List<UiEventStateAnimatorParameterSetter> _upEvents;

		void Start()
		{
			_downEvents = CheckForEvent(Events, UiElementEventStateEnum.OnPointerDown);
			_upEvents = CheckForEvent(Events, UiElementEventStateEnum.OnPointerUp);
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

		List<UiEventStateAnimatorParameterSetter> CheckForEvent(List<UiEventStateAnimatorParameterSetter> eventList, UiElementEventStateEnum sendOnEvent)
		{
			var matchedEvents = new List<UiEventStateAnimatorParameterSetter>();
			foreach (var uiEvent in eventList)
			{
				if (uiEvent.SendOn == sendOnEvent)
				{
					matchedEvents.Add(uiEvent);
				}
			}
			return matchedEvents;
		}


		void SendEvents(UiEventStateAnimatorParameterSetter uiEvent)
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

	}
}
