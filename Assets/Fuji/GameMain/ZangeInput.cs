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
    [SerializeField] private Image failureScreen;

    //zanges検索のために、NCMBQuery作成
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("Zange");

    //入力欄が更新されるたび実行、Null、入力無し、スペースのみのいずれかのとき送信ボタンが押せない。
    public void OnInputChange()
    {
        if (failureScreen.rectTransform.localScale.y != 0)
        {
            sendButton.interactable = !(string.IsNullOrWhiteSpace(zangeInputField.text)
                                     || string.IsNullOrEmpty(zangeInputField.text));
            int typeSENumber = Random.Range(0, 5);
            SEManager.Instance.PlaySE("Zange" + typeSENumber);
        }
    }
    //NCMBにザンゲを登録
    public void sendToNCMB()
    {
        int typeSENumber = Random.Range(0, 5);
        SEManager.Instance.PlaySE("Zange" + typeSENumber);
        //検索条件設定。zangeの値が入力欄であるもの
        zangesQuery.WhereEqualTo("zangeText", zangeInputField.text);
        //一致するNCMBオブジェクトをリスト化
        zangesQuery.FindAsync((List<NCMBObject> objList, NCMBException findAsyncError) =>
        {
            //エラーなら
            if (findAsyncError != null)
            {
                infoText.text = "エラーが起きました。";
                //エラーなら復活OKとする
                gameUIscript.Revenge();
            }
            else
            {
                //検索の結果、リストの長さが0ならまだないザンゲなので登録
                if (objList.Count == 0)
                {
                    //このタイミングでNCMBオブジェクト生成することで、ID更新ができる。
                    NCMBObject zanges = new NCMBObject("Zange");
                    //zangesのzangeフィールドに入力欄の内容を追加する。
                    zanges.Add("zangeText", zangeInputField.text);
                    //保存。エラーがあってもなくても復活
                    zanges.SaveAsync();
                    gameUIscript.Revenge();
                    //次回表示時のためにテキストをリセット
                    infoText.text = "懺悔を入力し、" + "\n" + "リベンジしますか?";
                }
                //既出の懺悔なら登録しない
                else
                {
                    infoText.text = "その懺悔は既出です。";
                }
            }
        });
    }
}
