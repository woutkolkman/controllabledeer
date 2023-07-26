using UnityEngine;

namespace ControllableDeer
{
    class Hooks
    {
        public static void Apply()
        {
            //at tickrate
            On.RainWorldGame.Update += RainWorldGameUpdateHook;

            //at framerate
            On.RainWorldGame.RawUpdate += RainWorldGameRawUpdateHook;
        }


        public static void Unapply()
        {
            //TODO
        }


        //at tickrate
        static void RainWorldGameUpdateHook(On.RainWorldGame.orig_Update orig, RainWorldGame self)
        {
            orig(self);

            if (self?.world?.activeRooms?.Count <= 0)
                return;

            foreach (Room room in self.world.activeRooms)
            {
                for (int i = 0; i < room?.physicalObjects?.Length; i++)
                {
                    for (int j = 0; j < room.physicalObjects[i].Count; j++)
                    {
                        PhysicalObject obj = room.physicalObjects[i][j];
                        if (!(obj is Deer))
                            continue;
                    }
                }
            }
        }


        //at framerate
        static void RainWorldGameRawUpdateHook(On.RainWorldGame.orig_RawUpdate orig, RainWorldGame self, float dt)
        {
            orig(self, dt);
            if (self.GamePaused || self.pauseUpdate || !self.processActive || self.pauseMenu != null)
                return;
        }
    }
}
