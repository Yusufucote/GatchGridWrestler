using UnityEngine;
using System.Collections;

namespace WrestlingRing {
	public class TeamStartingPositions : MonoBehaviour {

		[SerializeField]
		RingPosition _positionOne;

		[SerializeField]
		RingPosition _positionTwo;

		[SerializeField]
		RingPosition _positionThree;

		public RingPosition PositionTwo { get => _positionTwo; set => _positionTwo = value; }
		public RingPosition PositionOne { get => _positionOne; set => _positionOne = value; }
		public RingPosition PositionThree { get => _positionThree; set => _positionThree = value; }
	}
}
