using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class CameraMicRecorder : MonoBehaviour
{
    [SerializeField] private Button _recordButton;
    [SerializeField] private Button _takeButton;
    [SerializeField] private RawImage _preview;

    private bool _isRecording = false;
    private TMP_Text _buttonText;

    private AndroidJavaObject _recorderObject;
    private string _latestFilePath = "";

    private void Awake()
    {
        Initialize();

        _buttonText = _recordButton.GetComponentInChildren<TMP_Text>();
        _recordButton.onClick.AddListener(ToggleRecording);
        _takeButton.onClick.AddListener(TakeFrame);
    }

    private void Initialize()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        Debug.Log("Initializing Android Recorder");
        
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        int cameraFacing = 0; // 0 = Front, 1 = Back
        _recorderObject = new AndroidJavaObject("tokyo.mesoncamerarecorder.CameraRecorder", activity, name, cameraFacing);
        _recorderObject.Call("startCamera");
#endif
    }

    private void ToggleRecording()
    {
        if (_isRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        if (_isRecording) return;

        Debug.Log("Start Recording");

        if (_recorderObject == null)
        {
            Debug.LogWarning("Recorder Object is null");
            return;
        }

        _isRecording = true;
        _buttonText.text = "Stop Recording";
        _recorderObject.Call("captureVideo");
    }

    private void StopRecording()
    {
        if (!_isRecording) return;

        _isRecording = false;
        _buttonText.text = "Start Recording";
        _recorderObject.Call("captureVideo");
    }

    private void TakeFrame()
    {
        if (string.IsNullOrEmpty(_latestFilePath)) return;
        
        _recorderObject.Call("getFrameAtTime", _latestFilePath, 1000);
    }

    public void CapturedVideo(string filePath)
    {
        Debug.Log(filePath);
        
        _latestFilePath = filePath;
    }

    public void CreatedFrame(string filePath)
    {
        Debug.Log(filePath);

        // byte[] data = File.ReadAllBytes(filePath);
        //
        // Debug.Log($"Length: {data.Length}");
        //
        // Texture2D texture = new Texture2D(1, 1);
        // texture.LoadImage(data);
        // texture.Apply();
    }
}