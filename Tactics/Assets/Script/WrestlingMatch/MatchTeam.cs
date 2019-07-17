using PlayerWrestler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTeam 
{
	private readonly List<MatchWrestler> _teamMates;
	public List<MatchWrestler> TeamMates { get => _teamMates;}

	public MatchTeam(List<MatchWrestler> teamMates) {
		_teamMates = teamMates;

		foreach (var wrestler in _teamMates) {
			wrestler.Team = _teamMates;
		}
	}

}
