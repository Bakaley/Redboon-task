using System;
using UnityEngine;

namespace Pathfinding
{

    
    [RequireComponent(typeof(LineRenderer))]
    public class RectangleDrawer : MonoBehaviour
    {
        [SerializeField] LineRenderer _lineRenderer;

        private Vector3[] _positions = new Vector3[4];

        private Vector2 min;
        private Vector2 max;

        public Rectangle Rectangle => new Rectangle() {Min = min, Max = max};

        public void Redraw(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
            
            _positions[0] = new Vector3(min.x, min.y, 0);
            _positions[1] = new Vector3(min.x, max.y, 0);
            _positions[2] = new Vector3(max.x, max.y, 0);
            _positions[3] = new Vector3(max.x, min.y, 0);

            _lineRenderer.SetPositions(_positions);
        }
    }
}
