using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Tools.Editor.Templater.TemplaterConfig;

namespace Tools.Editor.Templater
{
    internal sealed class TemplaterSettingsProvider : SettingsProvider
    {
        private TemplaterSettingsProvider(string path, SettingsScope scopes,
            IEnumerable<string> keywords = null) : base(path, scopes, keywords) { }

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            GUILayout.Space(20);

            var templatesFolder = TemplaterSettings.instance.TemplateFolder;

            EditorGUILayout.BeginHorizontal();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Selected Templates folder", templatesFolder);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("...", GUILayout.Width(25)))
            {
                var selectedFolder = EditorUtility.OpenFolderPanel("Select the Templates folder",
                    TemplaterSettings.instance.TemplateFolder, string.Empty);
                if (string.IsNullOrEmpty(selectedFolder))
                {
                    return;
                }

                TemplaterSettings.instance.TemplateFolder = selectedFolder;
                Repaint();
            }

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Regenerate templates"))
            {
                TemplateGenerator.Regenerate();
            }

            GUILayout.Space(20);

            if (GUILayout.Button("Set to Default Templates"))
            {
                TemplaterSettings.instance.SetDefaultFolder();
            }
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new TemplaterSettingsProvider(TemplaterPath, SettingsScope.User);
        }
    }
}