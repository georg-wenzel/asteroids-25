[System.Serializable]
public class LeaderboardEntry
{
    public int id;
    public string nickname;
    public string placement;
    public string score;
}

[System.Serializable]
public class LeaderboardEntries
{
    public LeaderboardEntry[] entries;
}
