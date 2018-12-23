using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	[SerializeField]
	PlayerMovementAction movementAction;

	private Ring ring;

	private RingPosition currentPosition; 

	internal void Initialize(Ring newRing, Vector2 startingPosiiton, InputManager inputManager)
	{
		ring = newRing;
		movementAction.Initialize(ring, inputManager);
		SetStartingPosition(startingPosiiton);
	}

	private void SetStartingPosition(Vector2 player1StartingPosition)
	{
		movementAction.SetStartingPosition(player1StartingPosition);
	}
}
