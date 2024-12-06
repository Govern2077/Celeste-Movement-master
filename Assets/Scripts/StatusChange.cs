using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusChange : MonoBehaviour
{
    [Header("�Ƿ�Ϊ��ɫ")]
    public bool isBlack;

    private void Awake()
    {
        EventCenter.Instance.Subscribe("ChangeStatus", ChangeStatus);
    }

    void ChangeStatus()
    {
        this.GetComponent<BoxCollider2D>().enabled=!this.GetComponent<BoxCollider2D>().enabled;
    }
}
