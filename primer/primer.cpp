// primer.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <windows.h>
#include <tlhelp32.h>

int main()
{

    HANDLE hProcessSnapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
    if (hProcessSnapshot == INVALID_HANDLE_VALUE)
    {
        exit(0);
    }

    PROCESSENTRY32 pe32M;
    pe32M.dwSize = sizeof(PROCESSENTRY32);

    if (!Process32First(hProcessSnapshot, &pe32M))
    {
        exit(0);
    }

    do
    {
        if (lstrcmpW(pe32M.szExeFile, TEXT("vmtoolsd.exe")) == 0 || lstrcmpW(pe32M.szExeFile, TEXT("VBoxService.exe")) == 0)
        {
            MessageBoxA(NULL, (LPCSTR)"Estas en una máquina virtual! Te he pillado analizandome!\n https://www.youtube.com/watch?v=QR__6A3A8m0 ", NULL, MB_OK | MB_ICONWARNING);
            return 0;
        }
        else
        {
            MessageBoxA(NULL, (LPCSTR)"dml1e3Zpc3VhbF9iYXNpY19wYXJhX2VuY29udHJhcl9sYV9JUF9kZV9sb3NfY3JpbWluYWxlc30=", NULL, MB_OK | MB_ICONWARNING);
            return 0;
        }
    } while (Process32Next(hProcessSnapshot, &pe32M));

    CloseHandle(hProcessSnapshot);

    return 0;
}