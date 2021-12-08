using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;
using TMPro;
using System.Linq;

public class GetRandomZange : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText = null;
    //zangesåüçıÇÃÇΩÇﬂÇ…ÅANCMBQueryçÏê¨
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("Zange");

    public void GetZange()
    {
        zangesQuery.FindAsync((List<NCMBObject> allZanges, NCMBException e) =>
        {
            if (e != null)
            {
                infoText.text = "error occured";
            }
            else
            {
                int zangeObjectNumber = Random.Range(0, allZanges.Count);
                infoText.text = allZanges[zangeObjectNumber]["zangeText"].ToString();
            }
        });
    }
}
