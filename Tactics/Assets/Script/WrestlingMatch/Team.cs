using PlayerWrestler;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrestlingMatch {
	[Serializable]
	public class Team  {

		[SerializeField]
		List<WrestlerDataScriptableObject> wrestlingTeam;
		public List<WrestlerDataScriptableObject> WrestlingTeam {
			get {
				return wrestlingTeam;
			}
		}
	}
}
