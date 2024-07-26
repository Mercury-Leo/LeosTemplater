using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tools.Editor.Templater;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class TemplateGenerator
{
    private static string _menuItemsPath;
    private static readonly string MenuItemsPath = _menuItemsPath ?? GetMenuItemsPath();

    private const string MenuItemsClassName = "TemplatesMenuItems.cs";
    private const string CreationPath = "Assets/Create/Templates/";

    private const string Header =
        @"//Auto generated Code, do not change!
//Generated by Leo's Templater 
using System.IO;
using UnityEditor;
    
namespace Tools.Editor.Templater
{
    internal sealed class TemplateMenuItems
    {
";

    private const string Footer = @"
    }
}";

    static TemplateGenerator()
    {
        EditorApplication.projectChanged += OnProjectChanged;
    }

    private static string GetMenuItemsPath()
    {
        var packagePath = TemplaterUtility.FindScriptDirectory(nameof(TemplateGenerator));
        if (string.IsNullOrEmpty(packagePath))
        {
            Debug.LogError("Failed to find package path");
            return Application.dataPath;
        }

        return Path.Combine(packagePath, MenuItemsClassName).FixSlashes();
    }

    /// <summary>
    /// Regenerates the MenuItem file
    /// </summary>
    public static void Regenerate()
    {
        GenerateMenuItems(GetAllTemplates());
    }

    private static void OnProjectChanged()
    {
        var files = GetAllTemplates().ToHashSet();

        if (!File.Exists(MenuItemsPath))
        {
            GenerateMenuItems(files);
        }

        if (TemplatesUpdated(files))
        {
            GenerateMenuItems(files);
        }
    }

    private static void GenerateMenuItems(IEnumerable<string> files)
    {
        var builder = new StringBuilder();

        builder.Append(Header);

        foreach (var file in files)
        {
            builder.AppendLine(GenerateMenuItemCode(file));
        }

        builder.Append(Footer);

        try
        {
            File.WriteAllText(MenuItemsPath, builder.ToString());
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {MenuItemsPath}: {e}");
        }

        if (File.Exists(MenuItemsPath))
        {
            AssetDatabase.ImportAsset(MenuItemsPath, ImportAssetOptions.ForceUpdate);
        }
    }

    private static string GenerateMenuItemCode(string filePath, int priority = 40)
    {
        var path = filePath.FixSlashes();
        var templateFolder = TemplaterSettings.instance.TemplateFolder;
        var folderIndex = path.IndexOf(templateFolder, StringComparison.Ordinal);

        if (folderIndex < 0)
        {
            return string.Empty;
        }

        // Finds the relative path of the template, used for templates inside folders
        var relativePath = path[(folderIndex + templateFolder.Length)..].TrimStart('/', '\\');

        // Removes all the extensions as templates come as .cs.txt
        while (Path.HasExtension(relativePath))
        {
            relativePath = Path.ChangeExtension(relativePath, null);
        }

        // Used for menuitem name 
        var templatePath = relativePath.TrimEnd('.').Replace(" ", string.Empty);

        // Used for function name (Templates/someFolder/SomeTemplate.cs.txt = someFolderSomeTemplate)
        var itemName = templatePath.Replace("/", string.Empty).Replace("\\", string.Empty);
        var itemClass = $"{itemName}Class.cs";

        return $@"
        [MenuItem(""{CreationPath}{templatePath}"", priority = {priority})]
        public static void Create{itemName}MenuItem()
        {{
            if (!File.Exists(""{path}""))
            {{
                return;
            }}
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(""{path}"", ""{itemClass}"");
        }}";
    }

    /// <summary>
    /// If Templates hash has changed set the new hash
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    private static bool TemplatesUpdated(IEnumerable<string> files)
    {
        var currentHash = TemplatesHash.HashFiles(files);
        var storedHash = TemplatesHash.GetHash();

        var hasUpdated = currentHash != storedHash;
        if (hasUpdated)
        {
            TemplatesHash.SetHash(currentHash);
        }

        return hasUpdated;
    }

    private static IEnumerable<string> GetAllTemplates()
    {
        return string.IsNullOrEmpty(TemplaterSettings.instance.TemplateFolder)
            ? Enumerable.Empty<string>()
            : Directory.GetFiles(TemplaterSettings.instance.TemplateFolder, "*.txt", SearchOption.AllDirectories);
    }
}