using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class References : Singleton<References>
{
    private readonly List<GameObject> _allLevelObjects = new();
    private readonly List<Table> _tables = new();
    private readonly List<Chair> _chairs = new();
    private readonly List<WaitArea> _waitAreas = new();
    private readonly List<NoodlePot> _noodlePots = new();
    private GameObject _root;
    
    [Header("Configurations")] 
    [SerializeField] private GameSettings _settings;
    [SerializeField] private Identifiers _identifiers;
    
    [Header("Anchor Customer")]
    [Tooltip("Please set a GameObject here from a customer, who has the desired scaling.\nThis customer will be the anchor for the species size calculation.")]
    [SerializeField] private Customer _anchor;
    [SerializeField] private SpeciesData _data;

    public override void Awake()
    {
        base.Awake();
        
        var scene = SceneManager.GetActiveScene();
        HandleLevelData(scene);

        if (!AnchorCustomer) return;
        AnchorCustomer.Data = Model.Create<CustomerData>(x => { x._species = _data; });
        AnchorCustomer.gameObject.SetActive(false);
    }

    private void HandleLevelData(Scene scene)
    {
        if (scene.buildIndex != LevelManager.MAIN_LEVEL_INDEX) return;
        
        _root = scene.GetRootGameObjects().First(x => x.CompareTag("Level Root Object"));
        AnalyseLevelObjects();
    }

    public GameSettings GetLocalSettings() => _settings;

    private void AnalyseLevelObjects()
    {
        var levelObjects = _root.GetChildrenRecursively().ToArray(); 
        foreach (var levelObject in levelObjects)
        {
            // Saves more performance then using .ToArray()
            _allLevelObjects.Add(levelObject); 
            AssignLevelObjectsToLists(levelObject);
        } 
    }

    private void AssignLevelObjectsToLists(GameObject levelObject)
    {
        // Saves a lot of performance rather than iterating 3 times over all level objects
        var table = levelObject.GetComponent<Table>();
        var chair = levelObject.GetComponent<Chair>();
        var waitArea = levelObject.GetComponent<WaitArea>();
        var pot = levelObject.GetComponent<NoodlePot>();
        
        if (table is not null) _tables.Add(table);
        if (chair is not null) _chairs.Add(chair);
        if (waitArea is not null) _waitAreas.Add(waitArea);
        if (pot is not null) _noodlePots.Add(pot);
    }

    public GameObject RootObject => _root;
    public Customer AnchorCustomer => _anchor;
    public IEnumerable<Table> Tables => _tables;
    public IEnumerable<Chair> Chairs => _chairs;
    public IEnumerable<WaitArea> WaitAreas => _waitAreas;
    public IEnumerable<NoodlePot> NoodlePots => _noodlePots;
    public IEnumerable<GameObject> LevelObjects => _allLevelObjects;
    public static Identifiers Ids => Instance._identifiers;
}