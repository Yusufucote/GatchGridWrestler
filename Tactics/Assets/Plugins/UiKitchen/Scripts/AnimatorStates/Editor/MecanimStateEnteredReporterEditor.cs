using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

namespace UiKitchen
{
	[CustomEditor(typeof(MecanimStateEnteredReporter))]
	public class MecanimStateReporterEditor : Editor
	{
		public const string DrawGuidKey = "DrawMecanimStateReporterGUID";

		public override void OnInspectorGUI()
		{
			var reporter = (MecanimStateEnteredReporter)target;
			var serializedReporter = new SerializedObject(reporter);
			bool objectChanged = false;

			var serializedIsEntryState = serializedReporter.FindProperty("_isEntryState");
			var isEntryState = serializedIsEntryState.boolValue;
			EditorGUILayout.PropertyField(serializedIsEntryState);
			objectChanged |= (isEntryState != serializedIsEntryState.boolValue);

			SerializedProperty serializedTag = serializedReporter.FindProperty("_onEnteredTag");
			var tag = serializedTag.intValue;
			EditorGUILayout.PropertyField(serializedTag);
			objectChanged |= (tag != serializedTag.intValue);

			DrawGuid(reporter.GuidString);

			// Update and display the name
			SerializedProperty serializedStateName = serializedReporter.FindProperty("_stateName");
			var selectedState = EditorMecanimUtils.GetSelectedState();
			if (EditorMecanimUtils.IsBehaviourOnAnimatorState(selectedState, reporter))
			{
				// The selected state contains this behaviour, store the name from the selected state
				if (serializedStateName.stringValue != selectedState.name)
				{
					serializedStateName.stringValue = selectedState.name;
					objectChanged |= true;
				}
			}

			if (objectChanged)
			{
				serializedReporter.ApplyModifiedProperties();
				EditorUtility.SetDirty(reporter);
			}
		}

		private void DrawGuid(string guid)
		{
			var drawGuid = EditorPrefs.GetBool(DrawGuidKey, false);

			if (drawGuid || Event.current.alt)
			{
				GUILayout.Label("Guid: " + guid);
			}

			var previousRect = GUILayoutUtility.GetLastRect();
			if (Event.current.type == EventType.ContextClick &&
				previousRect.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
				EditorPrefs.SetBool(DrawGuidKey, !drawGuid);
			}
		}
	}
}


