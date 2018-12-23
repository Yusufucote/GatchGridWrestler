using UnityEngine;
using System.Collections;
using UiKitchen;
using System.Collections.Generic;

public class PlayerMovementAction : MonoBehaviour, IPlayerAction
{
	[SerializeField]
	Transform playerRoot;

	private Ring ring;

	RingPosition currentRingPosition;
	List<RingPosition> currentlySelectedRingPositions;
	InputManager inputManager;

	internal void Initialize(Ring Newring, InputManager newInputManager)
	{
		currentlySelectedRingPositions = new List<RingPosition>();
		ring = Newring;
		inputManager = newInputManager;

		inputManager.horizontalButtonPressed += HandleOnHorizontalButtonPressed;
		inputManager.verticalButtonPressed += HandleOnVerticleButtonPressed;
	}

	private void HandleOnVerticleButtonPressed(object sender, VerticalButtonPressedEventArgs e)
	{
		if (e.direction  == UpDownEnum.Up) {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x, currentRingPosition.Position.y + 1);
			HandleMoveToDestinationVector(destinationVector);
		}
		else {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x, currentRingPosition.Position.y - 1);
			HandleMoveToDestinationVector(destinationVector);
		}
	}

	private void HandleOnHorizontalButtonPressed(object sender, HorizontalButtonPressedEventArgs e)
	{
		if (e.direction == LeftRightEnum.Left) {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x - 1, currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
		else {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x + 1, currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
	}

	private void HandleMoveToDestinationVector(Vector2 destinationVector)
	{
		RingPosition destanation = ring.GetRingPosition(destinationVector);
		if (destanation != null) {
			playerRoot.position = destanation.Transform.position;
			currentRingPosition = destanation;
		}
		else {
			CheckCurrentPosition();
		}
	}

	private void CheckCurrentPosition()
	{
		switch (currentRingPosition.RingPositionType) {
			case RingPositionType.None:
				Debug.Log("Something is not quite right");
				break;
			case RingPositionType.Ropes:
				HandleRopeBounce();
				break;
			case RingPositionType.Croner:
				currentRingPosition.GetTurnBuckleTransform();
				playerRoot.position = new Vector3(currentRingPosition.Transform.position.x - 50, currentRingPosition.Transform.position.y + 50, currentRingPosition.Transform.position.z);
				break;
			default:
				break;
		}
	}

	private void HandleRopeBounce()
	{
		if (currentRingPosition.Position.x == 0) {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x + 2, currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
		if (currentRingPosition.Position.x == 5) {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x - 2, currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
		if (currentRingPosition.Position.y == 0) {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x, currentRingPosition.Position.y + 2);
			HandleMoveToDestinationVector(destinationVector);
		}
		if (currentRingPosition.Position.y == 5) {
			Vector2 destinationVector = new Vector2(currentRingPosition.Position.x, currentRingPosition.Position.y - 2);
			HandleMoveToDestinationVector(destinationVector);
		}
	}

	public RingPosition SetStartingPosition(Vector2 ringPosition)
	{
		RingPosition startingRingPosition = ring.GetRingPosition(ringPosition);
		playerRoot.position = startingRingPosition.Transform.position;
		currentRingPosition = startingRingPosition;
		return startingRingPosition;
	}


}
