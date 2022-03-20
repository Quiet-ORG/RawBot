using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RawBot.State.Model
{
    public delegate void ItemHandler<in T>(T obj);

    public abstract class ListManager<TObject, TKey> : IEnumerable<TObject>
    {
        private readonly object _lock = new();
        private readonly List<TObject> _list = new();
        private readonly Dictionary<TKey, TObject> _lookup = new();

        public event ItemHandler<TObject> ItemAdded;
        public event ItemHandler<TObject> ItemRemoved;

        public List<TObject> Items
        {
            get
            {
                lock (_lock)
                {
                    return _list.ToList();
                }
            }
        }

        public TObject Get(TKey key)
        {
            lock (_lock)
            {
                return _lookup.GetValueOrDefault(key);
            }
        }

        public void Add(TObject item)
        {
            lock (_lock)
            {
                _list.Add(item);
                _lookup[GetKey(item)] = item;
            }

            OnItemAdded(item);
        }

        public void AddRange(IEnumerable<TObject> items, bool triggerEvent = true)
        {
            var toAdd = items.ToList();
            lock (_lock)
            {
                foreach (var item in toAdd)
                {
                    _list.Add(item);
                    _lookup[GetKey(item)] = item;
                }
            }

            if (triggerEvent)
            {
                toAdd.ForEach(OnItemAdded);
            }
        }

        public void Remove(TKey key)
        {
            lock (_lock)
            {
                var item = Get(key);
                Remove(item);
            }
        }

        public void Remove(TObject item)
        {
            bool removed;
            lock (_lock)
            {
                removed = _list.Remove(item);
                _lookup.Remove(GetKey(item));
            }

            if (removed)
            {
                OnItemRemoved(item);
            }
        }

        public void RemoveFirst(Predicate<TObject> predicate)
        {
            var removeIndex = -1;
            TObject toRemove = default;
            lock (_lock)
            {
                for (var i = 0; i < _list.Count; i++)
                {
                    if (predicate(_list[i]))
                    {
                        removeIndex = i;
                        toRemove = _list[i];
                        break;
                    }
                }

                if (removeIndex > -1)
                {
                    _list.RemoveAt(removeIndex);
                    _lookup.Remove(GetKey(toRemove));
                }
            }

            if (removeIndex > -1)
            {
                OnItemRemoved(toRemove);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _list.Clear();
                _lookup.Clear();
            }
        }

        public void Set(IEnumerable<TObject> set)
        {
            lock (_lock)
            {
                _list.Clear();
                _lookup.Clear();
                _list.AddRange(set);
                _list.ForEach(i => _lookup[GetKey(i)] = i);
            }
        }

        public TObject Find(Predicate<TObject> predicate)
        {
            lock (_lock)
            {
                return _list.Find(predicate);
            }
        }

        protected void OnItemAdded(TObject item)
        {
            ItemAdded?.Invoke(item);
        }

        protected void OnItemRemoved(TObject item)
        {
            ItemRemoved?.Invoke(item);
        }

        protected abstract TKey GetKey(TObject obj);

        public IEnumerator<TObject> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
