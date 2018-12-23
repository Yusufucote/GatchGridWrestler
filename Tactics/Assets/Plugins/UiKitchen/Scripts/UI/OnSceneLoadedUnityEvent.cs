using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UiKitchen
{
	public class OnSceneLoadedUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private string _sceneName;
		[SerializeField]
		private bool _triggerOnAwakeIfLoaded = true;
		[SerializeField]
		UnityEvent _onSceneLoaded;
		void OnEnable()
		{
			SceneManager.sceneLoaded -= SceneLoaded;
			SceneManager.sceneLoaded += SceneLoaded;

			if (_triggerOnAwakeIfLoaded)
			{
				bool foundScene = false;
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					if (SceneManager.GetSceneAt(i).name == _sceneName)
					{
						foundScene = true;
						break;
					}
				}

				if (foundScene)
					_onSceneLoaded.Invoke();
			}
		}

		void OnDisable()
		{
			SceneManager.sceneLoaded -= SceneLoaded;
		}

		private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			if (arg0.name == _sceneName)
			{
				_onSceneLoaded.Invoke();
			}
		}
	}
}
