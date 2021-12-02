using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//nineさんにNCMB側でのネーミングの確認をとってからコメントアウトを外すのだ
//using NCMB;

public class ZangeInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText = null;
    [SerializeField] private TMP_InputField zangeInputField = null;
    [SerializeField] private Button sendButton = null;

    //NCMBにZangesFromPlayersという名でNCMB Objectを作成する。
    //スクリプトではzangesと呼ぶ。
    NCMBObject zanges = new NCMBObject("ZangesFromPlayers");

    //zanges検索のために、NCMBQuery作成
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("QueryForZanges");

    //入力欄が更新されるたび実行、Null、入力無し、スペースのみのいずれかのとき送信ボタンが押せない。
    public void SetSendButtonState()
    {
        sendButton.interactable = !(string.IsNullOrWhiteSpace(zangeInputField.text)
                                 || string.IsNullOrEmpty(zangeInputField.text));
    }

    public void sendToNCMB()
    {
        //検索条件設定。zangeの値が入力欄なもの
        zangesQuery.WhereEqualTo("zange", zangeInputField.text);
        //一致するNCMBオブジェクトをリスト化
        zangesQuery.FindAsync((List<NCMBObject> objList, NCMBException fae) =>
        {
            //エラーなら
            if (fae != null)
            {
                infoText.text = "sorry, error occured :( thanks for playing!";
            }
            else
            {
                //リストの長さが0ならまだない懺悔なので登録
                if (objList.Count == 0)
                {
                    //zangesのzangeフィールドに入力欄の内容を追加する。
                    zanges.Add("zange", zangeInputField.text);
                    zanges.SaveAsync((NCMBException sae) =>
                    {
                        if (sae != null)
                        {
                            infoText.text = "sorry, error occured :( thanks for playing!";
                        }
                        else
                        {
                            infoText.text = "successfully sent :) thanks for playing!";
                        }
                    });
                }
                //既出の懺悔なら登録しない
                else
                {
                    infoText.text = "already exsists :o thanks for playing!";
                }
            }
        });
    }
}
