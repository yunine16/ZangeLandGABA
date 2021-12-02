using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//nine�����NCMB���ł̃l�[�~���O�̊m�F���Ƃ��Ă���R�����g�A�E�g���O���̂�
//using NCMB;

public class ZangeInput : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText = null;
    [SerializeField] private TMP_InputField zangeInputField = null;
    [SerializeField] private Button sendButton = null;

    //NCMB��ZangesFromPlayers�Ƃ�������NCMB Object���쐬����B
    //�X�N���v�g�ł�zanges�ƌĂԁB
    NCMBObject zanges = new NCMBObject("ZangesFromPlayers");

    //zanges�����̂��߂ɁANCMBQuery�쐬
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("QueryForZanges");

    //���͗����X�V����邽�ю��s�ANull�A���͖����A�X�y�[�X�݂̂̂����ꂩ�̂Ƃ����M�{�^���������Ȃ��B
    public void SetSendButtonState()
    {
        sendButton.interactable = !(string.IsNullOrWhiteSpace(zangeInputField.text)
                                 || string.IsNullOrEmpty(zangeInputField.text));
    }

    public void sendToNCMB()
    {
        //���������ݒ�Bzange�̒l�����͗��Ȃ���
        zangesQuery.WhereEqualTo("zange", zangeInputField.text);
        //��v����NCMB�I�u�W�F�N�g�����X�g��
        zangesQuery.FindAsync((List<NCMBObject> objList, NCMBException fae) =>
        {
            //�G���[�Ȃ�
            if (fae != null)
            {
                infoText.text = "sorry, error occured :( thanks for playing!";
            }
            else
            {
                //���X�g�̒�����0�Ȃ�܂��Ȃ������Ȃ̂œo�^
                if (objList.Count == 0)
                {
                    //zanges��zange�t�B�[���h�ɓ��͗��̓��e��ǉ�����B
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
                //���o�̜����Ȃ�o�^���Ȃ�
                else
                {
                    infoText.text = "already exsists :o thanks for playing!";
                }
            }
        });
    }
}
