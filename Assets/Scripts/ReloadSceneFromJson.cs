using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ReloadSceneFromJson : MonoBehaviour
{
    [System.Serializable]
    private class Config
    {
        public float reloadAfterSeconds = 30; // запасное значение
    }

    [Header("Имя JSON-файла в StreamingAssets")]
    public string jsonFileName = "config.json";

    [Header("Если чтение JSON не удалось — использовать это значение (сек)")]
    public float fallbackSeconds = 30;
    public float seconds = 30;
    public float time;


    [SerializeField] GameObject _videoPlayer;
    private void Start()
    {
        StartCoroutine(LoadConfigAndSchedule());
    }

    private IEnumerator LoadConfigAndSchedule()
    {
        string path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

        // Для универсальности используем UnityWebRequest (работает и в Editor/PC/Mac, и на Android/iOS)
        string url = path;
        if (!url.StartsWith("http") && !url.StartsWith("file://"))
            url = "file://" + url;

        //int seconds = fallbackSeconds;

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

#if UNITY_2020_2_OR_NEWER
            bool hasError = req.result != UnityWebRequest.Result.Success;
#else
            bool hasError = req.isNetworkError || req.isHttpError;
#endif

            if (hasError)
            {
                Debug.LogWarning($"[ReloadSceneFromJson] Не удалось прочитать JSON: {req.error}. " +
                                 $"Использую fallbackSeconds={fallbackSeconds}");
            }
            else
            {
                try
                {
                    var cfg = JsonUtility.FromJson<Config>(req.downloadHandler.text);
                    if (cfg != null)
                    {
                        seconds = Mathf.Max(0, cfg.reloadAfterSeconds);
                    }
                    else
                    {
                        Debug.LogWarning("[ReloadSceneFromJson] Пустой/невалидный JSON. " +
                                         $"Использую fallbackSeconds={fallbackSeconds}");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"[ReloadSceneFromJson] Ошибка парсинга JSON: {e.Message}. " +
                                     $"Использую fallbackSeconds={fallbackSeconds}");
                }
            }
        }
    }

    private void Update()
    {

        time = time + Time.deltaTime;
        if(time > seconds)
        {
          if(!_videoPlayer.activeSelf)  GoToStandBy();
        }

        if(Input.GetMouseButton(0))
        {
            time = 0;
        }
    }

    void GoToStandBy()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
