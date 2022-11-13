using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pathfinding
{
    public class InputDataManager : MonoBehaviour
    {
        //there is no need to break classes

        [SerializeField] private RectangleDrawer _rect1;
        [SerializeField] private RectangleDrawer _rect2;
        [SerializeField] private RectangleDrawer _rect3;
        [SerializeField] private Dot _dotA;
        [SerializeField] private Dot _dotSampler;
        [SerializeField] private Dot _dotC;
        [SerializeField] private EdgeDetector _edgeDetector;
        [SerializeField] private LineRenderer _pathDrawer;
        [Range(0, 0.25f)] [SerializeField] private float _angleThreshold = 0.125f;
        [SerializeField] private Pathfinder _pathfinder;

        private List<Rectangle> _rectangles = new List<Rectangle>();

        [ShowIf("IsFirstRectIncorrect")]
        [InfoBox("X and Y of min point should be less then X and Y of max point", InfoMessageType.Error)]
        [ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
        private string error1 = "Error in rect 1!";

        [OnValueChanged("RedrawFirstRect")]
        [SerializeField] private Vector2 _rect1Min;
        [OnValueChanged("RedrawFirstRect")]
        [SerializeField] private Vector2 _rect1Max;
    
        [ShowIf("IsSecondRectIncorrect")]
        [InfoBox("X and Y of min point should be less then X and Y of max point", InfoMessageType.Error)]
        [ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
        private string error2 = "Error in rect 2!";
        
        [OnValueChanged("RedrawSecondRect")]
        [SerializeField] private Vector2 _rect2Min;
        [OnValueChanged("RedrawSecondRect")]
        [SerializeField] private Vector2 _rect2Max;
    
        [ShowIf("IsThirdRectIncorrect")]
        [InfoBox("X and Y of min point should be less then X and Y of max point", InfoMessageType.Error)]
        [ShowInInspector]
        [Sirenix.OdinInspector.ReadOnly]
        private string error3 = "Error in rect 3!";
        
        [OnValueChanged("RedrawThirdRect")]
        [SerializeField] private Vector2 _rect3Min;
        [OnValueChanged("RedrawThirdRect")]
        [SerializeField] private Vector2 _rect3Max;

        [OnValueChanged("RedrawDotA")]
        [SerializeField] private Vector2 _dotAPostion;
        [OnValueChanged("RedrawDotC")]
        [SerializeField] private Vector2 _dotCPostion;
        
        private List<Edge> _detectedEdges = new List<Edge>();

        private List<Rectangle> RectangleList
        {
            get
            {
                _rectangles.Clear();
                _rectangles.Add(_rect1.Rectangle);
                _rectangles.Add(_rect2.Rectangle);
                _rectangles.Add(_rect3.Rectangle);
                return _rectangles;
            }
        }

        private bool IsFirstRectIncorrect()
        {
            return !(_rect1Min.x < _rect1Max.x && _rect1Min.y < _rect1Max.y);
        }
        
        private bool IsSecondRectIncorrect()
        {
            return !(_rect2Min.x < _rect2Max.x && _rect2Min.y < _rect2Max.y);
        }
        
        private bool IsThirdRectIncorrect()
        {
            return !(_rect3Min.x < _rect3Max.x && _rect3Min.y < _rect3Max.y);
        }
        
        private void RedrawFirstRect()
        {
            _rect1.Redraw(_rect1Min, _rect1Max);
            DetectEdges();
            ClearPath();
        }
        
        private void RedrawSecondRect()
        {
            _rect2.Redraw(_rect2Min, _rect2Max);
            DetectEdges();
            ClearPath();
        }
        
        private void RedrawThirdRect()
        {
            _rect3.Redraw(_rect3Min, _rect3Max);
            DetectEdges();
            ClearPath();
        }

        private void RedrawDotA()
        {
            _dotA.transform.position = new Vector3(_dotAPostion.x, _dotAPostion.y, 0);
            ClearPath();
        }
        
        private void RedrawDotC()
        {
            _dotC.transform.position = new Vector3(_dotCPostion.x, _dotCPostion.y, 0);
            ClearPath();
        }

        private void DetectEdges()
        {
            _detectedEdges = _edgeDetector.DetectEdges(_rect1.Rectangle, _rect2.Rectangle, _rect3.Rectangle);
        }
        
        [Button("Calculate edges")]
        private void CalculateEdges()
        {
            ClearPath();
            RedrawFirstRect();
            RedrawSecondRect();
            RedrawThirdRect();
            DetectEdges();
        }

        [Button("Calculate path")]
        private void CalculatePath()
        {
            _pathfinder.SetInputData(RectangleList);
            var results = _pathfinder.GetPath(_dotA.transform.position, _dotC.transform.position, _detectedEdges);
            Vector2[] resultsArray = results.ToArray();
            if (resultsArray.Length != 0)
            {
                Vector3[] dots = new Vector3[resultsArray.Length];
                for (int i = 0; i < dots.Length; i++)
                {
                    dots[i] = new Vector3(resultsArray[i].x, resultsArray[i].y, 0);
                }
                _pathDrawer.positionCount = dots.Length;
                _pathDrawer.SetPositions(dots);
            }
            else
            {
                Debug.Log("Couldn't find a path!");
            }
        }

        private void ClearPath()
        {
            _pathDrawer.positionCount = 0;
        }
        
    }
}
