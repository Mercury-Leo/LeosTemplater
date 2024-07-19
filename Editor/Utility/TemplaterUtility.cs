using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor.Templater
{
    public static class TemplaterUtility
    {
        public static string FindPackageFolder(string scriptName)
        {
            var scriptGuids = AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(scriptName));

            if (scriptGuids.Length == 0)
            {
                Debug.LogError("Failed to find package directory");
                return null;
            }

            var path = AssetDatabase.GUIDToAssetPath(scriptGuids[0]);
            return Path.GetDirectoryName(path);
        }

        public static string FixSlashes(this string input)
        {
            return input.Replace('\\', '/');
        }
    }
}