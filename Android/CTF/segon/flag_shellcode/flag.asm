global _start
section .text

_start:
	
push byte 0x0a
push dword "ife}"
push dword "al_l"
push dword "e_re"
push dword "s_th"
push dword "_thi"
push dword "u{is"
push dword ":)vi"
inc     ebx
mov     ecx, esp
mov     dl, 32 
mov     al, 4
int     0x80

xor     ebx, ebx
mov     al, 1
int     0x80
