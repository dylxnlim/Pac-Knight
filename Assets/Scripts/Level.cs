using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int testLength;
    public Vector3Int startPos;
    public Vector3Int endPos;
    public Tilemap wallMap;
    public Tilemap pelletMap;
    public Tilemap nodeMap;
    public Tilemap pathMap;
    public Tilemap nodeCastMap;
    
    //Tiles
    public Tile bush;
    public Tile grass;
    public Tile rock;
    public Tile path;
    public Tile entranceLeft;
    public Tile entranceRight;
    public TileBase node;
    public Tile nodeCast;
    public Tile caveWallTop;
    public Tile caveWallBottom;
    public Tile caveCeilingLeft;
    public Tile caveCeilingRight;
    public Tile caveCeilingTopLeft;
    public Tile caveCeilingTopRight;
    public Tile caveWallBottomLeft;
    public Tile caveWallBottomRight;

    //Item
    public TileBase pellet;
    public TileBase sword;
    public TileBase boot;
    public TileBase clock;
    public TileBase heart;

    //List
    public List<Vector3Int> spawnZone;
    public List<Vector3Int> generatedPellets;
    public List<Vector3Int> generatedNodes;
    public List<Vector3Int> generatedPaths;
    public List<Vector3Int> generatedNodeCasts;

    //Perlin noise generation settings
    private int randOffset;
    public int xOffset;
    public int yOffset;
    public float magnify;
    List<Tile> tileList;

    [ContextMenu("Generate")]
    public void Generate(int rounds)
    {
        RemoveGeneratedPellets();
        RemoveGeneratedNodes();
        RemoveGeneratedPaths();
        generatedPellets = new List<Vector3Int>();
        generatedNodes = new List<Vector3Int>();
        generatedPaths = new List<Vector3Int>();

 

        randOffset = Random.Range(0, 10);
        FillLevel();
        
        PathLeft(new Vector3Int(-5, 6, 0), testLength * rounds);
        PathRight(new Vector3Int(4, 6, 0), testLength * rounds);
        PathLeft(new Vector3Int(-5, 12, 0), testLength * rounds);
        PathRight(new Vector3Int(4, 12, 0), testLength * rounds);
        if (Random.value < 0.50f)
        {
            pelletMap.SetTile(new Vector3Int(-5, 12, 0), null);
            DeterminePowerUp(new Vector3Int(-5, 12, 0));
        }
        else
        {
            pelletMap.SetTile(new Vector3Int(4, 12, 0), null);
            DeterminePowerUp(new Vector3Int(4, 12, 0));
        }
    }

    private void Awake()
    {
        AddSpawnZone();
        tileList = new List<Tile>();
        tileList.Add(bush);
        tileList.Add(grass);
        tileList.Add(rock);
    }

    private void AddSpawnZone()
    {
        for(int i = -5; i < 5; i++)
        {
            for(int j = 6; j < 13; j++)
            {
                spawnZone.Add(new Vector3Int(i, j, 0));
            }
        }
    }

    private void GenerateSpawnZone()
    {
        //Path
        for(int i = -5; i < 5; i++)
        {
            pathMap.SetTile(new Vector3Int(i, 6, 0), path);
            pathMap.SetTile(new Vector3Int(i, 12, 0), path);
        }
        for(int j = 7; j < 12; j++)
        {
            pathMap.SetTile(new Vector3Int(-5, j, 0), path);
            pathMap.SetTile(new Vector3Int(4, j, 0), path);
        }
        //Cave
        for (int i = -3; i < 3; i++)
        {
            wallMap.SetTile(new Vector3Int(i, 11, 0), caveWallTop);
            wallMap.SetTile(new Vector3Int(i, 7, 0), caveWallBottom);
        }
        for (int j = 8; j < 11; j++)
        {
            wallMap.SetTile(new Vector3Int(-4, j, 0), caveCeilingRight);
            wallMap.SetTile(new Vector3Int(3, j, 0), caveCeilingRight);
        }
        wallMap.SetTile(new Vector3Int(-4, 11, 0), caveCeilingTopLeft);
        wallMap.SetTile(new Vector3Int(3, 11, 0), caveCeilingTopRight);
        wallMap.SetTile(new Vector3Int(-4, 7, 0), caveWallBottomLeft);
        wallMap.SetTile(new Vector3Int(3, 7, 0), caveWallBottomRight);
        wallMap.SetTile(new Vector3Int(-1, 11, 0), entranceLeft);
        wallMap.SetTile(new Vector3Int(0, 11, 0), entranceRight);
    }

    private void RemoveGeneratedPellets()
    {
        for(int i = 0; i < generatedPellets.Count; i++)
        {
            pelletMap.SetTile(generatedPellets[i], null);
        }
    }
    private void RemoveGeneratedNodes()
    {
        for (int i = 0; i < generatedNodes.Count; i++)
        {
            nodeMap.SetTile(generatedNodes[i], null);
            nodeCastMap.SetTile(generatedNodeCasts[i], null);
        }
    }
    private void RemoveGeneratedPaths()
    {
        for (int i = 0; i < generatedPaths.Count; i++)
        {
            pathMap.SetTile(generatedPaths[i], null);
        }
    }
    public int tileIdFromPerlin(int x, int y)
    {
        float raw = Mathf.PerlinNoise
        (
            ((x - xOffset + randOffset) / magnify),
            ((y - yOffset + randOffset) / magnify)
        );

        float clamp = Mathf.Clamp(raw, 0.0f, 1.0f);

        float scalePerlin = clamp * tileList.Count;
        if(scalePerlin == tileList.Count + 1)
        {
            scalePerlin = tileList.Count;
        }
        
        return Mathf.FloorToInt(scalePerlin);
    }
    public void FillLevel()
    {
        for (int i = startPos.x; i < endPos.x; i++)
        {
            for (int j = endPos.y; j < startPos.y; j++)
            {
                if (!spawnZone.Contains(new Vector3Int(i, j, 0)))
                {
                    int tileId = tileIdFromPerlin(i, j);
                    wallMap.SetTile(new Vector3Int(i, j, 0), tileList[tileId]);
                }
            }
        }
    }

    private bool checkPelletPos(Vector3Int pos)
    {
        for (int i = 0; i < generatedPellets.Count; i++)
        {
            if (generatedPellets[i] == pos)
            {
                return true;
            }
        }
        return false;
    }

    private void PathLeft(Vector3Int pos, int count)
    {
        int loops = Random.Range(2, 6);
        int counter = 0;

        nodeMap.SetTile(pos, node);
        nodeCastMap.SetTile(pos, nodeCast);

        if (loops > 0)
        {
            for (int i = 0; i < loops; i++)
            {
                if(!spawnZone.Contains(pos))
                {
                    pelletMap.SetTile(pos, pellet);
                    generatedPellets.Add(pos);

                    if (pos.x > startPos.x + 2)
                    {
                        pos.x = pos.x - 1;
                        wallMap.SetTile(pos, null);
                        pathMap.SetTile(pos, path);
                        generatedPaths.Add(pos);
                        counter = counter + 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }
        
        if(count > 0)
        {
            if (Random.value < 0.5f)
            {
                PathDown(pos, count - counter);
            }
            else
            {
                PathUp(pos, count - counter);
            }
        }
        else
        {
            nodeMap.SetTile(pos, node);
            generatedNodes.Add(pos);
            nodeCastMap.SetTile(pos, nodeCast);
            generatedNodeCasts.Add(pos);

            pelletMap.SetTile(pos, null);
            DeterminePowerUp(pos);

            if (checkPelletPos(pos + new Vector3Int (0, 1, 0)) == false || checkPelletPos(pos + new Vector3Int (0, -1, 0)) == false)
            {
                if (Random.value < 0.5f)
                {
                    PathDown(pos, 5);
                }
                else
                {
                    PathUp(pos, 5);
                }
            }
        }
    }

    private void PathRight(Vector3Int pos, int count)
    {
        int loops = Random.Range(2, 6);
        int counter = 0;

        nodeMap.SetTile(pos, node);
        nodeCastMap.SetTile(pos, nodeCast);

        if (loops > 0)
        {
            for (int i = 0; i < loops; i++)
            {
                if (!spawnZone.Contains(pos)) 
                {
                    pelletMap.SetTile(pos, pellet);
                    generatedPellets.Add(pos);

                    if (pos.x < endPos.x - 2)
                    {
                        pos.x = pos.x + 1;
                        wallMap.SetTile(pos, null);
                        pathMap.SetTile(pos, path);
                        generatedPaths.Add(pos);
                        counter = counter + 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        if (count > 0)
        {
            if (Random.value < 0.5f)
            {
                PathDown(pos, count - counter);
            }
            else
            {
                PathUp(pos, count - counter);
            }
        }
        else
        {
            nodeMap.SetTile(pos, node);
            generatedNodes.Add(pos);
            nodeCastMap.SetTile(pos, nodeCast);
            generatedNodeCasts.Add(pos);

            pelletMap.SetTile(pos, null);
            DeterminePowerUp(pos);

            if (checkPelletPos(pos + new Vector3Int (0, 1, 0)) == false || checkPelletPos(pos + new Vector3Int (0, -1, 0)) == false)
            {
                if (Random.value < 0.5f)
                {
                    PathDown(pos, 5);
                }
                else
                {
                    PathUp(pos, 5);
                }
            }
        }
    }

    private void PathUp(Vector3Int pos, int count)
    {
        int loops = Random.Range(5, 8);
        int counter = 0;

        nodeMap.SetTile(pos, node);
        nodeCastMap.SetTile(pos, nodeCast);

        if (loops > 0)
        {
            for (int i = 0; i < loops; i++)
            {
                if(!spawnZone.Contains(pos))
                {
                    pelletMap.SetTile(pos, pellet);
                    generatedPellets.Add(pos);

                    if (pos.y < startPos.y - 2)
                    {
                        pos.y = pos.y + 1;
                        wallMap.SetTile(pos, null);
                        pathMap.SetTile(pos, path);
                        generatedPaths.Add(pos);
                        counter = counter + 1;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        if (count > 0)
        {
            if (Random.value < 0.5f)
            {
                PathLeft(pos, count - counter);
            }
            else
            {
                PathRight(pos, count - counter);
            }
        }
        else
        {
            nodeMap.SetTile(pos, node);
            generatedNodes.Add(pos);
            nodeCastMap.SetTile(pos, nodeCast);
            generatedNodeCasts.Add(pos);

            pelletMap.SetTile(pos, null);
            DeterminePowerUp(pos);

            if (checkPelletPos(pos + new Vector3Int (1, 0, 0)) == false || checkPelletPos(pos + new Vector3Int (-1, 0, 0)) == false)
            {
                if (Random.value < 0.5f)
                {
                    PathDown(pos, 5);
                }
                else
                {
                    PathUp(pos, 5);
                }
            }
        }
    }

    private void PathDown(Vector3Int pos, int count)
    {
        int loops = Random.Range(4, 8);
        int counter = 0;

        nodeMap.SetTile(pos, node);
        nodeCastMap.SetTile(pos, nodeCast);

        if (loops > 0)
        {
            for (int i = 0; i < loops; i++)
            {
                pelletMap.SetTile(pos, pellet);
                generatedPellets.Add(pos);

                if (pos.y > endPos.y + 2)
                {
                    pos.y = pos.y - 1;
                    wallMap.SetTile(pos, null);
                    pathMap.SetTile(pos, path);
                    generatedPaths.Add(pos);
                    counter = counter + 1;
                }
                else
                {
                    break;
                }
            }
        }

        if (count > 0)
        {
            if (Random.value < 0.5f)
            {
                PathLeft(pos, count - counter);
            }
            else
            {
                PathRight(pos, count - counter);
            }
        }
        else
        {
            nodeMap.SetTile(pos, node);
            generatedNodes.Add(pos);
            nodeCastMap.SetTile(pos, nodeCast);
            generatedNodeCasts.Add(pos);

            pelletMap.SetTile(pos, null);
            DeterminePowerUp(pos);

            if (checkPelletPos(pos + new Vector3Int (1, 0, 0)) == false || checkPelletPos(pos + new Vector3Int (-1, 0, 0)) == false)
            {
                if (Random.value < 0.5f)
                {
                    PathDown(pos, 5);
                }
                else
                {
                    PathUp(pos, 5);
                }
            }
        }
    }
    
    private void DeterminePowerUp(Vector3Int pos)
    {
        if (Random.value < 0.25f)
        {
            pelletMap.SetTile(pos, sword);
        }
        else if (Random.value >= 0.25f && Random.value < 0.50f)
        {
            pelletMap.SetTile(pos, boot);
        }
        else if (Random.value >= 0.50f && Random.value < 0.75f)
        {
            pelletMap.SetTile(pos, clock);
        }
        else
        {
            pelletMap.SetTile(pos, heart);
        }
        generatedPellets.Add(pos);
    }
}
