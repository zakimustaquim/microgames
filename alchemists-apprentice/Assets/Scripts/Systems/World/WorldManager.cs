using System;
using System.Collections;
using UnityEditor.MemoryProfiler;
using UnityEngine.SceneManagement;

public class WorldManager : LoggingMonoBehaviour
{
    public static WorldManager Instance { get; private set; }
    public WorldRegion currentRegion;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private IEnumerator LoadRegion(WorldRegion newRegion)
    {
        // optional: fade out here
        yield return SceneManager.LoadSceneAsync(newRegion.sceneName);
        currentRegion = newRegion;
        // optional: fade in and reposition player spawn
    }

    public void MoveToRegion(RegionConnection regionConnection)
    {
        if (currentRegion == null)
        {
            warn("Current region is not set.");
            return;
        }
        if (regionConnection == null)
        {
            err($"Connection '{regionConnection}' not found in region '{currentRegion.regionName}'.");
            return;
        }

        StartCoroutine(LoadRegion(regionConnection.targetRegion));
    }
}
