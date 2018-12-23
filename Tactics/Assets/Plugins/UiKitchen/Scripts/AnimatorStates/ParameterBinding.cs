using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiKitchen
{
	[System.Serializable]
	public class ParameterBinding
	{
		[SerializeField]
		private string _name = "None";
		public string Name { get { return _name; } }

		[SerializeField]
		AnimatorControllerParameterType _type;
		public AnimatorControllerParameterType Type { get { return _type; } }

		[SerializeField]
		private bool _inverted = false;
		public bool Inverted
		{
			get { return _inverted; }
			set { _inverted = value; }
		}

		public AnimatorStateBroadcasterBehaviour Receiver { get; set; }

		public ParameterBinding(SerializableAnimatorControllerParameter parameter)
		{
			_name = parameter.Name;
			_type = parameter.Type;
		}

		public ParameterBinding(AnimatorControllerParameter parameter)
		{
			_name = parameter.name;
			_type = parameter.type;
		}

		public void Subscribe(AnimatorStateBroadcasterBehaviour sender)
		{
			switch (Type)
			{
				case AnimatorControllerParameterType.Trigger:
					sender.TriggerSet -= TriggerSet;
					sender.TriggerSet += TriggerSet;
					break;
				case AnimatorControllerParameterType.Int:
					sender.IntSet -= IntChanged;
					sender.IntSet += IntChanged;
					break;
				case AnimatorControllerParameterType.Float:
					sender.FloatSet -= FloatChanged;
					sender.FloatSet += FloatChanged;
					break;
				case AnimatorControllerParameterType.Bool:
					sender.BoolSet -= BoolChanged;
					sender.BoolSet += BoolChanged;
					break;
			}
		}

		public void Unsubscribe(AnimatorStateBroadcasterBehaviour sender)
		{
			switch (Type)
			{
				case AnimatorControllerParameterType.Trigger:
					sender.TriggerSet -= TriggerSet;
					break;
				case AnimatorControllerParameterType.Int:
					sender.IntSet -= IntChanged;
					break;
				case AnimatorControllerParameterType.Float:
					sender.FloatSet -= FloatChanged;
					break;
				case AnimatorControllerParameterType.Bool:
					sender.BoolSet -= BoolChanged;
					break;
			}
		}

		private void TriggerSet(string name)
		{
			if (Name == name)
				Receiver.SetTrigger(name);
		}

		private void IntChanged(string name, int value)
		{
			if (Name == name)
			{
				if (Inverted)
					Receiver.SetInt(name, -value);
				else
					Receiver.SetInt(name, value);
			}
		}

		private void FloatChanged(string name, float value)
		{
			if (Name == name)
			{
				if (Inverted)
					Receiver.SetFloat(name, -value);
				else
					Receiver.SetFloat(name, value);
			}
		}

		private void BoolChanged(string name, bool value)
		{
			if (Name == name)
			{
				if (Inverted)
					Receiver.SetBool(name, !value);
				else
					Receiver.SetBool(name, value);
			}
		}
	}
}
