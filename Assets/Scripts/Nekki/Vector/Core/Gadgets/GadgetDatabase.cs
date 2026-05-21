namespace Nekki.Vector.Core.Gadgets
{
    public static class GadgetDatabase
    {
        public static readonly GadgetType[] All =
        {
            GadgetType.KillBot,
            GadgetType.SlowTime
        };

        public static string GetId(GadgetType type)
        {
            return type switch
            {
                GadgetType.KillBot => "GADGET_FORCEBLASTER",
                GadgetType.SlowTime => "GADGET_SLOWTIME",
                _ => null
            };
        }
    }
}