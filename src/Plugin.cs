using BepInEx;
using BepInEx.Logging;
using System;
using System.Security.Permissions;

// Allows access to private members
#pragma warning disable CS0618
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace SekoPack;

[BepInPlugin(MODID, MODNAME, MODVERSION), BepInDependency("io.github.dual.fisobs")]
public class ModPlugin : BaseUnityPlugin
{
    public static new ManualLogSource Logger;

    public const string MODID = "seko.pack";
    public const string MODNAME = "Seko Pack";
    public const string MODVERSION = "000";
    static bool IsInit;

    public void OnEnable()
    {
        Logger = base.Logger;
        On.RainWorld.OnModsInit += OnModsInit;
        RegisterFisobs(); 
    }

    private static void OnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        if (IsInit) return;
        IsInit = true;

        Logger.LogDebug($"[{MODID}] {MODNAME} Hello world! {MODVERSION}");

        LoadResources();
    }

    private static void RegisterFisobs()
    {
        
        Logger.LogDebug($"[{MODID}] {MODNAME} Registering Fisobs...");



        Logger.LogDebug($"[{MODID}] {MODNAME} Fisobs registered.");
    }

    private static void LoadResources()
    {
        
    }
}
