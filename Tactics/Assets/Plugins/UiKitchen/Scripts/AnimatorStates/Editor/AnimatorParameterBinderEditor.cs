using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

namespace UiKitchen
{
	[CustomEditor(typeof(AnimatorParameterBinderBehaviour))]
	public class AnimatorParameterBinderEditor : Editor
	{
		private SerializedObject _serializedParamBinder;
		private AnimatorParameterBinderBehaviour _paramBinder;

		private static GUIContent Delete = new GUIContent("Delete");

		private List<SerializableAnimatorControllerParameter> _availableParameters = null;

		Color _defaultBackgroundColor;

		public override void OnInspectorGUI()
		{
			bool contextClick = (Event.current.type == EventType.ContextClick);
			var mousePosition = Event.current.mousePosition;

			_defaultBackgroundColor = GUI.backgroundColor;

			_paramBinder = (AnimatorParameterBinderBehaviour)target;
			_serializedParamBinder = new SerializedObject(_paramBinder);

			var broadcasterProperty = _serializedParamBinder.FindProperty("_broadcaster");
			EditorGUILayout.PropertyField(broadcasterProperty);

			DetermineAvailableParameters();

			if (_paramBinder.Broadcaster != null)
			{
				GUILayout.Label("Bound Parameters", EditorStyles.boldLabel);

				if (_paramBinder.ParameterBindings.Count > 0)
				{
					for (int i = 0; i < _paramBinder.ParameterBindings.Count; i++)
					{
						// Alternate the background color each state element
						float alphaMod = Mathf.Min((i + 1) % 2, 0.5f);
						GUI.backgroundColor = new Color(_defaultBackgroundColor.r, _defaultBackgroundColor.g, _defaultBackgroundColor.b, _defaultBackgroundColor.a * alphaMod);

						var stateRect = DrawParamBinding(_paramBinder.ParameterBindings[i], i);

						if (contextClick && stateRect.Contains(mousePosition))
						{
							contextClick = false;
							Event.current.Use();

							ShowContextMenu(i);
						}
					}
				}
				else
				{
					GUILayout.Label("No bound parameters", EditorStyles.boldLabel);
				}
			}
			else
			{
				EditorGUILayout.HelpBox("Please choose a target Animator to bind to.", MessageType.Error);
			}

			GUI.enabled = (_availableParameters != null && 0 < _availableParameters.Count);
			if (GUILayout.Button("+"))
			{
				ShowCreationMenu();
			}
			GUI.enabled = true;
			_serializedParamBinder.ApplyModifiedProperties();
		}

		private void DetermineAvailableParameters()
		{
			if (_paramBinder.Broadcaster != null && _paramBinder.Receiver != null)
			{
				var broadcasterParams = _paramBinder.Broadcaster.Animator.parameters;
				var receiverParams = _paramBinder.Receiver.Animator.parameters;

				var unionParams = new List<SerializableAnimatorControllerParameter>();
				foreach (var broadcasterParam in broadcasterParams)
				{
					foreach (var receiverParam in receiverParams)
					{
						if (receiverParam.name == broadcasterParam.name &&
							receiverParam.type == broadcasterParam.type)
						{
							unionParams.Add(new SerializableAnimatorControllerParameter(receiverParam));
						}
					}
				}

				for (int i = 0; i < unionParams.Count; i++)
				{
					for (int j = 0; j < _paramBinder.ParameterBindings.Count; j++)
					{
						var boundParam = _paramBinder.ParameterBindings[j];
						if (boundParam.Name == unionParams[i].Name &&
							boundParam.Type == unionParams[i].Type)
						{
							unionParams.RemoveAt(i--);
						}
					}
				}
				_availableParameters = unionParams;
			}
			else
			{
				_availableParameters = null;
			}
		}

		private Rect DrawParamBinding(ParameterBinding parameterBinding, int index)
		{
			GUILayout.BeginHorizontal("Box");
			GUI.backgroundColor = _defaultBackgroundColor;
			GUILayout.Label(parameterBinding.Name);
			GUILayout.Label(parameterBinding.Type.ToString());

			var inverted = EditorGUILayout.Toggle("Inverted", parameterBinding.Inverted);
			if (inverted != parameterBinding.Inverted)
			{
				MarkDirty();
				parameterBinding.Inverted = inverted;
			}


			GUILayout.EndHorizontal();
			return GUILayoutUtility.GetLastRect();
		}

		private void ShowContextMenu(int index)
		{
			var contextMenu = new GenericMenu();
			contextMenu.AddItem(Delete, false, DeleteBinding, index);
			contextMenu.ShowAsContext();

		}
		private void DeleteBinding(object indexObj)
		{
			int index = (int)indexObj;
			if (index >= 0)
			{
				MarkDirty();
				_paramBinder.ParameterBindings.RemoveAt(index);
			}
		}

		private void ShowCreationMenu()
		{
			var creationMenu = new GenericMenu();
			for (int i = 0; i < _availableParameters.Count; i++)
			{
				creationMenu.AddItem(new GUIContent(_availableParameters[i].Name), false, CreateParameter, _availableParameters[i]);
			}
			creationMenu.ShowAsContext();
		}

		private void CreateParameter(object parameter)
		{
			MarkDirty();
			_paramBinder.ParameterBindings.Add(new ParameterBinding((SerializableAnimatorControllerParameter)parameter));
		}

		private void MarkDirty()
		{
			Undo.RecordObject(_paramBinder, "AnimatorParameterBinder change");
		}
	}
}