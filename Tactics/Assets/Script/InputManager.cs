using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public event EventHandler<HorizontalButtonPressedEventArgs> horizontalButtonPressed;
	public event EventHandler<VerticalButtonPressedEventArgs> verticalButtonPressed;

	void Update () {

		if (Input.GetButtonDown("Horizontal")) {
			SendHoizontalButtonPressedEvent();
		}
		if (Input.GetButtonDown("Vertical")) {
			SendVerticalButtonPressedEvent();
		}

	}


	private void SendHoizontalButtonPressedEvent()
	{
		if (Input.GetAxisRaw("Horizontal") > 0) {
			if (horizontalButtonPressed != null) {
				horizontalButtonPressed(this, new HorizontalButtonPressedEventArgs() { direction = LeftRightEnum.Right });
			}
		}
		else {
			if (horizontalButtonPressed != null) {
				horizontalButtonPressed(this, new HorizontalButtonPressedEventArgs() { direction = LeftRightEnum.Left });
			}
		}
	}

	private void SendVerticalButtonPressedEvent()
	{
		if (Input.GetAxisRaw("Vertical") > 0) {
			if (verticalButtonPressed != null) {
				verticalButtonPressed(this, new VerticalButtonPressedEventArgs() { direction = UpDownEnum.Up });
			}
		}
		else {
			if (verticalButtonPressed != null) {
				verticalButtonPressed(this, new VerticalButtonPressedEventArgs() { direction = UpDownEnum.Down });
			}
		}
	}
}

public class HorizontalButtonPressedEventArgs: EventArgs
{
	public LeftRightEnum direction;
}

public class VerticalButtonPressedEventArgs : EventArgs
{
	public UpDownEnum direction;
}

public enum UpDownEnum
{
	Up, Down
}
public enum LeftRightEnum
{
	Left, Right
}
