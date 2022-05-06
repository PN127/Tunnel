using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tunnel
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        private Text _versionText;
        [SerializeField]
        private string _version;

        private RectTransform _nextWindow;
        [SerializeField]
        private List<RectTransform> _windows;
        
        private bool OpenMenu;

        [Space]
        private float SizeOpen;
        private float SizeClose;
        [HideInInspector]
        public bool close;

        [Space]
        [Header("Objects for save")]
        [SerializeField]
        private List<Toggle> _toggleSettings;
        [SerializeField]
        private List<Slider> _sliderSettings;

        private void Start()
        {
            LoadSettings();
            _versionText.text = $"ver: {_version}";
            close = true;
            SizeOpen = 1;
            SizeClose = 0.01f;
        }
        private void Update()
        {
            SaveSettings();
        }

        public void LoadScene(int sceneid)
        {
            SceneManager.LoadScene(sceneid);
        }
        public void Exit()
        {
            UnityEditor.EditorApplication.isPaused = true;
        }

        public void ScaleUI(RectTransform target)
        {
            if (_windows.Count > 1)
            {
                if (!target.gameObject.activeInHierarchy && !_windows[1].gameObject.activeInHierarchy)
                    OpenMenu = true;
                if (!target.gameObject.activeInHierarchy && _windows[1].gameObject.activeInHierarchy)
                {
                    close = false;
                    _nextWindow = target;
                    OpenMenu = false;
                    StartCoroutine(ScaleUICoroutine(_windows[1], OpenMenu));
                    return;
                }
            }
            if (target.gameObject.activeInHierarchy)
            {
                OpenMenu = false;
                close = true;
            }
            else
                OpenMenu = true;

            StartCoroutine(ScaleUICoroutine(target, OpenMenu));
        }
        public IEnumerator ScaleUICoroutine(Transform transform, bool increase)
        {
            if (increase)
            {
                Time.timeScale = 0;
                transform.parent.gameObject.SetActive(true);
                transform.gameObject.SetActive(true);
                transform.localScale = new Vector3(SizeClose, SizeClose, 0);
                while (transform.localScale.x < SizeOpen)
                {
                    transform.localScale += new Vector3(0.01f, 0.01f, 0);
                    yield return null;
                }
            }
            else
            {
                while (transform.localScale.x > SizeClose)
                {
                    transform.localScale -= new Vector3(0.01f, 0.01f, 0);
                    yield return null;
                }
                transform.gameObject.SetActive(false);
                transform.parent.gameObject.SetActive(false);

                if (close)
                    Time.timeScale = 1;
                else
                    StartCoroutine(ScaleUICoroutine(_nextWindow, true));
            }
            StopCoroutine(ScaleUICoroutine(transform, increase));
        }

        public void OpenSecondWindow(RectTransform target)
        {
            close = false;
            StartCoroutine(ScaleUICoroutine(target, false));            
        }

        public void WritedownNextWindow(RectTransform window)
        {
            _nextWindow = window;
        }

        private void LoadSettings()
        {
            int t = 0;
            int s = 0;
            foreach (Toggle toggle in _toggleSettings)
            {
                if (PlayerPrefs.GetInt($"toggle{t}") == 1)
                    toggle.isOn = true;
                else
                    toggle.isOn = false;
                t++;
            }
            foreach (Slider slider in _sliderSettings)
            {
                slider.value = PlayerPrefs.GetFloat($"slider{s}");
                s++;
            }
        }

        private void SaveSettings()
        {
            int t = 0;
            int s = 0;
            foreach (Toggle toggle in _toggleSettings)
            {
                if (toggle.isOn)
                    PlayerPrefs.SetInt($"toggle{t}", 1);
                else
                    PlayerPrefs.SetInt($"toggle{t}", 0);
                t++;
            }
            foreach (Slider slider in _sliderSettings)
            {
                PlayerPrefs.SetFloat($"slider{s}", slider.value);                
                s++;
            }
        }
    }
}
