using UnityEngine;
using System.Collections;

namespace Abilities {
	[CreateAssetMenu(fileName = "Data", menuName = "CharacterData/Ability", order = 1)]
	public class AbilityScriptableObject : ScriptableObject {
		public Abilitity abilitity;
	}
}
