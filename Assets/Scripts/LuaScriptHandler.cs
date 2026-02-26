using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TowerKingdomWars
{
    public class LuaScriptHandler
    {
        private const string scriptFolder = "Assets\\Maps\\";

        private readonly Script script = new Script();

        private static MapBuilder mapBuilder = null;

        private static int sourceStrongholdNumber = -1;
        private static List<Vector2Int> path;
        private static int targetStrongholdNumber = -1;

        private static void SetMapSize(uint rows, uint columns)
        {
            mapBuilder = new MapBuilder(rows, columns);
        }

        private static void BeginPath(uint strongholdNumber)
        {
            if (mapBuilder == null)
            {
                throw new Exception("Map size needs to be specified first");
            }

            path = new List<Vector2Int>();
            sourceStrongholdNumber = (int)strongholdNumber;
        }

        private static void AddPathTile(uint row, uint col)
        {
            if (mapBuilder == null)
            {
                throw new Exception("Map size needs to be specified first");
            }

            if (path == null)
            {
                throw new Exception("BeginPath must be called before starting a path");
            }

            path.Add(new Vector2Int((int)row, (int)col));
        }

        private static void EndPath(uint strongholdNumber)
        {
            if (mapBuilder == null)
            {
                throw new Exception("Map size needs to be specified first");
            }

            if (path == null || sourceStrongholdNumber < 0)
            {
                throw new Exception("BeginPath must be called before starting a path");
            }

            targetStrongholdNumber = (int)strongholdNumber;
            mapBuilder.AddPath((uint)sourceStrongholdNumber, path, (uint)targetStrongholdNumber);
            path = null;
        }

        private static void SpawnCastle(uint centerRow, uint centerCol, uint playerSlot)
        {
            if (mapBuilder == null)
            {
                throw new Exception("Map size needs to be specified first");
            }

            mapBuilder.AddStructure(MapBuilder.StructureType.Castle, centerRow, centerCol, playerSlot);
        }

        private static void SpawnFort(uint centerRow, uint centerCol, uint playerSlot)
        {
            if (mapBuilder == null)
            {
                throw new Exception("Map size needs to be specified first");
            }

            mapBuilder.AddStructure(MapBuilder.StructureType.Fort, centerRow, centerCol, playerSlot);
        }

        public void Initialize()
        {
            FileSystemScriptLoader loader = new FileSystemScriptLoader();
            string luaPath = System.IO.Path.Combine(Application.streamingAssetsPath, scriptFolder);
            loader.ModulePaths = new string[]
            {
            System.IO.Path.Combine(luaPath, "?.lua"),
            };
            script.Options.ScriptLoader = loader;

            script.Globals["SetMapSize"] = (Action<uint, uint>)SetMapSize;
            script.Globals["BeginPath"] = (Action<uint>)BeginPath;
            script.Globals["AddPathTile"] = (Action<uint, uint>)AddPathTile;
            script.Globals["EndPath"] = (Action<uint>)EndPath;
            script.Globals["SpawnCastle"] = (Action<uint, uint, uint>)SpawnCastle;
            script.Globals["SpawnFort"] = (Action<uint, uint, uint>)SpawnFort;
        }

        public void LoadMap(string path)
        {
            try
            {
                script.DoFile(scriptFolder + path);
                if (mapBuilder != null)
                {
                    mapBuilder.Build();
                }
                mapBuilder = null;
            }
            catch 
            {
                Debug.LogError("Failed to load map");
            }
        }
    }
}