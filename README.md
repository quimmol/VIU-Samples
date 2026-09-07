# VIU-Samples

Colección de muestras de malware y *proof-of-concepts* utilizada como
material de apoyo en clases de **reversing** e **ingeniería inversa** en
el marco de un curso universitario (VIU).

El repositorio se mantiene público únicamente con fines **docentes** y de
**investigación en seguridad**.

---

## ⚠️ Disclaimer

> **AVISO IMPORTANTE — LEER ANTES DE DESCARGAR CUALQUIER FICHERO**

Este repositorio contiene **software malicioso real** o *proofs-of-concept*
funcionales (binarios, scripts, ejecutables, APKs, documentos con macros,
shellcode, etc.). Cada uno de estos ficheros puede:

- Cifrar, borrar o exfiltrar información del sistema donde se ejecute.
- Establecer persistencia, comunicarse con servidores externos (C2) o
  descargar cargas adicionales.
- Ser detectado como amenaza por antivirus/EDR (esto es esperado y no
  significa que el repositorio esté comprometido).

El material se publica **exclusivamente** con propósito **educativo** y de
**investigación defensiva** (análisis estático/dinámico, desarrollo de
firmas, detección, formación en *reverse engineering*).

### No uso ofensivo

Queda **estrictamente prohibido** utilizar el contenido de este repositorio
para:

- Atacar, comprometer o dañar sistemas, redes o datos que no sean de tu
  propiedad o para los que no tengas **autorización expresa y por escrito**.
- Distribuir, redistribuir o desplegar las muestras fuera de un entorno
  controlado.
- Cualquier actividad ilegal en la jurisdicción del usuario.

Al descargar, clonar o utilizar cualquier fichero de este repositorio
aceptas ser **el único responsable** del uso que hagas del mismo.

### Sin garantías / sin responsabilidad

El material se ofrece **"tal cual" (AS IS)**, sin garantía de ningún tipo,
expresa o implícita. El autor **no se hace responsable** de ningún daño,
pérdida de datos, incidente de seguridad o consecuencia legal derivada del
uso, mal uso o simple posesión de estos ficheros.

---

## Entorno de análisis recomendado

Estas muestras **nunca** deben ejecutarse en un equipo de uso personal ni
conectado a una red de producción. Uso mínimo recomendado:

- Máquina virtual **aislada** (VirtualBox / VMware / QEMU) sin *shared
  folders* activos.
- **Snapshot** limpio antes de cada ejecución.
- Red en modo **host-only** o `INetSim` / `FakeNet-NG` para simular
  Internet sin salida real.
- Ficheros dentro de archivos comprimidos con contraseña `infected` (si
  aplica) para evitar ejecución accidental y detección por el AV del host.
- Herramientas típicas: Ghidra, IDA, x64dbg, radare2/rizin, JADX, apktool,
  Frida, Wireshark, ProcMon, PE-bear, DIE.

## Estructura

Cada carpeta corresponde a una sesión o práctica del curso e incluye,
cuando aplica:

- Muestra(s) original(es).
- Notas de análisis / *walkthrough*.
- Indicadores de compromiso (IOCs) extraídos.
- Scripts auxiliares (unpackers, decodificadores, etc.).

## Reporte de problemas

Si detectas contenido publicado por error (datos personales, muestras
fuera del ámbito docente, credenciales reales embebidas, etc.) abre un
*issue* o contacta directamente antes de difundirlo.

## Licencia

Salvo indicación expresa en una subcarpeta, el material **propio** (notas,
scripts de análisis, documentación) se publica con fines educativos. Las
**muestras de malware de terceros** conservan la autoría original y se
distribuyen únicamente a efectos de estudio; no se reclama titularidad
alguna sobre ellas.
