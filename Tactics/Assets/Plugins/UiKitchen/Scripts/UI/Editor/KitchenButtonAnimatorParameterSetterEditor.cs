using System;
using UnityEditor;
using UnityEngine;

namespace UiKitchen
{
	[CustomEditor(typeof(KitchenButtonAnimatorParameterSetterBehaviour))]
	public class KitchenButtonAnimatorParameterSetterEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			KitchenButtonAnimatorParameterSetterBehaviour objectScript = (KitchenButtonAnimatorParameterSetterBehaviour)target;
			var serObj = new SerializedObject(objectScript);

			SerializedProperty objectSetter = serObj.FindProperty("_setter");
			EditorGUILayout.PropertyField(objectSetter);
			if (objectScript.Setter == null)
			{
				EditorGUILayout.HelpBox("Please Chose a Target", MessageType.Error);
			}
			EditorGUILayout.PrefixLabel("Events: ");
			if (objectScript.Events != null)
			{
				EditorGUILayout.BeginVertical("Box");
				for (int i = 0; i < objectScript.Events.Count; i++)
				{
					EditorGUILayout.BeginVertical("Box");
					EditorGUILayout.BeginHorizontal();
					objectScript.Events[i].SendOn = (UiElementEventStateEnum)EditorGUILayout.EnumPopup(objectScript.Events[i].SendOn);
					if (GUILayout.Button("-"))
					{
						objectScript.Events.Remove(objectScript.Events[i]);
						break;
					}
					EditorGUILayout.EndHorizontal();
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

					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
			}


			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("+"))
			{
				objectScript.Events.Add(null);
			}
			EditorGUILayout.EndHorizontal();

			serObj.ApplyModifiedProperties();
		}
	}
}
