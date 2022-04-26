using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Text _versionText;
    [SerializeField]
    private string _version;

    private void Start()
    {
        _versionText.text = $"ver: {_version}";
    }

    public void LoadScene(int sceneid)
    {
        SceneManager.LoadScene(sceneid);
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPaused = true;
    }


}
