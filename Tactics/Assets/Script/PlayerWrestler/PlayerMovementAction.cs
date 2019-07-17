using UnityEngine;
using System.Collections.Generic;

public class PlayerMovementAction 
{
	private Transform _playerRoot;
	private Ring _ring;
	private RingPosition _currentRingPosition;
	private List<RingPosition> _currentlySelectedRingPositions = new List<RingPosition>();

	public PlayerMovementAction(Transform playerRoot, Ring ring) {
		_playerRoot = playerRoot;
		_ring = ring;
	}

	private void HandleOnVerticleButtonPressed(object sender, VerticalButtonPressedEventArgs e)
	{
		if (e.direction  == UpDownEnum.Up) {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x, _currentRingPosition.Position.y + 1);
			HandleMoveToDestinationVector(destinationVector);
		}
		else {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x, _currentRingPosition.Position.y - 1);
			HandleMoveToDestinationVector(destinationVector);
		}
	}

	private void HandleOnHorizontalButtonPressed(object sender, HorizontalButtonPressedEventArgs e)
	{
		
		if (e.direction == LeftRightEnum.Left) {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x - 1, _currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
		else {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x + 1, _currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
	}

	private void HandleMoveToDestinationVector(Vector2 destinationVector)
	{
		RingPosition destanation = _ring.GetRingPosition(destinationVector);
		if (destanation != null) {
			_playerRoot.position = destanation.Transform.position;
			_currentRingPosition = destanation;
		}
		else {
			CheckCurrentPosition();
		}
	}

	private void CheckCurrentPosition()
	{
		switch (_currentRingPosition.RingPositionType) {
			case RingPositionType.None:
				Debug.Log("Something is not quite right");
				break;
			case RingPositionType.Ropes:
				HandleRopeBounce();
				break;
			case RingPositionType.Croner:
				_currentRingPosition.GetTurnBuckleTransform();
				_playerRoot.position = new Vector3(_currentRingPosition.Transform.position.x - 50, _currentRingPosition.Transform.position.y + 50, _currentRingPosition.Transform.position.z);
				break;
			default:
				break;
		}
	}

	private void HandleRopeBounce()
	{
		if (_currentRingPosition.Position.x == 0) {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x + 2, _currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
		if (_currentRingPosition.Position.x == 5) {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x - 2, _currentRingPosition.Position.y);
			HandleMoveToDestinationVector(destinationVector);
		}
		if (_currentRingPosition.Position.y == 0) {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x, _currentRingPosition.Position.y + 2);
			HandleMoveToDestinationVector(destinationVector);
		}
		if (_currentRingPosition.Position.y == 5) {
			Vector2 destinationVector = new Vector2(_currentRingPosition.Position.x, _currentRingPosition.Position.y - 2);
			HandleMoveToDestinationVector(destinationVector);
		}
	}

	public RingPosition SetStartingPosition(Vector2 ringPosition)
	{
		RingPosition startingRingPosition = _ring.GetRingPosition(ringPosition);
		_playerRoot.position = startingRingPosition.Transform.position;
		_currentRingPosition = startingRingPosition;
		return startingRingPosition;
	}
}
