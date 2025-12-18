using System.Collections;
using UnityEngine;

public class WinStarsUI : MonoBehaviour
{
    public GameObject[] stars; // 3 نجوم

    // NEW: الدالة تقبل متغيراً لتحديد عدد النجوم المراد عرضها
    public void ShowStars(int numberOfStars)
    {
        // 1. تفعيل اللوحة الأب (WinPanel) وجعل النجوم غير نشطة في البداية
        gameObject.SetActive(true);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(false);
        }

        // تمرير عدد النجوم إلى دالة الرسوم المتحركة
        StartCoroutine(AnimateStars(numberOfStars));
    }

    // NEW: الدالة الآن تقبل عدد النجوم (starsCount)
    IEnumerator AnimateStars(int starsCount)
    {
        // **NEW:** تكرار الأنميشن لعدد النجوم المحدد فقط
        for (int i = 0; i < starsCount; i++)
        {
            // الكود الأصلي لتشغيل الأنميشن
            stars[i].SetActive(true);
            stars[i].transform.localScale = Vector3.zero;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * 4f;
                stars[i].transform.localScale =
                    Vector3.Lerp(Vector3.zero, Vector3.one, t);
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
        }

        // الانتظار والإخفاء بعد اكتمال الرسوم المتحركة
        
        gameObject.SetActive(false);
    }
}