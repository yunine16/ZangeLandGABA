using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugOnlyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;
    GetRandomZange getRandomZange = null;

    [SerializeField] private Transform getComponentTargetTransform = null;
    [SerializeField] private GameObject getComponentTargetGameObject = null;

    private void Awake()
    {
        getRandomZange = getComponentTargetTransform.GetComponent<GetRandomZange>();
    }
    public void TestButton()
    {
        if (infoText != null)
        {
            //infoText.text = getRandomZange.GetZange();
        }
    }
}
