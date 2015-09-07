using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CateringKingCalculator.Extensions
{
    public partial class ObservableCollectionExt<T> : ObservableCollection<T>
    {
        public ObservableCollectionExt() : base() { }

        public ObservableCollectionExt(List<T> list) 
            : base((list != null) ? new List<T>(list.Count) : list)
        {
            CopyFrom(list);
        }

        public void Find(T item)
        {
            
        }

        private void CopyFrom(IEnumerable<T> collection)
        {
            IList<T> items = Items;
            if (collection != null && items != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        items.Add(enumerator.Current);
                    }
                }
            }
        }

    }
}
