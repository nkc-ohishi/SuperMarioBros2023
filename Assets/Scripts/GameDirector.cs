//------------------------------------------------------------------------------
// �ȖځF�Q�[���v���O���~���O
// ���e�F�Q�[���V�[���Ǘ��y��UI����
// ���t�F2023.10.31 Ken.D.Ohishi
//------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    // �R���|�[�l���g�擾�p
    public Text scoreLabel;         // �X�R�A���x��
    public Text coinCountLabel;     // �R�C���������x��
    public Text timeLabel;          // �c�莞�ԃ��x��

    // �O���Q�ƕϐ�
    public static int score;        // �X�R�A
    public static int coinCount;    // �R�C���擾����
    public static int gameState;    // �Q�[���̏��
    public static int lastTime;     // �c�莞��

    // ��������
    const int MAX_TIME = 400;
    const float TIME_INTERBAL = 0.4f;
    float lastTimeInterbal;

    // �S�[�����_������̑ҋ@����
    float stateTime;

    void Start()
    {
        lastTime = MAX_TIME;    // ��������
        lastTimeInterbal = 0;   // ���ԊԊu�v�Z�p
        gameState = 1;          // �Q�[���̏��
        score     = 0;          // �X�R�A
        stateTime = 2f;         // �S�[�����_������̑ҋ@����
    }

    void Update()
    {
        // �S�[����̓��_�������I����ĂQ�b��������V�[���؂�ւ�
        if(gameState == 2)
        {
            stateTime -= Time.deltaTime;
            if(stateTime <= 0)
            {
                SceneManager.LoadScene("ClearScene");
            }
            return;
        }

        // �S�[�������瓾�_����
        if (PlayerController.isPaul)
        {
            if (lastTime > 0)
            {
                lastTime--;     // �c�莞�Ԃ𓾓_�ɉ��Z
                score += 100;   // �c�莞�ԂP��100�|�C���g���Z
            }
            else
            {
                // �S�[����̓��_�����I��������Q�[���̏�Ԃ��Q�ɂ���
                gameState = 2;
            }

            // �c�莞�ԕ\��
            timeLabel.text = "TIME\n " + lastTime.ToString("D3");

            // �X�R�A�\��
            scoreLabel.text = "MARIO\n" + score.ToString("D6");

            return;
        }






        // �������ԏ��� ��0.4�b���Ɏ��Ԃ��P���炷
        lastTimeInterbal += Time.deltaTime;
        if(lastTimeInterbal > TIME_INTERBAL)
        {
            lastTime--;                         // �c�莞�Ԃ����炷
            lastTime = Mathf.Max(lastTime, 0);  // �O�����ɂȂ疳���悤�ɐ���
            lastTimeInterbal = 0;               // ���ԊԊu�v�Z�p�ϐ��N���A
        }

        // �c�莞�Ԃ�\��
        timeLabel.text = "TIME\n " + lastTime.ToString("D3");

        // �X�R�A�\��
        scoreLabel.text = "MARIO\n" + score.ToString("D6");

        // �R�C�������\��
        coinCountLabel.text = "\n  x" + coinCount.ToString("D2");
    }
}
