using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pathfinding
{
    public class EdgeDetector : MonoBehaviour
    {
        [ShowInInspector] [ReadOnly]
        private List<Edge> _edges = new List<Edge>();

        public List<Edge> DetectEdges(Rectangle rectangle1, Rectangle rectangle2, Rectangle rectangle3)
        {
            _edges.Clear();
            FindEdges(in _edges, rectangle1, rectangle2);
            FindEdges(in _edges, rectangle2, rectangle3);
            FindEdges(in _edges, rectangle1, rectangle3);
            return _edges;
        }

        private void FindEdges(in List<Edge> edges, Rectangle rectangle1, Rectangle rectangle2)
        {
            //first pair of sides
            if (MathUtils.IsNearlyEqual(rectangle1.Min.x,rectangle2.Max.x))
            {
                var needToAddEdge = IfEdgesIntersects(rectangle1.Min.y, rectangle1.Max.y,
                    rectangle2.Min.y, rectangle2.Max.y);
                if (needToAddEdge)
                {
                    (float, float) edgeBounds = FindEdgeBounds(rectangle1.Min.y, rectangle1.Max.y,
                    rectangle2.Min.y, rectangle2.Max.y);

                    Edge edge = new Edge()
                    {
                        First = rectangle1,
                        Second = rectangle2,
                        Start = new Vector3()
                        {
                            x = rectangle1.Min.x,
                            y = edgeBounds.Item1,
                            z = 0
                        },
                        End = new Vector3()
                        {
                            x = rectangle1.Min.x,
                            y = edgeBounds.Item2,
                            z = 0
                        }
                    };
                    edges.Add(edge);
                }
            }
            
            //second pair of sides
            if (MathUtils.IsNearlyEqual(rectangle1.Max.x, rectangle2.Min.x))
            {
                var needToAddEdge = IfEdgesIntersects(rectangle1.Min.y, rectangle1.Max.y,
                    rectangle2.Min.y, rectangle2.Max.y);
                if (needToAddEdge)
                {
                    (float, float) edgeBounds = FindEdgeBounds(rectangle1.Min.y, rectangle1.Max.y,
                        rectangle2.Min.y, rectangle2.Max.y);

                    Edge edge = new Edge()
                    {
                        First = rectangle1,
                        Second = rectangle2,
                        Start = new Vector3()
                        {
                            x = rectangle1.Max.x,
                            y = edgeBounds.Item1,
                            z = 0
                        },
                        End = new Vector3()
                        {
                            x = rectangle1.Max.x,
                            y = edgeBounds.Item2,
                            z = 0
                        }
                    };
                    edges.Add(edge);
                }
            }
            
            //third pair of sides
            if (MathUtils.IsNearlyEqual(rectangle1.Min.y, rectangle2.Max.y))
            {
                var needToAddEdge = IfEdgesIntersects(rectangle1.Min.x, rectangle1.Max.x,
                    rectangle2.Min.x, rectangle2.Max.x);
                if (needToAddEdge)
                {
                    (float, float) edgeBounds = FindEdgeBounds(rectangle1.Min.x, rectangle1.Max.x,
                        rectangle2.Min.x, rectangle2.Max.x);

                    Edge edge = new Edge()
                    {
                        First = rectangle1,
                        Second = rectangle2,
                        Start = new Vector3()
                        {
                            x = edgeBounds.Item1,
                            y = rectangle1.Min.y,
                            z = 0
                        },
                        End = new Vector3()
                        {
                            x = edgeBounds.Item2,
                            y = rectangle1.Min.y,
                            z = 0
                        }
                    };
                    edges.Add(edge);
                }
            }
            
            //fourth pair of sides
            if (MathUtils.IsNearlyEqual(rectangle1.Max.y, rectangle2.Min.y))
            {
                var needToAddEdge = IfEdgesIntersects(rectangle1.Min.x, rectangle1.Max.x,
                    rectangle2.Min.x, rectangle2.Max.x);
                if (needToAddEdge)
                {
                    (float, float) edgeBounds = FindEdgeBounds(rectangle1.Min.x, rectangle1.Max.x,
                        rectangle2.Min.x, rectangle2.Max.x);

                    Edge edge = new Edge()
                    {
                        First = rectangle1,
                        Second = rectangle2,
                        Start = new Vector3()
                        {
                            x = edgeBounds.Item1,
                            y = rectangle1.Max.y,
                            z = 0
                        },
                        End = new Vector3()
                        {
                            x = edgeBounds.Item2,
                            y = rectangle1.Max.y,
                            z = 0
                        }
                    };
                    edges.Add(edge);
                }
            }
        }

        private bool IfEdgesIntersects(float rect1Min, float rect1Max, float rect2Min, float rect2Max)
        {
            return MathUtils.IsIntervalContains(rect1Min, rect2Min, rect2Max)
                   || MathUtils.IsIntervalContains(rect1Max, rect2Min, rect2Max)
                   || MathUtils.IsIntervalContains(rect2Min, rect1Min, rect1Max)
                   || MathUtils.IsIntervalContains(rect2Max, rect1Min, rect1Max);
        }

        private (float, float) FindEdgeBounds(float rect1Min, float rect1Max, float rect2Min, float rect2Max)
        {
            float[] values = new[] {rect1Min, rect1Max, rect2Min, rect2Max};
            Array.Sort(values, (f1, f2) => f1.CompareTo(f2));
            return (values[1], values[2]);
        }
    }
}
