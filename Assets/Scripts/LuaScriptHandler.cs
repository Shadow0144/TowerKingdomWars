using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LuaScriptHandler
{
    private const string scriptFolder = "Assets\\Maps\\";

    private readonly Script script = new Script();

    private static MapBuilder mapBuilder = null;

    private static int buildingPathForCastleNumber = -1;
    private static List<Vector2Int> path;

    private static void SetMapSize(uint rows, uint columns)
    {
        mapBuilder = new MapBuilder(rows, columns);
    }

    private static void BeginPath(uint castleNumber)
    {
        if (mapBuilder == null)
        {
            throw new Exception("Map size needs to be specified first");
        }

        buildingPathForCastleNumber = (int)castleNumber;
        path = new List<Vector2Int>();
    }

    private static void AddPathTile(uint row, uint col)
    {
        if (mapBuilder == null)
        {
            throw new Exception("Map size needs to be specified first");
        }

        if (buildingPathForCastleNumber < 0)
        {
            throw new Exception("BeginPath must be called before starting a path");
        }

        path.Add(new Vector2Int((int)row, (int)col));
    }

    private static void EndPath()
    {
        if (mapBuilder == null)
        {
            throw new Exception("Map size needs to be specified first");
        }

        mapBuilder.AddPath((uint)buildingPathForCastleNumber, path);
        buildingPathForCastleNumber = -1;
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

    public void Initialize()
    {
        FileSystemScriptLoader loader = new FileSystemScriptLoader();
        string luaPath = Path.Combine(Application.streamingAssetsPath, scriptFolder);
        loader.ModulePaths = new string[]
        {
            Path.Combine(luaPath, "?.lua"),
        };
        script.Options.ScriptLoader = loader;

        script.Globals["SetMapSize"] = (Action<uint, uint>)SetMapSize;
        script.Globals["BeginPath"] = (Action<uint>)BeginPath;
        script.Globals["AddPathTile"] = (Action<uint, uint>)AddPathTile;
        script.Globals["EndPath"] = (Action)EndPath;
        script.Globals["SpawnCastle"] = (Action<uint, uint, uint>)SpawnCastle;
    }

    public void LoadMap(string path)
    { 
        script.DoFile(scriptFolder + path);
        if (mapBuilder != null)
        {
            mapBuilder.Build();
        }
        mapBuilder = null;
    }
}