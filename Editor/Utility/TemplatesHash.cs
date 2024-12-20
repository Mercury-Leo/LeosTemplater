using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;

#nullable enable
namespace LeosTemplater.Editor
{
    /// <summary>
    /// Creates a hash of the current templates.
    /// </summary>
    internal static class TemplatesHash
    {
        private static string? _hashFilePath;

        private const string TemplatesHashKey = nameof(TemplatesHashKey);

        public static string HashFiles(IEnumerable<string> files)
        {
            using var md5 = MD5.Create();
            foreach (var file in files)
            {
                var content = File.ReadAllBytes(file);
                var pathBytes = Encoding.UTF8.GetBytes(file);
                md5.TransformBlock(content, 0, content.Length, content, 0);
                md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);
            }

            md5.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
            return BitConverter.ToString(md5.Hash).Replace("-", string.Empty).ToLowerInvariant();
        }

        public static void SetHash(string hash)
        {
            EditorPrefs.SetString(TemplatesHashKey, hash);
        }

        public static string GetHash()
        {
            return EditorPrefs.HasKey(TemplatesHashKey) ? EditorPrefs.GetString(TemplatesHashKey) : string.Empty;
        }
    }
}