using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace SpheroPackageManager
{
	public class PackageManagerWindow : EditorWindow
	{
		private Vector2 _scrollbar;
		private readonly int _padSize = 5;
		private readonly string _packageNameVersionFmt = "{0}\tv.{1}";
		private static ExportPackageSerializableGroup _exportedPackages = null;
		private static ExportPackageSerializableGroup _importedPackages = null;
		private static List<string> _selectedPackages = null;
		private static List<string> _packagesForExport = null;

		[MenuItem("Sphero/Package Manager/Manage Packages")]
		private static void OpenWindow()
		{
			EditorWindow.GetWindow<PackageManagerWindow>("Package Manager");
		}

		private void OnGUI()
		{
			_scrollbar = GUILayout.BeginScrollView(_scrollbar, false, true);
			GUILayout.Label(new GUIContent("Relative path to local packages:", "Path is relative to the folder containing this project"));
			Paths.RelativeLocalExportedPackagePath = GUILayout.TextField(Paths.RelativeLocalExportedPackagePath);

			RefreshPackages();

			DrawImportedPackages();

			DrawLocalPackages();

			if (_packagesForExport.Count > 0)
			{
				DrawPackagesForExport();
			}


			GUILayout.EndScrollView();
		}

		private void DrawHeader(string label)
		{
			GUILayout.Space(_padSize);
			GUILayout.Label(label);
		}

		private void DrawImportedPackages()
		{
			DrawHeader("Imported Packages");

			if (_importedPackages != null && _importedPackages.Count > 0)
			{
				var packages = _importedPackages.Packages;
				for (int i = 0; i < packages.Count; i++)
				{
					var package = packages[i];
					GUILayout.BeginHorizontal();
					GUILayout.Label(string.Format(_packageNameVersionFmt, package.Name, package.Version));
					if (package.Name != "PackageManager" && !_packagesForExport.Contains(package.Name) && GUILayout.Button("Delete"))
					{
						PackageUtility.DeletePackage(package, true);
						RefreshPackages(true);
						i--;
					}
					GUILayout.EndHorizontal();
				}
			}
			else
			{
				GUILayout.Label("No imported packages");
			}
		}

		private void DrawLocalPackages()
		{
			DrawHeader("Available Packages");

			if (_exportedPackages != null && _exportedPackages.Count > 0)
			{
				var packages = _exportedPackages.Packages;
				for (int i = 0; i < packages.Count; i++)
				{
					var package = packages[i];
					if (!_packagesForExport.Contains(package.Name))
					{
						bool wasSelected = _selectedPackages.Contains(package.Name);
						bool isSelected = GUILayout.Toggle(wasSelected, string.Format(_packageNameVersionFmt, package.Name, package.Version));
						if (isSelected != wasSelected)
						{
							if (isSelected)
							{
								_selectedPackages.Add(package.Name);
							}
							else
							{
								_selectedPackages.Remove(package.Name);
							}
						}
					}
				}
			}
			else
			{
				GUILayout.Label("No local packages found");
			}

			GUILayout.Space(_padSize);
			if (GUILayout.Button("Import Selected Packages"))
			{
				ImportSelectedPackages();
			}
			if (GUILayout.Button("Clear Selection"))
			{
				_selectedPackages.Clear();
			}
		}

		private void ImportSelectedPackages()
		{
			foreach (var package in _exportedPackages.Packages)
			{
				if (_selectedPackages.Contains(package.Name))
				{
					PackageUtility.ImportLocal(package);
				}
			}
		}


		private void DrawPackagesForExport()
		{
			DrawHeader("Developing Packages");

			foreach (var packageName in _packagesForExport)
			{
				GUILayout.Label(packageName);
			}

			if (GUILayout.Button("Export Packages"))
			{
				PackageUtility.ExportLocal();
			}
		}

		private static void RefreshPackages(bool forced = false)
		{
			if (forced || _exportedPackages == null)
			{
				_exportedPackages = ExportPackageSerializableGroup.ReadFromFile(Paths.LocalExportedManifestFilePath);
			}

			if (forced || _importedPackages == null || _selectedPackages == null)
			{
				_importedPackages = ExportPackageSerializableGroup.ReadFromFile(Paths.ImportedManifestFilePath);
				_selectedPackages = _importedPackages.GetPackageNames();
			}

			if (forced || _packagesForExport == null)
			{
				var packagesForExport = ExportPackageSerializableGroup.ReadFromFile(Paths.ExportManifestFilePath);
				var packages = packagesForExport.Packages;
				_packagesForExport = new List<string>(packages.Count);
				foreach (var package in packages)
				{
					_packagesForExport.Add(package.Name);
				}
			}
		}
	}
}
