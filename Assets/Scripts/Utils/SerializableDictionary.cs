using System.Collections.Generic;
using UnityEngine;

namespace PG.Game.Helpers {
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
        ISerializationCallbackReceiver {
        [SerializeField] List<TKey> _keys = new List<TKey>();
        [SerializeField] List<TValue> _values = new List<TValue>();

        public SerializableDictionary() { }
        public SerializableDictionary(TKey key, TValue value) { }

        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            SaveTheDictionaryToLists();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            LoadTheDictionaryFromLists();
        }

        void SaveTheDictionaryToLists() {
            _keys.Clear();
            _values.Clear();
            foreach (var pair in this) {
                _keys.Add(pair.Key);
                _values.Add(pair.Value);
            }
        }

        void LoadTheDictionaryFromLists() {
            Clear();

            if (_keys.Count != _values.Count)
                throw new System.Exception(string.Format(
                    "there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < _keys.Count; i++)
                Add(_keys[i], _values[i]);
        }
    }
}