using Cysharp.Threading.Tasks;
using LegoBattaleRoyal.ApplicationLayer.SaveSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LegoBattaleRoyal.App.AppService
{
    public class SaveService : ISaveService, IDisposable
    {
        public event Action<string> OnFileDeleted;

        public string SaveDataDir { get; }
        public string ResourcesDataPath { get; } = Path.Combine(Application.dataPath, "Resources");

        public SaveService(string saveDataDir = "SaveData")
        {
            SaveDataDir = Path.Combine(Application.persistentDataPath, saveDataDir);
        }

        public bool Exists(string filename)
        {
            return File.Exists($"{SaveDataDir}/{filename}");
        }

        public bool Exists<T>() where T : class
        {
            var fileName = typeof(T).Name;
            return Exists(fileName);
        }

        public void Save(string filename, byte[] data)
        {
            try
            {
                CreateDirectoryInSaveData(filename);

                var path = Path.Combine(SaveDataDir, filename);

                Debug.Log("Saving Locally. Path: " + path);
                File.WriteAllBytes(path, data);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

#if UNITY_2021_2_OR_NEWER

        public async UniTask SaveAsync(string filename, byte[] data)
        {
            try
            {
                CreateDirectoryInSaveData(filename);

                var path = $"{SaveDataDir}/{filename}";

                Debug.Log("Saving Locally. Path: " + path);
                await File.WriteAllBytesAsync(path, data);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public UniTask SaveAsync<T>(string filename, T contentToSerialize)
        {
            return SaveAsync(filename, JsonConvert.SerializeObject(contentToSerialize));
        }

        public async UniTask SaveAsync(string filename, string content)
        {
            CreateDirectoryInSaveData(filename);

            var path = $"{SaveDataDir}/{filename}";

            Debug.Log("Saving Locally. Path: " + path);
            await File.WriteAllTextAsync(path, content);
        }

        public UniTask SaveAsync<T>(T contentToSerialize)
        {
            var fileName = typeof(T).Name;
            return SaveAsync(fileName, contentToSerialize);
        }

#endif

        /// <summary>
        ///     Saves local json file
        /// </summary>
        public void Save(string filename, string contents)
        {
            CreateDirectoryInSaveData(filename);

            var path = $"{SaveDataDir}/{filename}";

            Debug.Log("Saving Locally. Path: " + path);
            File.WriteAllText(path, contents);
        }

        public void Save<T>(string filename, T contentToSerialize)
        {
            Save(filename, JsonConvert.SerializeObject(contentToSerialize));
        }

        public Texture2D LoadTexture(string filename)
        {
            var path = $"{SaveDataDir}/{filename}";
            if (!File.Exists(path))
                return null;

            var t = new Texture2D(1, 1);
            t.LoadImage(File.ReadAllBytes(path));
            t.Apply();
            return t;
        }

        /// <summary>
        /// Loads local json file
        /// </summary>
        public JObject LoadJson(string fileName)
        {
            try
            {
                var path = $"{SaveDataDir}/{fileName}";
                if (File.Exists(path))
                    return JObject.Parse(File.ReadAllText(path));

                Debug.LogError("No file found. " + path);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        /// <summary>
        /// Loads and deserializes local file
        /// </summary>
        public T Load<T>(string fileName, params JsonConverter[] converts) where T : class
        {
            try
            {
                var path = $"{SaveDataDir}/{fileName}";
                if (File.Exists(path))
                {
                    var allText = File.ReadAllText(path);
                    return JsonConvert.DeserializeObject<T>(allText, converts);
                }

                Debug.LogError("No file found. " + path);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        /// <summary>
        /// Loads local file and returns byte[]
        /// </summary>
        public byte[] Load(string fileName)
        {
            try
            {
                var path = $"{SaveDataDir}/{fileName}";
                if (File.Exists(path))
                {
                    var data = File.ReadAllBytes(path);
                    return data;
                }

                Debug.LogError("No file found. " + path);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

#if UNITY_2021_2_OR_NEWER

        /// <summary>
        /// Loads local text file and T
        /// </summary>
        public async UniTask<T> LoadAsync<T>(params JsonConverter[] converts)
        {
            var fileName = typeof(T).Name;
            var path = $"{SaveDataDir}/{fileName}";
            if (!File.Exists(path))
                throw new NullReferenceException($"Missing file {path}");

            var allText = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<T>(allText, converts);
        }

        public async UniTask<byte[]> LoadAsync(string fileName)
        {
            try
            {
                var path = $"{SaveDataDir}/{fileName}";
                if (File.Exists(path))
                {
                    var data = await File.ReadAllBytesAsync(path);
                    return data;
                }

                Debug.LogError("No file found. " + path);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

#endif

        /// <summary>
        /// Loads local file and returns a string
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string TryReadFile(string fileName)
        {
            try
            {
                var path = $"{SaveDataDir}/{fileName}";
                return File.Exists(path) ? File.ReadAllText(path) : null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        public IEnumerable<FileInfo> GetAllSaveFiles(string path = "")
        {
            var finalPath = string.IsNullOrEmpty(path)
                ? SaveDataDir
                : Path.Combine(SaveDataDir, path);

            if (!Directory.Exists(finalPath))
                Directory.CreateDirectory(finalPath);

            return new DirectoryInfo(finalPath).GetFiles();
        }

        public byte[] GetFileData(string filename)
        {
            try
            {
                var path = $"{SaveDataDir}/{filename}";
                return File.Exists(path) ? File.ReadAllBytes(path) : null;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        /// <summary>
        /// Delete any local file
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            var path = $"{SaveDataDir}/{fileName}";

            if (File.Exists(path))
                File.Delete(path);
            else
                Debug.LogError("No file found.");
        }

        /// <summary>
        /// Deletes all files and directories within the SaveData folder
        /// </summary>
        public void DeleteAllLocal()
        {
            if (!Directory.Exists(SaveDataDir))
                return;

            Directory.Delete(SaveDataDir, true);
            OnFileDeleted?.Invoke("Local Data Deleted");
        }

        /// <summary>
        /// Creates a directory if one doesn't exist
        /// </summary>
        public void CreateDirectoryInSaveData(string path)
        {
            EnsureSaveDataDirectoryExists();

            var dirPath = Path.GetDirectoryName(Path.GetFullPath(Path.Combine(SaveDataDir, path)));
            if (!Directory.Exists(dirPath) && dirPath != null)
                Directory.CreateDirectory(dirPath);
        }

        public void EnsureSaveDataDirectoryExists()
        {
            if (!Directory.Exists(SaveDataDir))
                Directory.CreateDirectory(SaveDataDir);
        }

        /// <summary>
        /// Creates new TextAsset in Assets/Resources folder.
        /// EDITOR ONLY
        /// </summary>
        public TextAsset SaveToTextAsset(string name, string text, string ext = "json")
        {
#if UNITY_EDITOR
            var fileName = $"{name}.{ext}";
            var path = Path.Combine(ResourcesDataPath, fileName);

            new FileInfo(path).Directory.Create(); // ensure directory exist

            if (!File.Exists(path))
                File.CreateText(path).Close();

            File.WriteAllText(path, text);
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine("Assets", "Resources", fileName));
#else
            Debug.LogError($"Calling {nameof(SaveToTextAsset)} is not supported outside of editor.");
            return null;
#endif
        }

        /// <summary>
        /// Creates new TextAsset in Assets/Resources folder.
        /// EDITOR ONLY
        /// </summary>
        public TextAsset SaveToTextAsset(string name, object content, string ext = "json",
            Formatting formatting = Formatting.None)
        {
            var text = JsonConvert.SerializeObject(content, formatting);
            return SaveToTextAsset(name, text, ext);
        }

        /// <summary>
        /// Loads from TextAsset in Assets/Resources folder.
        /// </summary>
        public T LoadFromTextAsset<T>(string relativePath)
        {
            var asset = Resources.Load<TextAsset>(relativePath);
            if (asset == null)
            {
                Debug.LogError($"Could not find text-asset on relativePath {relativePath}");
                return default;
            }

            var result = JsonConvert.DeserializeObject<T>(asset.text);
            return result;
        }

        /// <summary>
        /// Loads from TextAsset in Assets/Resources folder.
        /// </summary>
        public async UniTask<T> LoadFromTextAssetAsync<T>(string relativePath)
        {
            var asset = (TextAsset)await Resources.LoadAsync<TextAsset>(relativePath);
            if (asset == null)
            {
                Debug.LogError($"Could not find text-asset on relativePath {relativePath}");
                return default;
            }

            var result = JsonConvert.DeserializeObject<T>(asset.text);
            return result;
        }

        public async UniTask<T> LoadFromTextAssetAsync<T>()
        {
            var relativePath = typeof(T).Name;
            var asset = (TextAsset)await Resources.LoadAsync<TextAsset>(relativePath);
            if (asset == null)
            {
                Debug.LogError($"Could not find text-asset on relativePath {relativePath}");
                return default;
            }

            var result = JsonConvert.DeserializeObject<T>(asset.text);
            return result;
        }

        /// <summary>
        /// Loads from TextAsset in Assets/Resources folder.
        /// </summary>
        public string LoadFromTextAsset(string relativePath)
        {
            var asset = Resources.Load<TextAsset>(relativePath);
            if (asset != null)
                return asset.text;

            Debug.LogError($"Could not find text-asset on relativePath {relativePath}");
            return default;
        }

        public IEnumerable<T> GetAllFromResources<T>(string relativePath = "") where T : Object
        {
            return Resources.LoadAll<T>(relativePath);
        }

        /// <summary>
        /// EDITOR ONLY
        /// </summary>
        public IEnumerable<FileInfo> GetAllFilesFromResources(string relativePath = "", params string[] excludeFileExt)
        {
#if !UNITY_EDITOR
            var finalPath = string.IsNullOrEmpty(relativePath)
                ? ResourcesDataPath
                : Path.Combine(ResourcesDataPath, relativePath);

            return new DirectoryInfo(finalPath)
                .GetFiles()
                .Where(x => !excludeFileExt.Contains(Path.GetExtension(x.Name)));
#else
            Debug.LogError($"Calling {nameof(GetAllFilesFromResources)} is not supported outside of editor.");
            return null;
#endif
        }

        public void OpenSaveFolder()
        {
#if PLATFORM_STANDALONE_WIN
            System.Diagnostics.Process.Start("explorer.exe", $@"{Path.GetFullPath(SaveDataDir)}");
#elif UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(Path.GetFullPath(SaveDataDir));
#endif
        }

        public void Dispose()
        {
            OnFileDeleted = null;
        }

        public void Save<T>(T contentToSerialize)
        {
            var fileName = typeof(T).Name;
            Save(fileName, contentToSerialize);
        }

        public T Load<T>(params JsonConverter[] converts) where T : class
        {
            var fileName = typeof(T).Name;
            return Load<T>(fileName, converts);
        }

        public TextAsset SaveToTextAsset(string name, object content, string ext = "json")
        {
            throw new NotImplementedException(); //TODO
        }
    }
}