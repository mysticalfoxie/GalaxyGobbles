using System.Collections.Generic;
using System.Linq;

public class IngredientReferences
{
    public IngredientData Egg { get; private set; }
    public IngredientData Seaweed { get; private set; }
    public IngredientData[] All { get; }

    public IngredientReferences()
    {
        All = IngredientSettings.Data.Ingredients;
        AnalyseDataAndAssignItems();
    }

    public void AnalyseDataAndAssignItems()
    {
        Egg = All.First(x => x.Type == IngredientType.INGR_01_Egg);
        Seaweed = All.First(x => x.Type == IngredientType.INGR_02_Seaweed);
    }
}

