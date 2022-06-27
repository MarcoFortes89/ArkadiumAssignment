using Assets.Scripts;
using Assets.Scripts.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class MatchManager : MonoBehaviour
{
    int gridLength = 4;
    float centerOffset;
    [Range(0.01f, 2f)]
    public float TimeBetweenSpawn = 0.1f;

    public List<Tile> tiles = new List<Tile>();

    public Transform SelectedContainer;

    public int PointsPerMatch = 100;
    public int CurrentScore = 0;

    private Tile selected;
    public Tile Selected
    {
        get { return selected; }
        set
        {
            selected = value;
            SelectedContainer.DestroyChildren();
            if (selected != null && SelectedContainer != null)
            {
                Instantiate(TileFactory.CreateTile(selected.TileType), SelectedContainer).layer = 5;
            }
        }
    }

    public EventHandler OnMultiplierChange, OnMultiplierDecay, OnSecondChange, OnScoreUpdated, OnPause;

    private int multiplier = 1;
    public int Multiplier
    {
        get { return multiplier; }
        set
        {
            if (multiplier != value)
            {
                if (value < 1)
                    multiplier = 1;
                else if (value > 5)
                    multiplier = 5;
                else
                    multiplier = value;
                OnMultiplierChange?.Invoke(multiplier, EventArgs.Empty);
            }
        }
    }


    Stopwatch multiplierTimer = new Stopwatch();
    public int MultiplierTimeout = 2;

    Stopwatch MatchWatch = new Stopwatch();
    TimeSpan matchTime;

    public bool IsPaused { get; private set; } = true;

    private void Start()
    {
        Core.Match = this;
        gridLength = Core.Stage.CubeSize;
        centerOffset = (gridLength - 1f) / 2;
        matchTime = new TimeSpan(0, 0, Core.Stage.Duration, 0);
        Transform cenario = GameObject.Find("Cenario").transform;
        cenario.localScale = new Vector3(1+ centerOffset*0.2f, 1 + centerOffset * 0.2f, 1 + centerOffset * 0.2f);
        cenario.position = new Vector3(0, - centerOffset ,0);
        TileFactory.Initialize();
        StartCoroutine(GenerateGrid());
    }

    int seconds = -1;
    private void Update()
    {
        if (multiplierTimer.IsRunning && multiplierTimer.ElapsedMilliseconds >= 1000 * MultiplierTimeout)
        {
            multiplierTimer.Restart();
            multiplierTimer.Stop();
            Multiplier = 1;
        }
        if (MatchWatch.Elapsed <= matchTime)
        {
            if (MatchWatch.Elapsed.Seconds != seconds)
            {
                seconds = MatchWatch.Elapsed.Seconds;
                OnSecondChange(matchTime - MatchWatch.Elapsed, EventArgs.Empty);
            }
        }
        else
            MatchWatch.Stop();
        if (multiplierTimer.IsRunning)
            OnMultiplierDecay(1 - (multiplierTimer.ElapsedMilliseconds + 0f) / (1000 * MultiplierTimeout), EventArgs.Empty);
    }

    IEnumerator GenerateGrid()
    {
        System.Random r = new System.Random();

        List<int> indexs = new List<int>();
        for (int i = 0; i < Mathf.Pow(gridLength, 3); i++)
        {
            indexs.Add(i);
        }

        for (int i = 0; i < Mathf.Pow(gridLength, 3) / 2; i++)
        {
            if (indexs.Count < 2)
                break;

            int currentPair = r.Next(1, Core.Stage.TileVariety);

            Tile tile1 = Instantiate(TileFactory.CreateTile(currentPair), transform).GetComponent<Tile>();
            int pos1Index = r.Next(0, indexs.Count - 1);
            tile1.transform.position = GetPosition(indexs[pos1Index]);
            tile1.OnSelected += EventTileSelected;
            tile1.Index = indexs[pos1Index];
            tile1.TileType = currentPair;
            tiles.Add(tile1);
            indexs.RemoveAt(pos1Index);

            yield return new WaitForSeconds(TimeBetweenSpawn);

            Tile tile2 = Instantiate(TileFactory.CreateTile(currentPair), transform).GetComponent<Tile>();
            int pos2Index = r.Next(0, indexs.Count - 1);
            tile2.transform.position = GetPosition(indexs[pos2Index]);
            tile2.OnSelected += EventTileSelected;
            tile2.Index = indexs[pos2Index];
            tile2.TileType = currentPair;
            tiles.Add(tile2);
            indexs.RemoveAt(pos2Index);
            yield return new WaitForSeconds(TimeBetweenSpawn);
        }
        AssignNeighborhood();
        MatchWatch.Start();
        IsPaused = false;
    }

    private void EventTileSelected(object sender, EventArgs e)
    {
        SelectTile(sender as Tile);
    }

    void SelectTile(Tile t)
    {
        if (t.IsValid)
        {
            if (Selected != null)
            {
                t.IsSelected = false;
                if (Selected.TileType == t.TileType && Selected != t)
                {
                    UpdateMultiplier();
                    AddScore();
                    Selected.Match(multiplier);
                    t.Match(multiplier);
                    Core.Audio.Play(ESfxType.Match, multiplier - 1);
                    tiles.Remove(Selected);
                    tiles.Remove(t);
                    if (tiles.Count == 0)
                    {
                        Pause();
                        GameOver();                     }
                    else
                        StartCoroutine(GameOverCheck());
                }
                else
                {
                    Selected.GetComponent<Animator>().SetTrigger("WrongClick");
                    t.GetComponent<Animator>().SetTrigger("WrongClick");
                    Core.Audio.Play(ESfxType.Wrong);
                }
                Selected = null;
            }
            else
            {
                t.IsSelected= true;
                Selected = t;
            }
        }
        else
        {
            t.GetComponent<Animator>().SetTrigger("WrongClick");
            Core.Audio.Play(ESfxType.Wrong);
            Selected = null;
        }
    }

    IEnumerator GameOverCheck()
    {
        List<Tile> validTiles = tiles.Where(x => x.IsValid).ToList();
        foreach (Tile t in validTiles)
        {
            if (validTiles.Where(x => x.TileType == t.TileType).Count() > 1)
                yield break;
        }
        Pause();
        Transform container = GameObject.Find("MenuContainer").transform;
        if (container.childCount < 1)
            Instantiate(Resources.Load("Prefabs/GAME OVER"), container);
        yield break;
    }

    private void GameOver()
    {
        SceneManager.LoadScene("End");
    }

    void UpdateMultiplier()
    {
        if (multiplierTimer.IsRunning)
        {
            Multiplier++;
            multiplierTimer.Restart();
        }
        else
            multiplierTimer.Start();
    }

    Vector3 GetPosition(int i)
    {
        int x = i % gridLength;
        int y = (int)Mathf.Floor((i + 0f) / Mathf.Pow(gridLength, 2)) % gridLength;
        int z = (int)Mathf.Floor((i + 0f) / gridLength) % gridLength;
        return new Vector3(x - centerOffset, y - centerOffset, z - centerOffset);
    }

    bool IsValidTile(Tile t)
    {
        int front_back = tiles.Find(x => x.IsFrontTile(t)) == null ? 0 : 1;
        front_back += tiles.Find(x => x.IsBackTile(t)) == null ? 0 : 1;

        int side = tiles.Find(x => x.IsRightTile(t)) == null ? 0 : 1;
        side += tiles.Find(x => x.IsLeftTile(t)) == null ? 0 : 1;
        return front_back < 2 && side < 2;
    }

    void AddScore()
    {
        CurrentScore += PointsPerMatch * multiplier;
        OnScoreUpdated?.Invoke(CurrentScore, EventArgs.Empty);
    }

    private void AssignNeighborhood()
    {

        foreach (Tile t in tiles)
        {
            t.Neighborhood = new Tile[4];
            int front = getFrontIndex(t.Index);
            int right = getRightIndex(t.Index);
            int back = getBackIndex(t.Index);
            int left = getLeftIndex(t.Index);
            t.Neighborhood[0] = tiles.Find(x => x.Index == front);
            t.Neighborhood[1] = tiles.Find(x => x.Index == right);
            t.Neighborhood[2] = tiles.Find(x => x.Index == back);
            t.Neighborhood[3] = tiles.Find(x => x.Index == left);
            foreach (Tile t_neighbor in t.Neighborhood)
            {
                if (t_neighbor != null)
                    t_neighbor.OnDestroyed += t.EventNeighborDestroyed;
            }
        }
    }

    int getFrontIndex(int index)
    {
        if ((index + 1) % gridLength == 0)
            return -1;
        else
            return index + 1;
    }

    int getBackIndex(int index)
    {
        if (index % gridLength == 0)
            return -1;
        else
            return index - 1;
    }

    int getRightIndex(int index)
    {
        if (index % (Mathf.Pow(gridLength, 2)) < gridLength)
            return -1;
        else
            return index - gridLength;
    }

    int getLeftIndex(int index)
    {
        if (index % (Mathf.Pow(gridLength, 2)) >= Mathf.Pow(gridLength, 2) - gridLength)
            return -1;
        else
            return index + gridLength;
    }

    public void Pause()
    {
        if (multiplierTimer.IsRunning)
            multiplierTimer.Stop();
        if (MatchWatch.IsRunning)
            MatchWatch.Stop();
        gameObject.SetActive(false);
        IsPaused = true;
        OnPause?.Invoke(null, EventArgs.Empty);
    }

    public void Resume()
    {
        IsPaused = false;
        gameObject.SetActive(true);
        MatchWatch.Start();
        multiplierTimer.Start();
    }
}