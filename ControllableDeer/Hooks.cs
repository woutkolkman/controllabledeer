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
            //TODO
        }


        //overwrite deer AI input
        static void DeerAIUpdateHook(On.DeerAI.orig_Update orig, DeerAI self)
        {
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

            //affects walk direction
            if (p != null) {
                if (p.input[0].y > 0) {
                    if (self.deer.mainBodyChunk != null)
                        self.deer.mainBodyChunk.vel += new Vector2(0f, 1f); //get unstuck
                    self.deer.resting = 0f;
                    if (self.stuckTracker != null)
                        self.stuckTracker.satisfiedWithThisPosition = false;
                }
                WorldCoordinate target = self.deer.coord;
                target.x += p.input[0].x * 5;
                target.y += p.input[0].y * 5;
                self.inRoomDestination = target;
            }
        }
    }
}
