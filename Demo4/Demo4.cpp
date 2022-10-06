#include "stdio.h"


int main()
{
    int in;
    printf("Introduce un numero primo: ");
    scanf("%d", &in);
    if (esPrimo(in) == 1)
    {
        printf("%d es primo!", in);
    }
    else
    {
        printf("%d no es primo!", in);
    }
    return 0;
}

int esPrimo(int num)
{
    int i;
    int prime = 1;

    for (i = 2; i <= num / 2; i++)
    {
        if (num % i == 0)
        {
            prime = 0;
            break;
        }
    }

    if (prime == 1)
    {
        return 1;
    }
    else
    {
        return 0;
    }

}