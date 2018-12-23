using System;
using System.Collections.Generic;
using System.Linq;
using UiKitchen.Utilities;
using UnityEditor;
using UnityEngine;

namespace UiKitchen
{
	[CustomEditor(typeof(AnimatorStateBinderBehaviour))]
	public class AnimatorStateBinderEditor : Editor
	{
		private AnimatorStateBinderBehaviour _stateBinder;

		private enum Filter
		{
			ShowAll,
			StateEntered,
			ParameterName,
			ParameterType
		}

		private Filter _currentFilter = Filter.ShowAll;

		private int _filterIndex = 0;

		private const string KeyFilterType = "UiKitchenFilterType";
		private const string KeyFilterIndex = "UiKitchenFilterIndex";

		private const string Unassigned = "Unassigned";

		// _broadcasterStates and _broadcasterStateNames must be listed in the same order for indexing
		private SerializableStateReference[] _broadcasterStates;
		private string[] _broadcasterStateNames;

		// _receiverParameters and _receiverParameterNames must be listed in the same order for indexing
		private SerializableAnimatorControllerParameter[] _receiverParameters;
		private string[] _receiverParameterNames;

		Color _defaultBackgroundColor;

		bool _contextClick = false;
		Vector2 _mousePosition;

		public override void OnInspectorGUI()
		{
			_stateBinder = (AnimatorStateBinderBehaviour) target;

			SetupForContextMenus();
			_defaultBackgroundColor = GUI.backgroundColor;

			var broadcasterProperty = serializedObject.FindProperty("_broadcaster");
			EditorGUILayout.PropertyField(broadcasterProperty);

			if (_stateBinder.Broadcaster != null)
			{
				GUILayout.Label("Bound States", EditorStyles.boldLabel);

				ExtractPopupArrays(_stateBinder);
				SelectFilterType();
				SelectFilterIndex();

				if (_stateBinder.StateBindings.Count > 0)
				{
					int numBindingsDrawn = 0;
					for (int i = 0; i < _stateBinder.StateBindings.Count; i++)
					{

						int stateIndex = GetStateIndex(_stateBinder.StateBindings[i].StateEntered);
						int parameterIndex = GetParamIndex(_stateBinder.StateBindings[i].Parameter);

						if (IncludedInFilter(_stateBinder.StateBindings[i], stateIndex, parameterIndex))
						{
							// Alternate the background color each state element
							float alphaMod = Mathf.Min((numBindingsDrawn + 1) % 2, 0.5f);
							GUI.backgroundColor = new Color(_defaultBackgroundColor.r, _defaultBackgroundColor.g, _defaultBackgroundColor.b, _defaultBackgroundColor.a * alphaMod);

							DrawStateBinding(_stateBinder.StateBindings[i], stateIndex, parameterIndex);

							// Track context click (right click)
							if (PreviousGUILayoutRightClicked())
							{
								ShowBindingItemContextMenu(i);
							}
							numBindingsDrawn++;
						}
					}
				}
				else
				{
					GUILayout.Label("No bound states", EditorStyles.boldLabel);
				}
			}
			else
			{
				EditorGUILayout.HelpBox("Please choose a target Animator to bind to.", MessageType.Error);
			}

			if (!Event.current.shift)
			{
				GUILayout.BeginHorizontal();
				DrawNewBindingButton();
				DrawMassNewButton();
				DrawMassDeleteButton();
				GUILayout.EndHorizontal();
			}
			else
			{
				if (GUILayout.Button("Remap Unassigned GUIDs"))
				{
					MarkDirty();
					var remappedStates = _stateBinder.RecoverUnassignedStates();
					Debug.Log(string.Format("Remapped {0} GUIDS.", remappedStates));
				}
			}

			DrawSortButton();

			serializedObject.ApplyModifiedProperties();
		}

		private void SetupForContextMenus()
		{
			_contextClick = (Event.current.type == EventType.ContextClick);
			_mousePosition = Event.current.mousePosition;
		}

		private bool PreviousGUILayoutRightClicked()
		{
			var clickRegion = GUILayoutUtility.GetLastRect();
			if (_contextClick && clickRegion.Contains(_mousePosition))
			{
				_contextClick = false;
				Event.current.Use();
				return true;
			}
			return false;
		}

		private void ExtractPopupArrays(AnimatorStateBinderBehaviour stateBinder)
		{
			// Generate the list of broadcaster states
			var broadcasterStates = stateBinder.Broadcaster.Animator.GetStateReferences();
			broadcasterStates = broadcasterStates.OrderBy(state => state.Name).ToList();
			var broadcasterStateNames = broadcasterStates.Select(x => x.Name).ToList();
			broadcasterStates.Insert(0, null);
			broadcasterStateNames.Insert(0, Unassigned);

			// Unity only acceps arrays for GUI, so convert the lists to arrays
			_broadcasterStates = broadcasterStates.ToArray();
			_broadcasterStateNames = broadcasterStateNames.ToArray();

			// Generate the list of receiver parameters
			var receiverParameters = stateBinder.Receiver.Animator.GetSerializableParameters();
			var receiverParameterNames = receiverParameters.Select(x => x.Name).ToList();
			receiverParameters.Insert(0, null);
			receiverParameterNames.Insert(0, Unassigned);

			// Unity only acceps arrays for GUI, so convert the lists to arrays
			_receiverParameters = receiverParameters.ToArray();
			_receiverParameterNames = receiverParameterNames.ToArray();
		}

		private void SelectFilterType()
		{
			_currentFilter = (Filter) EditorPrefs.GetInt(KeyFilterType, 0);
			_currentFilter = (Filter) EditorGUILayout.EnumPopup("Filter Type", (System.Enum) _currentFilter);
			EditorPrefs.SetInt(KeyFilterType, (int) _currentFilter);
		}

		private void SelectFilterIndex()
		{
			_filterIndex = EditorPrefs.GetInt(KeyFilterIndex, 0);
			switch (_currentFilter)
			{
				case Filter.StateEntered:
					_filterIndex = ClampIndex(_filterIndex, _broadcasterStateNames.Length);
					_filterIndex = EditorGUILayout.Popup("Filter", _filterIndex, _broadcasterStateNames);
					break;
				case Filter.ParameterName:
					_filterIndex = ClampIndex(_filterIndex, _receiverParameterNames.Length);
					_filterIndex = EditorGUILayout.Popup("Filter", _filterIndex, _receiverParameterNames);
					break;
				case Filter.ParameterType:
					AnimatorControllerParameterType typeFilter = (AnimatorControllerParameterType) _filterIndex;
					if (typeFilter != AnimatorControllerParameterType.Bool &&
						typeFilter != AnimatorControllerParameterType.Int &&
						typeFilter != AnimatorControllerParameterType.Float &&
						typeFilter != AnimatorControllerParameterType.Trigger)
					{
						typeFilter = AnimatorControllerParameterType.Bool;
					}
					typeFilter = (AnimatorControllerParameterType) EditorGUILayout.EnumPopup("Filter", (System.Enum) typeFilter);
					_filterIndex = (int) typeFilter;
					break;
			}
			EditorPrefs.SetInt(KeyFilterIndex, _filterIndex);
		}

		/// <summary>
		/// Get an index that represents the state in the broadcaster state arrays. Comparison is made by Guid.
		/// </summary>
		/// <param name="state">
		/// The state to look for.
		/// </param>
		private int GetStateIndex(SerializableStateReference state)
		{
			if (state != null)
			{
				for (int i = 1; i < _broadcasterStates.Length; i++)
				{
					if (_broadcasterStates[i].Guid == state.Guid)
						return i;
				}
			}
			// Couldn't find the state, default to "Unassigned"
			return 0;
		}

		/// <summary>
		/// Get an index that represents the parameter in the receiver param arrays. Comparison is made by name. 
		/// </summary>
		/// <param name="parameter">
		/// The parameter to look for.
		/// </param>
		private int GetParamIndex(SerializableAnimatorControllerParameter parameter)
		{
			if (parameter != null)
			{
				for (int i = 1; i < _receiverParameterNames.Length; i++)
				{
					if (_receiverParameterNames[i] == parameter.Name)
						return i;
				}
			}
			// Couldn't find the parameter, default to "Unassigned"
			return 0;
		}

		private bool IncludedInFilter(StateBinding stateBinding, int stateIndex, int parameterIndex)
		{
			switch (_currentFilter)
			{
				case Filter.StateEntered:
					return stateIndex == _filterIndex;
				case Filter.ParameterName:
					return parameterIndex == _filterIndex;
				case Filter.ParameterType:
					return (stateBinding.Parameter.Type == (AnimatorControllerParameterType) _filterIndex);
				default:
					return true;
			}
		}

		private void DrawStateBinding(StateBinding stateBinding, int stateIndex, int parameterIndex)
		{
			GUILayout.BeginVertical("Box");
			GUI.backgroundColor = _defaultBackgroundColor;

			// State GUI
			var selectedStateIndex = EditorGUILayout.Popup("State Entered", stateIndex, _broadcasterStateNames);

			// Param GUI
			var selectedParameterIndex = EditorGUILayout.Popup("Parameter", parameterIndex, _receiverParameterNames);
			DrawParameter(stateBinding.Parameter);

			// Delay GUI
			GUI.enabled = stateBinding.Parameter.Name != Unassigned;
			var isDelayed = EditorGUILayout.Toggle("Is Delayed", stateBinding.IsDelayed);
			GUI.enabled = stateBinding.Parameter.Name != Unassigned && stateBinding.IsDelayed;
			var delayTime = EditorGUILayout.DelayedFloatField("Seconds Delayed", stateBinding.DelayTime);
			GUI.enabled = true;

			// Apply State Changes
			if (selectedStateIndex != stateIndex)
			{
				MarkDirty();
				stateBinding.StateEntered = _broadcasterStates[selectedStateIndex];
				// Bump to the first valid parameter when setting a state
				if (selectedParameterIndex == 0 && _receiverParameters.Length > 1)
				{
					selectedParameterIndex = 1;
				}
			}

			// Apply Param Changes
			if (selectedParameterIndex != parameterIndex)
			{
				MarkDirty();
				stateBinding.Parameter = new SerializableAnimatorControllerParameter(_receiverParameters[selectedParameterIndex]);
			}

			// Apply Delay Changes
			if (isDelayed != stateBinding.IsDelayed)
			{
				MarkDirty();
				stateBinding.IsDelayed = isDelayed;
			}
			if (delayTime != stateBinding.DelayTime)
			{
				MarkDirty();
				stateBinding.DelayTime = delayTime;
			}

			GUILayout.EndVertical();
		}

		private void DrawParameter(SerializableAnimatorControllerParameter parameter)
		{
			if (parameter.Name != Unassigned)
			{
				switch (parameter.Type)
				{
					case AnimatorControllerParameterType.Bool:
						{
							var newValue = EditorGUILayout.Toggle("Bool:", parameter.BoolValue);
							if (parameter.BoolValue != newValue)
							{
								MarkDirty();
								parameter.BoolValue = newValue;
							}
							break;
						}
					case AnimatorControllerParameterType.Trigger:
						{
							var newValue = EditorGUILayout.Toggle("Trigger:", parameter.BoolValue);
							if (parameter.BoolValue != newValue)
							{
								MarkDirty();
								parameter.BoolValue = newValue;
							}
							break;
						}
					case AnimatorControllerParameterType.Int:
						{
							var newValue = EditorGUILayout.IntField("Int:", parameter.IntValue);
							if (parameter.IntValue != newValue)
							{
								MarkDirty();
								parameter.IntValue = newValue;
							}
							break;
						}
					case AnimatorControllerParameterType.Float:
						{
							var newValue = EditorGUILayout.FloatField("Float", parameter.FloatValue);
							if (parameter.FloatValue != newValue)
							{
								MarkDirty();
								parameter.FloatValue = newValue;
							}
							break;
						}
				}
			}
		}

		private static GUIContent DuplicateGC = new GUIContent("Duplicate");
		private static GUIContent DuplicateAcrossGC = new GUIContent("Duplicate to other states");
		private static GUIContent DeleteGC = new GUIContent("Delete");

		private void ShowBindingItemContextMenu(int index)
		{
			if (0 <= index && index < _stateBinder.StateBindings.Count)
			{
				var contextMenu = new GenericMenu();
				var binding = _stateBinder.StateBindings[index];

				contextMenu.AddItem(DuplicateGC, false, ContextDuplicate, index);
				contextMenu.AddItem(DuplicateAcrossGC, false, ContextDuplicateAcross, index);
				contextMenu.AddItem(DeleteGC, false, ContextDelete, index);
				contextMenu.ShowAsContext();
			}
		}

		private void ContextDuplicate(object indexObj)
		{
			int index = (int) indexObj;
			if (0 <= index && index < _stateBinder.StateBindings.Count)
			{
				MarkDirty();
				_stateBinder.StateBindings.Insert(index + 1, new StateBinding(_stateBinder.StateBindings[index]));
			}
		}

		private void ContextDuplicateAcross(object indexObj)
		{
			int index = (int) indexObj;
			if (_broadcasterStates.Length > 2 && 0 <= index && index < _stateBinder.StateBindings.Count)
			{
				MarkDirty();
				var original = _stateBinder.StateBindings[index];
				int originalStateIndex = GetStateIndex(original.StateEntered);
				for (int i = 1; i < _broadcasterStates.Length; i++)
				{
					if (i != originalStateIndex)
					{
						var duplicate = new StateBinding(original);
						duplicate.StateEntered = _broadcasterStates[i];
						_stateBinder.StateBindings.Add(duplicate);
					}
				}
			}
		}

		private void ContextDelete(object indexObj)
		{
			int index = (int) indexObj;
			if (0 <= index && index < _stateBinder.StateBindings.Count)
			{
				MarkDirty();
				_stateBinder.StateBindings.RemoveAt(index);
			}
		}

		private void DrawNewBindingButton()
		{
			if (GUILayout.Button("New") && Event.current.button == 0) // Only on left click
			{
				MarkDirty();
				var newBinding = new StateBinding();
				switch (_currentFilter)
				{
					case Filter.StateEntered:
						newBinding.StateEntered = new SerializableStateReference(_broadcasterStates[_filterIndex]);
						break;
					case Filter.ParameterName:
						newBinding.Parameter = new SerializableAnimatorControllerParameter(_receiverParameters[_filterIndex]);
						break;
					case Filter.ParameterType:
						newBinding.Parameter = new SerializableAnimatorControllerParameter(type: (AnimatorControllerParameterType) _filterIndex);
						break;
				}
				_stateBinder.StateBindings.Add(newBinding);
			}
		}

		private void DrawMassNewButton()
		{
			if (GUILayout.Button("Mass New") && Event.current.button == 0) // Only on left click
			{
				MarkDirty();
				var newBinding = new StateBinding();
				switch (_currentFilter)
				{
					case Filter.ShowAll:
						for (int i = 1; i < _broadcasterStates.Length; i++)
						{
							newBinding.StateEntered = _broadcasterStates[i];
							for (int j = 1; j < _receiverParameters.Length; j++)
							{
								newBinding.Parameter = _receiverParameters[j];
								_stateBinder.StateBindings.Add(new StateBinding(newBinding));
							}
						}
						break;
					case Filter.StateEntered:
						newBinding.StateEntered = _broadcasterStates[_filterIndex];
						for (int i = 1; i < _receiverParameters.Length; i++)
						{
							newBinding.Parameter = _receiverParameters[i];
							_stateBinder.StateBindings.Add(new StateBinding(newBinding));
						}
						break;
					case Filter.ParameterName:
						newBinding.Parameter = _receiverParameters[_filterIndex];
						for (int i = 1; i < _broadcasterStates.Length; i++)
						{
							newBinding.StateEntered = _broadcasterStates[i];
							_stateBinder.StateBindings.Add(new StateBinding(newBinding));
						}
						break;
					case Filter.ParameterType:
						for (int i = 1; i < _receiverParameters.Length; i++)
						{
							if (_receiverParameters[i].Type == (AnimatorControllerParameterType) _filterIndex)
							{
								newBinding.Parameter = _receiverParameters[i];
								for (int j = 1; j < _broadcasterStates.Length; j++)
								{
									newBinding.StateEntered = _broadcasterStates[j];
									_stateBinder.StateBindings.Add(new StateBinding(newBinding));
								}
							}
						}
						break;
				}
			}
		}

		private void DrawMassDeleteButton()
		{
			if (GUILayout.Button("Mass Delete") && Event.current.button == 0) // Only on left click
			{
				MarkDirty();
				switch (_currentFilter)
				{
					case Filter.ShowAll:
						_stateBinder.StateBindings.Clear();
						break;
					case Filter.StateEntered:
						// Remove all states that have the same "State Index" as the filter
						for (int i = 0; i < _stateBinder.StateBindings.Count; i++)
						{
							var stateIndex = GetStateIndex(_stateBinder.StateBindings[i].StateEntered);
							if (stateIndex == _filterIndex)
							{
								_stateBinder.StateBindings.RemoveAt(i--);
							}
						}
						break;
					case Filter.ParameterName:
						// Remove all parameters that have the same "Parameter Index" as the filter
						for (int i = 0; i < _stateBinder.StateBindings.Count; i++)
						{
							var paramIndex = GetParamIndex(_stateBinder.StateBindings[i].Parameter);
							if (paramIndex == _filterIndex)
							{
								_stateBinder.StateBindings.RemoveAt(i--);
							}
						}
						break;
					case Filter.ParameterType:
						// Remove all parameters of the filtered type
						for (int i = 0; i < _stateBinder.StateBindings.Count; i++)
						{
							if ((int) _stateBinder.StateBindings[i].Parameter.Type == _filterIndex)
							{
								_stateBinder.StateBindings.RemoveAt(i--);
							}
						}
						break;
				}
			}
		}

		private void DrawSortButton()
		{
			if (GUILayout.Button("Sort"))
			{
				var sortMenu = new GenericMenu();
				sortMenu.AddItem(SortByStateGC, false, ContextSortStateName);
				sortMenu.AddItem(SortByParamNameGC, false, ContextSortParamName);
				sortMenu.AddItem(SortByParamTypeGC, false, ContextSortParamType);
				sortMenu.ShowAsContext();
			}
		}

		private static GUIContent SortByStateGC = new GUIContent("State Name");
		private static GUIContent SortByParamNameGC = new GUIContent("Parameter Name");
		private static GUIContent SortByParamTypeGC = new GUIContent("Parameter type");

		private void ContextSortStateName()
		{
			if (_stateBinder.StateBindings.Count > 1)
			{
				MarkDirty();
				_stateBinder.StateBindings.Sort((bind1, bind2) => bind1.StateEntered.Name.CompareTo(bind2.StateEntered.Name));
			}
		}

		private void ContextSortParamName()
		{
			if (_stateBinder.StateBindings.Count > 1)
			{
				MarkDirty();
				_stateBinder.StateBindings.Sort((bind1, bind2) => bind1.Parameter.Name.CompareTo(bind2.Parameter.Name));
			}
		}

		private void ContextSortParamType()
		{
			if (_stateBinder.StateBindings.Count > 1)
			{
				MarkDirty();
				_stateBinder.StateBindings.Sort((bind1, bind2) => bind1.Parameter.Type.CompareTo(bind2.Parameter.Type));
			}
		}

		private void MarkDirty()
		{
			Undo.RecordObject(_stateBinder, "AnimatorStateBinder change");
		}

		private int ClampIndex(int index, int max)
		{
			return Mathf.Clamp(index, 0, max - 1);
		}
	}
}
