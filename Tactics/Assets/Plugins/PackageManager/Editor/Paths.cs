using UnityEngine;
using UnityEditor;

namespace SpheroPackageManager
{
	public static class Paths
	{
		public static string ManifestDirectory { get { return string.Format("{0}/{1}", Application.dataPath, "PluginsData/PackageManager/"); } }

		public static string ExportManifestFilePath { get { return ManifestDirectory + ExportManifestFileName; } }
		public static string ExportManifestFileName { get { return "ExportedPackages.json"; } }

		public static string ImportedManifestFilePath { get { return ManifestDirectory + ImportManifestFileName; } }
		public static string ImportManifestFileName { get { return "ImportedPackages.json"; } }

		private const string LocalPackagePathKey = "SpheroBasePackagePath";
		private static string _relativeLocalExportedPackagePath = null;
		/// <summary>
		/// The path to the packages folder, relative to the folder this project is in.
		/// </summary>
		public static string RelativeLocalExportedPackagePath
		{
			get
			{
				var path = EditorPrefs.GetString(LocalPackagePathKey);
				if (string.IsNullOrEmpty(path))
				{
					path = "SpheroPackages";
				}
				_relativeLocalExportedPackagePath = path;
				return path;
			}
			set
			{
				if (_relativeLocalExportedPackagePath != value)
				{
					_relativeLocalExportedPackagePath = value;
					EditorPrefs.SetString(LocalPackagePathKey, value);
				}
			}
		}

		public static string LocalExportedPackagePath
		{
			get
			{
				return string.Format("{0}/../../{1}/", Application.dataPath, RelativeLocalExportedPackagePath);
			}
		}

		public static string LocalExportedManifestFilePath
		{
			get
			{
				return LocalExportedPackagePath + ExportManifestFileName;
			}
		}

		public static string GetLocalUnityPackagePath(ExportPackage package)
		{
			return string.Format("{0}{1}.unitypackage", LocalExportedPackagePath, package.Name);
		}
	}
}
