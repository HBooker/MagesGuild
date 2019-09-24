using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPuzzle : MonoBehaviour
{
    public float tilePadding = 2.0f;
    public float marginLeft = 2.0f;
    public float marginTop = 2.0f;

    private List<SpriteRenderer> puzzleTiles;

    public int numShuffles = 5;
    public float timeBetweenShuffles = 1.0f;

    private int shufflesRemaining = 0;
    private float timeToShuffle = 0.0f;

    public int layerNumber = 8;

    private bool gameStarted = false;
    private List<Vector3> originalPositions;

    // Start is called before the first frame update
    void Start()
    {
        puzzleTiles = new List<SpriteRenderer>();
        originalPositions = new List<Vector3>();

        // The puzzle starts out solved, so we record the solved state for later comparisons.
        GetComponentsInChildren<SpriteRenderer>(puzzleTiles);
        puzzleTiles.ForEach(tile => originalPositions.Add(tile.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        timeToShuffle -= Time.deltaTime;

        // Pre-game shuffle animation
        if (timeToShuffle <= 0.0f && shufflesRemaining > 0)
        {
            ShuffleBoard();
            --shufflesRemaining;
            timeToShuffle = timeBetweenShuffles;

            // Start the game if this was the last shuffle
            gameStarted = (shufflesRemaining == 0);
        }

        if (!gameStarted)
        {
            return;
        }

        // On left-click
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseIntersectionWithTile();
        }

        // DEBUG - Solve the puzzle
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            SolvePuzzle();
        }

        bool victory = CheckWin();

        if (victory)
        {
            gameStarted = false;
            this.transform.parent.position = new Vector3(0.0f, 0.0f, -30.0f);
            this.SolvePuzzle();
        }
    }
    
    public void ShuffleBoard()
    {
        puzzleTiles = new List<SpriteRenderer>();
        GetComponentsInChildren<SpriteRenderer>(puzzleTiles);

        int tileCount = puzzleTiles.Count;
        List<SpriteRenderer> newTiles = new List<SpriteRenderer>();
        List<Vector3> tilePositions = new List<Vector3>();

        puzzleTiles.ForEach(tile => tilePositions.Add(CloneV3(tile.transform.position)));

        // Randomize tile positions
        for (int i = 0; i < tileCount; ++i)
        {
            int rIndex = (int)(Random.value * (tileCount - i - float.Epsilon));
            newTiles.Add(puzzleTiles[rIndex]);
            puzzleTiles.RemoveAt(rIndex);
            newTiles[i].transform.position = tilePositions[i];
        }

        puzzleTiles = newTiles;
    }

    private Vector3 CloneV3(Vector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    private void CheckMouseIntersectionWithTile()
    {
        RaycastHit hitInfo;
        int tileLayerMask = 1 << layerNumber;

        Vector3 from = Input.mousePosition - new Vector3(0.0f, 0.0f, 20.0f);
        Vector3 to = new Vector3(0.0f, 0.0f, 100.0f);

        // Cast a ray at the mouse position
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, tileLayerMask))
        {
            SwapTileWithBlank(hitInfo.transform.gameObject);
        }
    }

    private void SwapTileWithBlank(GameObject tile)
    {
        // Find the blank tile and swap positions with it
        GameObject blankTile = GameObject.Find("blank");
        Vector3 tmp = tile.transform.position;
        tile.transform.position = blankTile.transform.position;
        blankTile.transform.position = tmp;

        // Find both tiles and swap their positions in the board state array
        SpriteRenderer blankTileSR = blankTile.GetComponent<SpriteRenderer>();
        SpriteRenderer tileSR = tile.GetComponent<SpriteRenderer>();

        int blankIndex = puzzleTiles.IndexOf(blankTileSR);
        int tileIndex = puzzleTiles.IndexOf(tileSR);

        puzzleTiles[blankIndex] = tileSR;
        puzzleTiles[tileIndex] = blankTileSR;
    }

    public bool CheckWin()
    {
        List<SpriteRenderer> tiles = new List<SpriteRenderer>();
        GetComponentsInChildren<SpriteRenderer>(tiles);

        for (int i = 0; i < originalPositions.Count; ++i)
        {
            if (originalPositions[i] != tiles[i].transform.position)
            {
                return false;
            }
        }

        return true;
    }

    private void SolvePuzzle()
    {
        List<SpriteRenderer> tiles = new List<SpriteRenderer>();
        GetComponentsInChildren<SpriteRenderer>(tiles);

        // Set all tile positions to their original (solved) positions
        for (int i = 0; i < puzzleTiles.Count; ++i)
        {
            tiles[i].transform.position = originalPositions[i];
        }
    }

    public void StartGame()
    {
        // Set the conditions to begin a new shuffle animation and game start
        gameStarted = false;
        shufflesRemaining = numShuffles;
        timeToShuffle = timeBetweenShuffles;
    }
}
