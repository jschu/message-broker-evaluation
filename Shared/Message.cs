using System;
using System.Collections.Generic;

namespace Shared
{
    public record Message(DateTime Timestamp, Boolean LastMessage, List<byte[]> Data);
}
