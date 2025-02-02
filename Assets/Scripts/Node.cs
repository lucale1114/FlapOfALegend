using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// The Node.
/// </summary>
namespace Pathfinding
{
    public class Node : MonoBehaviour
    {

        /// <summary>
        /// The connections (neighbors).
        /// </summary>
        [SerializeField]
        public List<Node> m_Connections = new List<Node>();

        /// <summary>
        /// Gets the connections (neighbors).
        /// </summary>
        /// <value>The connections.</value>
        public virtual List<Node> connections
        {
            get
            {
                return m_Connections;
            }
        }

        public Node this[int index]
        {
            get
            {
                return m_Connections[index];
            }
        }

        void OnValidate()
        {

            // Removing duplicate elements
            m_Connections = m_Connections.Distinct().ToList();
        }

    }
}