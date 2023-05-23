'Proceso CAJA
Set oShell = CreateObject ("Wscript.Shell")
Set fso = CreateObject("Scripting.FileSystemObject")

Dim namePathDownload
Dim prefijoNombreApp
Dim prefijoNombreUpdateApp
Dim pathJarUpdate
Dim pathProcesosActivos

'Este path debe ser el mismo que genera el app.xml al realizar el mvn
namePathDownload = "C:\SIWEB\caja\app\"
prefijoNombreApp = "CPla"
prefijoNombreUpdateApp = "updateCaja"
pathJarUpdate = "C:\SIWEB\caja\"
pathProcesosActivos = "C:\SIWEB\caja\procesosActivosCaja.txt"

On Error Resume Next
Err.Clear

'Borra el txt que registrar el número de procesos activos de caja.exe y no se haya borrado de la última ejecución
fso.DeleteFile pathProcesosActivos

'Busca todos los procesos en ejecución con el nombre caja.exe y vacía el total en un txt.
strArgs = "cmd /c tasklist /fi " & """" & "imagename eq caja.exe" & """ 2<NUL | find /I /C " & """" & "caja.exe"&""" > " & pathProcesosActivos
oShell.Run strArgs, 0, true

'Dar tiempo para que se genere el txt de procesos activos
'WScript.sleep 4000

'Lee el txt con el número de proceso activos de caja.exe
 numProcess = fso.OpenTextFile(pathProcesosActivos).ReadAll()
	
'Valida si existe más de un proceso de la app en ejecución y borra el txt generado para registrar el número de procesos activos de caja.exe
if numProcess => 2 then
	MsgBox("La aplicaci" + Chr(243) + "n ya se encuentra en ejecuci" + Chr(243) + "n, favor de esperar.")
	fso.DeleteFile pathProcesosActivos
	WScript.Quit 1
End if

'Borra el txt generado para registrar el número de procesos activos de caja.exe
fso.DeleteFile pathProcesosActivos

'Busca el archivo de actualización
set folder = fso.GetFolder(pathJarUpdate)
Set GetUpdateFile = Nothing

set colFiles = folder.Files
for each file in colFiles
	if instr (file.Name, prefijoNombreUpdateApp) then
		Set GetUpdateFile = file	
	end if
next

'Envía mensaje de advertencia si no encuentra el archivo de actualización
If GetUpdateFile is Nothing Then
	MsgBox("No se encontr" + Chr(243) + " el archivo de actualizaci" + Chr(243) + "n de la aplicaci" + Chr(243) + "n, favor de validar con soporte t" + Chr(233) + "cnico")
	WScript.Quit 1
End if

'Ejecuta archivo de actualización
strArgs = "cmd /c java -jar " & GetUpdateFile
oShell.Run strArgs, 0, true

'Busca el archivo de inicialización
set folder = fso.GetFolder(namePathDownload)
Set GetInitializationFile = Nothing
	
set colFiles = folder.Files
for each file in colFiles
	if instr (file.Name, prefijoNombreApp) then
		Set GetInitializationFile = file	
	end if
next
	
'Envía mensaje de advertencia si no encuentra el archivo de inicialización
If GetInitializationFile is Nothing Then
	MsgBox("No se encontr" + Chr(243) + " el archivo de inicializaci" + Chr(243) + "n de la aplicaci" + Chr(243) + "n, favor de validar con soporte t" + Chr(233) + "cnico")
End if

'Ejecuta archivo de inicialización
strArgs = "cmd /c java -jar " & GetInitializationFile
oShell.Run strArgs, 0, false