using UnityEngine;
using UnityEngine.UI;

public class LevelIndicator : MonoBehaviour
    {
        private Text _text;

        public void Awake()
        {
            _text = this.GetRequiredComponent<Text>();
        }

        public void OnLevelLoaded()
        {
            var level = (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
            _text.text = $"Level #{level}";
        }
    }