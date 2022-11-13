using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _dotName;

    public void SetDotName(string newName)
    {
        _dotName.text = newName;
    }
}
