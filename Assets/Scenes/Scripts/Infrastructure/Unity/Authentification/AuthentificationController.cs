using Cysharp.Threading.Tasks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace LegoBattaleRoyal.Infrastructure.Unity.Authentification
{
    public class AuthentificationController
    {
        public async UniTask SignInAsync()
        {
            await UnityServices.InitializeAsync();
            await SignInAnonymouslyAsync();
        }

        private async UniTask SignInAnonymouslyAsync()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log("Sign in anonymously succeeded!");

                if (AuthenticationService.Instance.IsAuthorized) //is sign in?
                {
                    // Shows how to get a playerID
                    Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

                    // Shows how to get an access token
                    Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");

                    return;
                }
                throw new Exception("auth failed");
            }
            //popup general -  // Notify the player with the proper error message
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
            catch (Exception ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
    }
}