namespace Backend.Models;

public class SteamApiResponse
{
    public AppList Applist { get; set; }
}

public class AppList
{
    public List<SteamApp> Apps { get; set; }
}

public class SteamApp
{
    public int Appid { get; set; }
    public string Name { get; set; }
}