PowerShell module for Kraken.io
=============

## Getting Started
A merge assembly PowerShell Binary Module will be provided soon. For now, just compile the project, Import-Module kraken.powershell.dll and make sure all supporting assemblies are available.

## Code Samples

```powershell
# Import 

Import-Module 'C:\path\..\kraken.powershell.dll' -Verbose
```

```powershell
# 1) Upload files

$files = Get-ChildItem 'C:\Users\Kevin\Pictures\Krakentest' -Filter *.png

$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $true  

$result | Format-Table

# Reporting

$savedBytes = ($result | Measure-Object SavedBytes -Sum).Sum

$savedBytes
```

```powershell

 # 2) Upload files with callback

$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $false  -CallBackUrl 'http://devslice.net/callback'

$result | Format-Table

```

```powershell
 # 3) Public images with xbox.com

$files = @('http://compass.xbox.com/assets/67/2e/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg',
            'http://compass.xbox.com/assets/4f/e1/4fe1a0fc-22ff-4130-a3ec-a74c19cf8bcb.jpg',
            'http://compass.xbox.com/assets/db/08/db080bcc-6d81-451a-a867-cb9f96399599.jpg',
            'http://compass.xbox.com/assets/14/5e/145e00ee-22d3-4226-b5ca-4ab821230f60.jpg')


$result = Optimize-ImageUrl -FileUrl $files -Key $key -Secret $secret -Wait $true  

$result | Format-Table
```

```powershell
# 4) Upload images to Azure Blob storage

$files = Get-ChildItem 'C:\Users\Kevin\Pictures\Krakentest' -Filter *.png

$result = Optimize-ImageToAzure -FilePath $files.FullName -Key $key -Secret $secret -Wait $true -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -AzurePath 'powershell/' 

$result | Format-Table
```

```powershell
# 5) Upload public images to Azure Blob storage

$files = @('http://compass.xbox.com/assets/67/2e/672e8768-d75a-4c68-82bd-e9b003e997e3.jpg')

$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -AzurePath 'powershell/' 

$result | Format-Table
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