using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    [Header("Hint UI")]
    [SerializeField] private Image hintImage;

    [Header("Hint Settings")]
    [Range(0.1f, 1f)]
    [SerializeField] private float hintOpacity = 1f;

    [Range(0.1f, 0.5f)]
    [SerializeField] private float screenWidthRatio = 0.25f;

    public void SetupHint(Texture2D jigsawTexture)
    {
        if (hintImage == null || jigsawTexture == null)
        {
            return;
        }

        Sprite hintSprite = Sprite.Create(
            jigsawTexture,
            new Rect(0, 0, jigsawTexture.width, jigsawTexture.height),
            new Vector2(0.5f, 0.5f)
        );

        hintImage.sprite = hintSprite;
        hintImage.preserveAspect = true;

        Color c = hintImage.color;
        hintImage.color = new Color(c.r, c.g, c.b, hintOpacity);

        ResizeHint();

        hintImage.gameObject.SetActive(true);
    }

    private void ResizeHint()
    {
        RectTransform rect = hintImage.rectTransform;

        float targetWidth = Screen.width * screenWidthRatio;
        float aspectRatio = hintImage.sprite.rect.height / hintImage.sprite.rect.width;

        rect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            targetWidth
        );

        rect.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical,
            targetWidth * aspectRatio
        );
    }

    public void HideHint()
    {
        if (hintImage != null)
            hintImage.gameObject.SetActive(false);
    }
}