using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class References : SingletonMonoBehaviour<References>
{
    [Header("References")]
    [SerializeField]
    private GameObject _itemPrefab;
    [SerializeField]
    private GameObject _customerPrefab;

    [Header("Game Data")]
    [SerializeField]
    private ItemData[] _items;
    
    [SerializeField]
    private SpeciesData[] _species;

    private readonly List<GameObject> _allLevelObjects = new();
    
    private readonly List<Table> _tables = new();
    private readonly List<WaitArea> _waitAreas = new();
    private readonly List<NoodlePot> _noodlePots = new();
    
    
    public override void Awake()
    {
        base.Awake();
        
        Items = new ItemReferences(_items);

        var scene = SceneManager.GetActiveScene();
        var root = scene.GetRootGameObjects().First(x => x.CompareTag("Level Root Object"));
        AnalyseLevelObjects(root);

    }

    private void AnalyseLevelObjects(GameObject root)
    {
        foreach (var levelObject in root.GetAllChildren())
        {
            // Saves more performance then using .ToArray()
            _allLevelObjects.Add(levelObject); 
            AssignLevelObjectsToLists(levelObject);
        } 
    }

    private void AssignLevelObjectsToLists(GameObject levelObject)
    {
        // Saves a lot performance rather then iterating 3 times over all level objects
        var table = levelObject.GetComponent<Table>();
        var waitArea = levelObject.GetComponent<WaitArea>();
        var pots = levelObject.GetComponent<NoodlePot>();
        
        if (table is not null) _tables.Add(table);
        if (waitArea is not null) _waitAreas.Add(waitArea);
        if (pots is not null) _noodlePots.Add(pots);
    }

    public GameObject ItemPrefab => _itemPrefab;
    public GameObject CustomerPrefab => _customerPrefab;
    public IEnumerable<Table> Tables => _tables;
    public IEnumerable<WaitArea> WaitAreas => _waitAreas;
    public IEnumerable<NoodlePot> NoodlePots => _noodlePots;
    public IEnumerable<GameObject> LevelObjects => _allLevelObjects;
    
    public IEnumerable<SpeciesData> Species => _species;
    public ItemReferences Items { get; private set; }
}

public class ItemReferences
{
    public IEnumerable<ItemData> All { get; }

    public ItemData Noodles { get; private set; }

    public ItemReferences(IEnumerable<ItemData> items)
    {
        All = items;
        
        AnalyseDataAndAssignItems();
    }

    public void AnalyseDataAndAssignItems()
    {
        Noodles = All.First(x => x.Type == ItemType.ITEM_01_Noodles);
    }
}