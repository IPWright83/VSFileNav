// Guids.cs
// MUST match guids.h
using System;

namespace VSNav
{
    static class GuidList
    {
        public const string guidVSNavPkgString = "d1fa1e4a-6cb3-42cd-9141-787813a57c02";
        public const string guidVSNavCmdSetString = "8e743567-5699-4476-8559-4d231e002c9f";

        public static readonly Guid guidVSNavCmdSet = new Guid(guidVSNavCmdSetString);
    };
}