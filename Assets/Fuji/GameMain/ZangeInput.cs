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

    //zanges�����̂��߂ɁANCMBQuery�쐬
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("Zange");

    private void Start()
    {
        //NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;
    }

    //���͗����X�V����邽�ю��s�ANull�A���͖����A�X�y�[�X�݂̂̂����ꂩ�̂Ƃ����M�{�^���������Ȃ��B
    public void OnInputChange()
    {
        sendButton.interactable = !(string.IsNullOrWhiteSpace(zangeInputField.text)
                                 || string.IsNullOrEmpty(zangeInputField.text));
        string typeSENumber = Random.Range(0, 4).ToString();
        SEManager.Instance.PlaySE("Typing" + typeSENumber);
    }

    public void sendToNCMB()
    {
        //���������ݒ�Bzange�̒l�����͗��Ȃ���
        zangesQuery.WhereEqualTo("zangeText", zangeInputField.text);
        //��v����NCMB�I�u�W�F�N�g�����X�g��
        zangesQuery.FindAsync((List<NCMBObject> objList, NCMBException fae) =>
        {
            //�G���[�Ȃ�
            if (fae != null)
            {
                infoText.text = "sorry, error occured";
                gameUIscript.Revenge();
            }
            else
            {
                //���X�g�̒�����0�Ȃ�܂��Ȃ������Ȃ̂œo�^
                if (objList.Count == 0)
                {
                    //���̃^�C�~���O��NCMB�I�u�W�F�N�g�������邱�ƂŁAID�X�V���ł���B
                    NCMBObject zanges = new NCMBObject("Zange");
                    //zanges��zange�t�B�[���h�ɓ��͗��̓��e��ǉ�����B
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
                //���o�̜����Ȃ�o�^���Ȃ�
                else
                {
                    infoText.text = "already exsists";
                }
            }
        });
    }
}
