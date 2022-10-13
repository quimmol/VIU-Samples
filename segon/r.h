#pragma once

#ifdef CLASE_EXPORTS
#define CLASE_API __declspec(dllexport)
#else
#define CLASE_API __declspec(dllimport)
#endif

extern "C" CLASE_API void donde_estoy();

extern "C" CLASE_API void hola();