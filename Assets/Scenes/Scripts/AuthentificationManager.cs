using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

//TODO namespace
namespace LegoBattaleRoyal
{
    public class AuthentificationManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        private readonly string _unityTokenID = "ca4a8820-3e62-415d-88e6-fd3e36f2692d";

        private void Start()
        {
            StartAuthentificationInitAsync().Forget();

            _button.onClick.AddListener(OnSignIn);
        }

        public async UniTask StartAuthentificationInitAsync()
        {
            await UnityServices.InitializeAsync();
        }

        private async UniTask SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                // Shows how to get the playerID
                Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        private void OnSignIn()
        {
            PlayerAccountService.Instance.SignedIn += SignInWithUnity;
        }

        private void SignInWithUnity()
        {
            SignInAsync().Forget();
        }

        public async UniTask SignInAsync()
        {
            await SignInWithUnityAsync(_unityTokenID);
        }

        private async UniTask SignInWithUnityAsync(string token)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUnityAsync(token);

                Debug.Log("Authentication with Unity SUCCESS.");
            }
            catch (AuthenticationException error)
            {
                Debug.Log("ERROR. Sign in failed.");

                Debug.LogException(error);
            }
            catch (RequestFailedException requestException)
            {
                Debug.Log("ERROR. Request Failed Exception.");

                Debug.LogException(requestException);
            }
        }

        private async UniTask LinkWithUnityAsync(string accessToken)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithUnityAsync(accessToken);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex)
            when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        private async UniTask UnlinkUnityAsync()
        {
            try
            {
                await AuthenticationService.Instance.UnlinkUnityAsync();

                Debug.Log("Unlink is successful.");
            }
            catch (AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}