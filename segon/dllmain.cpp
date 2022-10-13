// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
#include <iostream>
#include <windows.h>
#include <tlhelp32.h>
#include <string>
using namespace std;

void entrada();
void salida();

BOOL alertas = false;

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            entrada();
        case DLL_THREAD_ATTACH:
        case DLL_THREAD_DETACH:
        case DLL_PROCESS_DETACH:
            salida();
            break;
    }
    return TRUE;
}

void donde_estoy()
{
    #pragma comment(linker, "/EXPORT:" __FUNCTION__ "=" __FUNCDNAME__)

    string sam = "dml1e6FIYWNrX1";
    string frodo = "RoZV9QbGFuZXQhfQ";
    string anillo = "==";

    alertas = TRUE;

    string all = sam + frodo + anillo;

    MessageBoxA(NULL, (LPCSTR)all.c_str(), "Enhorabuena!!", MB_OK | MB_ICONINFORMATION);
}

void hola()
{
    #pragma comment(linker, "/EXPORT:" __FUNCTION__ "=" __FUNCDNAME__)
    MessageBoxA(NULL, (LPCSTR)"dml1e290cmFfdmV6X3NlcmF9", "Enhorabuena??", MB_OK | MB_ICONWARNING);
}

void entrada()
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

    BOOLEAN flag = FALSE;
    do
    {
        if (lstrcmpW(pe32M.szExeFile, TEXT("vmtoolsd.exe")) == 0 || lstrcmpW(pe32M.szExeFile, TEXT("VBoxService.exe")) == 0)
        {
            MessageBoxA(NULL, (LPCSTR)"Estas en una máquina virtual! Te he pillado analizandome!\n https://www.youtube.com/watch?v=u3CKgkyc7Qo ", NULL, MB_OK | MB_ICONWARNING);
            flag = TRUE;
            break;
        }
    } while (Process32Next(hProcessSnapshot, &pe32M));

    if (!flag)
    {
        MessageBoxA(NULL, (LPCSTR)"Donde me estas ejecutando?", NULL, MB_OK | MB_ICONWARNING);
    }

    CloseHandle(hProcessSnapshot);
}

void salida()
{
    if (!alertas)
    {
        MessageBoxA(NULL, (LPCSTR)"dml1e2VzdGFfbm9fZXN9", "Enhorabuena??", MB_OK | MB_ICONWARNING);
    }
}