using UnityEngine;
using System;

/// <summary>
///     Add this component to entities that generate a force.
/// </summary>
public class ForceCreator : MonoBehaviour {
    /// <summary>
    ///     This attribute must be an int and specify the layer this force is applied to. The value is treated as
    ///     a mask, meaning a bitwise operation (and) is made to determine if the entity should be treeated in the
    ///     force computation.
    /// </summary>
    public int forceLayerMask;
}