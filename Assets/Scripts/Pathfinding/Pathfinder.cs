using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class Pathfinder : MonoBehaviour, IPathFinder
    {
        private readonly float _intersectionOffsetModifier = 0.01f;
        private readonly float _rayLength = 500;
        
        private IEnumerable<Rectangle> _rectangles;
        private IEnumerable<Edge> _edges;


        private List<Vector2> _startDotAngles = new List<Vector2>();
        private List<Vector2> _endDotAngles = new List<Vector2>();
        
        private List<AngleRay> _startPointAngleRays = new List<AngleRay>();
        private List<AngleRay> _endPointAngleRays = new List<AngleRay>();
        
        private List<AngleRayIntersectionResult> _intersectionResults = new List<AngleRayIntersectionResult>();
        
        private List<Vector2> _resultingDots = new List<Vector2>();

        private struct DotRectStruct
        {
            public Vector2 DotPosition;
            public Rectangle Rectangle;
        }
        
        private struct AngleRay
        {
            public Vector2 RayStartPoint;
            public Vector2 RayEndPoint;
        }
        
        private struct AngleRayIntersectionResult
        {
            public Vector2 IntersectionPoint;
            public float pathLength;
        }

        private DotRectStruct _startDot;
        private DotRectStruct _endDot;
        
        public void SetInputData(IEnumerable<Rectangle> rectangles)
        {
            _rectangles = rectangles;
        }

        public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges)
        {
            _edges = edges;
            _resultingDots.Clear();
            
            if (IsPathExists(_rectangles, A, C))
            {
                BuildPath();
            }
            return _resultingDots;
        }

        //actually we could put this method somewhere else
        private bool IsPathExists(IEnumerable<Rectangle> rectangles, Vector2 A, Vector2 C)
        {
            bool startDotInited = false;
            bool endDotInited = false;
            foreach (var rectangle in rectangles)
            {
                if (!startDotInited && IsRectangleContains(rectangle, A))
                {
                    if (IsRectangleIsolated(rectangle))
                    {
                        Debug.Log("Dot A is isolated from other rectangles, couldn't find path");
                        return false;
                    }
                    _startDot = new DotRectStruct() {DotPosition = A, Rectangle = rectangle};
                    startDotInited = true;
                }

                if (!endDotInited && IsRectangleContains(rectangle, C))
                {
                    if (IsRectangleIsolated(rectangle))
                    {
                        Debug.Log("Dot C is isolated from other rectangles, couldn't find path");
                        return false;
                    }
                    _endDot = new DotRectStruct() {DotPosition = C, Rectangle = rectangle};
                    endDotInited = true;
                }
            }
            if(!startDotInited) Debug.Log("Dot A does not belong to any rectangle, couldn't find path");
            if(!endDotInited) Debug.Log("Dot C does not belong to any rectangle, couldn't find path");
            return startDotInited && endDotInited;
        }

        private bool IsRectangleContains(Rectangle rectangle, Vector2 dot)
        {
            return MathUtils.IsIntervalContains(dot.x, rectangle.Min.x, rectangle.Max.x)
                   && MathUtils.IsIntervalContains(dot.y, rectangle.Min.y, rectangle.Max.y);
        }
        

        private bool IsRectangleIsolated(Rectangle rectangle)
        {
            foreach (var edge in _edges)
            {
                if (edge.First == rectangle || edge.Second ==  rectangle) return false;
            }
            return true;
        }

        private bool IsTwoRectanglesHaveEdge(Rectangle rectangle1, Rectangle rectangle2, out Edge outEdge)
        {
            foreach (var edge in _edges)
            {
                if (edge.First == rectangle1 && edge.Second == rectangle2
                    || edge.First == rectangle2 && edge.Second == rectangle1)
                {
                    outEdge = edge;
                    return true;
                }
            }
            outEdge = new Edge();
            return false;
        }

        private void BuildPath()
        {
            //Straight line checks
            if (SameRectangleCheck()) return;
            if (NeighbourRectangleCheck()) return;
            if (StraightLineCheck()) return;
            //Complex line with 1 turn checks
            CalculateAnglesInFieldOfView();
            FillAngleRayLists();
            if(AngleRayCrossCheck()) return;
            //Complex line with 2 turns checks
        }

        private bool SameRectangleCheck()
        {
            if (_startDot.Rectangle == _endDot.Rectangle)
            {
                _resultingDots.Add(_startDot.DotPosition);
                _resultingDots.Add(_endDot.DotPosition);
                return true;
            }
            return false;
        }

        private bool NeighbourRectangleCheck()
        {
            if (IsTwoRectanglesHaveEdge(_startDot.Rectangle, _endDot.Rectangle, out Edge jointEdge))
            {
                if (MathUtils.IntersectLineSegments2D(_startDot.DotPosition,
                    _endDot.DotPosition,
                    jointEdge.Start,
                    jointEdge.End,
                    out Vector2 point))
                {
                    _resultingDots.Add(_startDot.DotPosition);
                    _resultingDots.Add(_endDot.DotPosition);
                    return true;
                }
            }
            return false;
        }

        private bool StraightLineCheck()
        {
            int intersectionCount = 0;
            foreach (var edge in _edges)
            {
                if (MathUtils.IntersectLineSegments2D(_startDot.DotPosition,
                    _endDot.DotPosition,
                    edge.Start,
                    edge.End,
                    out Vector2 point))
                {
                    intersectionCount++;
                }
            }

            if (intersectionCount == 2)
            {
                _resultingDots.Add(_startDot.DotPosition);
                _resultingDots.Add(_endDot.DotPosition);
                return true;
            }
            return false;
        }

        private void FillAngleRayLists()
        {
            _startPointAngleRays.Clear();
            _endPointAngleRays.Clear();
            
            foreach (var angle in _startDotAngles)
            {
                _startPointAngleRays.Add(CastAngleRay(_startDot.DotPosition, angle));
            }
            
            foreach (var angle in _endDotAngles)
            {
                _endPointAngleRays.Add(CastAngleRay(_endDot.DotPosition, angle));
            }
        }
        
        private bool AngleRayCrossCheck()
        {
            _intersectionResults.Clear();
            foreach (var rayFromStart in _startPointAngleRays)
            {
                foreach (var rayFromEnd in _endPointAngleRays)
                {
                    if (MathUtils.IntersectLineSegments2D(rayFromStart.RayStartPoint, rayFromStart.RayEndPoint,
                        rayFromEnd.RayStartPoint, rayFromEnd.RayEndPoint, out Vector2 intersection))
                    {
                        AngleRayIntersectionResult result = new AngleRayIntersectionResult()
                        {
                            IntersectionPoint = intersection,
                            pathLength = (_startDot.DotPosition - intersection).magnitude +
                                         (_endDot.DotPosition - intersection).magnitude
                        };
                        _intersectionResults.Add(result);
                    }
                }
            }

            if (_intersectionResults.Count != 0)
            {
                AngleRayIntersectionResult minResult = new AngleRayIntersectionResult()
                {
                    IntersectionPoint = Vector2.zero,
                    pathLength = Single.MaxValue
                };
                foreach (var intersectionResult in _intersectionResults)
                {
                    if (intersectionResult.pathLength < minResult.pathLength)
                    {
                        minResult = intersectionResult;
                    }
                }
                _resultingDots.Add(_startDot.DotPosition);
                _resultingDots.Add(minResult.IntersectionPoint);
                _resultingDots.Add(_endDot.DotPosition);
                return true;
            }
            return false;
        }
        
        private AngleRay CastAngleRay(Vector2 dot, Vector2 anglePoint)
        {
            Vector2 direction = (anglePoint - dot).normalized;
            Vector2 intersectionOffset = direction * _intersectionOffsetModifier;
            foreach (var rectangle in _rectangles)
            {
                if (IsRectangleContains(rectangle, anglePoint + intersectionOffset))
                {
                    Vector2 rayEndPoint = IntersectRectangle(anglePoint + intersectionOffset, direction, rectangle);
                    if (FindRectOfPoint(rayEndPoint, out Rectangle foundRect))
                    {
                        rayEndPoint = IntersectRectangle(rayEndPoint, direction, foundRect);
                    }

                    AngleRay ray = new AngleRay()
                    {
                        RayStartPoint = dot,
                        RayEndPoint = rayEndPoint
                    };

                    return ray;
                }
            }
            //that's not possible
            return new AngleRay();
        }

        private Vector2 IntersectRectangle(Vector2 startPoint, Vector2 direction, Rectangle rectangle)
        {
            Vector2 finalPoint = startPoint + direction * _rayLength;
            Vector2 intersectionPoint;
            Vector2 exitPoint;

            if (MathUtils.IntersectLineSegments2D(startPoint, finalPoint,
                new Vector2(rectangle.Min.x, rectangle.Min.y),
                new Vector2(rectangle.Max.x, rectangle.Min.y), 
                out intersectionPoint))
            {
                exitPoint = intersectionPoint + direction * _intersectionOffsetModifier;
                return exitPoint;
            }

            if (MathUtils.IntersectLineSegments2D(startPoint, finalPoint,
                new Vector2(rectangle.Max.x, rectangle.Min.y),
                new Vector2(rectangle.Max.x, rectangle.Max.y), 
                out intersectionPoint))
            {
                exitPoint = intersectionPoint + direction * _intersectionOffsetModifier;
                return exitPoint;
            }
            
            
            if (MathUtils.IntersectLineSegments2D(startPoint, finalPoint,
                new Vector2(rectangle.Max.x, rectangle.Max.y),
                new Vector2(rectangle.Min.x, rectangle.Max.y), 
                out intersectionPoint))
            {
                exitPoint = intersectionPoint + direction * _intersectionOffsetModifier;
                return exitPoint;
            }
            
            if (MathUtils.IntersectLineSegments2D(startPoint, finalPoint,
                new Vector2(rectangle.Min.x, rectangle.Max.y),
                new Vector2(rectangle.Min.x, rectangle.Min.y), 
                out intersectionPoint))
            {
                exitPoint = intersectionPoint + direction * _intersectionOffsetModifier;
                return exitPoint;
            }
            //that's not possible
            exitPoint = new Vector2();
            return exitPoint;
        }

        private void CalculateAnglesInFieldOfView()
        {
            Edge startMainEdge = new Edge();
            Edge endMainEdge = new Edge();
            _startDotAngles.Clear();
            _endDotAngles.Clear();
            
            foreach (var edge in _edges)
            {
                if (edge.First == _startDot.Rectangle || edge.Second == _startDot.Rectangle)
                {
                    _startDotAngles.Add(edge.Start);
                    _startDotAngles.Add(edge.End);
                    startMainEdge = edge;
                }

                if (edge.First == _endDot.Rectangle || edge.Second == _endDot.Rectangle)
                {
                    _endDotAngles.Add(edge.Start);
                    _endDotAngles.Add(edge.End);
                    endMainEdge = edge;
                }
            }
            
            foreach (var edge in _edges)
            {
                if (edge.First != _startDot.Rectangle && edge.Second != _startDot.Rectangle)
                {
                    if (MathUtils.IntersectLineSegments2D(_startDot.DotPosition, edge.Start, startMainEdge.Start,
                        startMainEdge.End, out Vector2 intersection1))
                    {
                        _startDotAngles.Add(edge.Start);
                    }

                    if (MathUtils.IntersectLineSegments2D(_startDot.DotPosition, edge.End, startMainEdge.Start,
                        startMainEdge.End, out Vector2 intersection2))
                    {
                        _startDotAngles.Add(edge.End);
                    }
                }

                if (edge.First != _endDot.Rectangle && edge.Second != _endDot.Rectangle)
                {
                    if(MathUtils.IntersectLineSegments2D(_endDot.DotPosition, edge.Start, endMainEdge.Start,
                        endMainEdge.End, out Vector2 intersection1)) _endDotAngles.Add(edge.Start);
                    if(MathUtils.IntersectLineSegments2D(_endDot.DotPosition, edge.End, endMainEdge.Start,
                        endMainEdge.End, out Vector2 intersection2)) _endDotAngles.Add(edge.End);
                }
            }
        }


        private bool FindRectOfPoint(Vector2 point, out Rectangle outRectangle)
        {
            foreach (var rectangle in _rectangles)
            {
                if (IsRectangleContains(rectangle, point))
                {
                    outRectangle = rectangle;
                    return true;
                }
            }
            outRectangle = new Rectangle();
            return false; 
        }
    }
}