using Watcher;
using Fisobs.Core;
using Fisobs.Creatures;
using System.Collections.Generic;
using DevInterface;

using UnityEngine;
using MoreSlugcats;
using Fisobs.Creatures;
using Fisobs.Core;
using Fisobs.Sandbox;
using static PathCost.Legality;
using UnityEngine;
using System.Collections.Generic;
using DevInterface;
using RWCustom;
using Random = UnityEngine.Random;
using System;
using ScavengerCosmetic;
using MonoMod.RuntimeDetour;

namespace SekoPack.Creatures.LeaderScavA;

public class Hooks
{
    public static void Init()
    {
        new Hook(
                typeof(Scavenger).GetProperty(nameof(Scavenger.Elite)).GetGetMethod(), 
                typeof(Hooks).GetMethod(nameof(Hooks.LeaderIsElite)));
    }

    public static bool LeaderIsElite(Func<Scavenger, bool> orig, Scavenger self)
    {
        return orig(self) || self.Template.type == SekoPack.Creatures.LeaderScavA.Enum.CreatureTemplateType.leaderScavA;
    }
}