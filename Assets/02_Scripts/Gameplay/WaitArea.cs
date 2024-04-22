
    using UnityEngine;

    public class WaitArea : MonoBehaviour
    {
        [Header("Ordering the slots")]
        [SerializeField] 
        [Range(0, 10)] 
        private int _order;
        
        public int Order => _order;
        public Customer Customer { get; set; }
    }