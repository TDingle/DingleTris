using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;
    public Piece activePiece { get; private set; }
    public Piece nextPiece { get; private set; }
    public Piece savedPiece { get; private set; }

    public bool swapCheck = true;
    public int linesCleared = 0;
    public int score = 0;

    public Vector3Int startPos = new Vector3Int(-1,8,0);
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public Vector3Int previewPosition = new Vector3Int(8, 1, 0);
    public Vector3Int holdPosition = new Vector3Int(-10, 7, 0);


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

        nextPiece = gameObject.AddComponent<Piece>();
        nextPiece.enabled = false;

        savedPiece = gameObject.AddComponent<Piece>();
        savedPiece.enabled = false;


        for (int i = 0; i < tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();    
            
        }

    }
    void Start()
    {
        SetNextPiece();
        SpawnPiece();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwapPiece();
        }
    }
    private void SetNextPiece()
    {
        if (nextPiece.cells != null)
        {
            Clear(nextPiece);
        }

        int random = Random.Range(0, tetrominos.Length);
        TetrominoData data = tetrominos[random];

        nextPiece.Initialize(this, previewPosition, data);
        Set(nextPiece);
    }
    public void SpawnPiece()
    {
        activePiece.Initialize(this, startPos, nextPiece.data);
        if (IsValidPos(activePiece, startPos))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
        SetNextPiece();
        
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
    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;
       
        
        while (row < bounds.yMax)
        {
           
            if (IsLineFull(row))
            {
                LineClear(row);
                linesCleared++;
            }
            else
            {
                row++;
            }
        }
        Points();

    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

      
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tilemap.SetTile(position, null);
        }
        

        
        while (row < bounds.yMax)
        {
            
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }
        
    }
    public void SwapPiece()
    {
        if (swapCheck == true)
        {
            TetrominoData savedData = savedPiece.data;
            TetrominoData nextData = nextPiece.data;
        
            if (savedData.cells == null)
            {
                savedPiece.Initialize(this, holdPosition, activePiece.data);
                Set(savedPiece);

                Clear(activePiece);
                activePiece.Initialize(this, startPos, nextData);
                SetNextPiece();
                swapCheck = false;
            }
            else if (savedData.cells != null)
            {
                Clear(savedPiece);

                savedPiece.Initialize(this, holdPosition, activePiece.data);
                Set(savedPiece);

                Clear(activePiece);
                activePiece.Initialize(this, startPos, savedData);
                Set(savedPiece);
                swapCheck = false;
            }
        }
    }
     public void GameOver()
    {
        tilemap.ClearAllTiles();
    }

    public void Points()
    {
        if (linesCleared == 1)
            score += 40;
        else if (linesCleared == 2)
            score += 100;
        else if (linesCleared == 3)
            score += 300;
        else if (linesCleared == 4)
            score += 1200;
        Debug.Log(score);
        linesCleared = 0;
    }
   
    

}
