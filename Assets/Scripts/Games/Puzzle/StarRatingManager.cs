using UnityEngine;
using UnityEngine.UI;
public class StarRatingManager : MonoBehaviour
{
    private const float ThreeStarsTime = 120f;
    private const float TwoStarsTime = 180f;

    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite starOnSprite;
    [SerializeField] private Sprite starOffSprite;
    public int CalculateAndDisplayStars(float timeInSeconds)
    {
        int stars = 0;
        if (timeInSeconds <= ThreeStarsTime)
        {
            stars = 3;
        }
        else if (timeInSeconds <= TwoStarsTime)
        {
            stars = 2;
        }
        else
        {
            stars = 1;
        }

        UpdateStarUI(stars);

        return stars;
    }
    private void UpdateStarUI(int starsCount)
    {
        if (starImages == null || starImages.Length != 3)
        {
            return;
        }

        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < starsCount)
            {
                starImages[i].sprite = starOnSprite;
            }
            else
            {
                starImages[i].sprite = starOffSprite;
            }
            starImages[i].gameObject.SetActive(true);
        }
    }
    public void HideStars()
    {
        if (starImages != null)
        {
            foreach (Image star in starImages)
            {
                star.gameObject.SetActive(false);
            }
        }
    }
}