using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAlert : MonoBehaviour
{

    public GameObject alert30SecObj;  // 60초 알림용 오브젝트
    public GameObject alertBossObj;   // 30초 보스 출현 알림 오브젝트
    private bool alerted30SecBefore = false;
    private bool alertedAtBoss = false;

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

        if (GameManager.instance.bossDefeated)
            return;

        if (!alerted30SecBefore && remainTime <= 90f)
        {
            alerted30SecBefore = true;
            StartCoroutine(ShowAlert(alert30SecObj));
        }

        if (!alertedAtBoss && remainTime <= 60f)
        {
            alertedAtBoss = true;
            StartCoroutine(ShowAlert(alertBossObj));
        }
    }

    IEnumerator ShowAlert(GameObject target)
    {
        target.SetActive(true);
        yield return new WaitForSeconds(3f);
        target.SetActive(false);
    }

}
