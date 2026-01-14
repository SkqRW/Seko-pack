
namespace Items.Randotan;

# region Object
public partial class Randotan : Rock
{
    public Randotan(AbstractPhysicalObject abstr, World world) : base(abstr, world)
    {
        bodyChunks = new BodyChunk[1];
        bodyChunks[0] = new BodyChunk(this, 0, new Vector2(0f, 0f), radius, 0.2f);
        bodyChunkConnections = [];
        collisionLayer = DefaultCollLayer;
    }

    public override int DefaultCollLayer => 1;
    public int spark_iteration => 10;
    public float radius => 12f;

    public override void HitWall()
    {
        if (this.room.BeingViewed)
        {
            for (int i = 0; i < spark_iteration; i++)
            {
                this.room.AddObject(new Spark(base.firstChunk.pos + this.throwDir.ToVector2() * (base.firstChunk.rad - 1f), Custom.DegToVec(UnityEngine.Random.value * 360f) * 10f * UnityEngine.Random.value + -this.throwDir.ToVector2() * 10f, new Color(1f, 1f, 1f), null, 2, 4));
            }
        }
        color = Color.red;
        this.room.ScreenMovement(new Vector2?(base.firstChunk.pos), this.throwDir.ToVector2() * 1.5f, 0f);
        this.room.PlaySound(SoundID.Rock_Hit_Wall, base.firstChunk);
        this.SetRandomSpin();
        this.ChangeMode(Weapon.Mode.Free);
        Broke();
    }

    public override void HitByWeapon(Weapon weapon)
    {
        base.HitByWeapon(weapon);
        color = Color.yellow;

        if (weapon is Spear)
        {
            color = Color.magenta;
            Broke();
        }
    }

    private void Broke()
    {
        // Obtener la velocidad actual del coco para usar como velocidad de impacto
        Vector2 currentVelocity = base.firstChunk.vel;
        
        if (this.room.BeingViewed)
        {
            for (int i = 0; i < spark_iteration; i++)
            {
                this.room.AddObject(new Spark(base.firstChunk.pos + Custom.DegToVec(UnityEngine.Random.value * 360f) * UnityEngine.Random.value * 10f, Custom.DegToVec(UnityEngine.Random.value * 360f) * 10f * UnityEngine.Random.value, new Color(1f, 1f, 1f), null, 2, 4));
            }
            
            // Agregar efecto de agua derramándose con velocidad de impacto
            for (int i = 0; i < 3; i++)
            {
                Vector2 spillDirection = Custom.DegToVec(UnityEngine.Random.value * 180f - 90f); // Hacia abajo principalmente
                //this.room.AddObject(new CocoWater(base.firstChunk.pos, currentVelocity));
            }
        }
        
        // Sonido de líquido derramándose
        this.room.PlaySound(SoundID.Slugcat_Eat_Dangle_Fruit, base.firstChunk, false, 0.8f, 0.8f);
        
        // MAKE THE SEED OBJECT HERE
        UnityEngine.Debug.Log("Randotan Broke");

        AbstractPhysicalObject abstractPhysicalObject = new AbstractPhysicalObject(this.room.world, AbstractPhysicalObject.AbstractObjectType.Rock, null, room.GetWorldCoordinate(this.firstChunk.pos), this.room.game.GetNewID());
        Items.Randotan.Effects.RandotanSeed slimeMold = new Items.Randotan.Effects.RandotanSeed(abstractPhysicalObject, this.room.world);
        slimeMold.PlaceInRoom(this.room);
        UnityEngine.Debug.Log("Randotan Broke SEED release");
        visible = false;
        this.Destroy();
    }
    public bool visible = true;
}

public partial class Randotan : IDrawable
{
    public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
    {
        sLeaser.sprites = new FSprite[1];
        sLeaser.sprites[0] = new FSprite("Circle20", true);
        sLeaser.sprites[0].scale = this.bodyChunks[0].rad / 10f;
        AddToContainer(sLeaser, rCam, null);
    }

    public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
    {
        sLeaser.sprites[0].x = this.bodyChunks[0].pos.x - camPos.x;
        sLeaser.sprites[0].y = this.bodyChunks[0].pos.y - camPos.y;
        
        if (base.slatedForDeletetion || this.room != rCam.room)
        {
            sLeaser.CleanSpritesAndRemove();
        }
    }

    public override void ApplyPalette(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, RoomPalette palette)
    {
        color = Color.magenta;
        sLeaser.sprites[0].color = color;
    }

    public override void AddToContainer(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, FContainer newContainer)
    {
        newContainer ??= rCam.ReturnFContainer("Items");
        foreach (FSprite sprite in sLeaser.sprites)
            newContainer.AddChild(sprite);
    }
}

# endregion

# region Properties
public class RandotanProperties : ItemProperties
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

public class RandotanAbstr : AbstractPhysicalObject
{
    public RandotanAbstr(World world, WorldCoordinate pos, EntityID ID) : base(world, Enums.AbstractObjectType.Randotan, null, pos, ID)
    {
        scaleX = 1;
        scaleY = 1;
        saturation = 0.5f;
    }

    public override void Realize()
    {
        base.Realize();
        if (realizedObject == null)
            realizedObject = new Randotan(this, Room.realizedRoom.world);
    }

    public override string ToString()
    {
        return this.SaveToString($"{saturation};{scaleX};{scaleY}");
    }

    public float saturation;
    public float scaleX;
    public float scaleY;
}

public class RandotanFisob : Fisob
{

    public RandotanFisob() : base(Enums.AbstractObjectType.Randotan)
    {
        SandboxPerformanceCost = new(linear: 0.2f, 0f);
        RegisterUnlock(Enums.SandboxUnlockID.Randotan, parent: MultiplayerUnlocks.SandboxUnlockID.Slugcat, data: 0);
    }

    public override AbstractPhysicalObject Parse(World world, EntitySaveData entitySaveData, SandboxUnlock unlock)
    {
        // Randotan data is floats separated by ; characters.
        string[] p = entitySaveData.CustomData.Split(';');

        if (p.Length < 3)
        {
            p = new string[3];
        }

        var result = new RandotanAbstr(world, entitySaveData.Pos, entitySaveData.ID)
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

    private static readonly RandotanProperties properties = new();

    public override ItemProperties Properties(PhysicalObject forObject)
    {
        return properties;
    }
}

public class Enums
{
    public class AbstractObjectType
    {
        public static AbstractPhysicalObject.AbstractObjectType Randotan = new(nameof(Randotan), true);
        public void UnregisterValues()
        {
            if (Randotan != null)
            {
                Randotan.Unregister();
                Randotan = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID Randotan = new(nameof(Randotan), true);

        public void UnregisterValues()
        {
            if (Randotan != null)
            {
                Randotan.Unregister();
                Randotan = null;
            }
        }
    }
}

