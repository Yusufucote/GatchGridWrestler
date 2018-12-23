using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour {

	[SerializeField]
	List<RingPosition> ringPositionList;

	Dictionary<Vector2, RingPosition> ringPositions;

	private void Awake()
	{
		ringPositions = new Dictionary<Vector2, RingPosition>();

		foreach (var ringPosition in ringPositionList) {
			ringPositions.Add(ringPosition.Position, ringPosition);
		}
	}

	public RingPosition GetRingPosition(Vector2 positionVector)
	{
		if (ringPositions.ContainsKey(positionVector)) {
			return ringPositions[positionVector];
		}
		else {
			return null;
		}
	}

	public List<RingPosition> GetRingPositionsInRadius(int radius, RingPosition centerPosition)
	{
		List<RingPosition> newRingPositions = new List<RingPosition>();
		Vector2 centerVector = centerPosition.Position;
		RingPosition north = null;
		RingPosition east = null;
		RingPosition south = null;
		RingPosition west = null;

		for (int i = 1; i <= radius; i++) {
			if (ringPositions.ContainsKey(new Vector2(centerVector.x, centerVector.y + i))) {
				north = ringPositions[new Vector2(centerVector.x, centerVector.y + i)];
				newRingPositions.Add(north);
			}
			if (ringPositions.ContainsKey(new Vector2(centerVector.x + i, centerVector.y))) {
				east = ringPositions[new Vector2(centerVector.x + i, centerVector.y)];
				newRingPositions.Add(east);
			}
			if (ringPositions.ContainsKey(new Vector2(centerVector.x, centerVector.y - i))) {
				south = ringPositions[new Vector2(centerVector.x, centerVector.y -i )];
				newRingPositions.Add(south);
			}
			if (ringPositions.ContainsKey(new Vector2(centerVector.x - i, centerVector.y))) {
				west = ringPositions[new Vector2(centerVector.x-i, centerVector.y)];
				newRingPositions.Add(west);
			}

			if (radius > 1) {
				for (int x = 1; x <= radius - 1; x++) {
					// Future Joe: There is a issure here. if a Direction  is gone there can still be points on the line. need to work aroudn that. 
					if (north != null) {
						newRingPositions.Add(GetRingPositionInDirection(x, north, new Vector2(1, -1)));
					}
					if (east != null) {
						newRingPositions.Add(GetRingPositionInDirection(x, east, new Vector2(-1, -1)));
					}
					if (south != null) {
						newRingPositions.Add(GetRingPositionInDirection(x, south, new Vector2(-1, 1)));
					}
					if (west != null) {
						newRingPositions.Add(GetRingPositionInDirection(x, west, new Vector2(1, 1)));
					}

				}
			}

		}

		return newRingPositions;
	}

	private RingPosition GetRingPositionInDirection(int distance, RingPosition position, Vector2 directionVector)
	{
		return  ringPositions[new Vector2(position.Position.x + (distance * directionVector.x), position.Position.y + (distance * directionVector.y))];
	}
}
