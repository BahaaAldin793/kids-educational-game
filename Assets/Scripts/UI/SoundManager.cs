using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Slider volumeSlider;
    [SerializeField] Image soundIconOn;
    [SerializeField] Image soundIconOff;

    private bool muted = false;
    private float previousVolume = 1f;

    void Start()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 1f);
        }

        Load();

        // التأكد من ضبط الأيقونة والصوت عند بداية اللعبة
        OnSliderChange();
    }

    public void OnButtonPress()
    {
        if (muted == false)
        {
            // تفعيل الكتم
            muted = true;
            previousVolume = volumeSlider.value;
            volumeSlider.value = 0;
        }
        else
        {
            // إلغاء الكتم
            muted = false;
            // لو القيمة السابقة كانت صفر (يعني كان مكتوم)، نرجعه للنص مثلاً أو 1
            volumeSlider.value = previousVolume > 0 ? previousVolume : 1f;
        }

        // أهم سطر: لازم ننادي الدالة دي عشان تطبق التغيير فعلياً
        OnSliderChange();
    }

    // دي الدالة اللي بتتحكم في كل حاجة (الصوت والأيقونة)
    public void OnSliderChange()
    {
        // 1. تغيير الصوت الفعلي
        AudioListener.volume = volumeSlider.value;

        // 2. التحكم في الأيقونة بناءً على مكان السلايدر
        if (volumeSlider.value <= 0.001f) // استخدام رقم صغير جدا بدل الصفر عشان الدقة
        {
            muted = true;
            soundIconOn.enabled = false;
            soundIconOff.enabled = true;
        }
        else
        {
            muted = false;
            soundIconOn.enabled = true;
            soundIconOff.enabled = false;
        }

        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 1f);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
    }
}