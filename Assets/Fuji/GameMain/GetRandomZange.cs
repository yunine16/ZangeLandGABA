using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using TMPro;

public class GetRandomZange : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText = null;
    //zangesåüçıÇÃÇΩÇﬂÇ…ÅANCMBQueryçÏê¨
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("Zange");
    bool initialized = false;
    private string resultString = "init";

    private void Start()
    {
        zangesQuery.FindAsync((List<NCMBObject> allZanges, NCMBException e) =>
        {
            if (e != null)
            {
                resultString = "error";
            }
            else
            {
                int zangeObjectNumber = Random.Range(0, allZanges.Count);
                resultString = allZanges[zangeObjectNumber]["zangeText"].ToString();
            }
        });
    }
    public string GetZange()
    {
        zangesQuery.FindAsync((List<NCMBObject> allZanges, NCMBException e) =>
        {
            if (e != null)
            {
                resultString = "error";
            }
            else
            {
                int zangeObjectNumber = Random.Range(0, allZanges.Count);
                resultString = allZanges[zangeObjectNumber]["zangeText"].ToString();
            }
        });
        return resultString;
    }
}
