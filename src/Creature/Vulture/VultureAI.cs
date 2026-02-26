using System;
using MoreSlugcats;
using Noise;
using RWCustom;
using UnityEngine;
using Watcher;


namespace SekoPack.Creatures.TVulture
{
    public class TVultureAI : VultureAI
    {
        public TVultureAI(AbstractCreature creature, World world) : base(creature, world)
        {
            //base.AddModule(new CuriousVulture(this, 1, 2, 40, .5f, 10, 10, 100));
            //base.AddModule(new Tracker(this, 1, 2, 40, .5f, 10, 10, 100));
            base.AddModule(new FriendTracker(this));
            base.utilityComparer.AddComparedModule(base.friendTracker, null, 0.9f, 1.2f);           
        }


        // Definen behauver is in updat >:(
        public override void Update()
        {
            base.Update();

            if (creature == null || creature.world == null || creature.world.game == null || creature.world.game.Players.Count == 0)
            {
                return;
            }
            AbstractCreature c  = creature?.world?.game?.Players[0];
            if (c != null)
            {
                base.friendTracker.friend = c.realizedCreature;
            }

            this.behavior = Enum.VultureBehavior.Curious;

            if(this.behavior == Enum.VultureBehavior.Curious)
            {
                this.creature.abstractAI.SetDestination(base.friendTracker.friendDest);
                if (this.vulture.grasps[0] != null && this.vulture.grasps[0].grabbed != null && this.vulture.grasps[0].grabbed == base.friendTracker.friend)
                {
                    this.vulture.ReleaseGrasp(0);
                }

                Tracker tracker = base.tracker;
			    Creature friend = base.friendTracker.friend;
                this.focusCreature = tracker.RepresentationForCreature((friend != null) ? friend.abstractCreature : null, false);
            }




            SKDEBUG.LogDebug($"Behaviur: {this.behavior}, {this.behavior is null} | friend? {base.friendTracker.friend}");
        }
    }
};