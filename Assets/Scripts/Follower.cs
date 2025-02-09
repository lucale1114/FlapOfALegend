using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Follower.
/// </summary>
namespace Pathfinding
{
    [ExecuteInEditMode]
    public class Follower : MonoBehaviour
    {
        public event Action Travelling;
        [SerializeField]
        protected Graph m_Graph;
        [SerializeField]
        protected float m_Speed = 0.010f;
        public Node currentNode;
        public bool Moving;

        protected Path m_Path = new Path();
        protected Node m_Current;

        public void Start()
        {
            currentNode = GameObject.Find("StartNode").GetComponent<Node>();
        }

        public void StartFollow(Node m_Start, Node m_End)
        {
            if (Moving)
            {
                return;
            }
            m_Path = m_Graph.GetShortestPath(m_Start, m_End);
            Moving = true;
            Follow(m_Path);
            Travelling?.Invoke();
        }

        /// <summary>
        /// Follow the specified path.
        /// </summary>
        /// <param name="path">Path.</param>
        public void Follow(Path path)
        {
            StopCoroutine("FollowPath");
            m_Path = path;
            transform.position = m_Path.nodes[0].transform.GetChild(0).position;
            StartCoroutine("FollowPath");
        }

        /// <summary>
        /// Following the path.
        /// </summary>
        /// <returns>The path.</returns>
        IEnumerator FollowPath()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += Update;
#endif
            var e = m_Path.nodes.GetEnumerator();
            while (e.MoveNext())
            {
                m_Current = e.Current;

                // Wait until we reach the current target node and then go to next node
                yield return new WaitUntil(() =>
              {
                  return transform.position == m_Current.transform.GetChild(0).position;
              });
            }
            m_Current = null;
            Moving = false;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= Update;
#endif
        }

        void Update()
        {
            if (m_Current != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, m_Current.transform.GetChild(0).position, m_Speed * Time.deltaTime);
            }
        }

    }
}
