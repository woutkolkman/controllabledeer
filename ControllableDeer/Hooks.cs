using UnityEngine;

namespace ControllableDeer
{
    class Hooks
    {
        public static void Apply()
        {
            //overwrite deer AI input
            On.DeerAI.Update += DeerAIUpdateHook;
        }


        public static void Unapply()
        {
            On.DeerAI.Update -= DeerAIUpdateHook;
        }


        //overwrite deer AI input
        static void DeerAIUpdateHook(On.DeerAI.orig_Update orig, DeerAI self)
        {
            bool lastLPIA = self.lastPlayerInAntlers;
            orig(self);

            Player p = null;

            //get first non-NPC player in antlers with valid input
            if (self.deer?.playersInAntlers?.Count > 0) {
                foreach (Deer.PlayerInAntlers dpia in self.deer.playersInAntlers) {
                    if (dpia.player?.input?.Length > 0 && !dpia.player.isNPC && 
                        (dpia.player.input[0].x != 0 || dpia.player.input[0].y != 0)) {
                        p = dpia.player;
                        break;
                    }
                }
            }
            if (!self.lastPlayerInAntlers)
                return;

            //temporarily pausing control for this deer gives the option to still ride the deer to its original destination
            CWTs.DeerAIData daid = CWTs.GetOrCreate(self);
            if (!lastLPIA && daid != null)
                daid.controlPaused = 40;
            if (daid?.controlPaused > 0) {
                daid.controlPaused--;
                return;
            }

            if (p == null)
                return;

            //affects walk direction
            if (p.input[0].y > 0) {
                if (self.deer.mainBodyChunk != null)
                    self.deer.mainBodyChunk.vel += new Vector2(0f, 1f); //get unstuck
                if (self.stuckTracker != null)
                    self.stuckTracker.satisfiedWithThisPosition = false;
            }
            WorldCoordinate target = self.deer.coord;
            target.x += p.input[0].x * 5;
            target.y += p.input[0].y * 5;
            self.inRoomDestination = target;

            //prevent bowing and resting
            if (p.input[0].x != 0 || p.input[0].y != 0) {
                self.deerPileCounter = 80;
                self.kneelCounter = 0;
                self.deer.hesistCounter = 0;
                self.minorWanderTimer = 0;
                self.layDownAndRestCounter = 0;
                self.restingCounter = 0;
                self.deer.resting = 0f;
                self.sporePos = null;
                self.goToPuffBall = null;
            }

            /*//jump forwards on command
            if (p.input[0].pckp) {
                if (self.stuckTracker != null)
                    self.stuckTracker.stuckCounter = int.MaxValue;
                self.behavior = DeerAI.Behavior.GetUnstuck;
            }*/
        }
    }
}
