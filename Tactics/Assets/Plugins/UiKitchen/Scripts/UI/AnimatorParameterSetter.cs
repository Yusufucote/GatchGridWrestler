using System;
using UnityEngine;

namespace UiKitchen
{
	[Serializable]
	public abstract class AnimatorParameterSetter
	{
		[SerializeField]
		AnimatorControllerParameterType _type;
		public AnimatorControllerParameterType Type
		{
			get
			{
				return _type;
			}

			set
			{
				_type = value;
			}
		}


		[SerializeField]
		string _name;
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}


		[SerializeField]
		float _floatVlaue;
		public float FloatVlaue
		{
			get
			{
				return _floatVlaue;
			}

			set
			{
				_floatVlaue = value;
			}
		}


		[SerializeField]
		int _intVlaue;
		public int IntVlaue
		{
			get
			{
				return _intVlaue;
			}

			set
			{
				_intVlaue = value;
			}
		}

	}
}
