// Copyright (c) Microsoft Corporation, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0.

namespace System.Web.Optimization
{
    using System.Globalization;

    internal static class ExceptionUtil
    {
        internal static ArgumentException ParameterNullOrEmpty(string parameter)
        {
            return new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The string parameter '{0}' cannot be null or empty.", parameter), parameter);
        }

        internal static ArgumentException PropertyNullOrEmpty(string property)
        {
            return new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The value assigned to property '{0}' cannot be null or empty.", property), property);
        }

        internal static Exception ValidateVirtualPath(string virtualPath, string argumentName)
        {
            if (String.IsNullOrEmpty(virtualPath))
            {
                return ExceptionUtil.ParameterNullOrEmpty(argumentName);
            }
            if (!virtualPath.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
            {
                return new ArgumentException(String.Format(CultureInfo.CurrentCulture, "The URL '{0}' is not valid.Only application relative URLs(~/ url) are allowed.", virtualPath), argumentName);
            }
            return null;
        }
    }
}
