using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class Identifiers : ScriptableObject
{ 
    public const string EMPTY_STRING = "< not set >";
    
    [Header("Machines")]
    [SerializeField] private string _noodleBowl = EMPTY_STRING;
    [SerializeField] private string _noodlePotEmpty = EMPTY_STRING;
    [SerializeField] private string _noodlePotCooking = EMPTY_STRING;
    [SerializeField] private string _noodlePotCooked = EMPTY_STRING;
    [SerializeField] private string _noodlePotOvercooked = EMPTY_STRING;
    [SerializeField] private string _cuttingBoard = EMPTY_STRING;
    [SerializeField] private string _cuttingBoardUI = EMPTY_STRING;
    [SerializeField] private string _trash = EMPTY_STRING;
    
    [Header("States")]
    [SerializeField] private string _waitForSeat = EMPTY_STRING;
    [SerializeField] private string _waitForCheckout = EMPTY_STRING;
    [SerializeField] private string _thinking = EMPTY_STRING;
    [SerializeField] private string _thinkBubble = EMPTY_STRING;
    [SerializeField] private string _eating = EMPTY_STRING;
    [SerializeField] private string _questionMark = EMPTY_STRING;
    [SerializeField] private string _thinkBubbleTable = EMPTY_STRING;
    [SerializeField] private string _thinkBubbleTableMultiHorizontal = EMPTY_STRING;
    [SerializeField] private string _thinkBubbleTableMultiVertical = EMPTY_STRING;
    [SerializeField] private string _thinkBubbleCuttingBoard = EMPTY_STRING;
    
    [Header("Items")]
    [SerializeField] private string _noodles = EMPTY_STRING;
    
    #region Properties

    public string NoodleBowl => _noodleBowl;
    public string NoodlePotEmpty => _noodlePotEmpty;
    public string NoodlePotCooking => _noodlePotCooking;
    public string NoodlePotCooked => _noodlePotCooked;
    public string NoodlePotOvercooked => _noodlePotOvercooked;
    public string Noodles => _noodles;
    public string WaitForSeat => _waitForSeat;
    public string WaitForCheckout => _waitForCheckout;
    public string ThinkBubble => _thinkBubble;
    public string Thinking => _thinking;
    public string ThinkBubbleTable => _thinkBubbleTable;
    public string ThinkBubbleTableMultiHorizontal => _thinkBubbleTableMultiHorizontal;
    public string ThinkBubbleTableMultiVertical => _thinkBubbleTableMultiVertical;
    public string Eating => _eating;
    public string QuestionMark => _questionMark;
    public string CuttingBoardUI => _cuttingBoardUI;
    public string CuttingBoard => _cuttingBoard;
    public string ThinkBubbleCuttingBoard => _thinkBubbleCuttingBoard;
    public string Trash => _trash;
    
    #endregion

    #region Boilerplate

    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Identifiers.asset";
    
    private static Identifiers _data;
    public static Identifiers Value => _data ??= GetOrCreateSettings();
    
    internal static Identifiers GetOrCreateSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<Identifiers>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
#else
        return References.Ids;
#endif
    }

    private static Identifiers CreateDefaultSettings()
    {
        return CreateInstance<Identifiers>();
    }

    private void OnValidate()
    {
        GameSettings.GetItemById(NoodleBowl);
        GameSettings.GetItemById(NoodlePotEmpty);
        GameSettings.GetItemById(NoodlePotCooking);
        GameSettings.GetItemById(NoodlePotCooked);
        GameSettings.GetItemById(NoodlePotOvercooked);
        GameSettings.GetItemById(Noodles);
        GameSettings.GetItemById(WaitForSeat);
        GameSettings.GetItemById(WaitForCheckout);
        GameSettings.GetItemById(ThinkBubble);
        GameSettings.GetItemById(Thinking);
        GameSettings.GetItemById(ThinkBubbleTable);
        GameSettings.GetItemById(ThinkBubbleTableMultiHorizontal);
        GameSettings.GetItemById(ThinkBubbleTableMultiVertical);
        GameSettings.GetItemById(Eating);
        GameSettings.GetItemById(QuestionMark);
        GameSettings.GetItemById(CuttingBoardUI);
        GameSettings.GetItemById(CuttingBoard);
        GameSettings.GetItemById(ThinkBubbleCuttingBoard);
        GameSettings.GetItemById(Trash);
    }

    #endregion
}