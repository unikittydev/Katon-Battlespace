using Game.Voxels;
using Game.Voxels.Data;
using Game.Voxels.Editor;
using Game.Voxels.Editor.Data;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Класс, сохраняющий и загружающий игровые данные.
    /// </summary>
    public class GameDataManager : MonoBehaviour
    {
        /// <summary> 
        /// Корневой каталог для сохранения данных.
        /// </summary>
        private static string baseDirectory;

        /// <summary>
        /// Каталоги для сохранения данных типов.
        /// </summary>
        private static readonly Dictionary<Type, string> directories = new Dictionary<Type, string>()
        {
            { typeof(VoxelWorldEditor), "Ship Saves/" },
            { typeof(VoxelWorldComponent), "Ship Saves/" },
            { typeof(PlayerInventory), "Voxel Editor/" },
            { typeof(PlayerController), "Voxel Editor/" }
        };

        /// <summary>
        /// Подкаталоги для типов, которые сохраняются в нескольких экземплярах.
        /// </summary>
        private static readonly Dictionary<Type, string> multiSubdirectories = new Dictionary<Type, string>
        {
            { typeof(VoxelWorldEditor), "autosave" },
            { typeof(VoxelWorldComponent), "autosave" },
        };

        /// <summary>
        /// Стандартные названия файлов для типов.
        /// </summary>
        private static readonly Dictionary<Type, string> fileNames = new Dictionary<Type, string>()
        {
            { typeof(VoxelWorldEditor), "editorData" },
            { typeof(VoxelWorldComponent), "voxelWorld" },
            { typeof(PlayerInventory), "inventory" },
            { typeof(PlayerController), "playerController" },
        };

        /// <summary>
        /// Данные типов по умолчанию (для первого запуска игры).
        /// </summary>
        private static readonly Dictionary<Type, object> defaultData = new Dictionary<Type, object>()
        {
            { typeof(VoxelWorldEditor), new ShipStatsData() },
            { typeof(VoxelWorldComponent), new VoxelWorldData() },
            { typeof(PlayerInventory), new InventoryData() },
            { typeof(PlayerController), new PlayerControllerData() },
        };

        private void Awake()
        {
            baseDirectory = $"{Application.persistentDataPath}/";
            CreateDefaultData();
        }

        /// <summary>
        /// Создаёт данные игры по умолчанию.
        /// </summary>
        private static void CreateDefaultData()
        {
            foreach (var kv in defaultData)
                if (!File.Exists(GetFilePath(kv.Key)))
                    Save(kv.Key, JsonUtility.ToJson(kv.Value));
        }

        /// <summary>
        /// Сохраняет данные типа.
        /// </summary>
        /// <param name="json"> Данные типа в JSON-формате. </param>
        public static void Save(Type type, string json)
        {
            try
            {
                string directory = GetFileDirectory(type);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                using BinaryWriter writer = new BinaryWriter(File.OpenWrite(GetFilePath(type, directory)));
                writer.Write(json);
            }
            catch (Exception e)
            {
                Debug.LogError(e.GetType() + "\n" + e.Message);
            }
        }

        /// <summary>
        /// Сохраняет данные типа.
        /// </summary>
        /// <param name="json"> Данные типа в JSON-формате. </param>
        public static void Save(Type type, string subdir, string json)
        {
            string defaultMultiSubdir = multiSubdirectories[type];
            multiSubdirectories[type] = subdir;
            Save(type, json);
            multiSubdirectories[type] = defaultMultiSubdir;
        }

        /// <summary>
        /// Загружает стандартные данные типа.
        /// </summary>
        public static string LoadDefault(Type type)
        {
            return JsonUtility.ToJson(defaultData[type]);
        }

        /// <summary>
        /// Загружает данные типа в JSON-формате.
        /// </summary>
        public static string Load(Type type)
        {
            string json = null;

            try
            {
                string directory = GetFileDirectory(type);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                using BinaryReader reader = new BinaryReader(File.OpenRead(GetFilePath(type, directory)));
                json = reader.ReadString();
            }
            catch (Exception e)
            {
                Debug.LogError(e.GetType() + "\n" + e.Message);
            }

            return json;
        }

        /// <summary>
        /// Загружает данные типа в JSON-формате.
        /// </summary>
        public static string Load(Type type, string subdir)
        {
            string defaultMultiSubdir = multiSubdirectories[type];
            multiSubdirectories[type] = subdir;
            string json = Load(type);
            multiSubdirectories[type] = defaultMultiSubdir;

            return json;
        }

        /// <summary>
        /// Загружает массив данных типа в JSON-формате.
        /// </summary>
        public static string[] LoadAll(Type type)
        {
            if (!multiSubdirectories.ContainsKey(type))
                return null;

            string directory = string.Concat(baseDirectory, GameDataManager.directories[type]);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string[] directories = Directory.GetDirectories(directory);
            string[] jsonArray = new string[directories.Length];

            string defaultMultiSubdir = multiSubdirectories[type];

            for (int i = 0; i < directories.Length; i++)
            {
                multiSubdirectories[type] = directories[i].Substring
                    (baseDirectory.Length + GameDataManager.directories[type].Length);
                jsonArray[i] = Load(type);
            }

            multiSubdirectories[type] = defaultMultiSubdir;

            return jsonArray;
        }

        /// <summary>
        /// Возвращает полный путь файла данных типа.
        /// </summary>
        private static string GetFilePath(Type type)
        {
            return string.Concat(GetFileDirectory(type), fileNames[type], ".json");
        }

        /// <summary>
        /// Возвращает полный путь файла данных типа.
        /// </summary>
        private static string GetFilePath(Type type, string directory)
        {
            return string.Concat(directory, fileNames[type], ".json");
        }

        /// <summary>
        /// Возвращает каталог, где хранится файл данных типа.
        /// </summary>
        private static string GetFileDirectory(Type type)
        {
            if (!multiSubdirectories.ContainsKey(type))
                return string.Concat(baseDirectory, directories[type]);
            return string.Concat(baseDirectory, directories[type], multiSubdirectories[type], "/");
        }
    }
}
