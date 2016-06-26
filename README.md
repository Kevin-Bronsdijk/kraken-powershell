PowerShell module for Kraken.io
=============
[![Build status](https://ci.appveyor.com/api/projects/status/u6u2a8i6h313x0mu?svg=true)](https://ci.appveyor.com/project/Kevin-Bronsdijk/kraken-powershell)

***
### Advanced image optimization using Powershell and Kraken.io

The kraken-powershell module is a wrapper around [kraken-net](https://github.com/Kevin-Bronsdijk/kraken-net) and simplifies optimizing images from using a command prompt

kraken-powershell focuses on batch processing without the need of writing a full application. Due to this, only a subset of what kraken-net offers is available. This can change in the future however depends on your enhancement requests. 

For more about information kraken-net can be found [here](https://github.com/Kevin-Bronsdijk/kraken-net) or consult the official [kraken.io](https://kraken.io) documentation.

***
* [Getting Started](#getting-started)
* [Installation](#installation)
* [Upload files](#upload-files)
* [Reporting](#reporting)
* [Upload files with callback](#upload-files-with-callback)
* [Public images](#public-images)
* [Azure Blob Storage](#azure-blob-storage)
  * [Upload images](#upload-images)
  * [Public images](#public-images)
  * [Maintain folder structure](#maintain-folder-structure)

## Getting Started

First you need to sign up for the [Kraken API](http://kraken.io/plans/) and obtain your unique **API Key** and **API Secret**. You will find both under [API Credentials](http://kraken.io/account/api-credentials). Once you have set up your account, you can start using the kraken-powershell module.

## Installation

Download the PowerShell Binary Module (kraken.powershell.dll and the supporting assemblies) from the folder named module [module](https://github.com/Kevin-Bronsdijk/kraken-powershell/tree/master/module)(kraken.powershell.zip).

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

### Upload files

Uploading and optimizing multiple files at the same time is simple. The progress indicator will keep you informed regarding the optimization progress.

You can download the compressed files on your own or specify the location using the `-LocalStoragePath`

```powershell
$files = Get-ChildItem 'C:\path\..\myfolder' -Filter *.png, *.jpg, *.jpeg
$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $true  
$result | Format-Table
```
All individual requests made to kraken will be listed as shown below:
```
Success FileName                OriginalSize KrakedSize SavedBytes KrakedUrl                                            StatusCode
------- --------                ------------ ---------- ---------- ---------                                            ----
   True image1.png               74185      60324      13861 https://dl.kraken.io/api/9a7c7c7......79e4474/image1.png   200
   True image2.png               20231      17282       2949 https://dl.kraken.io/api/772e0b4......a95605a/image2.png   200
   True image3.png               22081      16995       5086 https://dl.kraken.io/api/9a7c7c7......72e0b46/image3.png   200
   True image4.png               11188       9764       1424 https://dl.kraken.io/api/a95605a......79e4473/image4.png   200

```

### Reporting

It’s very easy to generate simple repots based on the data exposed by Kraken.io. The sample below creates a sum of all saved bytes from one batch.

```powershell
$savedBytes = ($result | Measure-Object SavedBytes -Sum).Sum
$savedBytes
```
The result:
```
23320
```
### Upload files with callback

The callback option can be used when you have a separate process responsible for dealing with the compressed images. Just provide the Url to receive callback notifications about completed compression requests.

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

### Public images

It’s not required to upload your images if they are already available online. Just use the Url instead. 

```powershell
$files = @('http://compass.xbox.com/assets/image1.jpg',
           'http://compass.xbox.com/assets/image2.jpg',
           'http://compass.xbox.com/assets/image3.jpg',
           'http://compass.xbox.com/assets/image4.jpg')
            
$result = Optimize-ImageUrl -FileUrl $files -Key $key -Secret $secret -Wait $true  
$result | Format-Table
```

```
Success FileName        OriginalSize KrakedSize SavedBytes KrakedUrl                                         
------- --------        ------------ ---------- ---------- ---------                                                                                            
   True image1.jpg        56524        51675         4849       https://dl.kraken.io/api/9a7......474/image1.jpg
   True image2.jpg       120945       116149         4796       https://dl.kraken.io/api/ae1......ec3/image2.jpg
   True image3.jpg        73536        62889        10647       https://dl.kraken.io/api/992......5f0/image3.jpg
   True image4.jpg       141438       137558         3880       https://dl.kraken.io/api/da2......0e1/image4.jpg
```

## Azure Blob Storage

it’s possible to give kraken.io the instructions to store the compressed images directly within your Azure Blob Storage container. This will be performed directly and eliminates the need to download the images to your local system first.

### Upload images

```powershell
$files = Get-ChildItem 'C:\path\..\myfolder' -Filter *.png, *.jpg, *.jpeg
$result = Optimize-ImageToAzure -FilePath $files.FullName -Key $key -Secret $secret -Wait $true -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer 'test' -AzurePath 'powershell/' 
$result | Format-Table
```
```
Success FileName      OriginalSize KrakedSize SavedBytes KrakedUrl                                                      StatusCode
------- --------      ------------ ---------- ---------- ---------                                                      ----------
   True image1.png      74185      60324      13861 https://kraken1.blob.core.windows.net/test/powershell/image1.png    200
   True image2.png      20231      17282       2949 https://kraken1.blob.core.windows.net/test/powershell/image2.png    200
   True image2.png      22081      16995       5086 https://kraken1.blob.core.windows.net/test/powershell/image3.png    200
   True image4.png      11188       9764       1424 https://kraken1.blob.core.windows.net/test/powershell/image4.png    200
```

### Public images

It’s not required to upload your images if they are already available online. Just use the Url instead. 

```powershell
$files = @('http://compass.xbox.com/assets/image1.jpg')

$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer 'test' -AzurePath 'sample2/' 
$result | Format-Table
```
```
Success FileName    OriginalSize  KrakedSize  SavedBytes  KrakedUrl     
------- --------    ------------  ----------  ----------  ---------
True    image1.jpg  56524         51675       4849        https://kraken1.blob.core.windows.net/test/sample2/image1.jpg
```

### Maintain folder structure

If you want the maintain the same folder structure within your Azure Blob Storage, make sure to specify the -KeepPath option (Public images only). 

This option will leave out the source container name when both the source and destination are Azure Blob Storage based. 

```powershell
$files = @('http://compass.xbox.com/assets/67/2e/image1.jpg')

$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
-AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer 'test' -KeepPath $true
$result.KrakedUrl
```
The file was created with a matching folder structure;
```
https://kraken1.blob.core.windows.net/test/assets/67/2e/image1.jpg
```

## LICENSE - MIT

Copyright (c) 2016

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
