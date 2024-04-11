
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class SelectionHandler : MonoBehaviour
    {
        public static SelectionHandler Instance { get; private set; }

        private Customer _selectedCustomer;
        private readonly List<Table> _tables = new(); 
        
        public void Awake()
        {
            if (Instance is not null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        public void Select(ISelectable selectable)
        {
            if (selectable.Selected) return;
            if (_selectedCustomer is not null) return;
            if (selectable is not Customer customer) return;
            
            selectable.Select();
            _selectedCustomer = customer;
        }

        public void Register(TouchableMonoBehaviour selectable)
        {
            if (selectable is Table table) _tables.Add(table);
            // TODO: Add customer management here: if (selectable is Customer customer) _customer.Add(customer);
            selectable.Click += OnGameObjectClicked;
        }

        public void OnGameObjectClicked(object @object, EventArgs eventArgs)
        {
            HandleCustomerAssignment(@object);
        }

        private void HandleCustomerAssignment(object @object)
        {
            if (_selectedCustomer is null) return;
            if (@object is not Table table) return;
            if (table.SeatedCustomer) return;

            _tables
                .FirstOrDefault(x => x.SeatedCustomer == _selectedCustomer)
                ?.ClearSeat();
            
            table.Seat(_selectedCustomer);
            _selectedCustomer.Deselect();
            _selectedCustomer = null;
        }
    }