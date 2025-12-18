using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGameManager : MonoBehaviour
{
    [Header("Game Elements")]
    [Range(2, 6)]
    [SerializeField] private int difficulty = 4;
    [SerializeField] private Transform gameHolder;
    [SerializeField] private Transform piecePrefab;

    [Header("UI Elements")]
    [SerializeField] private List<Texture2D> imageTextures;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private Image levelSelectPrefab;
    [SerializeField] private GameObject playAgainButton;
    [SerializeField] private GameObject mainMenuButton;


    [Header("Game State Managers")]
    [SerializeField] private TimerManager timerManager;
    [SerializeField] private StarRatingManager starRatingManager;
    [SerializeField] private HintManager hintManager;
    [SerializeField] private SoundManager soundManager;


    private List<Transform> pieces;
    private Vector2Int dimensions;
    private float width;
    private float height;

    private Transform draggingPiece = null;
    private Vector3 offset;

    private int piecesCorrect;

    void Start()
    {

        foreach (Texture2D texture in imageTextures)
        {
            Image image = Instantiate(levelSelectPrefab, levelSelectPanel);
            image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

            image.GetComponent<Button>().onClick.AddListener(delegate { StartGame(texture); });
        }
    }

    public void StartGame(Texture2D jigsawTexture)
    {

        levelSelectPanel.gameObject.SetActive(false);

        if (timerManager != null) timerManager.StartTimer();


        pieces = new List<Transform>();


        dimensions = GetDimensions(jigsawTexture, difficulty);


        height = 1f / dimensions.y;
        float aspect = (float)jigsawTexture.width / jigsawTexture.height;
        width = aspect / dimensions.x;


        CreateJigsawPieces(jigsawTexture);


        Scatter();


        UpdateBorder();


        if (hintManager != null)
        {
            hintManager.SetupHint(jigsawTexture);
        }


        piecesCorrect = 0;
    }
    Vector2Int GetDimensions(Texture2D jigsawTexture, int difficulty)
    {
        Vector2Int dimensions = Vector2Int.zero;

        if (jigsawTexture.width < jigsawTexture.height)
        {
            dimensions.x = difficulty;
            dimensions.y = (difficulty * jigsawTexture.height) / jigsawTexture.width;
        }
        else
        {
            dimensions.x = (difficulty * jigsawTexture.width) / jigsawTexture.height;
            dimensions.y = difficulty;
        }
        return dimensions;
    }


    void CreateJigsawPieces(Texture2D jigsawTexture)
    {


        for (int row = 0; row < dimensions.y; row++)
        {
            for (int col = 0; col < dimensions.x; col++)
            {

                Transform piece = Instantiate(piecePrefab, gameHolder);
                piece.localPosition = new Vector3(
                    (-width * dimensions.x / 2) + (width * col) + (width / 2),
                    (-height * dimensions.y / 2) + (height * row) + (height / 2),
                    -1);
                piece.localScale = new Vector3(width, height, 1f);


                piece.name = $"Piece {(row * dimensions.x) + col}";
                pieces.Add(piece);

                float width1 = 1f / dimensions.x;
                float height1 = 1f / dimensions.y;

                Vector2[] uv = new Vector2[4];
                uv[0] = new Vector2(width1 * col, height1 * row);
                uv[1] = new Vector2(width1 * (col + 1), height1 * row);
                uv[2] = new Vector2(width1 * col, height1 * (row + 1));
                uv[3] = new Vector2(width1 * (col + 1), height1 * (row + 1));

                Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                mesh.uv = uv;

                piece.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", jigsawTexture);
            }
        }
    }


    private void Scatter()
    {

        float orthoHeight = Camera.main.orthographicSize;
        float screenAspect = (float)Screen.width / Screen.height;
        float orthoWidth = (screenAspect * orthoHeight);


        float pieceWidth = width * gameHolder.localScale.x;
        float pieceHeight = height * gameHolder.localScale.y;

        orthoHeight -= pieceHeight;
        orthoWidth -= pieceWidth;


        foreach (Transform piece in pieces)
        {
            float x = Random.Range(-orthoWidth, orthoWidth);
            float y = Random.Range(-orthoHeight, orthoHeight);
            piece.position = new Vector3(x, y, -1);
        }
    }

    private void UpdateBorder()
    {
        LineRenderer lineRenderer = gameHolder.GetComponent<LineRenderer>();

        float halfWidth = (width * dimensions.x) / 2f;
        float halfHeight = (height * dimensions.y) / 2f;


        float borderZ = 0f;


        lineRenderer.SetPosition(0, new Vector3(-halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, halfHeight, borderZ));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, -halfHeight, borderZ));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, -halfHeight, borderZ));


        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;


        lineRenderer.enabled = true;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {

                draggingPiece = hit.transform;
                offset = draggingPiece.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                offset += Vector3.back;
            }
        }


        if (draggingPiece && Input.GetMouseButtonUp(0))
        {
            SnapAndDisableIfCorrect();
            draggingPiece.position += Vector3.forward;
            draggingPiece = null;
        }


        if (draggingPiece)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            newPosition += offset;
            draggingPiece.position = newPosition;
        }
    }

    private void SnapAndDisableIfCorrect()
    {

        int pieceIndex = pieces.IndexOf(draggingPiece);


        int col = pieceIndex % dimensions.x;
        int row = pieceIndex / dimensions.x;


        Vector2 targetPosition = new((-width * dimensions.x / 2) + (width * col) + (width / 2),
                                            (-height * dimensions.y / 2) + (height * row) + (height / 2));


        if (Vector2.Distance(draggingPiece.localPosition, targetPosition) < (width / 2))
        {

            draggingPiece.localPosition = targetPosition;

            draggingPiece.GetComponent<BoxCollider2D>().enabled = false;
            soundManager.PlaySnapSound();

            piecesCorrect++;
            if (piecesCorrect == pieces.Count)
            {
                float finalTime = 0f;
                if (timerManager != null)
                {
                    finalTime = timerManager.StopTimer();


                }
                if (starRatingManager != null)
                {
                    starRatingManager.CalculateAndDisplayStars(finalTime);
                }
                if (soundManager != null)
                {
                    soundManager.PlayWinSound();
                }
                playAgainButton.SetActive(true);
                mainMenuButton.SetActive(true);
            }
        }
    }

    public void RestartGame()
    {

        foreach (Transform piece in pieces)
        {
            Destroy(piece.gameObject);
        }
        pieces.Clear();
        if (timerManager != null) timerManager.ResetTimer();
        if (starRatingManager != null) starRatingManager.HideStars();
        hintManager.HideHint();

        gameHolder.GetComponent<LineRenderer>().enabled = false;

        playAgainButton.SetActive(false);
        mainMenuButton.SetActive(false);
        levelSelectPanel.gameObject.SetActive(true);
    }
    public void GoToMainMenu()
    {

        SceneManager.LoadScene("MainMenu");
    }
}