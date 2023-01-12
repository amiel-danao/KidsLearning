﻿using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace KidsLearning
{
    public class RESTConnection : MonoBehaviour
    {
        [SerializeField] private static string _domain = "https://cvsukidslearning.pythonanywhere.com/";
        [SerializeField] private static string _apiEndpoint = "api/";
        [SerializeField] private static string _apiSaveEndpoint = "scores/";
        [SerializeField] private string _testUrlParams;
        private static Dictionary<string, string> _urlParams;
        private static readonly Dictionary<string, string> LESSON_NAMES = new Dictionary<string, string>() { 
            { "1" , "Learn ABC" },
            { "2" , "Spelling" },
            { "3" , "Math" }
        };

        [DllImport("__Internal")]
        private static extern string GetURLFromQueryStr();

        public Action<int> SavedScoreEvent;

        public static bool HasKey(string key)
        {
            return _urlParams.ContainsKey(key);
        }

        public static string GetValue(string key)
        {
            return _urlParams[key];
        }

        public string GetCurrentLevel()
        {
            try
            {
                var levelName = SceneManager.GetActiveScene().name;
                return int.Parse(levelName.Replace("level", "")).ToString();
            }
            catch (FormatException)
            {
                return null;
            }
         }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
            var paramz = _testUrlParams;
#else
            var paramz = GetURLFromQueryStr();
#endif
            _urlParams = ExtractUrlParams(paramz);
            Debug.Log(paramz);
        }
        private void Start()
        {
            Debug.Log($"user id = {GetUserId()}");
        }

        public string GetUserId()
        {
            if (_urlParams.ContainsKey("user"))
            {
                return _urlParams["user"];
            }
            return null;
        }

        public void TrySaveScore(int score, float time, string summary)
        {
            var currentLevel = GetCurrentLevel();
            if (currentLevel == null) return;
            if (LESSON_NAMES.TryGetValue(currentLevel, out string lessonName))
            {
                StartCoroutine(SaveScore(lessonName, score, time, summary));
            }
        }

        public IEnumerator SaveScore(string lessonName, int score, float time, string summary)
        {
            var data = new Dictionary<string, string>{
                {"user", GetUserId() },
                {"score", score.ToString() },
                {"time", time.ToString() },
                {"lesson_name", lessonName },
                {"summary", summary }
            };
            var form = new WWWForm();

            foreach (var item in data)
            {
                form.AddField(item.Key, item.Value);
                Debug.Log($"saving data : {item.Key} = {item.Value}\n");
            }

            using UnityWebRequest www = UnityWebRequest.Post($"{_domain}{_apiEndpoint}{_apiSaveEndpoint}", form);
            //www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                SavedScoreEvent?.Invoke(score);
                Debug.Log($"Save score complete! \n");
            }
        }

        private static Dictionary<string, string> ExtractUrlParams(string url)
        {
            var result = new Dictionary<string, string>();
            var queryStartIndex = url.IndexOf("?") + 1;
            if (queryStartIndex > 0)
            {
                var queryString = url[queryStartIndex..];
                var paramTokens = queryString.Split('&');
                foreach (var paramToken in paramTokens)
                {
                    var keyValue = paramToken.Split('=');
                    if (keyValue.Length == 2)
                    {
                        result.Add(keyValue[0], keyValue[1]);
                    }
                }
            }
            return result;
        }

    }
}