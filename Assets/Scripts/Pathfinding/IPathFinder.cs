using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pathfinding
{
    public struct Rectangle
    {
        public Vector2 Min;
        public Vector2 Max;
        
        public static bool operator ==(Rectangle r1, Rectangle r2) 
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Rectangle c1, Rectangle r2) 
        {
            return !c1.Equals(r2);
        }
    }
        
    [InlineEditor]
    public struct Edge
    {
        public Rectangle First;
        public Rectangle Second;
        public Vector3 Start;
        public Vector3 End;
    }
    
    public interface IPathFinder {
        IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges);
    }
}
