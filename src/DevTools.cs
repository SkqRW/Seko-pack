namespace SKTools;

public static class DevTools
{
    public static void LogDebug(string message)
    {
        UnityEngine.Debug.Log("[Seko Pack] " + message);
    }
}