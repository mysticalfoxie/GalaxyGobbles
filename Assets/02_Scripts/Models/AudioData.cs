
    using UnityEngine;
    using UnityEngine.Serialization;

    [CreateAssetMenu(fileName = "Audio", menuName = "Galaxy Gobbles/Audio", order = 2)]
    public class AudioData : ScriptableObject
    {
        [FormerlySerializedAs("_audio")]
        [Header("Audio")]
        [SerializeField] private AudioClip _source;
        [SerializeField] [Range(0, 100)] private int _volume = 100;
        [SerializeField] private bool _loop;
        
        public AudioClip Source => _source;
        public bool Loop => _loop;
        public int Volume => _volume;
    }