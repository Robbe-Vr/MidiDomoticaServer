using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiDomotica.Core.Api
{
    public static class ResponseHandler
    {
        public static ResponseResult Error(string reason, ErrorCode error = ErrorCode.Unknown_Error, object objectInAction = null, object context = null)
        {
            return new ResponseResult()
            {
                Error = true,
                ErrorTitle = error.ToString().Replace('_', ' '),
                ErrorMsg = reason,
                ObjectInAction = objectInAction,
                Context = context,
                StatusCode = ErrorToStatusCode(error),
                Result = "Error Occurred.",
                ResultType = "text",
            };
        }

        private static int ErrorToStatusCode(ErrorCode code)
        {
            switch (code)
            {
                case ErrorCode.Request_Data_Incomplete:
                    return 422;

                case ErrorCode.Server_Error:
                case ErrorCode.Unknown_Error:
                    return 500;

                case ErrorCode.User_Not_Authenticated:
                    return 401;

                case ErrorCode.User_Not_Authorized:
                    return 403;

                default:
                    return 200;
            }
        }

        public static ResponseResult Success(object result)
        {
            return new ResponseResult()
            {
                Error = false,

                StatusCode = 200,
                Result = result,
                ResultType = "text",
            };
        }

        public static ResponseResult Success(string result)
        {
            return new ResponseResult()
            {
                Error = false,

                StatusCode = 200,
                Result = result,
                ResultType = "text",
            };
        }

        public static ResponseResult Success(bool result)
        {
            return new ResponseResult()
            {
                Error = false,

                StatusCode = 200,
                Result = result,
                ResultType = "text",
            };
        }

        public static ResponseResult Success(int result)
        {
            return new ResponseResult()
            {
                Error = false,

                StatusCode = 200,
                Result = result,
                ResultType = "text",
            };
        }

        public static ResponseResult Success(double result)
        {
            return new ResponseResult()
            {
                Error = false,
                StatusCode = 200,

                Result = result,
                ResultType = "text",
            };
        }

        public static ResponseResult Success<T>(T result) where T : class
        {
            return new ResponseResult()
            {
                Error = false,

                StatusCode = 200,
                Result = result,
                ResultType = "json",
            };
        }
    }

    public enum ErrorCode
    {
        Unknown_Error = 0,

        User_Not_Authenticated,
        User_Not_Authorized,

        Request_Data_Incomplete,

        Server_Error,
    }
}
