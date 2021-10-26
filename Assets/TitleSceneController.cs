using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class TitleSceneController : MonoBehaviour
{
    public float fadeTime = 2f;
    public CanvasGroup titleCanvas;
    public CanvasGroup fadeCanvas;
    public CanvasGroup buttonsCanvas;

    public Light2D bonfireLeft;
    public Light2D bonfireRight;

    public AudioSource bonfireSound;
    public AudioSource bassUISound;

    bool changeScene = false;

    public void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1;
        //bonfireSound.Play();
        bonfireSound.ignoreListenerPause = true;
    }

    private void Start()
    {
        buttonsCanvas.alpha = 0;
        titleCanvas.alpha = 0;
        LeanTween.alphaCanvas(titleCanvas, 1, fadeTime).setOnComplete(FadeInButtonCanvas);
        LeanTween.alphaCanvas(fadeCanvas, 0, fadeTime).setOnComplete(SetFadeCanvasToNotBlockRaycast);
    }

    private void SetFadeCanvasToNotBlockRaycast()
    {
        fadeCanvas.blocksRaycasts = false;
    }

    private void FadeInButtonCanvas()
    {
        buttonsCanvas.blocksRaycasts = true;
        buttonsCanvas.interactable = true;
        LeanTween.alphaCanvas(buttonsCanvas, 1 ,1f);
    }

    public void PlayAudioSound()
    {
        bassUISound.Play();
    }

    public void GoNextScene()
    {
        changeScene = true;
        StartCoroutine("FadeScene");
        LeanTween.alphaCanvas(buttonsCanvas, 0, fadeTime);
        LeanTween.alphaCanvas(titleCanvas, 0, fadeTime);
    }

    private void Update()
    {
        if(changeScene)
        {
            bonfireLeft.pointLightOuterRadius -= Time.deltaTime * 0.5f;
            bonfireLeft.pointLightOuterRadius = Mathf.Clamp(bonfireLeft.pointLightOuterRadius, 0, 10);

            bonfireRight.pointLightOuterRadius -= Time.deltaTime * 0.5f;
            bonfireRight.pointLightOuterRadius = Mathf.Clamp(bonfireRight.pointLightOuterRadius, 0, 10);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

#if UNITY_STANDALONE_WIN
        Application.Quit();
#endif

#if UNITY_WEBGL
        openWindow("https://itch.io/jam/ludwig-2021");
#endif
    }

    IEnumerator FadeScene()
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(1);
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);
}
