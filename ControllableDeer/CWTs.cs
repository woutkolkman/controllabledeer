using System.Runtime.CompilerServices;

namespace ControllableDeer
{
    public class CWTs
    {
        public static readonly ConditionalWeakTable<DeerAI, DeerAIData> deerAICWT = new ConditionalWeakTable<DeerAI, DeerAIData>();


        public static DeerAIData GetOrCreate(DeerAI self)
        {
            if (self != null)
                return deerAICWT.GetOrCreateValue(self);
            return null;
        }


        public class DeerAIData
        {
            public int controlPaused = 0;
        }
    }
}
