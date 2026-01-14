using BepInEx;
using BepInEx.Logging;
using System.Security.Permissions;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace SekoPack;

[BepInPlugin(MODID, MODNAME, MODVERSION)]
public class ModPlugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    public const string MODID = "seko.pack";
    public const string MODNAME = "Seko Pack";
    public const string MODVERSION = "000";
    bool IsInit;

    public void OnEnable()
    {
        Logger = base.Logger;
        On.RainWorld.OnModsInit += OnModsInit;
    }

    private void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        if (IsInit) return;
        IsInit = true;

        // Initialize assets, your mod config, and anything that uses RainWorld here
        Logger.LogDebug($"[{MODID}] {MODNAME} Hello world! {MODVERSION}");
    }
}
