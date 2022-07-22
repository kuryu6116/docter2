using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
namespace AITalker
{
    [RequireComponent(typeof(Button))]
    public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
    {
        [SerializeField]
        PointClass pointClass;

#if UNITY_IOS
        private string gameId = "3950302";
#elif UNITY_ANDROID
    private string gameId = "3950303";
#endif
        private string gameId = "3950303";
        Button myButton;
        [SerializeField]
        public string myPlacementId = "rewardedVideo";

        void Start()
        {
            myButton = GetComponent<Button>();

            // Set interactivity to be dependent on the Placement’s status:
            myButton.interactable = Advertisement.IsReady(myPlacementId);

            // Map the ShowRewardedVideo function to the button’s click listener:
            if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);

            // Initialize the Ads listener and service:
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, true);
        }

        // Implement a function for showing a rewarded video ad:
        void ShowRewardedVideo()
        {
            Advertisement.Show(myPlacementId);
        }

        // Implement IUnityAdsListener interface methods:
        public void OnUnityAdsReady(string placementId)
        {
            // If the ready Placement is rewarded, activate the button: 
            if (placementId == myPlacementId)
            {
                myButton.interactable = true;
            }
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                pointClass.SumPoint(50);
            }
            else if (showResult == ShowResult.Skipped)
            {
                pointClass.SumPoint(10);
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            // Log the error.
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            // Optional actions to take when the end-users triggers an ad.
        }
    }
}