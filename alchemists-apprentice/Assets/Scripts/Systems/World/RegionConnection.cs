/// <summary>
/// A class representing a connection from
/// one region to another.
/// </summary>
[System.Serializable]
public class RegionConnection
{
    public string connectionName;
    public WorldRegion targetRegion;
    public string entranceId; // optional identifier for specific entrance points
}
