using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Simple class for the serialization of List<List<T>

[Serializable]
public class ListWrapper<T>
{
    public List<T> list;
}
