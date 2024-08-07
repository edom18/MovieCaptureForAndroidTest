using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CameraMicRecorder : MonoBehaviour
{
    [SerializeField] private Button _recordButton;

    private bool _isRecording = false;
    private TMP_Text _buttonText;

    private AndroidJavaObject _recorderObject;
    private string OutputPath => Application.persistentDataPath + "/recording.mp4";

    private void Awake()
    {
        Initialize();

        _buttonText = _recordButton.GetComponentInChildren<TMP_Text>();
        _recordButton.onClick.AddListener(ToggleRecording);
    }

    private void Initialize()
    {
#if UNITY_ANDROID
        Debug.Log("Initializing Android Recorder");
        
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        _recorderObject = new AndroidJavaObject("tokyo.mesoncamerarecorder.CameraRecorder", activity, name);
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

    public void CapturedVideo(string filePath)
    {
        Debug.Log(filePath);
    }
}