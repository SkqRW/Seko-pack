using System;

namespace SekoPack.Creatures.TVulture;


public class Enum
{
    public class CreatureTemplateType
    {
        public static CreatureTemplate.Type TVulture = new(nameof(TVulture), true);
        public void UnregisterValues()
        {
            if (TVulture != null)
            {
                TVulture.Unregister();
                TVulture = null;
            }
        }
    }

    public class SandboxUnlockID
    {
        public static MultiplayerUnlocks.SandboxUnlockID TVulture = new(nameof(TVulture), true);

        public void UnregisterValues()
        {
            if (TVulture != null)
            {
                TVulture.Unregister();
                TVulture = null;
            }
        }
    }

    public class VultureBehavior
    {
        public static VultureAI.Behavior Curious;

        public void UnregisterValues()
        {
            if (Curious != null)
            {
                Curious.Unregister();
                Curious = null;
            }
        }
    }
}

public class Hooks
{

    public static VultureAI.Behavior Curious;

    public static void init()
    {
        On.VultureAI.Behavior.ctor += VultureAI_ctor;
    }

    
    private static void VultureAI_ctor(On.VultureAI.Behavior.orig_ctor orig, VultureAI.Behavior self, string value, bool register)
    {
        orig(self, value, register);
        
        if (Curious == null)
        {
            Curious = new VultureAI.Behavior("Curious", true);
        }
    }
}
