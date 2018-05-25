Test csharp code for MemoryMappedFile.



MemoryMappedFile API is supported starting from .NET framework 4.



# Intro

From MSDN,

> A memory-mapped file contains the contents of a file in virtual memory. This mapping between a file and memory space enables an application, including multiple processes, to modify the file by reading and writing directly to the memory. Starting with the .NET Framework 4, you can use managed code to access memory-mapped files in the same way that native Windows functions access memory-mapped files, as described in [Managing Memory-Mapped Files](https://msdn.microsoft.com/library/ms810613.aspx). 

In a word, MMF is a technology we can used for IPC(Inter process communication).



# About this code

This demo code is very simple. It has two project to simulate two process.



Test1 project create a mmf from a disk file.  Once started, Test1 will check the second byte which will be set when Test2 started.


mmf_vs_normal_io project is a simple test about IO performance. Normally speaking, mmf will have at least two times performance boost compared with normal IO.



# Notes

Remember to use Mutex to sync when operating same MMF because MMF is not thread-safe. See this [link](https://stackoverflow.com/questions/16351747/how-to-parallel-process-data-in-memory-mapped-file).