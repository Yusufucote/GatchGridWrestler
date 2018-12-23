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
	public class ExportPackageSerializableGroup
	{
		[SerializeField, FormerlySerializedAs("Packages")]
		private List<ExportPackage> _packages = new List<ExportPackage>();
		public ReadOnlyCollection<ExportPackage> Packages { get { return new ReadOnlyCollection<ExportPackage>(_packages); } }

		public int Count { get { return _packages.Count; } }

		public List<string> GetPackageNames()
		{
			var names = new List<string>(_packages.Count);
			for (int i = 0; i < _packages.Count; i++)
			{
				names.Add(_packages[i].Name);
			}
			return names;
		}

		public void AddPackage(ExportPackage package)
		{
			if (package != null && !string.IsNullOrEmpty(package.Name))
			{
				for (int i = 0; i < _packages.Count; i++)
				{
					var groupPackage = _packages[i];
					if (groupPackage.Name == package.Name)
					{
						_packages.RemoveAt(i--);
						groupPackage = null;
					}

					if (groupPackage != null)
					{
						for (int j = 0; j < package.Paths.Length; j++)
						{
							for (int k = 0; k < groupPackage.Paths.Length; k++)
							{
								if (package.Paths[j] == groupPackage.Paths[k])
								{
									Debug.LogError(string.Format("Error: Packages {0} and {1} both contain the same path: {2}\nThis will result in unpredictable behavior on import.", package.Name, groupPackage.Name, package.Paths[j]));
								}
							}
						}
					}
				}

				_packages.Add(package);
			}
		}

		public void RemovePackage(ExportPackage package)
		{
			if (package != null && !string.IsNullOrEmpty(package.Name))
			{
				for (int i = 0; i < _packages.Count; i++)
				{
					if (_packages[i].Name == package.Name)
					{
						_packages.RemoveAt(i--);
						break;
					}
				}
			}
		}

		public static ExportPackageSerializableGroup ReadFromFile(string filePath)
		{
			if (PackageUtility.VerboseLogs)
				Debug.Log("Reading export package from " + filePath);

			string json = null;
			ExportPackageSerializableGroup packages = null;
			try
			{
				if (File.Exists(filePath))
				{
					json = File.ReadAllText(filePath);

					if (PackageUtility.VerboseLogs)
					{
						Debug.Log("Read json " + json);
					}

					packages = JsonUtility.FromJson<ExportPackageSerializableGroup>(json);
					if (PackageUtility.VerboseLogs && packages != null)
					{
						Debug.Log("Read packages " + packages.Count);
					}
				}
				else
				{
					if (PackageUtility.VerboseLogs)
						Debug.Log("Export package file does not exist, creating new.");
					packages = new ExportPackageSerializableGroup();
				}

			}
			catch (Exception ex)
			{
				packages = new ExportPackageSerializableGroup();

				if (PackageUtility.VerboseLogs)
					Debug.LogError("Failed to read export package with exception " + ex.Message);
			}

			return packages;
		}

		public void WriteToFile(string path, string filename)
		{
			var json = JsonUtility.ToJson(this, true);
			var filePath = path + filename;

			if (PackageUtility.VerboseLogs)
			{
				Debug.Log("Exporting to " + filePath);
				Debug.Log("Exporting data " + json);
			}

			Directory.CreateDirectory(path);
			File.WriteAllText(filePath, json);
			AssetDatabase.Refresh();
		}

		public void Export(string exportPath, bool commandLineExport)
		{
			for (int i = 0; i < _packages.Count; i++)
			{
				var package = _packages[i];
				var exportFilename = string.Format("{0}{1}.unitypackage", exportPath, package.Name);

				if (PackageUtility.VerboseLogs)
				{
					string assetPaths = string.Empty;
					for (int j = 0; j < package.Paths.Length; j++)
					{
						assetPaths += package.Paths[j] + "\n";
					}
					Debug.Log(string.Format("Exporting UnityPackage {0} with asset paths:\n{1}Package written to: {2}", package.Name, assetPaths, exportFilename));
				}
				Directory.CreateDirectory(exportPath);
				try
				{
					var exportOptions = ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies;
					if (commandLineExport)
					{
						AssetDatabase.ExportPackage(package.Paths, exportFilename, exportOptions);
						Debug.Log("Exported package to " + exportFilename);
					}
					else
					{
						AssetDatabase.ExportPackage(package.Paths, exportFilename, exportOptions | ExportPackageOptions.Interactive);
					}
				}
				catch (Exception ex)
				{
					string pathLog = string.Empty;
					foreach (var path in package.Paths)
					{
						pathLog += path + "\n";
					}
					Debug.LogError(string.Format("Failed to export {0} with file paths:\n{1}Error: {2}", package.Name, pathLog, ex.Message));
				}
			}
		}

		public static ExportPackageSerializableGroup Merge(params ExportPackageSerializableGroup[] groups)
		{
			ExportPackageSerializableGroup mergedGroup = null;

			if (groups != null && groups.Length > 0)
			{
				mergedGroup = new ExportPackageSerializableGroup();
				mergedGroup._packages = new List<ExportPackage>(groups[0]._packages);

				for (int i = 1; i < groups.Length; i++)
				{
					var group = groups[i];
					for (int j = 0; j < group._packages.Count; j++)
					{
						mergedGroup.AddPackage(group._packages[j]);
					}
				}
			}

			return mergedGroup;
		}
	}
}
