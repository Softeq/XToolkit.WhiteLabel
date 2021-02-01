// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.Remote.Primitives
{
    [Flags]
    public enum LogVerbosity
    {
        /// <summary>
        ///    Use default value.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        ///    Without any verbosity.
        /// </summary>
        None = 1,

        /// <summary>
        ///    Log basic request data.
        /// </summary>
        RequestHeaders = 2,

        /// <summary>
        ///    Log request body.
        /// </summary>
        RequestBody = 4,

        /// <summary>
        ///    Log all response data.
        /// </summary>
        RequestAll = RequestHeaders | RequestBody,

        /// <summary>
        ///    Log basic response data.
        /// </summary>
        ResponseHeaders = 8,

        /// <summary>
        ///    Log response body.
        /// </summary>
        ResponseBody = 16,

        /// <summary>
        ///    Log all responses data.
        /// </summary>
        ResponseAll = ResponseHeaders | ResponseBody,

        /// <summary>
        ///     Log all requests and response data.
        /// </summary>
        All = ResponseAll | RequestAll
    }
}
