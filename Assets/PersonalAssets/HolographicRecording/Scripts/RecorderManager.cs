using System.IO;
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

namespace UnityEngine.Recorder.Examples
{
    public class RecorderManager : MonoBehaviour
    {
        private bool isRecording = false;

        private Transform parent;
        static readonly string s_WindowTitle = "Recorder";

        public void StartRecording(){
            parent = transform.parent;
            transform.parent = null;
            isRecording = true;
            var recorderWindow = EditorWindow.GetWindow<RecorderWindow>(false, s_WindowTitle);
            recorderWindow.StartRecording();
        }

        public void StopRecording(){
            transform.parent = parent;
            isRecording = false;
            var recorderWindow = EditorWindow.GetWindow<RecorderWindow>(false, s_WindowTitle);
            recorderWindow.StopRecording();
        }

        public void toggleRecording(){
            if(isRecording)StopRecording();
            else StartRecording();
        }

    }
}
