using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class References : SingletonMonoBehaviour<References>
{
    private readonly List<GameObject> _allLevelObjects = new();
    private readonly List<Table> _tables = new();
    private readonly List<WaitArea> _waitAreas = new();
    private readonly List<NoodlePot> _noodlePots = new();
    
    [Header("Settings")] 
    [SerializeField] 
    private GeneralSettings _general;
     
    [SerializeField] 
    private ItemSettings _items;
     
    [SerializeField] 
    private LevelSettings _level;
 
    [SerializeField] 
    private SpeciesSettings _species;
 
    [SerializeField] 
    private ReferencesSettings _reference;
    
    public override void Awake()
    {
        base.Awake();
        
        var scene = SceneManager.GetActiveScene();
        var root = scene.GetRootGameObjects().First(x => x.CompareTag("Level Root Object"));
        AnalyseLevelObjects(root);

        Items = new ItemReferences();
        
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

    public IEnumerable<Table> Tables => _tables;
    public IEnumerable<WaitArea> WaitAreas => _waitAreas;
    public IEnumerable<NoodlePot> NoodlePots => _noodlePots;
    public IEnumerable<GameObject> LevelObjects => _allLevelObjects;

    public ItemReferences Items { get; private set; }
    
    public GeneralSettings GeneralSettings => _general;
    public ItemSettings ItemSettings => _items;
    public LevelSettings LevelSettings => _level;
    public SpeciesSettings SpeciesSettings => _species;
    public ReferencesSettings ReferenceSettings => _reference;
}