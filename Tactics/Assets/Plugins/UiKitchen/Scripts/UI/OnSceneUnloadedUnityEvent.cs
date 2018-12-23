using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace UiKitchen
{
	public class OnSceneUnloadedUnityEvent : MonoBehaviour
	{
		[SerializeField]
		private string _sceneName;

		[SerializeField]
		private bool _triggerOnAwakeIfNotLoaded = true;

		[SerializeField]
		private bool _gcCollectOnUnload = true;

		[SerializeField]
		private float _secondsDelay = 0f;

		[SerializeField]
		UnityEvent _onSceneUnloaded;

		void OnEnable()
		{
			SceneManager.sceneUnloaded -= SceneUnloaded;
			SceneManager.sceneUnloaded += SceneUnloaded;

			if (_triggerOnAwakeIfNotLoaded)
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

				if (!foundScene)
					_onSceneUnloaded.Invoke();
			}
		}

		void OnDisable()
		{
			SceneManager.sceneUnloaded -= SceneUnloaded;
		}

		private void SceneUnloaded(Scene arg0)
		{
			if (arg0.name == _sceneName)
			{
				if (_gcCollectOnUnload)
				{
					Resources.UnloadUnusedAssets();
					GC.Collect();
				}

				if (_secondsDelay > 0f)
				{
					StartCoroutine(FireEventAfterDelay());
				}
				else
				{
					_onSceneUnloaded.Invoke();
				}
			}
		}

		private IEnumerator FireEventAfterDelay()
		{
			yield return new WaitForSecondsRealtime(_secondsDelay);
			_onSceneUnloaded.Invoke();
		}
	}
}
