using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// GameManager untuk game drag-and-drop mencocokkan huruf (A, B, C, D).
/// Menangani skor dan menampilkan PanelSelamat (panel kemenangan) di akhir permainan.
///
/// CARA PASANG DI UNITY:
/// 1. Attach script ini ke GameObject "GameManager" di Hierarchy.
/// 2. Di Inspector, isi field:
///    - Panel Selamat  -> drag GameObject "PanelSelamat"
///    - Text Judul     -> drag child "textJudul" (TextMeshProUGUI)
///    - Text Skor Akhir-> drag child "textSkorAkhir" (TextMeshProUGUI)
///    - Total Item     -> isi 4 (karena ada A, B, C, D)
/// 3. Pastikan GameObject "PanelSelamat" di-nonaktifkan (uncheck) di awal scene,
///    supaya baru muncul saat game selesai.
/// 4. Script DragAndDrop kamu sudah otomatis memanggil:
///       GameManager.Instance.TambahHurufSelesai();
///    setiap kali sebuah huruf berhasil nempel ke targetnya. Tidak perlu diubah lagi.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Panel Kemenangan")]
    [Tooltip("GameObject panel yang muncul saat semua item selesai dicocokkan")]
    public GameObject panelSelamat;

    [Tooltip("Text judul di dalam panel, contoh: 'Selamat!'")]
    public TextMeshProUGUI textJudul;

    [Tooltip("Text skor akhir di dalam panel, contoh: 'Skor: 3/4'")]
    public TextMeshProUGUI textSkorAkhir;

    [Header("Pengaturan Skor")]
    [Tooltip("Jumlah total item yang harus dicocokkan (A, B, C, D = 4)")]
    public int totalItem = 4;

    [Tooltip("Judul yang ditampilkan jika skor sempurna")]
    public string judulSempurna = "Sempurna!";

    [Tooltip("Judul yang ditampilkan jika permainan selesai tapi tidak sempurna")]
    public string judulSelesai = "Selamat, Selesai!";

    [Header("Navigasi Menu")]
    [Tooltip("Nama scene menu utama. Pastikan scene ini sudah ditambahkan di File > Build Settings > Scenes In Build")]
    public string namaSceneMenu = "Menu";

    [Tooltip("Tombol 'Kembali' di pojok kiri atas yang mau disembunyikan saat panel kemenangan muncul")]
    public GameObject tombolKembali;

    private int itemSelesai = 0;

    private void Awake()
    {
        // Singleton sederhana supaya bisa dipanggil dari script lain lewat GameManager.Instance
        Instance = this;

        if (panelSelamat != null)
            panelSelamat.SetActive(false);
    }

    /// <summary>
    /// Dipanggil oleh DragAndDrop.cs setiap kali satu huruf berhasil nempel ke targetnya.
    /// Karena tiap huruf sudah dipasangkan ke targetnya masing-masing di Inspector,
    /// setiap panggilan method ini otomatis dihitung sebagai jawaban benar.
    /// </summary>
    public void TambahHurufSelesai()
    {
        itemSelesai++;

        Debug.Log($"[GameManager] Huruf selesai: {itemSelesai}/{totalItem}");

        if (itemSelesai >= totalItem)
        {
            TampilkanPanelKemenangan();
        }
    }

    /// <summary>
    /// Menampilkan panel kemenangan beserta skor akhir.
    /// </summary>
    private void TampilkanPanelKemenangan()
    {
        if (panelSelamat != null)
            panelSelamat.SetActive(true);

        if (tombolKembali != null)
            tombolKembali.SetActive(false);

        if (textJudul != null)
            textJudul.text = judulSempurna;

        if (textSkorAkhir != null)
            textSkorAkhir.text = $"Skor: {itemSelesai}/{totalItem}";
    }

    /// <summary>
    /// Opsional: panggil ini kalau mau tombol "Main Lagi" mereset permainan.
    /// </summary>
    public void ResetGame()
    {
        itemSelesai = 0;

        if (panelSelamat != null)
            panelSelamat.SetActive(false);

        if (tombolKembali != null)
            tombolKembali.SetActive(true);
    }

    /// <summary>
    /// Dipanggil dari tombol "Ke Menu" di PanelSelamat (lewat OnClick di Inspector).
    /// Pindah ke scene menu utama, sesuai nama di field "Nama Scene Menu".
    /// </summary>
    public void KeMenu()
    {
        SceneManager.LoadScene(namaSceneMenu);
    }
}