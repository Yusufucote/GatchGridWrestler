using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	[CreateAssetMenu(fileName = "Data", menuName = "CharacterData/Character", order = 1)]
	public class WrestlerDataScriptableObject : ScriptableObject {

		public WrestlerData wrestlerData;

	}
}
