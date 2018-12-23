using UnityEngine;
using System.Collections;

public class CloseButton : AbstractButtonPressedEventReporter
{

	[SerializeField]
	bool enabledOnStart;

	private void Start()
	{
		animator.SetBool("on", enabledOnStart);
	}

	public override void DisableButton()
	{
		animator.SetBool("on", false);
	}

	public override void EnableButton()
	{
		animator.SetBool("on", true);
	}

	public override void OnButtonPressed()
	{
		
	}

}
