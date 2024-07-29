using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

[CreateAssetMenu(fileName = "CFG_Identifiers", menuName = "Galaxy Gobbles/Configurations/Identifiers", order = 2)]
public class Identifiers : ScriptableObject
{ 
    [Header("Machines")]
    [SerializeField] private ItemData _noodleBowl;
    [SerializeField] private ItemData _noodlePotEmpty;
    [SerializeField] private ItemData _noodlePotCooking;
    [SerializeField] private ItemData _noodlePotCooked;
    [SerializeField] private ItemData _noodlePotOvercooked;
    [SerializeField] private ItemData _cuttingBoard;
    [SerializeField] private ItemData _cuttingBoardUI;
    [SerializeField] private ItemData _trash;
    
    [Header("States")]
    [SerializeField] private ItemData _waitForSeat;
    [SerializeField] private ItemData _waitForCheckout;
    [SerializeField] private ItemData _thinking;
    [SerializeField] private ItemData _thinkBubble;
    [SerializeField] private ItemData _eating;
    [SerializeField] private ItemData _questionMark;
    [SerializeField] private ItemData _thinkBubbleTable;
    [SerializeField] private ItemData _thinkBubbleTableMultiHorizontal;
    [SerializeField] private ItemData _thinkBubbleTableMultiVertical;
    [SerializeField] private ItemData _thinkBubbleCuttingBoard;
    [SerializeField] private ItemData _poisonCloud;
    [SerializeField] private ItemData _dying;
    [SerializeField] private ItemData _angry;
    [SerializeField] private ItemData _poisoned;
    [SerializeField] private ItemData _cleaning;
    
    [Header("Items")]
    [SerializeField] private ItemData _noodles;
    [SerializeField] private ItemData _sake;

    [Header("UI")] 
    [SerializeField] private ItemData _bountyToken;
    
    #region Properties

    public ItemData NoodleBowl => _noodleBowl;
    public ItemData NoodlePotEmpty => _noodlePotEmpty;
    public ItemData NoodlePotCooking => _noodlePotCooking;
    public ItemData NoodlePotCooked => _noodlePotCooked;
    public ItemData NoodlePotOvercooked => _noodlePotOvercooked;
    public ItemData Noodles => _noodles;
    public ItemData WaitForSeat => _waitForSeat;
    public ItemData WaitForCheckout => _waitForCheckout;
    public ItemData ThinkBubble => _thinkBubble;
    public ItemData Thinking => _thinking;
    public ItemData ThinkBubbleTable => _thinkBubbleTable;
    public ItemData ThinkBubbleTableMultiHorizontal => _thinkBubbleTableMultiHorizontal;
    public ItemData ThinkBubbleTableMultiVertical => _thinkBubbleTableMultiVertical;
    public ItemData Eating => _eating;
    public ItemData QuestionMark => _questionMark;
    public ItemData CuttingBoardUI => _cuttingBoardUI;
    public ItemData CuttingBoard => _cuttingBoard;
    public ItemData ThinkBubbleCuttingBoard => _thinkBubbleCuttingBoard;
    public ItemData Trash => _trash;
    public ItemData PoisonCloud => _poisonCloud;
    public ItemData Dying => _dying;
    public ItemData Angry => _angry;
    public ItemData Poisoned => _poisoned;
    public ItemData Cleaning => _cleaning;
    public ItemData BountyToken => _bountyToken;
    public ItemData Sake => _sake;
    
    #endregion

    #region Boilerplate

    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Identifiers.asset";
    
    private static Identifiers _data;
    public static Identifiers Value => _data ??= GetSettings();
    
    internal static Identifiers GetSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<Identifiers>(SETTINGS_PATH);
        if (settings != null) return settings;

        throw new Exception("Could not find the identifiers configuration.");
#else
        return References.Ids;
#endif
    }

#if UNITY_EDITOR
    [MenuItem("Galaxy Gobbles/Reload Identifiers")]
    public static void ReloadSettings()
    {
        AssetDatabase.ReleaseCachedFileHandles();
        AssetDatabase.Refresh();
        _data = GetSettings();
    }
#endif

    private void OnValidate()
    {
        GameSettings.GetItemMatch(NoodleBowl);
        GameSettings.GetItemMatch(NoodlePotEmpty);
        GameSettings.GetItemMatch(NoodlePotCooking);
        GameSettings.GetItemMatch(NoodlePotCooked);
        GameSettings.GetItemMatch(NoodlePotOvercooked);
        GameSettings.GetItemMatch(Noodles);
        GameSettings.GetItemMatch(WaitForSeat);
        GameSettings.GetItemMatch(WaitForCheckout);
        GameSettings.GetItemMatch(ThinkBubble);
        GameSettings.GetItemMatch(Thinking);
        GameSettings.GetItemMatch(ThinkBubbleTable);
        GameSettings.GetItemMatch(ThinkBubbleTableMultiHorizontal);
        GameSettings.GetItemMatch(ThinkBubbleTableMultiVertical);
        GameSettings.GetItemMatch(Eating);
        GameSettings.GetItemMatch(QuestionMark);
        GameSettings.GetItemMatch(CuttingBoardUI);
        GameSettings.GetItemMatch(CuttingBoard);
        GameSettings.GetItemMatch(ThinkBubbleCuttingBoard);
        GameSettings.GetItemMatch(Trash);
    }

    #endregion
}