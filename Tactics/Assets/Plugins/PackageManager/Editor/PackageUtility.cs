using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpheroPackageManager
{
	public static class PackageUtility
	{
		public static readonly bool VerboseLogs = false;
		private static ExportPackageSerializableGroup _importedManifest = null;
		private static ExportPackageSerializableGroup ImportedManifest
		{
			get
			{
				if (_importedManifest == null)
					_importedManifest = ExportPackageSerializableGroup.ReadFromFile(Paths.ImportedManifestFilePath);
				return _importedManifest;
			}
		}

		public static void ExportForRemoteDeploy()
		{
			var exportPath = Application.dataPath + "/../RemoteDeploy/";
			var packages = ExportPackageSerializableGroup.ReadFromFile(Paths.ExportManifestFilePath);
			packages.Export(exportPath, true);
			File.Copy(Paths.ExportManifestFilePath, exportPath + Paths.ExportManifestFileName, true);
		}

		[MenuItem("Sphero/Package Manager/Export Packages")]
		public static void ExportLocal()
		{
			var pathToExport = string.Format("{0}/../../Packages/", Application.dataPath);
			var packagesToExport = ExportPackageSerializableGroup.ReadFromFile(Paths.ExportManifestFilePath);
			packagesToExport.Export(pathToExport, false);

			ExportPackageSerializableGroup exportedPackages = null;

			exportedPackages = ExportPackageSerializableGroup.ReadFromFile(pathToExport + Paths.ExportManifestFileName);

			exportedPackages = ExportPackageSerializableGroup.Merge(packagesToExport, exportedPackages);
			exportedPackages.WriteToFile(pathToExport, Paths.ExportManifestFileName);
		}

		public static bool ImportLocal(ExportPackage package)
		{
			string unityPackagePath = string.Empty;
			if (package != null)
			{
				unityPackagePath = Paths.GetLocalUnityPackagePath(package);
			}
			return Import(unityPackagePath, package);
		}

		public static bool Import(string unityPackageFilePath, ExportPackage package)
		{
			bool success = false;
			if (package != null)
			{
				if (File.Exists(unityPackageFilePath))
				{
					DeletePackage(package, false);
					AssetDatabase.ImportPackage(unityPackageFilePath, false);
					success = true;
					ImportedManifest.AddPackage(package);
					WriteImportedManifest();
					Debug.Log("Imported package " + package.Name);
				}
				else
				{
					Debug.LogError("Unity package does not exist: " + unityPackageFilePath);
				}

				if (!success)
				{
					Debug.LogError("Failed to import package " + package.Name);
				}
			}
			else
			{
				Debug.LogError("Cannot import null packages");
			}
			return success;
		}

		public static void DeletePackage(ExportPackage package, bool updateManifest)
		{
			var dataPath = Application.dataPath.Replace("Assets", string.Empty);
			foreach (var path in package.Paths)
			{
				var deletePath = dataPath + path;
				try
				{
					Directory.Delete(deletePath, true);
					File.Delete(deletePath + ".meta");
					if (VerboseLogs)
						Debug.Log("Deleted " + deletePath);

					if (updateManifest)
					{
						ImportedManifest.RemovePackage(package);
						WriteImportedManifest();
					}
					else
					{
						AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
					}
				}
				catch
				{
					if (VerboseLogs)
						Debug.Log("Failed to delete " + deletePath);
				}
			}
		}

		private static void WriteImportedManifest()
		{
			var manifest = ImportedManifest;
			manifest.WriteToFile(Paths.ManifestDirectory, Paths.ImportManifestFileName);
			if (VerboseLogs)
				Debug.Log(string.Format("Wrote import manifest with {0} packages", manifest.Count));
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}

		// NOTE: This is kept around to be used if/when a GUI is built for managing the package export manifests
		public static string[] GetSelectedObjectPaths()
		{
			var selectedGUIDs = Selection.assetGUIDs;
			if (selectedGUIDs != null && selectedGUIDs.Length > 0)
			{
				string[] paths = new string[selectedGUIDs.Length];
				for (int i = 0; i < selectedGUIDs.Length; i++)
				{
					paths[i] = AssetDatabase.GUIDToAssetPath(selectedGUIDs[i]);

					if (VerboseLogs)
						Debug.Log("Found selected path " + paths[i]);
				}
				return paths;
			}
			return null;
		}
	}
}
