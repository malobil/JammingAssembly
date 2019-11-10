using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Normal, Rare, Bedrock, Larbnain, NainPloseur, Garbage, Player }

public class Script_GameManager : MonoBehaviour
{
    public static Script_GameManager Instance { get; private set; }

    public float gameTime = 180f;
    private float currentGameTime;
    private bool gameIsOver = false;
    public Vector2 gridSize = new Vector2(10, 10);
    public int playersStartSpace = 5;
    public float gridCellSize = 64f;
    public int playerCount = 2;
    public int decalage = 0;

    public int normalRockLife = 2;
    public int garbageRockLife = 1;
    public int rareRockLife = 3;
    public int bedRockLife = 5;
    public int normalRockGoldValue = 5;
    public int rareRockGoldValue = 10;
    public int bedRockGoldValue = 15;
   

    public int larbnainCost = 10;
    public int nainploseurCost = 10;

    public Transform gridParent;

    public GameObject prefabNormalRock;
    public GameObject prefabGarbageRock;
    public GameObject prefabGround;
    public GameObject prefabRareRock;
    public GameObject prefabBedRock;

    public List<GameObject> prefabNainPloseur;

    public List<GameObject> larbnains;

    public List<GameObject> prefabPlayers;

    public List<int> playersGold;

    public List<Transform> cameras;
    public float cameraUnzoomStep = 5f;
    public float cameraUnzoomZone = 20f;


    [Range(0, 100)]
    public int chanceRareRock = 35;

    [Range(0, 100)]
    public int chanceBedRock = 10;

    private List<Grid> playersGrids = new List<Grid>();
    private Vector2 playersStartCell;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        CreateGrid();
        AttributeBaseMap();
        SpawnMap();
        DuplicateMap();
        SpawnPlayers();
        currentGameTime = gameTime;
    }

    private void Update()
    {
        if(currentGameTime > 0)
        {
            currentGameTime -= Time.deltaTime;
            Script_UIManager.Instance.UpdateTimer(currentGameTime);
        }
        else if(currentGameTime <= 0 && !gameIsOver)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        List<int> endScores = new List<int>();
        int winnerIdx = 0;
        int winnerScore = 0;

        for(int i = 0; i < playerCount; i++)
        {
            int playerScore = GetPlayerEmptyCell(i);

            if(playerScore > winnerScore)
            {
                winnerScore = playerScore;
                winnerIdx = i;
            }

            endScores.Add(playerScore);
        }

        Script_UIManager.Instance.ShowFinish(winnerIdx, endScores);

        gameIsOver = true;
        Time.timeScale = 0;
    }

    void CreateGrid()
    {
        playersStartCell = new Vector2(Mathf.Floor(gridSize.x / 2), Mathf.Floor(gridSize.y / 2));
        Grid newGrid = new Grid();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                Cell newCell = new Cell();
                newCell.cellPosition = new Vector2(x, y);
                newCell.cellName = (x + "/" + y);
                newGrid.gridCells.Add(newCell);
            }
        }
        playersGrids.Add(newGrid);
    }

    void AttributeBaseMap()
    {
        for (int i = 0; i < playersGrids[0].gridCells.Count; i++)
        {
            if (playersGrids[0].gridCells[i].cellPosition.x >= playersStartCell.x - playersStartSpace && playersGrids[0].gridCells[i].cellPosition.x <= playersStartCell.x + playersStartSpace
                && playersGrids[0].gridCells[i].cellPosition.y >= playersStartCell.y - playersStartSpace && playersGrids[0].gridCells[i].cellPosition.y <= playersStartCell.y + playersStartSpace)
            {
                GetCellByPostion(0, playersGrids[0].gridCells[i].cellPosition).cellType = CellType.Empty;
            }
            else
            {
                int rdm = Random.Range(1, 101);

                if(rdm <= chanceBedRock)
                {
                     GetCellByPostion(0, playersGrids[0].gridCells[i].cellPosition).cellType = CellType.Bedrock;
                }
                else if (rdm <= chanceRareRock)
                {
                    GetCellByPostion(0, playersGrids[0].gridCells[i].cellPosition).cellType = CellType.Rare;
                }
                else
                {
                    GetCellByPostion(0, playersGrids[0].gridCells[i].cellPosition).cellType = CellType.Normal;
                }
            }
        }
    }

    void SpawnMap()
    {
        foreach (Cell cells in playersGrids[0].gridCells)
        {
            SpawnRock(0, GetCellByPostion(0, cells.cellPosition).cellPosition, cells.cellType);
        }
    }

    void DuplicateMap()
    {
        Transform newGridInst = Instantiate(gridParent, new Vector2(gridSize.x + 500f, 0), Quaternion.identity);
        Grid newGrid = new Grid();

        for (int j = 0; j < newGridInst.childCount; j++)
        {
            Cell newCell = new Cell();
            newCell.cellGameObject = newGridInst.GetChild(j).gameObject;
            newCell.cellPosition = playersGrids[0].gridCells[j].cellPosition;
            newCell.cellName = playersGrids[0].gridCells[j].cellName;
            newCell.cellType = playersGrids[0].gridCells[j].cellType;
            newCell.rockLife = playersGrids[0].gridCells[j].rockLife;
            newGrid.gridCells.Add(newCell);
        }

        playersGrids.Add(newGrid);
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playerCount; i++)
        {
            GameObject newPlayer = Instantiate(prefabPlayers[i], GetCellByPostion(i, playersStartCell).cellGameObject.transform.position, Quaternion.identity);
            newPlayer.GetComponent<Script_ControlManager>().SetPlayerNum(i);
            newPlayer.GetComponent<Script_ControlManager>().SetPlayerCell(GetCellByPostion(i, playersStartCell));
            SetCellType(i, GetCellByPostion(i, playersStartCell).cellPosition, CellType.Player, null, null);
            cameras[i].position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, -10f);
            playersGold.Add(0);
            Script_UIManager.Instance.UpdateGoldText(i, playersGold[i]);
        }
    }

    public Cell GetCellByPostion(int playerGrid, Vector2 cellPos)
    {
        return playersGrids[playerGrid].gridCells.Find(x => x.cellPosition == cellPos);
    }

    public void SpawnRock(int playerGrid, Vector2 cellPos, CellType newCellType)
    {
        Cell newCell = GetCellByPostion(playerGrid, cellPos);
        Vector2 cellSpawnPos;
        Vector2 cellScreenPos;

        if (newCell.cellGameObject != null)
        {
            cellSpawnPos = newCell.cellGameObject.transform.position;
            cellScreenPos = cameras[playerGrid].GetComponent<Camera>().WorldToViewportPoint(newCell.cellGameObject.transform.position);

            if(cellScreenPos.x <= 0.1f)
            {
              UnzoomCam(playerGrid);
            }
            else if (cellScreenPos.x >= 0.9f)
            {
              UnzoomCam(playerGrid);
            }
            else if (cellScreenPos.y <= 0.1f)
            {
                UnzoomCam(playerGrid);
            }
            else if (cellScreenPos.y >= 0.9f)
            {
                UnzoomCam(playerGrid);
            }

            Destroy(newCell.cellGameObject);
        }
        else
        {
            cellSpawnPos = newCell.cellPosition;
        }

        if(newCell.larbnainIn != null)
        {
            newCell.larbnainIn.Die();
        }

        switch (newCellType)
        {
            case CellType.Bedrock:
                newCell.cellGameObject = Instantiate(prefabBedRock, cellSpawnPos, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Bedrock;
                newCell.rockLife = bedRockLife;
                break;

            case CellType.Empty:
                newCell.cellGameObject = Instantiate(prefabGround, cellSpawnPos, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Empty;
                break;

            case CellType.Normal:
                newCell.cellGameObject = Instantiate(prefabNormalRock, cellSpawnPos, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Normal;
                newCell.rockLife = normalRockLife;
                break;

            case CellType.Rare:
                newCell.cellGameObject = Instantiate(prefabRareRock, cellSpawnPos, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Rare;
                newCell.rockLife = rareRockLife;
                break;

            case CellType.Garbage:
                newCell.cellGameObject = Instantiate(prefabGarbageRock, cellSpawnPos, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Garbage;
                newCell.rockLife = garbageRockLife;
                break;
        }
    }

    public void SetCellType(int playerGrid, Vector2 cellPos, CellType newCellType, Script_Larbnain larbnain,Script_NainPloseur nainPloseur)
    {
        Cell newCell = GetCellByPostion(playerGrid, cellPos);

        switch (newCellType)
        {
            case CellType.Bedrock:
                newCell.cellType = CellType.Bedrock;
                newCell.larbnainIn = null;
                break;

            case CellType.Empty:
                newCell.cellType = CellType.Empty;
                newCell.larbnainIn = null;
                break;

            case CellType.Normal:
                newCell.cellType = CellType.Normal;
                newCell.rockLife = normalRockLife;
                newCell.larbnainIn = null;
                break;

            case CellType.Rare:
                newCell.cellType = CellType.Rare;
                newCell.rockLife = rareRockLife;
                newCell.larbnainIn = null;
                break;

            case CellType.Larbnain:
                newCell.cellType = CellType.Larbnain;
                newCell.larbnainIn = larbnain;
                break;

            case CellType.NainPloseur:
                newCell.cellType = CellType.NainPloseur;
                newCell.nainPloseurIn = nainPloseur;
                break;

            case CellType.Player:
                newCell.cellType = CellType.Player;
                break;

            case CellType.Garbage:
                newCell.cellType = CellType.Garbage;
                break;
        }
    }

    public void DamageARock(Vector2 cellPos, int damage, int playerIdx)
    {
        Cell cellDig = GetCellByPostion(playerIdx, cellPos);
        cellDig.rockLife -= damage;

        if(cellDig.rockLife <= 0)
        {
            int goldAdd = 0;

            switch(cellDig.cellType)
            {
                case CellType.Normal:
                    goldAdd = normalRockGoldValue;
                    break;

                case CellType.Rare:
                    goldAdd = rareRockGoldValue;
                    break;

                case CellType.Bedrock:
                    goldAdd = bedRockGoldValue;
                    break;
            }

            AddGold(goldAdd,playerIdx);

            SpawnRock(playerIdx, cellDig.cellPosition, CellType.Empty);

        }
    }

    void AddGold(int goldValue, int playerIdx)
    {
        playersGold[playerIdx] += goldValue;
        Script_UIManager.Instance.UpdateGoldText(playerIdx, playersGold[playerIdx]);
    }

    public void SpawnANainPloseur(int playerIdx, Cell targetCell, int sender)
    {
        if (playersGold[sender] >= nainploseurCost && targetCell.cellType != CellType.Player && targetCell.cellType != CellType.Larbnain && targetCell.cellType != CellType.NainPloseur)
        {
            GameObject newNainPloseur = Instantiate(prefabNainPloseur[playerIdx], targetCell.cellGameObject.transform.position, Quaternion.identity);
            newNainPloseur.GetComponent<Script_NainPloseur>().SetVariables(targetCell, playerIdx);
            playersGold[sender] -= nainploseurCost;
            Script_UIManager.Instance.UpdateGoldText(sender, playersGold[sender]);
        }
    }

    public void SpawnALarbnain(int playerIdx, Cell targetCell, Direction larbnainDirection)
    {
        if(playersGold[playerIdx] >= larbnainCost)
        {
            GameObject newLarbnain = Instantiate(larbnains[playerIdx], targetCell.cellGameObject.transform.position, Quaternion.identity);
            newLarbnain.GetComponent<Script_Larbnain>().SetVariables(targetCell, playerIdx, larbnainDirection);
            playersGold[playerIdx] -= larbnainCost;
            Script_UIManager.Instance.UpdateGoldText(playerIdx, playersGold[playerIdx]);
        }
    }

    void UnzoomCam(int playerIdx)
    {
        cameras[playerIdx].GetComponent<Camera>().orthographicSize += cameraUnzoomStep;
    }

    public int GetPlayerEmptyCell(int playerIdx)
    {
        int cellNum = 0;

        foreach(Cell cells in playersGrids[playerIdx].gridCells)
        {
            if(cells.cellType == CellType.Empty)
            {
                cellNum++;
            }
        }

        return cellNum;
    }
}

[System.Serializable]
public class Cell
{
    public string cellName;
    public Vector2 cellPosition;
    public GameObject cellGameObject;
    public Script_Larbnain larbnainIn;
    public Script_NainPloseur nainPloseurIn;
    public CellType cellType;
    public int rockLife = 1;
}

[System.Serializable]
public class Grid
{
    public List<Cell> gridCells = new List<Cell>();
}
