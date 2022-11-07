using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TetrisBlocks
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}
[System.Serializable]
public struct TData
{
    public TetrisBlocks tetromino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallkicks {get; private set; }

    public void Initialize()
    {
        this.cells = BlockData.Cells[this.tetromino];
        this.wallkicks = BlockData.WallKicks[this.tetromino];
    }
}   