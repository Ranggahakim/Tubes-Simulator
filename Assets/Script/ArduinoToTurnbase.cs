using UnityEngine;
using System;

public class ArduinoToTurnbase : MonoBehaviour
{
    public TurnbaseSystem tbs;
    // ID tombol fisik ini (1, 2, atau 3)
    public int buttonID;

    // Referensi ke GameLogicManager untuk mendapatkan karakter yang sedang aktif
    // public GameLogicManager gameLogicManager;

    // Event yang akan dipicu ketika tombol ini ditekan
    // public static event Action<CharacterScriptable, int> OnCharacterAction; // Kini di sini!

    // Variabel untuk melacak status tombol sebelumnya agar hanya memicu saat ditekan (edge detection)
    private bool lastButtonState = false; // true = pressed, false = released

    // Di dalam CharacterActionButton.cs
    void OnEnable()
    {
        ArduinoSerialHandler.OnButtonStateChanged += HandleButtonStateChanged; // Baris ini yang mendaftarkan metode                                                                               // ...
    }

    void OnDisable()
    {
        // if (arduinoHandler != null)
        // {
        //     arduinoHandler.OnButtonStateChanged -= HandleButtonStateChanged;
        //     Debug.Log($"CharacterActionButton (ID: {buttonID}): Berhenti berlangganan event tombol.");
        // }
    }

    void HandleButtonStateChanged(string buttonStates)
    {
        // Pastikan buttonStates memiliki panjang yang cukup
        if (buttonStates.Length < buttonID)
        {
            Debug.LogWarning($"CharacterActionButton (ID: {buttonID}): String status tombol tidak valid: {buttonStates}");
            return;
        }

        // Dapatkan status tombol ini dari string (misal: '1' untuk ditekan, '0' untuk dilepas)
        bool currentButtonState = buttonStates[buttonID - 1] == '0';

        // Hanya jika status tombol berubah dari TIDAK DITEKAN menjadi DITEKAN (Press down event)
        if (currentButtonState && !lastButtonState)
        {
            tbs.PencetTombol(buttonID);
            // CharacterScriptable activeCharacter = null;
            // if (gameLogicManager != null)
            // {
            //     activeCharacter = gameLogicManager.GetCurrentlySelectedCharacter();
            // }

            // if (activeCharacter != null)
            // {
            //     Debug.Log($"[CharacterActionButton ID:{buttonID}] Tombol fisik ditekan oleh {activeCharacter.characterName}!");
            //     OnCharacterAction?.Invoke(activeCharacter, buttonID); // Picu event aksi karakter
            //     // Opsional: berikan feedback visual/audio via Arduino
            //     // arduinoHandler.SetBuzzer(true);
            //     // Invoke("TurnOffBuzzer", 0.1f);
            // }
            // else
            // {
            //     Debug.LogWarning($"[CharacterActionButton ID:{buttonID}] Tombol ditekan, tetapi tidak ada karakter yang teridentifikasi.");
            // if (arduinoHandler != null)
            // {
            //     arduinoHandler.SetBuzzer(true);
            //     Invoke("TurnOffBuzzer", 0.1f); // Buzzer cepat untuk error
            // }
            // }
        }
        lastButtonState = currentButtonState;
        // Update status tombol terakhir
    }

    // Metode bantuan untuk mematikan buzzer jika digunakan
    // void TurnOffBuzzer()
    // {
    //     if (arduinoHandler != null)
    //     {
    //         arduinoHandler.SetBuzzer(false);
    //     }
    // }
}