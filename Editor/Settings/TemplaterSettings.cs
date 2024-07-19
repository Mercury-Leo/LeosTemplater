using UnityEditor;
using UnityEngine;
using static Tools.Editor.Templater.TemplaterConfig;

namespace Tools.Editor.Templater
{
    [FilePath(TemplaterProjectSettingsPath, FilePathAttribute.Location.ProjectFolder)]
    public class TemplaterSettings : ScriptableSingleton<TemplaterSettings>
    {
        [SerializeField] private string _templatesFolder;

        [SerializeField] [Tooltip("Will included at the top of the newly created file")]
        private string _header;

        [SerializeField] [Tooltip("Will included at the bottom of the newly created file")]
        private string _footer;

        public string TemplateFolder
        {
            get => _templatesFolder;
            set
            {
                if (_templatesFolder == value)
                {
                    return;
                }

                _templatesFolder = value;
                Save(true);
            }
        }

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