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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
                Debug.Log(allZanges[zangeObjectNumber]["zangeText"]);
                //ArrayListå^?Å@NCMB Objectå^
                infoText.text = allZanges[zangeObjectNumber].ObjectId;
            }
        });
    }
}
