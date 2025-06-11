using UnityEngine;
using System;
using System.Linq; // Untuk menggunakan .FirstOrDefault() atau .Where()

public class GameLogicManager : MonoBehaviour
{
    // Ini diperlukan jika kamu ingin mengontrol LED/Buzzer sebagai respons
    public ArduinoSerialHandler arduinoHandler;

    // Daftar semua ScriptableObject Character yang ada di game
    // Cara terbaik adalah dengan membuat folder Resource dan memuatnya secara dinamis
    // Atau, kamu bisa menyeretnya secara manual di Inspector jika jumlahnya sedikit
    public CharacterScriptable[] allCharacters;

    // Event baru yang akan dipicu ketika karakter terdeteksi
    public static event Action<CharacterScriptable> OnCharacterDetected;

    private CharacterScriptable currentlySelectedCharacter; // Untuk menyimpan karakter yang saat ini terdeteksi

    void OnEnable()
    {
        ArduinoSerialHandler.OnRFIDDetected += HandleRFIDDetected;
        // Tambahkan langganan event untuk tombol fisik jika belum
        ArduinoSerialHandler.OnButtonStateChanged += HandleButtonStateChanged;

        Debug.Log("GameLogicManager: Berlangganan event Arduino.");
        // Untuk contoh ini, kita akan memuat semua karakter dari folder Resources
        LoadAllCharacters();
    }

    void OnDisable()
    {
        ArduinoSerialHandler.OnRFIDDetected -= HandleRFIDDetected;
        ArduinoSerialHandler.OnButtonStateChanged -= HandleButtonStateChanged;

        Debug.Log("GameLogicManager: Berhenti berlangganan event Arduino.");
    }

    void LoadAllCharacters()
    {
        // Memuat semua ScriptableObject CharacterScriptable dari folder Resources/Characters
        // Pastikan semua aset CharacterScriptable kamu berada di dalam folder Resources/Characters/
        allCharacters = Resources.LoadAll<CharacterScriptable>("Characters");
        if (allCharacters.Length == 0)
        {
            Debug.LogWarning("Tidak ada aset CharacterScriptable ditemukan di Resources/Characters/. Pastikan sudah dibuat.");
        }
        else
        {
            Debug.Log($"Ditemukan {allCharacters.Length} karakter.");
        }
    }

    void HandleRFIDDetected(string rfidUID)
    {
        Debug.Log($"[GameLogicManager] RFID Card Detected! UID: {rfidUID}");

        // Cari karakter yang cocok dengan RFID UID ini
        CharacterScriptable detectedCharacter = null;

        foreach (CharacterScriptable character in allCharacters)
        {
            // Debug.Log($"{rfidUID} && {character.rfidUID}");
            if (rfidUID == character.rfidUID)
            {
                detectedCharacter = character;
            }
        }

        if (detectedCharacter != null)
        {
            Debug.Log($"Karakter terdeteksi: {detectedCharacter.string_nama}");
            currentlySelectedCharacter = detectedCharacter;

            // Memicu event bahwa karakter telah terdeteksi
            OnCharacterDetected?.Invoke(detectedCharacter);

            // Contoh respons visual/audio dari Arduino
            if (arduinoHandler != null)
            {
                arduinoHandler.SetLedRGB(Color.blue); // LED biru untuk deteksi berhasil
                arduinoHandler.SetBuzzer(true);
                Invoke("TurnOffBuzzerAndLED", 0.5f);
            }
            // Kamu bisa menampilkan UI karakter di sini
            // misalnya, CharacterPortrait.sprite = detectedCharacter.characterPortrait;
        }
        else
        {
            Debug.LogWarning($"RFID UID {rfidUID} tidak cocok dengan karakter manapun.");
            currentlySelectedCharacter = null; // Reset karakter yang dipilih

            // Contoh respons visual/audio dari Arduino untuk RFID tidak dikenal
            if (arduinoHandler != null)
            {
                arduinoHandler.SetLedRGB(Color.red); // LED merah untuk RFID tidak dikenal
                arduinoHandler.SetBuzzer(true);
                Invoke("TurnOffBuzzerAndLED", 1.0f);
            }
        }
    }

    void TurnOffBuzzerAndLED()
    {
        if (arduinoHandler != null)
        {
            arduinoHandler.SetBuzzer(false);
            arduinoHandler.SetLedRGB(Color.black); // Matikan LED
        }
    }

    // Metode penanganan tombol fisik (tetap sama dari sebelumnya)
    void HandleButtonStateChanged(string buttonStates)
    {
        // ... (Logika penanganan tombol fisikmu, misalnya untuk memulai pertarungan)
        bool btn1Pressed = buttonStates[0] == '1';
        bool btn2Pressed = buttonStates[1] == '1';
        bool btn3Pressed = buttonStates[2] == '1';

        if (btn1Pressed)
        {
            Debug.Log("Tombol Fisik 1 Ditekan!");
            // Contoh: Mulai pertarungan jika ada karakter yang terdeteksi
            if (currentlySelectedCharacter != null)
            {
                Debug.Log($"Memulai pertarungan dengan {currentlySelectedCharacter.string_nama}!");
                // Panggil metode untuk memulai game fighting
                // Misalnya: StartFight(currentlySelectedCharacter);
            }
            else
            {
                Debug.Log("Tidak ada karakter terdeteksi untuk memulai pertarungan.");
            }
        }
        // ... logika untuk tombol 2 dan 3
    }

    // Metode publik untuk mendapatkan karakter yang saat ini dipilih
    public CharacterScriptable GetCurrentlySelectedCharacter()
    {
        return currentlySelectedCharacter;
    }
}