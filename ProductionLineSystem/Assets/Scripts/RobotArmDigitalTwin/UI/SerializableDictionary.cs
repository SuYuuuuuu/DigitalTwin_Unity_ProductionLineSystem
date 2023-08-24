using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SerializableDictionary<Tkey,TValue>
{
    [SerializeField]
    public List<Tkey>  keys;
    [SerializeField]
    public List<TValue> values;
}
