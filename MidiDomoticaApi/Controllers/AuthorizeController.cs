using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MidiDomoticaApi.Authorization;
using MidiDomoticaApi.Filters;
using MidiDomoticaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SYWCentralLogging;

namespace MidiDomoticaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ProcessingController
    {
        private AuthorizationSettings _settings;

        public AuthorizeController(AuthorizationSettings settings)
        {
            _settings = settings;
        }

        [HttpPost("{action}")]
        [Authorized]
        public IActionResult Validate(TokenModel model)
        {
            return Process(() =>
            {
                try
                {
                   
                    return Ok(new
                    {
                        Result =  AuthManager.ValidateToken(model.MidiDomotica_AccessToken),
                    });
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while validating token {model.MidiDomotica_AccessToken}!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("{action}")]
        public IActionResult Refresh(RefreshModel model)
        {
            return Process(() =>
            {
                try
                {
                    string refreshedAccessToken = AuthManager.RefreshToken(model.MidiDomotica_RefreshToken);

                    if (refreshedAccessToken != "Not found.")
                    {
                        return Ok(new
                        {
                            AccessToken = refreshedAccessToken,
                            Success = true,
                        });
                    }
                    else
                    {
                        Logger.Log($"Error while refreshing token {model.MidiDomotica_RefreshToken}!");
                        return Ok(new
                        {
                            Success = false
                        });
                    }
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while refreshing token {model.MidiDomotica_RefreshToken}!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Low);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("{action}")]
        public IActionResult LogIn(AuthorizationModel model)
        {
            return Process(() =>
            {
                try
                {
                    string sourceIp = HttpContext.Request.Headers.ContainsKey("X-Real-Ip") ? HttpContext.Request.Headers["X-Real-Ip"]
                                    : HttpContext.Request.Headers.ContainsKey("X-Forwarded-For") ? HttpContext.Request.Headers["X-Forwarded-For"]
                                    : $"Unknown source via {HttpContext.Connection.RemoteIpAddress}:{HttpContext.Connection.RemotePort}";

                    if (model.AppSecret == _settings.AppSecret)
                    {
                        if (model.Password == _settings.AppPassword)
                        {
                            string[] result = AuthManager.SignIn();

                            if (result.Length == 2)
                            {
                                return Ok(new
                                {
                                    Result = true,
                                    RefreshToken = result[0],
                                    AccessToken = result[1],
                                });
                            }
                            else
                            {
                                return Ok(new
                                {
                                    Message = result[0],
                                    Result = false,
                                });
                            }
                        }
                        else
                        {
                            Logger.Log($"Failed login attempt from {sourceIp}. Incorrect password.");
                        }
                    }
                    else
                    {
                        Logger.Log($"Failed login attempt from {sourceIp}. Incorrect secret.");
                    }

                    return Ok(new
                    {
                        Message = "Incorrect login!",
                        Result = false,
                    });
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while logging in!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Mediocre);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }

        [HttpPost("{action}")]
        [Authorized]
        public IActionResult Logout()
        {
            return Process(() =>
            {
                try
                {
                    AuthManager.SignOut();

                    return Ok();
                }
                catch (Exception e)
                {
                    Logger.Log($"Error while logging in!\nError: {e.Message}\nStackTrace:\n{e.StackTrace}", SeverityLevel.Mediocre);
                    return Problem(
                        detail: $"Error Occurred while processing your request!\nError: {e.Message}",
                        title: "Error Occurred!",
                        statusCode: 501
                    );
                }
            });
        }
    }
}
