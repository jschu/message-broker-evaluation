using System;
using System.Collections.Generic;

namespace Shared
{
    public record Message(DateTime Timestamp, int MessageNumber, int NumberOfMessages, int MessageSizeInKB, List<byte[]> Data);
}
