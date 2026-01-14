
namespace Items.Randotan.Effects;

# region Object
public class RandotanSeed : Rock, IDrawable, IPlayerEdible
{

    public RandotanSeed(AbstractPhysicalObject abstractPhysicalObject, World world) : base(abstractPhysicalObject, world)
    {
        bodyChunks = new BodyChunk[1];
        bodyChunks[0] = new BodyChunk(this, 0, default, radius, 0.2f);
        bodyChunkConnections = [];
    }

    public int spark_iteration => 10;
    public float radius => 12f;

    public int bites = 2;
    public int BitesLeft => this.bites;

    public int FoodPoints => 1;

    public bool Edible => true;

    public bool AutomaticPickUp => false;

    public bool visible = true;

    public void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[1];
        sLeaser.sprites[0] = new FSprite("Circle20", true);
        sLeaser.sprites[0].scale = this.bodyChunks[0].rad / 10f;
        AddToContainer(sLeaser, rCam, null);
    }

    public void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        sLeaser.sprites[0].x = this.bodyChunks[0].pos.x - camPos.x;
        sLeaser.sprites[0].y = this.bodyChunks[0].pos.y - camPos.y;
        
        if (base.slatedForDeletetion || this.room != rCam.room)
        {
            sLeaser.CleanSpritesAndRemove();
        }
    }

    public void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        color = Color.yellow;
        sLeaser.sprites[0].color = color;
    }

    public void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        newContainer ??= rCam.ReturnFContainer("Items");
        foreach (FSprite sprite in sLeaser.sprites)
            newContainer.AddChild(sprite);
    }

    public void BitByPlayer(Creature.Grasp grasp, bool eu)
    {
        this.bites--;
		this.room.PlaySound((this.bites == 0) ? SoundID.Slugcat_Eat_Dangle_Fruit : SoundID.Slugcat_Bite_Dangle_Fruit, base.firstChunk);
		base.firstChunk.MoveFromOutsideMyUpdate(eu, grasp.grabber.mainBodyChunk.pos);
		if (this.bites < 1)
		{
			(grasp.grabber as Player).ObjectEaten(this);
			grasp.Release();
			this.Destroy();
		}
    }

    public void ThrowByPlayer()
    {
    }
}

# endregion

# region Properties
public class RandotanSeedProperties : ItemProperties
{
    public override void Throwable(Player player, ref bool throwable)
        => throwable = true;

    public override void ScavCollectScore(Scavenger scavenger, ref int score)
        => score = 3;

    public override void ScavWeaponPickupScore(Scavenger scav, ref int score)
        => score = 3;

    public override void ScavWeaponUseScore(Scavenger scav, ref int score)
        => score = 0;

    public override void Grabability(Player player, ref Player.ObjectGrabability grabability)
    {
        grabability = Player.ObjectGrabability.BigOneHand;
    }
}

# endregion

public class RandotanSeedAbstr : AbstractPhysicalObject
{
    public RandotanSeedAbstr(World world, WorldCoordinate pos, EntityID ID) : base(world, Enums.AbstractObjectType.RandotanSeed, null, pos, ID)
    {
        scaleX = 1;
        scaleY = 1;
        saturation = 0.5f;
    }

    public override void Realize()
    {
        base.Realize();
        if (realizedObject == null)
            realizedObject = new RandotanSeed(this, this.Room.world);
    }

    public override string ToString()
    {
        return this.SaveToString($"{saturation};{scaleX};{scaleY}");
    }

    public float saturation;
    public float scaleX;
    public float scaleY;
}

public class RandotanSeedFisob : Fisob
{

    public RandotanSeedFisob() : base(Enums.AbstractObjectType.RandotanSeed)
    {
        SandboxPerformanceCost = new(linear: 0.2f, 0f);
        RegisterUnlock(Enums.SandboxUnlockID.RandotanSeed, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat, data: 0);
    }

    public override AbstractPhysicalObject Parse(World world, EntitySaveData entitySaveData, SandboxUnlock unlock)
    {
        // RandotanSeed data is floats separated by ; characters.
        string[] p = entitySaveData.CustomData.Split(';');

        if (p.Length < 3)
        {
            p = new string[3];
        }

        var result = new RandotanSeedAbstr(world, entitySaveData.Pos, entitySaveData.ID)
        {
            saturation = float.TryParse(p[0], out var s) ? s : 0.5f,
            scaleX = float.TryParse(p[1], out var x) ? x : 1f,
            scaleY = float.TryParse(p[2], out var y) ? y : 1f
        };

        // If this is coming from a sandbox unlock, apply any special logic based on unlock data
        if (unlock is SandboxUnlock u)
        {
            // You can add custom logic here based on u.Data if needed
            // For example: result.saturation = u.Data / 100f;
        }

        return result;
    }

    private static readonly RandotanSeedProperties properties = new();

    public override ItemProperties Properties(PhysicalObject forObject)
    {
        return properties;
    }
}

public class Enums
{
    public class AbstractObjectType
    {
        public static AbstractPhysicalObject.AbstractObjectType RandotanSeed = new(nameof(RandotanSeed), true);
        public void UnregisterValues()
        {
            if (RandotanSeed != null)
            {
                RandotanSeed.Unregister();
                RandotanSeed = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID RandotanSeed = new(nameof(RandotanSeed), true);

        public void UnregisterValues()
        {
            if (RandotanSeed != null)
            {
                RandotanSeed.Unregister();
                RandotanSeed = null;
            }
        }
    }
}

