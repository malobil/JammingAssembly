using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType { Empty, Normal, Rare, Bedrock }

public class Script_GameManager : MonoBehaviour
{
    public static Script_GameManager Instance { get; private set; }

    public Vector2 gridSize = new Vector2(10, 10);
    public int playersStartSpace = 5;
    public float gridCellSize = 64f;
    public int playerCount = 2;
    public int decalage = 0;

    public Transform gridParent;
    private int currentDecal = 0;

    public GameObject prefabNormalRock;
    public GameObject prefabGround;
    public GameObject prefabRareRock;
    public GameObject prefabBedRock;

    public GameObject prefabPlayer;

    public List<Transform> cameraPlayerOne;


    [Range(0, 100)]
    public int chanceRareRock = 35;

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
        CreateGrid();
        AttributeBaseMap();
        AttributeBedRock();
        SpawnMap();
        DuplicateMap();
        SpawnPlayers();

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
        for(int i = 0; i < playersGrids[0].gridCells.Count; i++)
        {
            if (playersGrids[0].gridCells[i].cellPosition.x >= playersStartCell.x - playersStartSpace && playersGrids[0].gridCells[i].cellPosition.x <= playersStartCell.x + playersStartSpace
                && playersGrids[0].gridCells[i].cellPosition.y >= playersStartCell.y - playersStartSpace && playersGrids[0].gridCells[i].cellPosition.y <= playersStartCell.y + playersStartSpace)
            {
                GetCellByPostion(0, playersGrids[0].gridCells[i].cellPosition).cellType = CellType.Empty;
            }
            else
            {
                int rdm = Random.Range(1, 101);

                if (rdm <= chanceRareRock)
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
        foreach(Cell cells in playersGrids[0].gridCells)
        {
            SpawnRock(0, GetCellByPostion(0, cells.cellPosition).cellPosition, cells.cellType);
        }
    }

    void DuplicateMap()
    {
        for(int i = 0; i < playerCount-1; i++)
        {
            Instantiate(gridParent, new Vector2(gridSize.x + 100f * i, 0), Quaternion.identity);
            playersGrids.Add(playersGrids[0]);
        }
    }

    void SpawnPlayers()
    {
        for (int i = 0; i < playerCount-1; i++)
        {
            GameObject newPlayer = Instantiate(prefabPlayer, GetCellByPostion(0, playersStartCell).cellGameObject.transform.position, Quaternion.identity);
            cameraPlayerOne[i].position = new Vector3(newPlayer.transform.position.x, newPlayer.transform.position.y, -10f);
            
        }
    }

    void AttributeBedRock()
    {
        for(int x = 0; x < gridSize.x / decalage; x++)
        {
            for (int i = currentDecal; i < gridSize.x - currentDecal; i++)
            {
                GetCellByPostion(0, new Vector2(currentDecal, i)).cellType = CellType.Bedrock;
                GetCellByPostion(0, new Vector2((gridSize.x - 1) - currentDecal, i)).cellType = CellType.Bedrock;
                GetCellByPostion(0, new Vector2(i, currentDecal)).cellType = CellType.Bedrock;
                GetCellByPostion(0, new Vector2(i, (gridSize.y - 1) - currentDecal)).cellType = CellType.Bedrock;
            }

            currentDecal += decalage;

            Debug.Log("spawn bed");
        }
       
    }

    public Cell GetCellByPostion(int playerGrid, Vector2 cellPos)
    {
        return playersGrids[playerGrid].gridCells.Find(x => x.cellPosition == cellPos);
    }

    void AttributeRock(int playerGrid, Vector2 cellPos, CellType newCellType)
    {

    }

    public void SpawnRock(int playerGrid, Vector2 cellPos, CellType newCellType)
    {
        Cell newCell = GetCellByPostion(playerGrid, cellPos);

        if(newCell.cellGameObject != null)
        {
            Destroy(newCell.cellGameObject);
        }

        switch (newCellType)
        {
            case CellType.Bedrock:
                newCell.cellGameObject = Instantiate(prefabBedRock, newCell.cellPosition, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Bedrock;
                break;

            case CellType.Empty:
                newCell.cellGameObject = Instantiate(prefabGround, newCell.cellPosition, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Empty;
                break;

            case CellType.Normal:
                newCell.cellGameObject = Instantiate(prefabNormalRock, newCell.cellPosition, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Normal;
                break;

            case CellType.Rare:
                newCell.cellGameObject = Instantiate(prefabRareRock, newCell.cellPosition, Quaternion.identity, gridParent);
                newCell.cellType = CellType.Rare;
                break;
        }
    }
}

[System.Serializable]
public class Cell
{
    public string cellName;
    public Vector2 cellPosition;
    public GameObject cellGameObject;
    public CellType cellType;
}

[System.Serializable]
public class Grid
{
    public List<Cell> gridCells = new List<Cell>();
}
