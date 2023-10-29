using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LegoBattaleRoyal.ApplicationLayer.SaveSystem
{
    public interface ISaveService
    {
        event Action<string> OnFileDeleted;

        public string SaveDataDir { get; }
        public string ResourcesDataPath { get; }

        bool Exists(string filename);

        bool Exists<T>() where T : class;

        void Save(string filename, byte[] data);

        UniTask SaveAsync(string filename, byte[] data);

        void Save(string filename, string contents);

        void Save<T>(string filename, T contentToSerialize);

        void Save<T>(T contentToSerialize);

        Texture2D LoadTexture(string filename);

        JObject LoadJson(string fileName);

        T Load<T>(string fileName, params JsonConverter[] converts) where T : class;

        T Load<T>(params JsonConverter[] converts) where T : class;

        byte[] Load(string fileName);

        UniTask<byte[]> LoadAsync(string fileName);

        string TryReadFile(string fileName);

        IEnumerable<FileInfo> GetAllSaveFiles(string path = "");

        byte[] GetFileData(string filename);

        void Delete(string fileName);

        void DeleteAllLocal();

        void CreateDirectoryInSaveData(string path);

        void EnsureSaveDataDirectoryExists();

        TextAsset SaveToTextAsset(string name, string text, string ext = "json");

        TextAsset SaveToTextAsset(string name, object content, string ext = "json");

        T LoadFromTextAsset<T>(string relativePath);

        string LoadFromTextAsset(string relativePath);

        IEnumerable<FileInfo> GetAllFilesFromResources(string relativePath = "", params string[] excludeFileExt);

        void OpenSaveFolder();

        UniTask<T> LoadFromTextAssetAsync<T>(string relativePath);

        UniTask<T> LoadFromTextAssetAsync<T>();

        UniTask<T> LoadAsync<T>(params JsonConverter[] converts);

        UniTask SaveAsync<T>(string filename, T contentToSerialize);

        UniTask SaveAsync<T>(T contentToSerialize);
    }
}