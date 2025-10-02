using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoFromStreamingAssets : MonoBehaviour
{
    [Tooltip("Имя файла в Assets/StreamingAssets (например intro.mp4)")]
    public string fileName = "intro.mp4";

    [Tooltip("Автозапуск при старте")]
    public bool autoplay = true;

    private VideoPlayer vp;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        vp.isLooping = true;
        // Если хотите звук через AudioSource — раскомментируйте и добавьте AudioSource на объект
        // var audio = GetComponent<AudioSource>();
        // if (audio != null) { vp.audioOutputMode = VideoAudioOutputMode.AudioSource; vp.SetTargetAudioSource(0, audio); }
    }

    private void Start()
    {
        if (autoplay)
            StartCoroutine(LoadAndPlay());
    }

    public IEnumerator LoadAndPlay()
    {
        string playablePath = null;

#if UNITY_ANDROID && !UNITY_EDITOR
        // На Android StreamingAssets упакованы в APK: копируем во временную папку один раз
        string src = Path.Combine(Application.streamingAssetsPath, fileName);
        string dst = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(dst))
        {
            string url = src;
            if (!url.StartsWith("jar:") && !url.StartsWith("file://") && !url.StartsWith("http"))
                url = "jar:file://" + url; // подстраховка

            using (var req = UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();
#if UNITY_2020_2_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
#else
                if (req.isNetworkError || req.isHttpError)
#endif
                {
                    Debug.LogError($"[VideoFromStreamingAssets] Не удалось скопировать видео: {req.error}");
                    yield break;
                }
                try
                {
                    File.WriteAllBytes(dst, req.downloadHandler.data);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[VideoFromStreamingAssets] Ошибка записи {dst}: {e.Message}");
                    yield break;
                }
            }
        }
        playablePath = dst;
        string urlToPlay = playablePath;
        if (!urlToPlay.StartsWith("file://")) urlToPlay = "file://" + urlToPlay;
        vp.url = urlToPlay;
#else
        // ПК/консоли/Editor — читаем прямо из StreamingAssets
        string path = Path.Combine(Application.streamingAssetsPath, fileName);
        if (!File.Exists(path))
        {
            Debug.LogError($"[VideoFromStreamingAssets] Файл не найден: {path}");
            yield break;
        }
        string urlToPlay = path;
        if (!urlToPlay.StartsWith("file://")) urlToPlay = "file://" + urlToPlay;
        vp.url = urlToPlay;
#endif

        // Подготовка и старт
        vp.errorReceived += OnVideoError;
        vp.prepareCompleted += OnPrepared;

        vp.Prepare();
        while (!vp.isPrepared)
            yield return null;

        vp.Play();
    }

    private void OnPrepared(VideoPlayer source)
    {
        // Можете что-то сделать в момент готовности
        // Debug.Log("[VideoFromStreamingAssets] Video prepared");
    }

    private void OnVideoError(VideoPlayer source, string message)
    {
        Debug.LogError($"[VideoFromStreamingAssets] Video error: {message}");
    }
}
