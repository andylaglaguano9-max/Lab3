Add-Type -AssemblyName System.IO.Compression.FileSystem
$file = Get-ChildItem -Path "C:\Users\PC-MASTER\6TO SEMESTRE\WEB 2\SEGUNDO PARCIAL\Practica 2" -Filter "*Informe*.docx" | Select-Object -First 1
$path = $file.FullName
$zip = [System.IO.Compression.ZipFile]::OpenRead($path)
$entry = $zip.GetEntry("word/document.xml")
$stream = $entry.Open()
$reader = New-Object System.IO.StreamReader($stream)
$xmlStr = $reader.ReadToEnd()
$reader.Close()
$stream.Close()
$zip.Dispose()
$text = $xmlStr -replace '<w:p\b[^>]*>', "`n" -replace '<[^>]+>', '' -replace '&lt;', '<' -replace '&gt;', '>'
Write-Host $text
