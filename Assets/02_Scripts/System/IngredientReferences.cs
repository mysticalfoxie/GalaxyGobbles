using System.Collections.Generic;
using System.Linq;

public class IngredientReferences
{
    public IEnumerable<IngredientData> All { get; }

    public IngredientData Red { get; private set; }

    public IngredientReferences()
    {
        All = IngredientSettings.Data.Ingredients;
        AnalyseDataAndAssignItems();
    }

    public void AnalyseDataAndAssignItems()
    {
        Red = All.First(x => x.Type == IngredientType.ING_01_Red);
    }
}

