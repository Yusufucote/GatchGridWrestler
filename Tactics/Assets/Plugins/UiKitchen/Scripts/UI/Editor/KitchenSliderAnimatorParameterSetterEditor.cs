using UnityEditor;
using UnityEngine;

namespace UiKitchen
{
	[CustomEditor(typeof(KitchenSliderAnimatorParameterSetterBehaviour))]
	public class KitchenSliderAnimatorParameterSetterEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			KitchenSliderAnimatorParameterSetterBehaviour objectScript = (KitchenSliderAnimatorParameterSetterBehaviour)target;
			DrawSetterGUI(objectScript);
			DrawSliderGUI(objectScript);
			DrawEventsGUI(objectScript);
			AddNewEentGUI(objectScript);

		}

		static void DrawSetterGUI(KitchenSliderAnimatorParameterSetterBehaviour objectScript)
		{
			var serObj = new SerializedObject(objectScript);
			SerializedProperty objectSetter = serObj.FindProperty("_setter");
			EditorGUILayout.PropertyField(objectSetter);

			if (objectScript.Setter == null)
			{
				EditorGUILayout.HelpBox("Please Chose a Target", MessageType.Error);
			}
			serObj.ApplyModifiedProperties();
		}

		static void DrawSliderGUI(KitchenSliderAnimatorParameterSetterBehaviour objectScript)
		{
			var serObj = new SerializedObject(objectScript);
			SerializedProperty objectSlider = serObj.FindProperty("_slider");
			EditorGUILayout.PropertyField(objectSlider);
			serObj.ApplyModifiedProperties();
		}

		static void DrawEventsGUI(KitchenSliderAnimatorParameterSetterBehaviour objectScript)
		{
			EditorGUILayout.PrefixLabel("Events: ");
			if (objectScript.Events != null)
			{
				EditorGUILayout.BeginVertical("Box");
				for (int i = 0; i < objectScript.Events.Count; i++)
				{
					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.BeginHorizontal();
					objectScript.Events[i].SendOn = (ValuedUiElementEventStateEnum)EditorGUILayout.EnumPopup(objectScript.Events[i].SendOn);
					if (GUILayout.Button("-"))
					{
						objectScript.Events.Remove(objectScript.Events[i]);
						break;
					}
					EditorGUILayout.EndHorizontal();
					DrawRelationalOpperatorGUI(objectScript, i);

					DrawParameterTypeUI(objectScript, i);
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
			}
		}

		static void DrawRelationalOpperatorGUI(KitchenSliderAnimatorParameterSetterBehaviour objectScript, int i)
		{
			EditorGUILayout.BeginHorizontal();
			objectScript.Events[i].RelationalOperator = (RelationalOperatorsEnum)EditorGUILayout.EnumPopup("Value ", objectScript.Events[i].RelationalOperator);
			objectScript.Events[i].CompairsonValue = EditorGUILayout.FloatField(objectScript.Events[i].CompairsonValue);
			EditorGUILayout.EndHorizontal();
		}

		static void DrawParameterTypeUI(KitchenSliderAnimatorParameterSetterBehaviour objectScript, int i)
		{
			EditorGUILayout.BeginHorizontal();
			objectScript.Events[i].Type = (AnimatorControllerParameterType)EditorGUILayout.EnumPopup("Set Paramiter Type: ", objectScript.Events[i].Type);
			EditorGUILayout.EndHorizontal();
			objectScript.Events[i].Name = EditorGUILayout.TextField("Parameter Name: ", objectScript.Events[i].Name);
			EditorGUILayout.BeginHorizontal();
			switch (objectScript.Events[i].Type)
			{
				case AnimatorControllerParameterType.Float:
					objectScript.Events[i].FloatVlaue = EditorGUILayout.FloatField("Float Value: ", objectScript.Events[i].FloatVlaue);
					break;
				case AnimatorControllerParameterType.Int:
					objectScript.Events[i].IntVlaue = EditorGUILayout.IntField("Int Value: ", objectScript.Events[i].IntVlaue);
					break;
				default:
					break;
			}
			EditorGUILayout.EndHorizontal();
		}

		static void AddNewEentGUI(KitchenSliderAnimatorParameterSetterBehaviour objectScript)
		{
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				objectScript.Events.Add(null);
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
