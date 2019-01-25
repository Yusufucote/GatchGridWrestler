﻿using Abilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerWrestler {
	[Serializable]
	public class WrestlerData {

		public string Name;
		public float Agility;
		public int Hp;
		public float Move;
		public float Defense;
		public float Strength;
		public AbilityScriptableObject BaseAttack;
		public List<AbilityScriptableObject> SpecialAbilites;
	}
}
