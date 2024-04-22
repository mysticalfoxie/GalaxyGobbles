using UnityEngine;

public class SelectionHandler : MonoBehaviour
    {
        public static SelectionHandler Instance { get; private set; }
        public SelectableMonoBehaviour Selection { get; private set; }

        public void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(Instance);
        }

        public void Register(SelectableMonoBehaviour selectable)
        {
            selectable.Click += (@object, _) =>
            {
                if (@object is not SelectableMonoBehaviour se) 
                    return;
                
                HandleSelection(se);
            };
        }

        private void HandleSelection(SelectableMonoBehaviour selectable)
        {
            if (selectable.Selected)
            {
                selectable.Deselect();
                return;
            }
            
            if (!selectable.IsSelectable()) return;
            if (Selection is not null) 
                Selection.Deselect();
            
            selectable.Select();
            Selection = selectable;
        }
    }