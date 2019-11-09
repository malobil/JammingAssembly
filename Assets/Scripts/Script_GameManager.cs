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

    public GameObject prefabNormalRock;
    public GameObject prefabGround;
    public GameObject prefabRareRock;
    public GameObject prefabBedRock;

    public GameObject prefabPlayer;


    [Range(0, 100)]
    public int chanceRareRock = 35;

    private List<Grid> playersGrids = new List<Grid>();
    private Vector2 playersStartCell;

    private void Awake()
    {
        if(Instance == null)
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
    }

    void CreateGrid()
    {
        playersStartCell = new Vector2(Mathf.FloorToInt(gridSize.x/2), Mathf.FloorToInt(gridSize.y/2));
        GameObject gridBaseParent = new GameObject();
        gridBaseParent.name = "PlayerGrid";
        Grid newGrid = new Grid();

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject newCellInst;
                Cell newCell = new Cell();
                newCell.cellPosition = new Vector2(x, y);
                newCell.cellName = (x + "/" + y);
                newGrid.gridCells.Add(newCell);


                if (x >= playersStartCell.x - playersStartSpace && x <= playersStartCell.x + playersStartSpace
                 && y >= playersStartCell.y - playersStartSpace && y <= playersStartCell.y + playersStartSpace)
                {
                    newCell.cellType = CellType.Empty;
                    newCellInst = Instantiate(prefabGround, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity);
                }
                else
                {
                    int rdm = Random.Range(1, 101);

                    if (rdm <= chanceRareRock)
                    {
                        newCellInst = Instantiate(prefabRareRock, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity);
                        newCell.cellType = CellType.Rare;
                    }
                    else
                    {
                        newCellInst = Instantiate(prefabNormalRock, new Vector2(x * gridCellSize, y * gridCellSize), Quaternion.identity);
                        newCell.cellType = CellType.Normal;
                    }
                }
                
                newCellInst.name = newCell.cellName;
                newCellInst.transform.SetParent(gridBaseParent.transform);
                newCell.cellGameObject = newCellInst;

            }
        }

        playersGrids.Add(newGrid);

        for (int i = 0; i < playerCount - 1; i++)
        {
            Instantiate(gridBaseParent, new Vector2(gridSize.x + 100f, 0), Quaternion.identity);
            playersGrids.Add(playersGrids[0]);
        }

        SpawnPlayers();
    }

    void SpawnPlayers()
    {
        for(int i = 0; i < playerCount; i++)
        {
            Debug.Log(GetCellByPostion(i,playersStartCell));
            Instantiate(prefabPlayer, GetCellByPostion(i, playersStartCell).cellGameObject.transform.position, Quaternion.identity);
        }
    }

    public Cell GetCellByPostion(int playerGrid, Vector2 cellPos)
    {
        return playersGrids[playerGrid].gridCells.Find(x => x.cellPosition == playersStartCell) ;
    }
}

[System.Serializable]
public class Cell
{
    public string cellName;
    public Vector2 cellPosition;
    public GameObject cellGameObject ;
    public CellType cellType;
}

[System.Serializable]
public class Grid
{
    public List<Cell> gridCells = new List<Cell>();
}
