using System.IO;
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
            get => _templatesFolder ?? GetDefaultTemplatesPath();
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

        public void SetDefaultFolder()
        {
            TemplateFolder = GetDefaultTemplatesPath();
        }

        private string GetDefaultTemplatesPath()
        {
            var root = TemplaterUtility.FindScriptDirectory(nameof(TemplateGenerator));
            return Path.Combine(root, "Templates").FixSlashes();
        }
    }
}