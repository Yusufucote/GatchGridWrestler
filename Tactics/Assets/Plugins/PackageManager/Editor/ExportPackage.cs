using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpheroPackageManager
{
	[System.Serializable]
	public class ExportPackage
	{
		private const string DefaultName = "Unnamed";
		private const string DefaultPath = "Assets/Plugins";

		[SerializeField]
		private string _name = DefaultName;
		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(_name))
					return DefaultName;
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		[SerializeField, FormerlySerializedAs("Paths")]
		private string[] _paths = new string[] { DefaultPath };
		public string[] Paths { get { return _paths; } }

		[SerializeField]
		private int _version = 0;
		public int Version { get { return _version; } }
	}
}