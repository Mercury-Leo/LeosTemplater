using UnityEditor;
using UnityEngine;
using static Tools.Editor.Templater.TemplaterConfig;

namespace Tools.Editor.Templater
{
    [FilePath(TemplaterGlobalProjectSettingsPath, FilePathAttribute.Location.ProjectFolder)]
    internal sealed class TemplaterGlobalSettings : ScriptableSingleton<TemplaterGlobalSettings>
    {
        [SerializeField] private string _header = string.Empty;

        [SerializeField] private string _footer = string.Empty;

        public string Header
        {
            get => _header;
            set
            {
                if (_header == value)
                {
                    return;
                }

                _header = value;
                Save(true);
            }
        }

        public string Footer
        {
            get => _footer;
            set
            {
                if (_footer == value)
                {
                    return;
                }

                _footer = value;
                Save(true);
            }
        }
    }
}