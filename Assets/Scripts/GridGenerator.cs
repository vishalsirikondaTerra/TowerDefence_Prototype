using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int gridSizeX = 10;
    public int gridSizeY = 10;
    public int gridSizeZ = 10;
    public float spacing = 1.5f;
    public List<GameObject> cubes;
    public bool On;
    void Start()
    {
        if (!On) { return; }
        cubes = new List<GameObject>();
        GenerateGrid();
    }
    void OnValidate()
    {
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 position = new Vector3(x * spacing, y * spacing, z * spacing);
                    var cube = Instantiate(cubePrefab, position, Quaternion.identity);
                    cube.SetActive(true);
                    cubes.Add(cube);
                }
            }
        }
    }
    void ClearAll()
    {
        if (cubes != null)
        {
            for (int i = 0; i < cubes.Count; i++)
            {
                // DestroyImmed(cubes[i]);
            }
        }
        cubes = new List<GameObject>();
    }
}
