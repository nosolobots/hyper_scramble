using UnityEngine;

public class LevelMapManager : PersistentSingleton<LevelMapManager>
{
    [SerializeField] GameObject[] levelMapsList;
    [SerializeField] int nextLevelMap = 0;

    public int NextLeveLMap { get => nextLevelMap; set => nextLevelMap = value; }

    void InstantiateMap()
    {
        if (levelMapsList.Length == 0) return;

        if (nextLevelMap < 0 || nextLevelMap >= levelMapsList.Length)
        {
            nextLevelMap = 0;
        }

        Instantiate(levelMapsList[nextLevelMap], Vector3.zero, Quaternion.identity);
    }

    public void LoadNextMap()
    {
        if (nextLevelMap >= levelMapsList.Length)
        {
            nextLevelMap = 0;
        }

        nextLevelMap++;

        InstantiateMap();
    }
}
