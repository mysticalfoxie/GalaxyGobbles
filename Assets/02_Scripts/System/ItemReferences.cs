using System.Collections.Generic;
using System.Linq;

public class ItemReferences
{
    public IEnumerable<ItemData> All { get; }

    public ItemData Noodles { get; private set; }

    public ItemReferences()
    {
        All = GameSettings.Data.Items;
        AnalyseDataAndAssignItems();
    }

    public void AnalyseDataAndAssignItems()
    {
        Noodles = All.First(x => x.Id == ItemId.ID_01_NoodleBowl);
    }
}

