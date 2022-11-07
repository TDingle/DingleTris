using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.UI;

public class PlayGrid : MonoBehaviour
{
    
    public Tilemap tilemap { get; private set; }
    public TData[] tetrominos;
    public SingleBlock activeBlock { get; private set; }
    public SingleBlock nextBlock { get; private set; }
    public SingleBlock savedBlock { get; private set; }


    public bool swapCheck = true;
    public int linesCleared = 0;
    public int totalLinesCleared = 0;
    public int score = 0;
    public bool gameover = false;

    [SerializeField] ParticleSystem blocks = null;
    [SerializeField] GameObject _scoreText;

    public Vector3Int startPosition= new Vector3Int(-1,8,0);
    public Vector2Int gridSize = new Vector2Int(10, 20);

    public Vector3Int previewPosition = new Vector3Int(8, 1, 0);
    public Vector3Int holdPosition = new Vector3Int(-10, 7, 0);


    public RectInt GridBounds
    {
        get {
            Vector2Int position = new Vector2Int(-this.gridSize.x / 2, -this.gridSize.y / 2);
            return new RectInt(position, this.gridSize);
            }
    }
    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activeBlock = GetComponentInChildren<SingleBlock>();

        nextBlock = gameObject.AddComponent<SingleBlock>();
        nextBlock.enabled = false;

        savedBlock = gameObject.AddComponent<SingleBlock>();
        savedBlock.enabled = false;


        for (int i = 0; i < tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();    
            
        }

    }
    void Start()
    {
        SetNextBlock();
        SpawnBlock();
    }
    private void Update()
    { 

        if (Input.GetKeyDown(KeyCode.C))
        {
            SwapBlock();
        }
    }
    private void IncreaseScore()
    {
        GameObject Scoretag = GameObject.FindWithTag("Score");
        TextMeshPro comp = Scoretag.GetComponent<TextMeshPro>();
        comp.text = score.ToString();


    }
    private void SetNextBlock()
    {
        if (nextBlock.cells != null)
        {
            Clear(nextBlock);
        }

        int random = Random.Range(0, tetrominos.Length);
        TData data = tetrominos[random];

        nextBlock.Initialize(this, previewPosition, data);
        Set(nextBlock);
    }
    public void SpawnBlock()
    {
        activeBlock.Initialize(this, startPosition, nextBlock.data);
        if (IsValidPos(activeBlock, startPosition))
        {
            Set(activeBlock);
        }
        else
        {
            GameOver();
        }
        SetNextBlock();
        
    }

    public void Set(SingleBlock piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePos, piece.data.tile);
        }
    }

    public void Clear(SingleBlock piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePos, null);
        }
        
    }

    public bool IsValidPos(SingleBlock piece, Vector3Int pos)
    {
        RectInt bounds = this.GridBounds;
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
        RectInt bounds = GridBounds;
        int row = bounds.yMin;


        while (row < bounds.yMax)
        {
           
            if (IsLineFull(row))
            {
                LineClear(row);
                linesCleared++;
                totalLinesCleared++;
                Debug.Log(totalLinesCleared);
                spawnParticle();
                
                
            }
            else
            {
                row++;
            }
        }
        Points();
        IncreaseScore();
        
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = GridBounds;

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
        RectInt bounds = GridBounds;

      
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
    public void SwapBlock()
    {
        if (swapCheck == true)
        {
            TData savedData = savedBlock.data;
            TData nextData = nextBlock.data;
        
            if (savedData.cells == null)
            {
                savedBlock.Initialize(this, holdPosition, activeBlock.data);
                Set(savedBlock);

                Clear(activeBlock);
                activeBlock.Initialize(this, startPosition, nextData);
                SetNextBlock();
                swapCheck = false;
            }
            else if (savedData.cells != null)
            {
                Clear(savedBlock);

                savedBlock.Initialize(this, holdPosition, activeBlock.data);
                Set(savedBlock);

                Clear(activeBlock);
                activeBlock.Initialize(this, startPosition, savedData);
                Set(savedBlock);
                swapCheck = false;
            }
        }
    }
     public void GameOver()
    {
        tilemap.ClearAllTiles();
        gameover = true;
    }

    public void Points()
    {
        if (linesCleared == 1)
            score += 40*(activeBlock.currentLevel + 1);
        else if (linesCleared == 2)
            score += 100*(activeBlock.currentLevel + 1);
        else if (linesCleared == 3)
            score += 300*(activeBlock.currentLevel + 1);
        else if (linesCleared == 4)
            score += 1200*(activeBlock.currentLevel + 1);
        linesCleared = 0;
    }
   
    public void spawnParticle()
    {
        blocks.Play();
    }

}
