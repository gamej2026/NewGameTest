using System;
using UnityEngine;

namespace Honbul
{
    [Serializable]
    public class MemoryNodeData
    {
        public int Id;
        public int CorrectOrder;
        public string Title;
        public string Body;
        public Vector3 WorldOffset;

        public MemoryNodeData(int id, int correctOrder, string title, string body, Vector3 worldOffset)
        {
            Id = id;
            CorrectOrder = correctOrder;
            Title = title;
            Body = body;
            WorldOffset = worldOffset;
        }
    }
}
