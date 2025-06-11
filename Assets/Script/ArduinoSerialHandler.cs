using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System;
using System.Collections.Generic;

public class ArduinoSerialHandler : MonoBehaviour
{
    // Konfigurasi port serial
    public string portName = "COM3"; // Sesuaikan dengan port Arduino kamu (contoh: COM3 di Windows, /dev/ttyUSB0 di Linux, /dev/cu.usbmodemXXXX di macOS)
    public int baudRate = 9600;

    private SerialPort serialPort;
    private Thread readThread;
    private bool isRunning = false;

    // Event untuk mengirim data yang diterima dari Arduino
    public static event Action<string> OnRFIDDetected;
    public static event Action<string> OnButtonStateChanged;

    // Queue untuk menyimpan pesan yang diterima dari thread pembacaan
    private Queue<string> incomingMessageQueue = new Queue<string>();
    private readonly object queueLock = new object();

    void Start()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 1; // Set timeout for reading
            serialPort.Open();
            Debug.Log($"Serial Port {portName} opened successfully.");

            isRunning = true;
            readThread = new Thread(ReadSerialPort);
            readThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open serial port {portName}: {e.Message}");
        }
    }

    void ReadSerialPort()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string message = serialPort.ReadLine();
                lock (queueLock)
                {
                    incomingMessageQueue.Enqueue(message);
                }
            }
            catch (TimeoutException) { }
            catch (Exception e)
            {
                if (isRunning) // Only log if still running, not during shutdown
                {
                    Debug.LogError($"Error reading from serial port: {e.Message}");
                }
            }
        }
    }

    void Update()
    {
        lock (queueLock)
        {
            while (incomingMessageQueue.Count > 0)
            {
                string message = incomingMessageQueue.Dequeue();
                ProcessIncomingMessage(message);
            }
        }
    }

    void ProcessIncomingMessage(string message)
    {
        message = message.Trim(); // Remove newline characters
        Debug.Log("Received from Arduino: " + message);

        if (message.StartsWith("RFID:"))
        {
            string rfidUID = message.Substring(5).Trim();
            OnRFIDDetected?.Invoke(rfidUID);
        }
        else if (message.StartsWith("BTN:"))
        {
            string buttonStates = message.Substring(4).Trim();
            OnButtonStateChanged?.Invoke(buttonStates);
        }
    }

    // Fungsi untuk mengirim perintah ke Arduino
    public void SendSerialCommand(string command)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(command);
                Debug.Log("Sent to Arduino: " + command);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to send command to Arduino: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Serial port not open. Cannot send command.");
        }
    }

    // Fungsi untuk mengontrol LED RGB
    public void SetLedRGB(Color color)
    {
        // Konversi Color Unity ke format HEX RRGGBB
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        SendSerialCommand($"LED:{hexColor}");
    }

    // Fungsi untuk mengontrol Buzzer
    public void SetBuzzer(bool on)
    {
        if (on)
        {
            SendSerialCommand("BUZZ_ON");
        }
        else
        {
            SendSerialCommand("BUZZ_OFF");
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false; // Signal the read thread to stop
        if (readThread != null && readThread.IsAlive)
        {
            readThread.Join(); // Wait for the thread to finish
        }

        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial Port closed.");
        }
    }
}