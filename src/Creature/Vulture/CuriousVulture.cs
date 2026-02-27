namespace SekoPack.Creatures.TVulture;

public class CuriousVulture : Tracker
{
    public CuriousVulture(ArtificialIntelligence AI, int seeAroundCorners, int maxTrackedCreatures, int framesToRememberCreatures, float ghostSpeed, int ghostPush, int ghostPushSpeed, int ghostDismissalRange, bool useTrackedCreatures = false) : base(AI, seeAroundCorners, maxTrackedCreatures, framesToRememberCreatures, ghostSpeed, ghostPush, ghostPushSpeed, ghostDismissalRange, useTrackedCreatures)
    {
        
    }
}