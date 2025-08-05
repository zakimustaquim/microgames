using System.Collections;
using UnityEngine;

public class GridManager : LoggingMonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 20;
    [SerializeField] private int gridHeight = 20;
    private Transform[,] gridPositions;

    [SerializeField] private GameObject cellPrefab;

    private void Start()
    {
        DiscoverGrid();
        StartCoroutine(CreateTestGrid());
    }

    IEnumerator CreateTestGrid()
    {
        log("Creating grid...");

        float interval = 0.01f;
        for (int i = 0; i < gridPositions.GetLength(0); i++)
        {
            for (int j = 0; j < gridPositions.GetLength(1); j++)
            {
                // Transform cell = Instantiate(cellPrefab, transform).transform;
                // cell.name = $"Cell_{i}_{j}";
                // cell.localPosition = new Vector3(i, j, 0);
                // grid[i, j] = cell;

                Vector3 curr = gridPositions[i, j].position + new Vector3(0.25f, 0.25f, 0);
                // curr = Vector3.Scale(curr, new Vector3(1, 2, 1));
                log($"Spawning a new cell from {i}, {j}'s position: {curr}", 3);
                Instantiate(cellPrefab, curr, Quaternion.identity);

                yield return new WaitForSeconds(interval);
            }
        }

        log("Grid creation complete.");
    }

    private void DiscoverGrid()
    {
        // get children of this game object
        int childCount = gameObject.transform.childCount;
        if (childCount != gridHeight)
        {
            err($"Expected {gridHeight} children, but found {childCount}.", 5);
            return;
        }

        gridPositions = new Transform[gridWidth, gridHeight];
        for (int i = 0; i < childCount; i++)
        {
            Transform row = transform.GetChild(i);
            if (row.childCount != gridWidth)
            {
                err($"Expected {gridWidth} children in row {i}, but found {row.childCount}.", 5);
                return;
            }
            for (int j = 0; j < row.childCount; j++)
            {
                Transform cell = row.GetChild(j);
                gridPositions[i, j] = cell;
            }
        }
        log("Successfully discovered grid.", 5);
    }
}
