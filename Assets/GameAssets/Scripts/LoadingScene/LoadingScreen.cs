using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
public class LoadingScreen : MonoBehaviour
{
    [Header("Scene Reference")]
    [SerializeField] private AssetReference playScene;

    [Header("UI Elements")]
    [SerializeField] private Image loadingSlider;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private GameObject LoadingContent;
    [SerializeField] private bool isAddressablesEnabled = false;

    private float progress;
    private bool permissionAsked;
    private bool isSliderLoaded;
    private bool isSceneReady;
    private void Awake ()
    {
        
    }

    private void Start ()
    {
        isSliderLoaded = false;
        isAddressablesEnabled = playScene == null ? false : true;
        StartCoroutine(LoadScene(playScene));
    }

    private void Update ()
    {
        isAddressablesEnabled = playScene == null ? false : true;
        UpdateImageFillAmount(loadingSlider , progress);

        if (progress >= 1f && !permissionAsked)
        {
            permissionAsked = true;
        }

        if (progress >= 1f && permissionAsked)
        {
            if (!isSliderLoaded)
            {
                isSliderLoaded = true;
                // Show the start button or any other UI element
                startBtn.SetActive(true);
                LoadingContent.SetActive(false);
            }
        }
    }
    [ContextMenu("Activate")]
    public void Activate ()
    {
        if (isSceneReady) { return; }
        isSceneReady = true;
    }

    private void UpdateImageFillAmount ( Image image , float amount )
    {
        if (image != null)
            image.fillAmount = amount;
    }

    private void ShowProgress ( float amount )
    {
        progress = amount;
    }

    IEnumerator LoadScene ( AssetReference Which = null )
    {
        if (isAddressablesEnabled)
        {
            Debug.Log("loading Addressable!");
            //Not allowing scene activation immediately
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(Which , LoadSceneMode.Additive , false);
            while (!handle.IsDone)
            {
                ShowProgress(handle.PercentComplete);
                yield return new WaitForSeconds(0.2f);
            }

            Debug.Log("loading Addressable...");
            //One way to handle manual scene activation.
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                ShowProgress(handle.PercentComplete);
            }
            Debug.Log("loading Addressable...Done");
            yield return new WaitUntil(() => isSceneReady);

            Debug.Log($"isSceneReady: {isSceneReady}");

            GameManager.Instance.FetchConfigData();

            yield return new WaitUntil(() => GameManager.Instance.IsDataFetched());

            Debug.Log($"isDataFetched : {GameManager.Instance.IsDataFetched()}");
            handle.Result.ActivateAsync();
            ConfigMan.Instance.TheDebugObj.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            SceneManager.UnloadSceneAsync(0);
        }
        else
        {
            Debug.Log("loading Scene!");
            AsyncOperation handle = SceneManager.LoadSceneAsync("MainScene" , LoadSceneMode.Additive);
            handle.allowSceneActivation = false;

            while (handle.progress < 0.9f)
            {
                ShowProgress(handle.progress);
                yield return new WaitForSeconds(0.1f);
            }

            if(handle.progress>= 0.9f && !isSceneReady)
            {
                float displayedProgress = handle.progress;
                // gradually increase the progress to 1
                while (displayedProgress<1)
                {
                    displayedProgress += 0.05f;
                    ShowProgress(displayedProgress);
                    yield return new WaitForSeconds(0.1f);
                }
            }
            // Wait for your condition to be ready before activating scene
            yield return new WaitUntil(() => isSceneReady);
            GameManager.Instance.FetchConfigData();
            yield return new WaitUntil(() => GameManager.Instance.IsDataFetched());

            handle.allowSceneActivation = true;

            while (!handle.isDone)
            {
                yield return null;
            }

            ConfigMan.Instance.TheDebugObj.SetActive(false);
            //yield return new WaitForSeconds(0.5f);
            SceneManager.UnloadSceneAsync(0);
        }
    }
}