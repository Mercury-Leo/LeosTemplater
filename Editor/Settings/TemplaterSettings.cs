using UnityEditor;
using UnityEngine;
using static Tools.Editor.Templater.TemplaterConfig;

namespace Tools.Editor.Templater
{
    [FilePath(TemplaterProjectSettingsPath, FilePathAttribute.Location.ProjectFolder)]
    internal sealed class TemplaterSettings : ScriptableSingleton<TemplaterSettings>
    {
        [SerializeField] private string _templatesFolder;
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
    }
}