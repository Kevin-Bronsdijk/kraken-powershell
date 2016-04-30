PowerShell module for Kraken.io
=============

## Getting Started
A PowerShell Binary Module (merged assembly) will be provided soon. For now, just compile the project, use Import-Module kraken.powershell.dll and make sure all supporting assemblies are available.

## Code Samples

**Import the module**

```powershell
Import-Module 'C:\path\..\kraken.powershell.dll' -Verbose
```

```
VERBOSE: Importing cmdlet 'Optimize-Image'.
VERBOSE: Importing cmdlet 'Optimize-ImageUrl'.
VERBOSE: Importing cmdlet 'Optimize-ImageToAzure'.
VERBOSE: Importing cmdlet 'Optimize-ImageUrlToAzure'.
```
**Upload files**

```powershell
$files = Get-ChildItem 'C:\Users\Kevin\Pictures\Krakentest' -Filter *.png
$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $true  
$result | Format-Table
```

```
Success FileName                OriginalSize KrakedSize SavedBytes KrakedUrl                                                                           
------- --------                ------------ ---------- ---------- ---------                                                                           
   True app-insights.png               74185      60324      13861 https://dl.kraken.io/api/db/2c/3a/0be9d85ffe658ee1da641b6dc6/app-insights.png       
   True azureportalsettings.png        20231      17282       2949 https://dl.kraken.io/api/3c/2a/24/59ab8b37dc1dcd89dc12b5aa42/azureportalsettings.png
   True DevSlice.png                   22081      16995       5086 https://dl.kraken.io/api/49/3f/91/9622a17f62988d57d711e7f624/DevSlice.png           
   True krakenio.png                   11188       9764       1424 https://dl.kraken.io/api/b9/4b/19/40b3826329cf3e56078e9a6251/krakenio.png   
```

**Reporting**
```powershell
$savedBytes = ($result | Measure-Object SavedBytes -Sum).Sum
$savedBytes
```

```
23320
```
**Upload files with callback**

```powershell
$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $false  -CallBackUrl 'http://devslice.net/callback'
$result | Format-Table
```

```
Id                              
--                              
2c1d52b60f669d761d43ff771975c263
30b8dc74cfda849732d3f5b7dbd5ea73
55db2b3ce28192af1f6aa02e2853101f
ea5828bb38b32345a6dd64e74c87bfa4
```

**Public images from xbox.com**
```powershell
$files = @('http://compass.xbox.com/assets/67/2e/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg',
            'http://compass.xbox.com/assets/4f/e1/4fe1a0fc-22ff-4130-a3ec-a74c19cf8bcb.jpg',
            'http://compass.xbox.com/assets/db/08/db080bcc-6d81-451a-a867-cb9f96399599.jpg',
            'http://compass.xbox.com/assets/14/5e/145e00ee-22d3-4226-b5ca-4ab821230f60.jpg')
$result = Optimize-ImageUrl -FileUrl $files -Key $key -Secret $secret -Wait $true  
$result | Format-Table
```
```
Success FileName                                 OriginalSize KrakedSize SavedBytes KrakedUrl                                                                                            
------- --------                                 ------------ ---------- ---------- ---------                                                                                            
   True 672e8768-d75a-4c68-82bd-e9b003e997e3.jpg        56524      51675       4849 https://dl.kraken.io/api/98/5d/0e/9a7c7c73c762d9a594979e4474/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg
   True 4fe1a0fc-22ff-4130-a3ec-a74c19cf8bcb.jpg       120945     116149       4796 https://dl.kraken.io/api/6a/a7/3d/ae1db847a23ec3a2d40762ae25/4fe1a0fc-22ff-4130-a3ec-a74c19cf8bcb.jpg
   True db080bcc-6d81-451a-a867-cb9f96399599.jpg        73536      62889      10647 https://dl.kraken.io/api/ea/40/27/9921d5832085f02148640b327c/db080bcc-6d81-451a-a867-cb9f96399599.jpg
   True 145e00ee-22d3-4226-b5ca-4ab821230f60.jpg       141438     137558       3880 https://dl.kraken.io/api/42/10/15/da20e1bd42262d4ab01e9a0ba4/145e00ee-22d3-4226-b5ca-4ab821230f60.jpg
```
**Upload images to Azure Blob storage**
```powershell
$files = Get-ChildItem 'C:\Users\Kevin\Pictures\Krakentest' -Filter *.png
$result = Optimize-ImageToAzure -FilePath $files.FullName -Key $key -Secret $secret -Wait $true -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -AzurePath 'powershell/' 
$result | Format-Table
```
```
Success FileName                OriginalSize KrakedSize SavedBytes KrakedUrl                                                                    
------- --------                ------------ ---------- ---------- ---------                                                                    
   True app-insights.png               74185      60324      13861 https://seamist.blob.core.windows.net/test/powershell/app-insights.png       
   True azureportalsettings.png        20231      17282       2949 https://seamist.blob.core.windows.net/test/powershell/azureportalsettings.png
   True DevSlice.png                   22081      16995       5086 https://seamist.blob.core.windows.net/test/powershell/DevSlice.png           
   True krakenio.png                   11188       9764       1424 https://seamist.blob.core.windows.net/test/powershell/krakenio.png 
```
**Upload public images to Azure Blob storage**
```powershell
$files = @('http://compass.xbox.com/assets/67/2e/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg')
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -AzurePath 'powershell/' 
$result | Format-Table
```
```
Success FileName                                 OriginalSize KrakedSize SavedBytes KrakedUrl                                                                                     
------- --------                                 ------------ ---------- ---------- ---------                                                                                     
   True 672e8768-d75a-4c68-82bd-e9b003e997e3.jpg        56524      51675       4849 https://seamist.blob.core.windows.net/test/powershell/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg

```

## LICENSE - MIT

Copyright (c) 2016 Kevin Bronsdijk - http://devslice.net/

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without
restriction, including without limitation the rights to use,
copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the
Software is furnished to do so, subject to the following
conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.
