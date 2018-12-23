using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace UiKitchen
{
	public static class EditorMecanimUtils
	{
		public static AnimatorState GetSelectedState()
		{
			return Selection.activeObject as AnimatorState;
		}

		public static bool IsBehaviourOnAnimatorState(AnimatorState state, StateMachineBehaviour behaviour)
		{
			if (state != null)
			{
				var behaviours = state.behaviours;
				foreach (var child in behaviours)
				{
					if (child == behaviour)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
