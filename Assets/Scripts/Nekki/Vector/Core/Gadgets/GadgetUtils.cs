using System;
using Nekki.Vector.Core.Gadgets;

public static class GadgetUtils
{
    public static bool HasAnyEquippedGadget()
    {
        var shop = UserDataManager.Instance.ShopData;

        foreach (var g in GadgetDatabase.All)
        {
            var id = GadgetDatabase.GetId(g);

            if (shop.IsEquipped(id))
                return true;
        }

        return false;
    }

    public static bool IsEquipped(GadgetType type)
    {
        var id = GadgetDatabase.GetId(type);
        return UserDataManager.Instance.ShopData.IsEquipped(id);
    }

    public static int GetTotalCharges()
    {
        int total = 0;

        foreach (var g in GadgetDatabase.All)
        {
            var id = GadgetDatabase.GetId(g);
            total += UserDataManager.Instance.ShopData.GetCount(id);
        }

        return total;
    }
}