using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [Header("VFX의 지속시간(이 시간이 지나면 disable)")]
    [SerializeField] private float mLifeTime;

    private Coroutine? mCoDisable; // 비활성화 코루틴

    public void Play(Transform? targetTransform = null)
    {
        if (targetTransform is not null)
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }

        if (mCoDisable is not null)
            StopCoroutine(mCoDisable);

        mCoDisable = StartCoroutine(CoDisable());
    }

    private IEnumerator CoDisable()
    {
        yield return new WaitForSeconds(mLifeTime);
        gameObject.SetActive(false);
    }
}