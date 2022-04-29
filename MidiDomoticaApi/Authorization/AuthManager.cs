using Microsoft.Extensions.Logging;
using MidiDomoticaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomoticaApi.Authorization
{
    public static class AuthManager
    {
        private static SignedInToken signedInToken;

        public static string[] SignIn()
        {
            if (signedInToken == null || signedInToken.Expired)
            {
                string accessToken = GenerateAccessToken();
                string refreshToken = GenerateRefreshToken();

                signedInToken = new SignedInToken(accessToken, refreshToken);

                return new string[] { refreshToken, accessToken };
            }
            else
            {
                return new string[] { "Multiple logins detected!" };
            }
        }

        public static bool ValidateToken(string accessToken)
        {
            if (signedInToken?.AccessToken == null || signedInToken.Expired)
            {
                return false;
            }
            else
            {
                return signedInToken.AccessToken == accessToken;
            }
        }

        public static string RefreshToken(string refreshToken)
        {
            if (signedInToken?.RefreshToken == refreshToken)
            {
                string newAccessToken = GenerateAccessToken();

                signedInToken.Refresh(newAccessToken);

                Logger.Log("Token Refreshed!");

                return newAccessToken;
            }
            else return "Not found.";
        }

        public static void SignOut()
        {
            signedInToken = null;
        }

        private static string GenerateAccessToken()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!+";

            Random r = new Random();

            return new string(Enumerable.Repeat(chars, 19).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 9).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 15).Select(c => c[r.Next(c.Length)]).ToArray());
        }

        private static string GenerateRefreshToken()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!+";

            Random r = new Random();

            return new string(Enumerable.Repeat(chars, 19).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 9).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 12).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 15).Select(c => c[r.Next(c.Length)]).ToArray());
        }
    }

    public class SignedInToken
    {
        public SignedInToken(string accessToken, string refreshToken)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;

            ExpiratedDate = DateTime.Now.AddMinutes(2);
        }

        public void Reset(string newAccessToken, string newRefreshToken)
        {
            RefreshToken = newRefreshToken;
            Refresh(newAccessToken);
        }

        public void Refresh(string newAccessToken)
        {
            AccessToken = newAccessToken;

            ExpiratedDate = DateTime.Now.AddMinutes(2);
        }

        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
        public DateTimeOffset ExpiratedDate { get; private set; }

        public bool Expired { get { return ExpiratedDate < DateTime.Now; } }
        public bool Valid { get { return !String.IsNullOrEmpty(AccessToken) && !Expired; } }
    }
}
