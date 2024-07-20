using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Tools.Editor.Templater
{
    internal static class TemplatesHash
    {
        private static string _hashFilePath;
        private static readonly string HashFilePath = _hashFilePath ?? GetHashFilePath();

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
            try
            {
                File.WriteAllText(HashFilePath, hash);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save Templater hash: {e}");
            }
        }

        public static string GetHash()
        {
            try
            {
                if (File.Exists(HashFilePath))
                {
                    return File.ReadAllText(HashFilePath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to get Templater hash: {e}");
            }

            return string.Empty;
        }

        private static string GetHashFilePath()
        {
            var package = TemplaterUtility.FindScriptDirectory(nameof(TemplatesHash));
            if (string.IsNullOrEmpty(package))
            {
                Debug.LogError("Failed to find Templater package path");
                return string.Empty;
            }

            return Path.Combine(package, "TemplateHash.txt");
        }
    }
}