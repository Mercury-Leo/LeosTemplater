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
        private static readonly char[] Splitters = { '/', '\\', '.' };
        private static readonly List<string> WordsToDelete = new List<string>() { "Extensions", "Scripts", "Editor" };
        private const string NameSpace = "#NAMESPACE#";
        private const string FileExtensions = ".cs";
        private const string Assets = "Assets";
        private const string Dot = ".";
        private const string Meta = ".meta";
        private const string DefaultNameSpace = "Globals";

        public static void OnWillCreateAsset(string path)
        {
            path = path.Replace(Meta, string.Empty);
            var index = path.LastIndexOf(Dot, StringComparison.Ordinal);
            if (index < 0)
            {
                return;
            }

            var file = path[index..];
            if (file != FileExtensions)
            {
                return;
            }

            var namespaces = path.Split(Splitters).ToList();
            namespaces = namespaces.GetRange(1, namespaces.Count - 3);
            namespaces = namespaces.Except(WordsToDelete).ToList();

            var namespaceString = DefaultNameSpace;
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
            fileContent = fileContent.Replace(NameSpace, namespaceString);
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