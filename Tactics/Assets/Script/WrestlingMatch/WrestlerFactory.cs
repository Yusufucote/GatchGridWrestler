using PlayerWrestler;
using UnityEngine;
using WrestlingRing;

namespace WrestlingMatch {
	public class WrestlerFactory : MonoBehaviour {

		[SerializeField]
		MatchWrestler wrestlerPrefab;

		[SerializeField]
		TeamStartingPositions _teamOne;

		[SerializeField]
		TeamStartingPositions _teamTwo;

		[SerializeField]
		TeamStartingPositions _teamThree;

		[SerializeField]
		TeamStartingPositions _teamFour;



		public MatchWrestler InstantiateWrester(WrestlerData wrestlerData, int teamNumber, int positionInTeam,
			Match matchManager, InMatchWrestlingTargetDeterminator targetDeterminator, Ring ring) {

			MatchWrestler matchWrestler = Instantiate(wrestlerPrefab);

			RingPosition startingPosition = null; 
			switch (teamNumber) {
				case 0:
					startingPosition = GetStartingRingPosition(_teamOne, positionInTeam);
					break;
				case 1:
					startingPosition = GetStartingRingPosition(_teamTwo, positionInTeam);
					break;
				case 2:
					startingPosition = GetStartingRingPosition(_teamThree, positionInTeam);
					break;
				case 3:
					startingPosition = GetStartingRingPosition(_teamFour, positionInTeam);
					break;
				default:
					break;
			}

			matchWrestler.InitializeWrestler(matchManager, wrestlerData, targetDeterminator, ring, startingPosition);
;
			return matchWrestler;
		}

		private RingPosition GetStartingRingPosition(TeamStartingPositions teamPos, int positionInTeam) {
			switch (positionInTeam) {
				case 0:
					return teamPos.PositionOne;
				case 1:
					return teamPos.PositionTwo;
				case 2:
					return teamPos.PositionThree;
				default:
					Debug.Log("NO SUCH POSITION IN TEAM");
					return null;
			}

		}

	}
}
