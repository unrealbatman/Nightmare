using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class AudioPreviewer : MonoBehaviour
{
    private static bool isPreviewPlaying = false;
    private static AudioClip playingAudioClip = null;

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        UnityEngine.Object obj = EditorUtility.InstanceIDToObject(instanceId);
        if (obj is AudioClip audioClip)
        {
            if (isPreviewPlaying && playingAudioClip != null)
            {
                StopPreviewClip(playingAudioClip);
                if (playingAudioClip == audioClip)
                {
                    playingAudioClip = null;
                    isPreviewPlaying = false;
                    return false; // Prevent starting again if the same clip is clicked
                }
            }
            PlayPreviewClip(audioClip);
            playingAudioClip = audioClip;
            isPreviewPlaying = true;
            return true;
        }
        return false;
    }

    public static void PlayPreviewClip(AudioClip audioClip)
    {
        System.Reflection.Assembly unityAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtil = unityAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo methodInfo = audioUtil.GetMethod("PlayPreviewClip", BindingFlags.Static | BindingFlags.Public, null,
            new System.Type[] { typeof(AudioClip), typeof(Int32), typeof(Boolean) }, null);
        methodInfo.Invoke(null, new object[] { audioClip, 0, false });
    }

    public static void StopPreviewClip(AudioClip audioClip)
    {
        System.Reflection.Assembly unityAssembly = typeof(AudioImporter).Assembly;
        System.Type audioUtil = unityAssembly.GetType("UnityEditor.AudioUtil");
        MethodInfo methodInfo = audioUtil.GetMethod("StopAllPreviewClips", BindingFlags.Static | BindingFlags.Public);
        methodInfo.Invoke(null, null);
    }
}
