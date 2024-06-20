using UnityEngine;

public class WaitArea : MonoBehaviour
    {
        [Header("Ordering the slots")]
        [SerializeField] [Range(0, 10)] private int _order;
        [SerializeField] [Range(0, 10)] private int _column;
        
        public int Order => _order;
        public int Column => _column;
        public Customer Customer { get; private set; }
   
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.HSVToRGB(.8F, .7F, .7F);
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }

        public void SetCustomer(Customer customer)
        {
            Customer = customer;

            var floorPosition = gameObject.transform.position;
            var offsetY = Customer.Renderer.Bounds.size.y / 2;
            var position = new Vector3(floorPosition.x, floorPosition.y + offsetY, floorPosition.z);
            Customer.gameObject.transform.position = position;
            Customer.gameObject.SetActive(true);
        }

        public void RemoveCustomer()
        {
            Customer = null;
        }
    }