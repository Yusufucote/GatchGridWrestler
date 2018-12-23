using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UiKitchen.Utilities
{
	public static partial class MecanimUtils
	{
		/// <summary>
		/// Get a list of serializable state references from animator.
		/// <para>Each state must have MecanimStateEnteredReporter on it.</para>
		/// </summary>
		public static List<SerializableStateReference> GetStateReferences(this Animator animator)
		{
			var stateReferences = new List<SerializableStateReference>();
			if (animator != null)
			{
				var stateList = animator.GetBehaviourList<MecanimStateEnteredReporter>();
				foreach (var state in stateList)
				{
					stateReferences.Add(state.ToSerializableReference());
				}
			}
			return stateReferences;
		}

		/// <summary>
		/// Get a list of AnimatorControllerParameters converted to a serializable format.
		/// </summary>
		public static List<SerializableAnimatorControllerParameter> GetSerializableParameters(this Animator animator)
		{
			var list = new List<SerializableAnimatorControllerParameter>();
			if (animator != null)
			{
				var parameters = animator.parameters;
				for (int i = 0; i < parameters.Length; i++)
				{
					list.Add(new SerializableAnimatorControllerParameter(parameters[i]));
				}
			}
			return list;
		}

		public static List<T> GetBehaviourList<T>(this Animator animator)
		where T : StateMachineBehaviour
		{
			List<T> list = null;
			if (animator != null)
			{
				list = new List<T>(animator.GetBehaviours<T>());
			}
			return list;
		}
	}
}
