using System;
using System.Runtime.Serialization;

/// <summary>
/// Exceptions used
/// Author: math
/// </summary>

namespace b2xtranslator.StructuredStorage.Common
{
    [Serializable]
    public class MagicNumberException : Exception
    {
        public MagicNumberException()
            : base("Magic Number not found in file.")
        {
        }

        public MagicNumberException(string additionalMessage)
            : base("Magic Number not found in file. " + additionalMessage)
        {
        }
    }

    [Serializable]
    public class ValueNotZeroException : Exception
    {
        public ValueNotZeroException(string value)
            : base(value + " must be zero.")
        {
        }
    }

    [Serializable]
    public class ReadBytesAmountMismatchException : Exception
    {
        public ReadBytesAmountMismatchException()
            : base("The number of bytes read mismatches the specified amount.")
        {
        }
    }

    [Serializable]
    public class UnsupportedSizeException : Exception
    {
        public UnsupportedSizeException(string value)
            : base("The size of " + value + " is not supported.")
        {
        }
    }

    [Serializable]
    public class InvalidValueInHeaderException : Exception
    {
        public InvalidValueInHeaderException(string value)
            : base("The value for '" + value + "' in the header is invalid.")
        {
        }

        public InvalidValueInHeaderException(string value, string additionalMessage)
            : base("The value for '" + value + "' in the header is invalid. " + additionalMessage)
        {
        }
    }

    [Serializable]
    public class ChainCycleDetectedException : Exception
    {
        public ChainCycleDetectedException(string chain)
            : base(chain + " contains a cycle.")
        {
        }
    }

    [Serializable]
    public class ChainSizeMismatchException : Exception
    {
        public ChainSizeMismatchException(string name)
            : base("The number of sectors used by " + name + " does not match the specified size.")
        {
        }

        public ChainSizeMismatchException(string name, string additionalMessage)
            : base("The number of sectors used by " + name + " does not match the specified size. " + additionalMessage)
        {
        }
    }

    [Serializable]
    public class InvalidSectorInChainException : Exception
    {
        public InvalidSectorInChainException()
            : base("Chain could not be build due to an invalid sector id.")
        {
        }
    }

    [Serializable]
    public class StreamNotInitalizedException : Exception
    {
        public StreamNotInitalizedException()
            : base("The current stream is not initialized.")
        {
        }
    }

    [Serializable]
    public class InvalidValueInDirectoryEntryException : Exception
    {
        public InvalidValueInDirectoryEntryException(string value)
            : base("The value for '" + value + "' is invalid.")
        {
        }
    }

    [Serializable]
    public class WrongDirectoryEntryTypeException : Exception
    {
        public WrongDirectoryEntryTypeException()
            : base("The directory entry is not of type STGTY_STREAM.")
        {
        }
    }

    [Serializable]
    public class StreamNotFoundException : Exception
    {
        public StreamNotFoundException(string name)
            : base("Stream with name '" + name + "' not found.")
        {
        }
    }

    [Serializable]
    public class DirectoryEntryNotFoundException : Exception
    {
        public DirectoryEntryNotFoundException(string name)
            : base("DirectoryEntry with name '" + name + "' not found.")
        {
        }
    }

    [Serializable]
    public class FileHandlerNotCorrectlyInitializedException : Exception
    {
        public FileHandlerNotCorrectlyInitializedException()
            : base("The file handler is not correctly initialized.")
        {
        }
    }

    [Serializable]
    public class DiFatInconsistentException : Exception
    {
        public DiFatInconsistentException()
            : base("Inconsistancy found while writing DiFat.")
        {
        }
    }

    [Serializable]
    public class InvalidSectorSizeException : Exception
    {
        public InvalidSectorSizeException()
            : base("Inconsistancy found while writing a sector.")
        {
        }
    }

}
