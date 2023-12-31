using UnityEngine.Serialization;

[System.Serializable]
public class LeaderboardEntry
{
    public int id;
    public string nickname;
    public int placement;
    public int score;
    public int destroyed_enemies;
}

[System.Serializable]
public class LeaderboardEntries
{
    public LeaderboardEntry[] entries;
}
