using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public abstract class AbstractButtonPressedEventReporter : MonoBehaviour
{

	[SerializeField]
	public Animator animator;

	[SerializeField]
	Button button;

	public event EventHandler<EventArgs> ButtonPressed;

	private void Awake()
	{
		button.onClick.AddListener(SerndCloseButtonPressedEvent);
	}

	private void SerndCloseButtonPressedEvent()
	{
		if (ButtonPressed != null) {
			ButtonPressed(this, EventArgs.Empty);
		}
		OnButtonPressed();
	}
	public abstract void EnableButton();
	public abstract void OnButtonPressed();
	public abstract void DisableButton();

	private void OnDestroy()
	{
		button.onClick.RemoveListener(SerndCloseButtonPressedEvent);

	}
}
