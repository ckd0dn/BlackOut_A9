using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealPuzzle : MonoBehaviour
{
    public Light spotLight;
    public GameObject revealObj;
    private Coroutine coroutine;

    public IEnumerator CheckSpotLightColor()
    {
        while (true)
        {
            // �������� Ȱ��ȭ�Ǿ��ְ�, ������ �Ķ����ΰ�� revealObj Ȱ��ȭ
            if (spotLight.color == Color.blue && spotLight.isActiveAndEnabled)
            {
                revealObj.SetActive(true);
            }
            else
            {
                revealObj.SetActive(false);
            }
            yield return null;     
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        // �Ĵ翡 �����ϸ� ������ ����Ȯ�� �ڷ�ƾ ���� 
        if (other.CompareTag("Player"))
        {
            coroutine = StartCoroutine(CheckSpotLightColor());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(coroutine);
        coroutine = null;
        revealObj.SetActive(false);
    }


}
