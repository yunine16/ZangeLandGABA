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

    //zanges�����̂��߂ɁANCMBQuery�쐬
    NCMBQuery<NCMBObject> zangesQuery = new NCMBQuery<NCMBObject>("Zange");

    //���͗����X�V����邽�ю��s�ANull�A���͖����A�X�y�[�X�݂̂̂����ꂩ�̂Ƃ����M�{�^���������Ȃ��B
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
    //NCMB�ɃU���Q��o�^
    public void sendToNCMB()
    {
        int typeSENumber = Random.Range(0, 5);
        SEManager.Instance.PlaySE("Zange" + typeSENumber);
        //���������ݒ�Bzange�̒l�����͗��ł������
        zangesQuery.WhereEqualTo("zangeText", zangeInputField.text);
        //��v����NCMB�I�u�W�F�N�g�����X�g��
        zangesQuery.FindAsync((List<NCMBObject> objList, NCMBException findAsyncError) =>
        {
            //�G���[�Ȃ�
            if (findAsyncError != null)
            {
                infoText.text = "�G���[���N���܂����B";
                //�G���[�Ȃ畜��OK�Ƃ���
                gameUIscript.Revenge();
            }
            else
            {
                //�����̌��ʁA���X�g�̒�����0�Ȃ�܂��Ȃ��U���Q�Ȃ̂œo�^
                if (objList.Count == 0)
                {
                    //���̃^�C�~���O��NCMB�I�u�W�F�N�g�������邱�ƂŁAID�X�V���ł���B
                    NCMBObject zanges = new NCMBObject("Zange");
                    //zanges��zange�t�B�[���h�ɓ��͗��̓��e��ǉ�����B
                    zanges.Add("zangeText", zangeInputField.text);
                    //�ۑ��B�G���[�������Ă��Ȃ��Ă�����
                    zanges.SaveAsync();
                    gameUIscript.Revenge();
                    //����\�����̂��߂Ƀe�L�X�g�����Z�b�g
                    infoText.text = "��������͂��A" + "\n" + "���x���W���܂���?";
                }
                //���o�̜����Ȃ�o�^���Ȃ�
                else
                {
                    infoText.text = "���̜����͊��o�ł��B";
                }
            }
        });
    }
}
