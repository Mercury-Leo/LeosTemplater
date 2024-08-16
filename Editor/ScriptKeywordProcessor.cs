using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tools.Editor.Templater;
using UnityEditor;
using UnityEngine;

namespace Tools.Editor.Template
{
    /// <summary>
    /// Processes scripts created by the Templater
    /// Injects namespace if exist
    /// </summary>
    internal sealed class ScriptKeywordProcessor : AssetModificationProcessor
    {
        private static readonly char[] NamespaceSplitters = { '/', '\\', '.' };
        private const string NamespaceMarker = "#NAMESPACE#";
        private const string ClassFileExtension = ".cs";
        private const string MetaFileExtension = ".meta";
        private const string Assets = "Assets";
        private const string Dot = ".";
        private const string DefaultNamespace = "Global";

        public static void OnWillCreateAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            // Waits for the meta file to be created to be sure the script exists
            if (!path.EndsWith(MetaFileExtension))
            {
                return;
            }

            path = path.Replace(MetaFileExtension, string.Empty);

            var index = path.LastIndexOf(Dot, StringComparison.Ordinal);
            if (index < 0)
            {
                return;
            }

            var file = path[index..];
            if (file != ClassFileExtension)
            {
                return;
            }

            var namespaces = path.Split(NamespaceSplitters).ToList();
            namespaces = namespaces.GetRange(1, namespaces.Count - NamespaceSplitters.Length);

            var namespaceString = DefaultNamespace;
            for (var i = 0; i < namespaces.Count; i++)
            {
                if (i == 0)
                {
                    namespaceString = string.Empty;
                }

                namespaceString += namespaces[i];
                if (i < namespaces.Count - 1)
                {
                    namespaceString += Dot;
                }
            }

            index = Application.dataPath.LastIndexOf(Assets, StringComparison.Ordinal);
            path = Application.dataPath[..index] + path;
            if (!System.IO.File.Exists(path))
            {
                return;
            }

            var fileContent = System.IO.File.ReadAllText(path);
            fileContent = fileContent.Replace(NamespaceMarker, namespaceString);
            System.IO.File.WriteAllText(path, AttachSettingsContent(fileContent));
        }

        /// <summary>
        /// Creates file content with Header and Footer from the TemplaterGlobalSetting
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static string AttachSettingsContent(string content)
        {
            var builder = new StringBuilder();

            if (!string.IsNullOrEmpty(TemplaterGlobalSettings.instance.Header))
            {
                builder.AppendLine("/*");
                builder.AppendLine(TemplaterGlobalSettings.instance.Header);
                builder.AppendLine("*/");
            }

            builder.AppendLine();
            builder.AppendLine(content);
            builder.AppendLine();

            if (!string.IsNullOrEmpty(TemplaterGlobalSettings.instance.Footer))
            {
                builder.AppendLine("/*");
                builder.AppendLine(TemplaterGlobalSettings.instance.Footer);
                builder.AppendLine("*/");
            }

            return builder.ToString();
        }
    }
}