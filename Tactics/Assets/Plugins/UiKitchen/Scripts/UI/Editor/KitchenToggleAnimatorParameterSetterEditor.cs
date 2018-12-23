using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UiKitchen
{
	[CustomEditor(typeof(KitchenToggleAnimatorParameterSetterBehaviour))]
	public class KitchenToggleAnimatorParameterSetterEditor : Editor
	{
		public override void OnInspectorGUI()
		{

			KitchenToggleAnimatorParameterSetterBehaviour objectScript = (KitchenToggleAnimatorParameterSetterBehaviour)target;
			var serObj = new SerializedObject(objectScript);

			SettingFeildGUI(objectScript);
			ToggleFeildGUI(objectScript);

			SerializedProperty sendIfValue = serObj.FindProperty("_sendIf");
			EditorGUILayout.PropertyField(sendIfValue);

			SerializedProperty parameterType = serObj.FindProperty("_parameterType");
			EditorGUILayout.PropertyField(parameterType);

			ParameterSettingGUI(objectScript);

			serObj.ApplyModifiedProperties();
		}

		static void SettingFeildGUI(KitchenToggleAnimatorParameterSetterBehaviour objectScript)
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

		static void ToggleFeildGUI(KitchenToggleAnimatorParameterSetterBehaviour objectScript)
		{
			var serObj = new SerializedObject(objectScript);
			var toggleOnObject = objectScript.gameObject.GetComponent<Toggle>();
			if (toggleOnObject != null && objectScript.Toggle == null)
			{
				objectScript.Toggle = toggleOnObject;
			}
			SerializedProperty toggleSetter = serObj.FindProperty("_toggle");
			EditorGUILayout.PropertyField(toggleSetter);
			if (objectScript.Toggle == null)
			{
				EditorGUILayout.HelpBox("Please Chose a Toggle", MessageType.Error);
			}
			serObj.ApplyModifiedProperties();
		}

		static void ParameterSettingGUI(KitchenToggleAnimatorParameterSetterBehaviour objectScript)
		{
			var serObj = new SerializedObject(objectScript);
			EditorGUILayout.BeginVertical("Box");
			SerializedProperty parameterName = serObj.FindProperty("_parameterName");
			EditorGUILayout.PropertyField(parameterName);

			switch (objectScript.ParameterType)
			{
				case AnimatorControllerParameterType.Bool:
					SerializedProperty boolValue = serObj.FindProperty("_boolValue");
					EditorGUILayout.PropertyField(boolValue);
					break;

				case AnimatorControllerParameterType.Float:
					SerializedProperty floatValue = serObj.FindProperty("_floatValue");
					EditorGUILayout.PropertyField(floatValue);
					break;

				case AnimatorControllerParameterType.Int:
					SerializedProperty intValue = serObj.FindProperty("_intValue");
					EditorGUILayout.PropertyField(intValue);
					break;

				default:
					break;
			}

			EditorGUILayout.EndVertical();
			serObj.ApplyModifiedProperties();
		}
	}
}
