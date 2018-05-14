using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public SelectableType selectableType;
}

public enum SelectableType
{
    Resource,
}
