using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceRail.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Elements")]
        public Button playButton;
        public Button quitButton;

        private void Start()
        {
            // Find buttons if they weren't assigned in the inspector
            if (playButton == null)
                playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
            
            if (quitButton == null)
                quitButton = GameObject.Find("QuitButton")?.GetComponent<Button>();

            // Setup button listeners
            if (playButton != null)
                playButton.onClick.AddListener(OnPlayButtonClicked);
            
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            Debug.Log("Play button clicked!");
            // Start the game
            SceneManager.LoadScene("MainGame");
        }

        private void OnQuitButtonClicked()
        {
            Debug.Log("Quit button clicked!");
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        private void OnDestroy()
        {
            // Clean up event listeners to prevent memory leaks
            if (playButton != null)
                playButton.onClick.RemoveListener(OnPlayButtonClicked);
            
            if (quitButton != null)
                quitButton.onClick.RemoveListener(OnQuitButtonClicked);
        }
    }
}