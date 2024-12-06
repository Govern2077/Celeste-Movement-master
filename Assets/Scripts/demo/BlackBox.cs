using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBox : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        // 获取物体的 BoxCollider2D 组件
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查碰撞的物体是否有 "Black" 标签
        if (collision.gameObject.CompareTag("Black"))
        {
            // 关闭自己的 BoxCollider2D
            boxCollider.enabled = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 检查是否是与 "Black" 标签的物体脱离接触
        if (collision.gameObject.CompareTag("Black"))
        {
            // 重新启用 BoxCollider2D
            boxCollider.enabled = true;
        }
    }
}
