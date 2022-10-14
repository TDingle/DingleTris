using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;
    public Piece activePiece { get; private set; }
    public Vector3Int startPos;
    public Vector2Int boardSize = new Vector2Int(10, 20);


    public RectInt Bounds
    {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
            }
    }
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();    
            
        }
    }
    void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];

        this.activePiece.Initialize(this, startPos , data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePos, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePos, null);
        }
    }

    public bool IsValidPos(Piece piece, Vector3Int pos)
    {
        RectInt bounds = this.Bounds;
        for(int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + pos;
            if (!bounds.Contains((Vector2Int)tilePos))
            {
                return false;
            }
            if (this.tilemap.HasTile(tilePos))
            {
                return false;
            }
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
