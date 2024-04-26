
     public class Noodles : Item
    {
        private bool _boiled;
        private bool _overcooked;

        public static Noodles Create()
        {
            var prefab = ReferencesSettings.Data.ItemPrefab;
            var instance = Instantiate(prefab);
            var item = instance.GetRequiredComponent<Noodles>();
            item.Data = References.Instance.Items.Noodles;
            return item;
        }
    }