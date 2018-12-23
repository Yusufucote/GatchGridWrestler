using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingPosition : MonoBehaviour {

	[SerializeField]
	Vector2 position;
	public Vector2 Position {
		get {
			return position;
		}
	}

	[SerializeField]
	RingPositionType ringPositionType;
	public RingPositionType RingPositionType {
		get {
			return ringPositionType;
		}
	}

	[SerializeField]
	Button button;

	[SerializeField]
	Image image;

	[SerializeField]
	Transform turnBuckleAnchor;
	public Transform TurnBuckleAnchor {
		get {
			return turnBuckleAnchor;
		}
	}

	public Transform Transform {
		get {
			return gameObject.transform;
		}
	}


	public event EventHandler<RingPositionSelectedEventArgs> RingPositionSelected;


	public void EnablePosistion()
	{
		image.color = Color.green;
		button.interactable = true;
		button.onClick.AddListener(OnSelected);
	}

	public void OnSelected()
	{
		if (RingPositionSelected != null) {
			RingPositionSelected(this, new RingPositionSelectedEventArgs() { RingPosition = this });
		}
	}

	public void DisablePosition()
	{
		image.color = Color.white;
		button.interactable = false;
		button.onClick.RemoveListener(OnSelected);
	}

	public void GetTurnBuckleTransform()
	{
		if (this.ringPositionType == RingPositionType.Croner) {
			RectTransform rect = (RectTransform)transform;
			Debug.Log(rect.sizeDelta);


		}

	}
 

}

public class RingPositionSelectedEventArgs: EventArgs
{
	public RingPosition RingPosition;
}

public enum RingPositionType
{
	None, Ropes, Croner 
}
