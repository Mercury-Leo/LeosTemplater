using System.IO;
using UnityEditor;
using UnityEngine;
using static LeosTemplater.Editor.TemplaterConfig;

#nullable enable
namespace LeosTemplater.Editor
{
    [FilePath(TemplaterProjectSettingsPath, FilePathAttribute.Location.ProjectFolder)]
    internal sealed class TemplaterSettings : ScriptableSingleton<TemplaterSettings>
    {
        [SerializeField] private string? templatesFolder;

        private const string DefaultTemplatesFolder = "Templates";

        private void Awake()
        {
            SetDefaultFolder();
        }

        public string TemplateFolder
        {
            get => templatesFolder ?? GetDefaultTemplatesPath();
            set
            {
                if (templatesFolder == value)
                {
                    return;
                }

                templatesFolder = value;
                SaveDirty();
            }
        }

        public void SetDefaultFolder()
        {
            if (templatesFolder is null)
            {
                TemplateFolder = GetDefaultTemplatesPath();
            }
        }

        private string GetDefaultTemplatesPath()
        {
            var root = TemplaterUtility.FindScriptDirectory(nameof(TemplateGenerator));
            return Path.Combine(root, DefaultTemplatesFolder).FixSlashes();
        }

        private void SaveDirty()
        {
            Save(this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}