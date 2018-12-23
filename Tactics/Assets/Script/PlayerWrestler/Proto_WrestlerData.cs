using System;
using UnityEngine;

namespace PlayerWrestler {
	[Serializable]
	public class Proto_WrestlerData {

		public string Name;
		public float Agility;
		public int Hp;
		public float Move;
		public float Defense;
		public float Strength;

		public Proto_WrestlerData(string name, float agility, int hp, float move, float defense, float strength) {
			Name = name;
			Agility = agility;
			Hp = hp;
			Move = move;
			Defense = defense;
			Strength = strength;
		}
	}
}
