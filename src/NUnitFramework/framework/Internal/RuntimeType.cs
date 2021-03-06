// Copyright (c) Charlie Poole, Rob Prouse and Contributors. MIT License - see LICENSE.txt

using System;

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// Enumeration identifying a common language
    /// runtime implementation.
    /// </summary>
    public enum RuntimeType
    {
        /// <summary>Any supported runtime framework</summary>
        Any,
        /// <summary>Microsoft .NET Framework</summary>
        NetFramework,
        /// <summary>Microsoft .NET Framework</summary>
        [Obsolete("Use NetFramework instead. 'Net' is ambiguous now that the .NET Core runtime has been renamed .NET.", error: true)]
        Net = NetFramework,
        /// <summary>Microsoft Shared Source CLI</summary>
        SSCLI,
        /// <summary>Mono</summary>
        Mono,
        /// <summary>MonoTouch</summary>
        MonoTouch,
        /// <summary>Microsoft .NET Core, including .NET 5+</summary>
        NetCore
    }
}
