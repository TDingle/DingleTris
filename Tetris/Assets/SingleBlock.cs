using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class SingleBlock : MonoBehaviour
{
    public PlayGrid grid { get; private set; }
   
    public TData data { get; private set; }

    public Vector3Int position { get; private set; }

    public Vector3Int[] cells { get; private set; }

    public int rotationIndex { get; private set; }

    public CameraPostProcess setPal;

    public double stepDelay = 1;
    public double lockDelay = 0.5;

    public int currentLevel = 0;

    private double stepTime;
    private float lockTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (!grid.gameover)
        {
            LevelScale();
            BlockFunction();
        }
    }
    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);
        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }
    private void Lock()
    {
        this.grid.Set(this);
        this.grid.ClearLines();
        this.grid.SpawnBlock();
        this.grid.swapCheck = true;
    }
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.grid.IsValidPos(this, newPosition);
        if(valid)
        {
            this.position = newPosition;
            this.lockTime = 0f;
        }
        return valid;
    }

   
    public void Initialize(PlayGrid board, Vector3Int position, TData data)
    {
        this.grid = board;
        this.position = position;
        this.data = data;   
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }
        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }
    void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex; 
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotation(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotation(-direction);
        }
    }
    void ApplyRotation(int direction)
    {

        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;

            switch (this.data.tetromino)
            {
                case TetrisBlocks.I:
                case TetrisBlocks.O:
                    cell.x -= .5f;
                    cell.y -= .5f;
                    x = Mathf.CeilToInt((cell.x * BlockData.RotationMatrix[0] * direction) + (cell.y * BlockData.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * BlockData.RotationMatrix[2] * direction) + (cell.y * BlockData.RotationMatrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((cell.x * BlockData.RotationMatrix[0] * direction) + (cell.y * BlockData.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * BlockData.RotationMatrix[2] * direction) + (cell.y * BlockData.RotationMatrix[3] * direction));
                    break;
            }
            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallkicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallkicks[wallKickIndex,i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if(rotationDirection < 0)
        {
            wallKickIndex--;
        }
        return Wrap(wallKickIndex, 0, this.data.wallkicks.GetLength(0));
    }
    private int Wrap(int input, int min, int max)
    {
        if(input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
    private void BlockFunction()
    {
        this.grid.Clear(this);

        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Rotate(1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (Move(Vector2Int.down))
            {
                continue;
            }
            Lock();
        }

        if (Time.time >= this.stepTime)
        {
            Step();
        }
        this.grid.Set(this);
    }
    private void LevelScale()
    {
        int lineBeforeLevelIncrease = currentLevel * 10 + 10;
        if(currentLevel == 30)
        {
            stepDelay = .001;
            lockDelay = 0.2;
        }
        else if (grid.totalLinesCleared >= lineBeforeLevelIncrease)
        {
            currentLevel++;
            //setPal.SetPalette(currentLevel);
            stepDelay -= 0.034;
            lockDelay -= 0.004;
        }

        GameObject LevelTag = GameObject.FindWithTag("Level");
        TextMeshPro lvlComp = LevelTag.GetComponent<TextMeshPro>();
        lvlComp.text = currentLevel.ToString();
    }
}
