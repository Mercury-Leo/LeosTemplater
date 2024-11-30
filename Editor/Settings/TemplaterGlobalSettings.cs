using UnityEditor;
using UnityEngine;
using static LeosTemplater.Editor.TemplaterConfig;

namespace LeosTemplater.Editor
{
    [FilePath(TemplaterGlobalProjectSettingsPath, FilePathAttribute.Location.ProjectFolder)]
    internal sealed class TemplaterGlobalSettings : ScriptableSingleton<TemplaterGlobalSettings>
    {
        [SerializeField] private string header = string.Empty;
        [SerializeField] private string footer = string.Empty;

        public string Header
        {
            get => header;
            set
            {
                if (header == value)
                {
                    return;
                }

                header = value;
                SaveDirty();
            }
        }

        public string Footer
        {
            get => footer;
            set
            {
                if (footer == value)
                {
                    return;
                }

                footer = value;
                SaveDirty();
            }
        }

        private void SaveDirty()
        {
            Save(this);
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}