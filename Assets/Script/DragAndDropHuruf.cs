using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Pengaturan Target")]
    public RectTransform targetPosition; // Pasang UI Target (Bayangan Huruf) ke sini
    public float snapRadius = 150f;      // Diubah jadi 150f agar lebih gampang nempel

    [Header("Pengaturan Suara (Opsional)")]
    public AudioSource audioSource;
    public AudioClip suaraHuruf;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startPosition;
    private bool isLocked = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // Tambahkan CanvasGroup jika belum ada
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        // Simpan posisi awal di UI Canvas
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Dipanggil saat mouse pertama kali menekan huruf
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        CekJarakKeTarget();
    }

    void CekJarakKeTarget()
    {
        if (targetPosition == null) return;

        // Hitung jarak posisi UI dalam satuan Pixel Canvas
        float distance = Vector2.Distance(rectTransform.anchoredPosition, targetPosition.anchoredPosition);

        if (distance <= snapRadius)
        {
            // 1. Kunci posisi tepat di target
            rectTransform.anchoredPosition = targetPosition.anchoredPosition;
            isLocked = true;

            // 2. Putar suara jika ada
            if (audioSource != null && suaraHuruf != null)
            {
                audioSource.PlayOneShot(suaraHuruf);
            }

            // --- 3. LAPOR KE GAMEMANAGER (POSISI YANG BENAR DI DALAM IF) ---
            if (GameManager.Instance != null)
            {
                GameManager.Instance.TambahHurufSelesai();
            }
        }
        else
        {
            // Jika dilepas jauh, kembalikan ke posisi semula
            rectTransform.anchoredPosition = startPosition;
        }
    }
}