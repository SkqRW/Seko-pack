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

namespace SekoPack.Creatures.TVulture;

public class TVulture : Vulture
{
    public TVulture(AbstractCreature abstractCreature, World world) : base(abstractCreature, world)
    {

    }

     public override void InitiateGraphicsModule() => graphicsModule ??= new TVultureGraphics(this);

}

internal class TVultureGraphics : VultureGraphics
{
    public TVultureGraphics(TVulture vulture) : base(vulture)
    {

    }

    public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        base.DrawSprites(sLeaser, rCam, timeStacker, camPos);


        float num2 = Custom.AimFromOneVectorToAnother(Vector2.Lerp(this.vulture.neck.tChunks[this.vulture.neck.tChunks.Length - 1].lastPos, this.vulture.neck.tChunks[this.vulture.neck.tChunks.Length - 1].pos, timeStacker), Vector2.Lerp(this.vulture.bodyChunks[4].lastPos, this.vulture.bodyChunks[4].pos, timeStacker));
        float num3 = (float)(8 - this.headGraphic) * Mathf.Sign(num2) * 22.5f;

        sLeaser.sprites[this.EyesSprite].rotation = num2 - num3;
		sLeaser.sprites[this.MaskSprite].rotation = num2 - num3;
		sLeaser.sprites[this.EyesSprite].element = Futile.atlasManager.GetElementWithName("KrakenEyes" + this.headGraphic.ToString());
		sLeaser.sprites[this.MaskSprite].element = Futile.atlasManager.GetElementWithName("SpikeMask" + this.headGraphic.ToString());
		sLeaser.sprites[this.EyesSprite].scaleX = ((num2 > 0f) ? -1f : 1f) * (1.15f);
		sLeaser.sprites[this.MaskSprite].scaleX = ((num2 > 0f) ? -1f : 1f) * (1.15f);
		sLeaser.sprites[this.EyesSprite].scaleY = 1.15f;
		sLeaser.sprites[this.MaskSprite].scaleY = 1.15f;
		sLeaser.sprites[this.MaskSprite].isVisible = (this.vulture.State as Vulture.VultureState).mask;
    }
}

public class TVultureCritob : Critob
{
    public TVultureCritob() : base(Enum.CreatureTemplateType.TVulture)
    {
        //Icon = new SimpleIcon("Kill", new(.75f, .15f, 0f));
        LoadedPerformanceCost = 100f;
        SandboxPerformanceCost = new(1.1f, .65f);
        RegisterUnlock(KillScore.Configurable(23), Enum.SandboxUnlockID.TVulture, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat, data: 0);
    }

    public override ArtificialIntelligence CreateRealizedAI(AbstractCreature acrit) => new TVultureAI(acrit, acrit.world);
    public override AbstractCreatureAI? CreateAbstractAI(AbstractCreature acrit) => new VultureAbstractAI(acrit.world, acrit);

public override CreatureState CreateState(AbstractCreature acrit) => new Vulture.VultureState(acrit);
    public override Creature CreateRealizedCreature(AbstractCreature acrit) => new TVulture(acrit, acrit.world);

    public override CreatureTemplate CreateTemplate()
    {
        // M4rblelous code from FatFireFly
        var t = new CreatureFormula(CreatureTemplate.Type.Vulture, Type, "TVulture")
        {
            TileResistances = new()
            {
                Air = new(1f, Allowed),
                OffScreen = new(1f, Allowed)
            },
            ConnectionResistances = new()
            {
                Standard = new(1f, Allowed),
                OutsideRoom = new(1f, Allowed),
                SkyHighway = new(1f, Allowed),
                OffScreenMovement = new(1f, Allowed),
                BetweenRooms = new(10f, Allowed)
            },
            DefaultRelationship = new(CreatureTemplate.Relationship.Type.Ignores, 0f),
            DamageResistances = new() { Base = 7f, Explosion = 102f, Electric = 51f },
            StunResistances = new() { Base = 6f, Explosion = 102f, Electric = 51f },
            HasAI = true,
            Pathing = PreBakedPathing.Ancestral(CreatureTemplate.Type.Vulture)
        }.IntoTemplate();
        t.abstractedLaziness = 10;
        t.canSwim = false;
        t.canFly = true;
        t.offScreenSpeed = 1f;
        t.bodySize = 6f;
        t.grasps = 1;
        t.stowFoodInDen = true;
        t.shortcutSegments = 5;
        t.visualRadius = 12000f;
        t.movementBasedVision = .4f;
        t.waterVision = 0f;
        t.throughSurfaceVision = 10f;
        t.hibernateOffScreen = true;
        t.dangerousToPlayer = 0.5f;
        t.communityInfluence = .25f;
        t.socialMemory = true;
        t.meatPoints = 15;
        t.lungCapacity = 300f;
        t.waterRelationship = CreatureTemplate.WaterRelationship.AirAndSurface;
        t.BlizzardAdapted = true;
        t.BlizzardWanderer = true;
        return t;
    }

    public override void EstablishRelationships()
    {
        var s = new Relationships(Type);
        s.Ignores(CreatureTemplate.Type.LizardTemplate);
        s.HasDynamicRelationship(CreatureTemplate.Type.Slugcat, .5f);
        s.Fears(CreatureTemplate.Type.Vulture, .9f);
        s.Fears(CreatureTemplate.Type.KingVulture, 1f);
        s.Eats(CreatureTemplate.Type.TubeWorm, .025f);
        s.Eats(CreatureTemplate.Type.Scavenger, .8f);
        s.Eats(CreatureTemplate.Type.CicadaA, .05f);
        s.Eats(CreatureTemplate.Type.LanternMouse, .3f);
        s.Eats(CreatureTemplate.Type.BigSpider, .35f);
        s.Eats(CreatureTemplate.Type.EggBug, .45f);
        s.Eats(CreatureTemplate.Type.JetFish, .1f);
        s.Fears(CreatureTemplate.Type.BigEel, 1f);
        s.Eats(CreatureTemplate.Type.Centipede, .8f);
        s.Eats(CreatureTemplate.Type.BigNeedleWorm, .25f);
        s.Fears(CreatureTemplate.Type.DaddyLongLegs, 1f);
        s.Eats(CreatureTemplate.Type.SmallNeedleWorm, .3f);
        s.Eats(CreatureTemplate.Type.DropBug, .2f);
        s.Fears(CreatureTemplate.Type.RedCentipede, .9f);
        s.Fears(CreatureTemplate.Type.TentaclePlant, .2f);
        s.Eats(CreatureTemplate.Type.Hazer, .15f);
        s.FearedBy(CreatureTemplate.Type.LanternMouse, .7f);
        s.EatenBy(CreatureTemplate.Type.Vulture, .5f);
        s.FearedBy(CreatureTemplate.Type.CicadaA, .3f);
        s.FearedBy(CreatureTemplate.Type.JetFish, .2f);
        s.IsInPack(CreatureTemplate.Type.Slugcat, 100f);
        s.FearedBy(CreatureTemplate.Type.Scavenger, .5f);
        s.EatenBy(CreatureTemplate.Type.DaddyLongLegs, 1f);
        if (ModManager.DLCShared)
        {
            s.IgnoredBy(DLCSharedEnums.CreatureTemplateType.ZoopLizard);
            s.Ignores(DLCSharedEnums.CreatureTemplateType.ZoopLizard);
        }
    }
}

