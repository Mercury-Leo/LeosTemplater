using System.IO;
using UnityEditor;
using UnityEngine;

namespace LeosTemplater.Editor
{
    internal static class TemplaterUtility
    {
        /// <summary>
        /// Finds a script's folder
        /// </summary>
        /// <param name="scriptName"></param>
        /// <returns></returns>
        public static string FindScriptDirectory(string scriptName)
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

        /// <summary>
        /// Fixes slashes for path
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FixSlashes(this string input)
        {
            return input.Replace('\\', '/');
        }

        public static string AddPrefix(this string message)
        {
            return $"[Leo's Templater] {message}";
        }
    }
}