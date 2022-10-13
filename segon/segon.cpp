#include <iostream>
#include <windows.h>
#include <tlhelp32.h>

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPWSTR    lpCmdLine,
    _In_ int       nCmdShow)
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
            MessageBoxA(NULL, (LPCSTR)"Estas en una máquina virtual! Te he pillado analizandome!\n https://www.youtube.com/watch?v=u3CKgkyc7Qo ", NULL, MB_OK | MB_ICONWARNING);
            return 0;
        }
    } while (Process32Next(hProcessSnapshot, &pe32M));

    CloseHandle(hProcessSnapshot);

    return 0;
}

#pragma comment(linker, "/export:donde_estoy")
void donde_estoy() 
{
    MessageBoxA(NULL, (LPCSTR)"dml1e6FIYWNrX1RoZV9QbGFuZXQhfQ==", "Enhorabuena!!", MB_OK | MB_ICONWARNING);
}