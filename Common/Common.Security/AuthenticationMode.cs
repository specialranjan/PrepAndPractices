namespace Common.Security
{
    using System;

    [Flags]
    public enum AuthenticationMode
    {
        None = 0,
        UserOnly = 1,
        Full = 2
    }
}
