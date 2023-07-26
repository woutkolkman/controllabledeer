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


        //deer AI input
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
                WorldCoordinate target = self.deer.coord;
                target.x += p.input[0].x * 10;
                target.y += p.input[0].y * 10;
                self.inRoomDestination = target;
            }
        }
    }
}
