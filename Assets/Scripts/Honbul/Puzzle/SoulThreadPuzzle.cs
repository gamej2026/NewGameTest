using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Honbul
{
    public class SoulThreadPuzzle : MonoBehaviour
    {
        public Camera cam;
        public SubtitleView story;
        public ObjectiveView objective;

        public event System.Action OnSolved;

        private readonly List<MemoryNode> nodes = new List<MemoryNode>();
        private readonly List<SoulThreadConnection> edges = new List<SoulThreadConnection>();

        private bool active;
        private MemoryNode dragStart;
        private LineRenderer previewLine;
        private bool wrongRoutineRunning;

        public void Build(MemoryNodeData[] datas, Vector3 origin)
        {
            nodes.Clear();
            ClearEdges();

            GameObject root = new GameObject("SoulThreadNodes");
            root.transform.SetParent(transform, false);

            Material nodeMaterial = HanokAlleyBuilder.UrpLit(GameConfig.MemoryGold, true);

            for (int i = 0; i < datas.Length; i++)
            {
                MemoryNodeData data = datas[i];
                GameObject nodeObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                nodeObject.name = "MemoryNode_" + data.Id;
                nodeObject.transform.SetParent(root.transform, false);
                nodeObject.transform.position = origin + data.WorldOffset;
                nodeObject.transform.localScale = Vector3.one * 0.45f;

                Renderer renderer = nodeObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.sharedMaterial = nodeMaterial;
                }

                if (nodeObject.GetComponent<Collider>() == null)
                {
                    nodeObject.AddComponent<SphereCollider>();
                }

                MemoryNode node = nodeObject.AddComponent<MemoryNode>();
                node.data = data;
                node.rend = renderer;

                nodes.Add(node);
                nodeObject.SetActive(false);
            }

            active = false;
        }

        public void Unlock()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i] != null)
                {
                    nodes[i].gameObject.SetActive(true);
                }
            }

            active = true;

            if (objective != null)
            {
                objective.SetObjective(PuzzleData.ObjectiveConnect());
            }
        }

        private void Update()
        {
            if (!active || cam == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (TryPickNode(out MemoryNode picked))
                {
                    dragStart = picked;
                    dragStart.SetHighlight(true);
                    CreatePreviewLine(dragStart.AnchorPoint);
                }
            }

            if (dragStart != null && Input.GetMouseButton(0))
            {
                UpdatePreviewLine();
            }

            if (dragStart != null && Input.GetMouseButtonUp(0))
            {
                if (TryPickNode(out MemoryNode endNode) && endNode != dragStart)
                {
                    AddEdge(dragStart, endNode);
                }

                dragStart.SetHighlight(false);
                dragStart = null;
                DestroyPreviewLine();
            }
        }

        private bool TryPickNode(out MemoryNode node)
        {
            node = null;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 300f))
            {
                return false;
            }

            node = hit.collider.GetComponent<MemoryNode>();
            if (node == null)
            {
                node = hit.collider.GetComponentInParent<MemoryNode>();
            }

            return node != null;
        }

        private void CreatePreviewLine(Vector3 start)
        {
            GameObject preview = new GameObject("SoulThreadPreview");
            preview.transform.SetParent(transform, false);
            previewLine = preview.AddComponent<LineRenderer>();
            previewLine.positionCount = 2;
            previewLine.widthMultiplier = 0.03f;
            previewLine.useWorldSpace = true;
            previewLine.material = new Material(Shader.Find("Sprites/Default"));
            previewLine.startColor = GameConfig.MemoryGold;
            previewLine.endColor = GameConfig.MemoryGold;
            previewLine.SetPosition(0, start);
            previewLine.SetPosition(1, start);
        }

        private void UpdatePreviewLine()
        {
            if (previewLine == null || dragStart == null)
            {
                return;
            }

            previewLine.SetPosition(0, dragStart.AnchorPoint);
            previewLine.SetPosition(1, MouseWorldPoint(dragStart.AnchorPoint.y));
        }

        private Vector3 MouseWorldPoint(float yPlane)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0f, yPlane, 0f));
            if (plane.Raycast(ray, out float enter))
            {
                return ray.GetPoint(enter);
            }

            return dragStart != null ? dragStart.AnchorPoint : Vector3.zero;
        }

        private void DestroyPreviewLine()
        {
            if (previewLine == null)
            {
                return;
            }

            Destroy(previewLine.gameObject);
            previewLine = null;
        }

        private bool AddEdge(MemoryNode a, MemoryNode b)
        {
            if (a == null || b == null || a == b)
            {
                return false;
            }

            for (int i = 0; i < edges.Count; i++)
            {
                SoulThreadConnection edge = edges[i];
                if (edge == null)
                {
                    continue;
                }

                bool sameDir = edge.a == a && edge.b == b;
                bool reverseDir = edge.a == b && edge.b == a;
                if (sameDir || reverseDir)
                {
                    return false;
                }
            }

            GameObject edgeObject = new GameObject("SoulThreadConnection_" + a.data.Id + "_" + b.data.Id);
            edgeObject.transform.SetParent(transform, false);
            SoulThreadConnection connection = edgeObject.AddComponent<SoulThreadConnection>();
            connection.Init(a, b);
            edges.Add(connection);

            TryValidate();
            return true;
        }

        private void TryValidate()
        {
            if (nodes.Count == 0)
            {
                return;
            }

            if (edges.Count != nodes.Count - 1)
            {
                Debug.Log("[SoulThreadPuzzle] Incomplete chain: " + edges.Count + " / " + (nodes.Count - 1));
                return;
            }

            Dictionary<MemoryNode, List<MemoryNode>> adjacency = new Dictionary<MemoryNode, List<MemoryNode>>();
            Dictionary<MemoryNode, int> degree = new Dictionary<MemoryNode, int>();

            for (int i = 0; i < nodes.Count; i++)
            {
                adjacency[nodes[i]] = new List<MemoryNode>();
                degree[nodes[i]] = 0;
            }

            for (int i = 0; i < edges.Count; i++)
            {
                SoulThreadConnection edge = edges[i];
                if (edge == null || edge.a == null || edge.b == null)
                {
                    continue;
                }

                adjacency[edge.a].Add(edge.b);
                adjacency[edge.b].Add(edge.a);
                degree[edge.a]++;
                degree[edge.b]++;
            }

            int endpoints = 0;
            MemoryNode startEndpoint = null;

            for (int i = 0; i < nodes.Count; i++)
            {
                int d = degree[nodes[i]];
                if (d > 2)
                {
                    Debug.Log("[SoulThreadPuzzle] Wrong chain: degree > 2");
                    WrongFeedback();
                    return;
                }

                if (d == 1)
                {
                    endpoints++;
                    if (startEndpoint == null)
                    {
                        startEndpoint = nodes[i];
                    }
                }
            }

            if (endpoints != 2 || startEndpoint == null)
            {
                Debug.Log("[SoulThreadPuzzle] Wrong chain: invalid endpoints");
                WrongFeedback();
                return;
            }

            HashSet<MemoryNode> visited = new HashSet<MemoryNode>();
            Queue<MemoryNode> queue = new Queue<MemoryNode>();
            queue.Enqueue(startEndpoint);
            visited.Add(startEndpoint);

            while (queue.Count > 0)
            {
                MemoryNode cur = queue.Dequeue();
                List<MemoryNode> nexts = adjacency[cur];
                for (int i = 0; i < nexts.Count; i++)
                {
                    MemoryNode next = nexts[i];
                    if (visited.Contains(next))
                    {
                        continue;
                    }

                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }

            if (visited.Count != nodes.Count)
            {
                Debug.Log("[SoulThreadPuzzle] Wrong chain: disconnected graph");
                WrongFeedback();
                return;
            }

            List<int> orderSequence = new List<int>();
            MemoryNode prev = null;
            MemoryNode current = startEndpoint;

            while (current != null)
            {
                orderSequence.Add(current.data.CorrectOrder);

                MemoryNode next = null;
                List<MemoryNode> neighbors = adjacency[current];
                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (neighbors[i] != prev)
                    {
                        next = neighbors[i];
                        break;
                    }
                }

                prev = current;
                current = next;
            }

            bool ascending = true;
            bool descending = true;

            if (orderSequence.Count != nodes.Count)
            {
                ascending = false;
                descending = false;
            }
            else
            {
                for (int i = 0; i < orderSequence.Count; i++)
                {
                    if (orderSequence[i] != i)
                    {
                        ascending = false;
                    }

                    if (orderSequence[i] != nodes.Count - 1 - i)
                    {
                        descending = false;
                    }
                }
            }

            if (ascending || descending)
            {
                Solve();
                return;
            }

            Debug.Log("[SoulThreadPuzzle] Wrong chain: order mismatch");
            WrongFeedback();
        }

        private void Solve()
        {
            active = false;
            GameState.PuzzleSolved = true;

            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i] != null)
                {
                    edges[i].SetColor(GameConfig.SpiritTeal);
                }
            }

            if (story != null)
            {
                story.ShowSequence(PuzzleData.StoryOnComplete(), 3.1f);
            }

            Debug.Log("[SoulThreadPuzzle] Solved");
            OnSolved?.Invoke();
        }

        private void WrongFeedback()
        {
            if (wrongRoutineRunning)
            {
                return;
            }

            StartCoroutine(WrongFeedbackRoutine());
        }

        private IEnumerator WrongFeedbackRoutine()
        {
            wrongRoutineRunning = true;

            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i] != null)
                {
                    edges[i].SetColor(Color.red);
                }
            }

            yield return new WaitForSeconds(0.25f);

            ClearEdges();
            wrongRoutineRunning = false;
        }

        private void ClearEdges()
        {
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i] != null)
                {
                    Destroy(edges[i].gameObject);
                }
            }

            edges.Clear();
        }
    }
}
