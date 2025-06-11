using TMPro;
using UnityEngine;
using UnityEngine.UI; // Penting untuk UI

public class CharacterSelectionUI : MonoBehaviour
{
    public Image characterPortraitImage; // Seret komponen Image dari Canvas UI di sini
    public TextMeshProUGUI characterNameText; // Seret komponen Text dari Canvas UI di sini
    public GameObject pressStartText; // Teks "Tekan Tombol untuk Mulai"

    void OnEnable()
    {
        GameLogicManager.OnCharacterDetected += UpdateCharacterUI;
        // Opsional: Langganan juga ke event tombol jika ingin mengelola UI terkait tombol
    }

    void OnDisable()
    {
        GameLogicManager.OnCharacterDetected -= UpdateCharacterUI;
    }

    void Start()
    {
        // Inisialisasi UI
        ClearCharacterUI();
    }

    void UpdateCharacterUI(CharacterScriptable character)
    {
        if (characterPortraitImage != null && character.characterPortrait != null)
        {
            characterPortraitImage.sprite = character.characterPortrait;
            characterPortraitImage.color = Color.white; // Pastikan tidak transparan
        }
        if (characterNameText != null)
        {
            characterNameText.text = character.string_nama;
        }
        if (pressStartText != null)
        {
            pressStartText.SetActive(true); // Tampilkan teks "Tekan Tombol untuk Mulai"
        }
    }

    void ClearCharacterUI()
    {
        // Kosongkan UI jika tidak ada karakter yang terdeteksi
        if (characterPortraitImage != null)
        {
            characterPortraitImage.sprite = null;
            characterPortraitImage.color = new Color(1, 1, 1, 0); // Buat transparan
        }
        if (characterNameText != null)
        {
            characterNameText.text = "Scan RFID Karakter";
        }
        if (pressStartText != null)
        {
            pressStartText.SetActive(false);
        }
    }

    // Kamu bisa menambahkan metode ini jika ingin membersihkan UI saat kartu dilepas
    // void HandleRFIDRemoved() { ClearCharacterUI(); }
}