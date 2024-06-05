
    using System;
    using TMPro;
    using UnityEngine;

    public class LevelIndicator : MonoBehaviour
    {
        private TMP_Text _tmpText;

        public void Awake()
        {
            _tmpText = this.GetRequiredComponent<TMP_Text>();
        }

        public void OnLevelLoaded()
        {
            var level = (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
            _tmpText.text = $"Level #{level}";
        }
    }