using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;
using TMPro;
using NCMB;

public class ZangeInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText = null;
    [SerializeField] private TMP_InputField zangeInputField = null;
    [SerializeField] private Button sendButton = null;
    [SerializeField] private UIFunctionsForGame gameUIscript;

    //zanges検索のために、NCMBQuery作成
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("Zange");

    private void Start()
    {
        //NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    //入力欄が更新されるたび実行、Null、入力無し、スペースのみのいずれかのとき送信ボタンが押せない。
    public void OnInputChange()
    {
        sendButton.interactable = !(string.IsNullOrWhiteSpace(zangeInputField.text)
                                 || string.IsNullOrEmpty(zangeInputField.text));
        string typeSENumber = Random.Range(0, 4).ToString();
        SEManager.Instance.PlaySE("Typing" + typeSENumber);
    }

    public void sendToNCMB()
    {
        //検索条件設定。zangeの値が入力欄なもの
        zangesQuery.WhereEqualTo("zangeText", zangeInputField.text);
        //一致するNCMBオブジェクトをリスト化
        zangesQuery.FindAsync((List<NCMBObject> objList, NCMBException fae) =>
        {
            //エラーなら
            if (fae != null)
            {
                infoText.text = "sorry, error occured";
                gameUIscript.Revenge();
            }
            else
            {
                //リストの長さが0ならまだない懺悔なので登録
                if (objList.Count == 0)
                {
                    //このタイミングでNCMBオブジェクト生成することで、ID更新ができる。
                    NCMBObject zanges = new NCMBObject("Zange");
                    //zangesのzangeフィールドに入力欄の内容を追加する。
                    zanges.Add("zangeText", zangeInputField.text);
                    zanges.SaveAsync((NCMBException sae) =>
                    {
                        if (sae != null)
                        {
                            infoText.text = "sorry, error occured";
                        }
                        else
                        {
                            infoText.text = "successfully sent";
                        }
                    });
                    gameUIscript.Revenge();
                }
                //既出の懺悔なら登録しない
                else
                {
                    infoText.text = "already exsists";
                }
            }
        });
    }
}
