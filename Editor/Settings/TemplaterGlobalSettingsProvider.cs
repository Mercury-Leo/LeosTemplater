using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static LeosTemplater.Editor.TemplaterConfig;

namespace LeosTemplater.Editor
{
    internal sealed class TemplaterGlobalSettingsProvider : SettingsProvider
    {
        private TemplaterGlobalSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null)
            : base(path, scopes, keywords) { }

        private Vector2 _headerScroller;
        private Vector2 _footerScroller;
        private string _tempHeader = TemplaterGlobalSettings.instance.Header;
        private string _tempFooter = TemplaterGlobalSettings.instance.Footer;

        private const float Width = 500f;
        private static readonly GUILayoutOption GUIWidth = GUILayout.Width(Width);

        public override void OnGUI(string searchContext)
        {
            base.OnGUI(searchContext);

            GUILayout.Space(20);

            GUILayout.Label("The text here will appear, commented out, at the top of scripts created via Template",
                EditorStyles.boldLabel);
            AddTextArea(ref _tempHeader, ref _headerScroller);

            GUILayout.Space(20);

            GUILayout.Label("The text here will appear, commented out, at the bottom  of scripts created via Template",
                EditorStyles.boldLabel);
            AddTextArea(ref _tempFooter, ref _footerScroller);

            GUILayout.Space(20);

            if (GUILayout.Button("Save", GUIWidth))
            {
                TemplaterGlobalSettings.instance.Header = _tempHeader;
                TemplaterGlobalSettings.instance.Footer = _tempFooter;
            }
        }

        private static void AddTextArea(ref string temp, ref Vector2 scroller)
        {
            scroller = GUILayout.BeginScrollView(scroller, GUIWidth, GUILayout.Height(200));

            temp = GUILayout.TextArea(temp, GUILayout.ExpandHeight(true),
                GUILayout.ExpandWidth(true),
                GUILayout.MaxHeight(400), GUILayout.MaxWidth(Width));

            GUILayout.EndScrollView();
        }

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            return new TemplaterGlobalSettingsProvider(TemplaterPath, SettingsScope.Project);
        }
    }
}