using UnityEngine;

public class WaitArea : MonoBehaviour
    {
        [Header("Ordering the slots")]
        [SerializeField] 
        [Range(0, 10)] 
        private int _order;
        
        public int Order => _order;
        public Customer Customer { get; private set; }
   
        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, new Vector3(0.5F, 0.1F, 0.5F));
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