PowerShell module for Kraken.io
=============
[![Build status](https://ci.appveyor.com/api/projects/status/u6u2a8i6h313x0mu?svg=true)](https://ci.appveyor.com/project/Kevin-Bronsdijk/kraken-powershell)

This PowerShell Module can be used to optimize images directly from within the PowerShell command prompt.

The data source can be a local drive or images already publicly available on the internet. Storing data within Azure Blob storage is supported for direct uploads and existing images access by the Url.

This Module is based on SeaMist, my .Net based API wrapper for the kraken.io REST API. Only a limited subset of features are supported. For more information visit https://github.com/Kevin-Bronsdijk/SeaMist or consult the official kraken.io documentation.

## Getting Started

Download the PowerShell Binary Module (merged assembly) from the folder named module (kraken.powershell.zip) or just compile the project on your own.

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
Success FileName                OriginalSize KrakedSize SavedBytes KrakedUrl                                                                            Stat
                                                                                                                                                        usCo
                                                                                                                                                          de
------- --------                ------------ ---------- ---------- ---------                                                                            ----
   True app-insights.png               74185      60324      13861 https://dl.kraken.io/api/ae/0f/77/faf9349157f9c980caac396fdb/app-insights.png         200
   True azureportalsettings.png        20231      17282       2949 https://dl.kraken.io/api/10/90/71/83f8100a08cb444d3ff2e22904/azureportalsettings.png  200
   True DevSlice.png                   22081      16995       5086 https://dl.kraken.io/api/fb/6b/f5/f6f6adf482747affc7e301a35a/DevSlice.png             200
   True krakenio.png                   11188       9764       1424 https://dl.kraken.io/api/bd/2c/c4/2e833c1cdbdc0f47b4fe107db9/krakenio.png             200

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
Success Id                               StatusCode
------- --                               ----------
   True 772e0b465aa847d687b03668978d9146        200
   True 0d0e009badde15adb211aca118ad88ad        200
   True 15f282e4ae8b8542fffc486a78f0f1fa        200
   True a95605ab59df2ca7ab4a51d7e82f85f9        200
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
Success FileName                OriginalSize KrakedSize SavedBytes KrakedUrl                                                                     StatusCode
------- --------                ------------ ---------- ---------- ---------                                                                     ----------
   True app-insights.png               74185      60324      13861 https://seamist.blob.core.windows.net/test/powershell/app-insights.png               200
   True azureportalsettings.png        20231      17282       2949 https://seamist.blob.core.windows.net/test/powershell/azureportalsettings.png        200
   True DevSlice.png                   22081      16995       5086 https://seamist.blob.core.windows.net/test/powershell/DevSlice.png                   200
   True krakenio.png                   11188       9764       1424 https://seamist.blob.core.windows.net/test/powershell/krakenio.png                   200
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
**Maintain the uri structure when uploading public images to Azure Blob storage**
```powershell
$files = @('http://compass.xbox.com/assets/67/2e/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg')
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
-AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -KeepPath $true
$result.KrakedUrl
```
```
https://seamist.blob.core.windows.net/test/assets/67/2e/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg
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
