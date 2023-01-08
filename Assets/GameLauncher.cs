using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

namespace KidsLearning.Assets
{
    public class GameLauncher : MonoBehaviour
    {
        [SerializeField] private TMP_Text _loadingText;
        private const string LOAD_ERROR_TEXT = "There's a problem loading the activity :( please reload the page";
        // Use this for initialization
        void Start()
        {
            iTween.ScaleTo(_loadingText.gameObject, iTween.Hash(
                "scale", Vector3.one * 1.2f,
                "time", 1f,
                "looptype", "pingpong",
                "easetype", iTween.EaseType.easeInOutCubic
            ));

            if (RESTConnection.HasKey("lesson"))
            {
                var lesson_no = RESTConnection.GetValue("lesson");
                var levelToLoad = $"Scenes/level{lesson_no}";
                int buildIndex = SceneUtility.GetBuildIndexByScenePath(levelToLoad);

                if (buildIndex == -1)
                {
                    _loadingText.text = LOAD_ERROR_TEXT;
                }
                else { 
                    SceneManager.LoadScene(levelToLoad);
                }
            }
            else
            {
                _loadingText.text = LOAD_ERROR_TEXT;
            }
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}