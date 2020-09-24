﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab3
{
    public class HashTable<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    { 
        public int Length { set; get; }
        private int _size { set; get; }

        

        private Node<TKey, TValue>[] _items = null;

        private bool[] _deleted = null;
        public HashTable()
        {
            Length = 0;
            _size = 32;
            _items = new Node<TKey, TValue>[_size];
            _deleted = new bool[_size];
        }

        public void Add(TKey Key, TValue Value)
        {
            int x = GetHash1(Key);
            int y = GetHash2(Key);
            if (_size- Length <= 0.1*_size)
                Resize();
            for (int i = 0; i < _size; i++)
            {
                if (_items[x] == null || _deleted[x])
                {
                    _deleted[x] = false;
                    _items[x] = new Node<TKey, TValue>(Key, Value);
                    Length++;
                    return;
                }
                x = (x + y) % _size;
            }
        }
        public bool ContainsKey(TKey key)
        {
            if (GetIndex(key) >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool GetValue(TKey key, out TValue value)
        {
            int index = GetIndex(key);
            if (index >= 0)
            {
                value = _items[index].Value;
                return true;
            }
            else
            {
                value = default(TValue);
                return false;
            }
        }
        private int GetIndex(TKey key)
        {
            int x = GetHash1(key);
            int y = GetHash2(key);
            for (int i = 0; i < _size; i++)
            {
                if (_items[x] != null)
                {
                    if (_items[x].Key.ToString() == key.ToString() && !_deleted[x])
                    {
                        return x;
                    }
                }
                else
                    return -1;
                x = (x + y) % _size;

            }
            return 0;
        }
        public void Remove(TKey Key)
        {
            int x = GetHash1(Key);
            int y = GetHash2(Key);
            for (int i = 0; i < _size; i++)
            {
                if (_items[x] != null)
                {
                    if (_items[x].Key.ToString() == Key.ToString())
                    {
                        _deleted[x] = true;
                        Length--;
                    }
                }
                else
                    return;
                x = (x + y) % _size;
            }
        }
        private void Resize()
        {
            this._size = _size * 2;
            Array.Resize(ref _items, _size);
            Array.Resize(ref _deleted, _size);
        }
        private int GetHash1(TKey Key)
        {

            return Math.Abs(Key.GetHashCode()) % _size;
        }
        private int GetHash2(TKey Key)
        {
            return 1 + Math.Abs(Key.GetHashCode()) % (_size - 2);
        }
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for(int i = 0; i < _items.Length; i++)
            {
                if (_items[i] != null)
                {
                    yield return new KeyValuePair<TKey, TValue>(_items[i].Key, _items[i].Value);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public TValue this[TKey key]
        {
            get
            {
                return _items[GetIndex(key)].Value;
            }
            set
            {
                _items[GetIndex(key)].Value = value;
            }
        }
    }
}
