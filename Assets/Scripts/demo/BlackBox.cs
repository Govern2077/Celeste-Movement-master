using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        // ��ȡ����� BoxCollider2D ���
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �����ײ�������Ƿ��� "Black" ��ǩ
        if (collision.gameObject.CompareTag("Black"))
        {
            // �ر��Լ��� BoxCollider2D
            boxCollider.enabled = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // ����Ƿ����� "Black" ��ǩ����������Ӵ�
        if (collision.gameObject.CompareTag("Black"))
        {
            // �������� BoxCollider2D
            boxCollider.enabled = true;
        }
    }
}
