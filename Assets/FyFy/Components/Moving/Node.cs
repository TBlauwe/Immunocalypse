using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Node.
/// </summary>
public class Node : MonoBehaviour
{
	/// <summary>
	/// The connections (neighbors).
	/// </summary>
	public List<Node> connections = new List<Node> ();
}
