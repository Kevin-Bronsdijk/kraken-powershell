###
$key = ''
$secret = ''
$azureAccount = ''
$azureKey = ''
$azureContainer = ''
$amazonKey = '';
$amazonSecret = '';
$amazonBucket = '';
$testimagepath = 'C:\Users\Kevin\Pictures\Krakentest';
$modulepath = 'C:\Users\Kevin\Documents\GitHub\kraken-powershell\output\kraken.powershell.dll'
$callbackurl = 'http://devslice.net/callback'
$urltest1 = 'https://seamist.blob.core.windows.net/test/powershell/app-insights.png'
$urltest2 = 'https://kraken.io/assets/images/kraken-logotype.png'
###

# Import 
Import-Module $modulepath

### Shared/simple requests

# 1) Upload files
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $true -Lossy $true
$result | Format-Table

# Reporting
$savedBytes = ($result | Measure-Object SavedBytes -Sum).Sum
$savedBytes

# 2) Upload files with callback
$result = Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $false -Lossy $true `
-CallBackUrl $callbackurl 
$result | Format-Table

# 3) Public images
$files = @($urltest1)
$result = Optimize-ImageUrl -FileUrl $files -Key $key -Secret $secret -Wait $true -Lossy $true
$result | Format-Table

# 4) Upload images to Azure Blob storage
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToAzure -FilePath $files.FullName -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer -AzurePath 'powershell/' 
$result | Format-Table

# 4B) Upload images to Azure Blob storage, no path
$files = Get-ChildItem $testimagepath  -Filter *.png
$result = Optimize-ImageToAzure -FilePath $files.FullName -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer
$result | Format-Table

# 5) Upload public images to Azure Blob storage
$files = @($urltest1)
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer -AzurePath 'powershell/' 
$result | Format-Table

# 5B) Upload public images to Azure Blob storage, no path
$files = @($urltest1)
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer 
$result | Format-Table

# 6) Maintain the uri when uploading public images to Azure Blob storage 
$files = @($urltest2)
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer -KeepPath $true 
$result.KrakedUrl
# https://seamist.blob.core.windows.net/test/assets/images/kraken-logotype.png

# 7) Blob to Blob, ignore source container name if the same as the destination
$files = @($urltest1)
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -KeepPath $true
$result.KrakedUrl
#https://seamist.blob.core.windows.net/test/powershell/app-insights.png
#NOT https://seamist.blob.core.windows.net/test/test/powershell/app-insights.png

# 8) Upload images to Amazon S3
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToS3 -FilePath $files.FullName -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -S3Path 'powershell/' 
$result | Format-Table

# 8B) Upload images to Amazon S3, no path
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToS3 -FilePath $files.FullName -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket  $amazonBucket
$result | Format-Table

# 9) Upload public images to Amazon S3
$files = @($urltest1)
$result = Optimize-ImageUrlToS3 -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -S3Path 'powershell/' 
$result | Format-Table

# 9B) Upload public images to Amazon S3, no path
$files = @($urltest1)
$result = Optimize-ImageUrlToS3 -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket 
$result | Format-Table

# 10) Maintain the uri when uploading public images to Amazon S3
$files = @($urltest2)
$result = Optimize-ImageUrlToS3 -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -KeepPath $true
$result.KrakedUrl

# 11) Url include shared headers and metadata for Azure Blob storage 
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$metadata = @{}
$metadata.Add('test1','value1')
$files = @($urltest1)
$result = Optimize-ImageUrlToAzure -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer -KeepPath $true `
 -Headers $headers -Metadata $metadata
$result | Format-Table

# 11B) upload include shared headers and metadata for Azure Blob storage 
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$metadata = @{}
$metadata.Add('test1','value1')
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToAzure -FilePath $files.FullName -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer `
 -Headers $headers -Metadata $metadata
$result | Format-Table

# 12)  Url include shared headers and metadata for Amazon S3
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$metadata = @{}
$metadata.Add('test1','value1')
$files = @($urltest2)
$result = Optimize-ImageUrlToS3 -FileUrl $files -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -KeepPath $true `
 -Headers $headers -Metadata $metadata
$result | Format-Table

# 12B)  upload include shared headers and metadata for Amazon S3
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$metadata = @{}
$metadata.Add('test1','value1')
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToS3 -FilePath $files.FullName -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket `
 -Headers $headers -Metadata $metadata
$result | Format-Table

### Non-shared OptimizeImageItems

# 1) Upload images to Azure Blob storage
$items = Get-ChildItem $testimagepath -Filter *.png | `
 foreach-object { 
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.FullName
     $item
 }
$result = Optimize-ImageToAzure -OptimizeImageItems  $items -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer
$result | Format-Table

# 2) Upload public images to Azure Blob storage
$urls = @($urltest1)
$urlItems = $urls |  `
 foreach-object { 
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.ToString()
     $item
 }
$result = Optimize-ImageUrlToAzure -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer
$result | Format-Table

# 3) Maintain the uri when uploading public images to Azure Blob storage 
$urls = @($urltest2)
$urlItems = $urls |  `
 foreach-object { 
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.ToString()
     $item
 }
$result = Optimize-ImageUrlToAzure -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer -KeepPath $true 
$result.KrakedUrl
# https://seamist.blob.core.windows.net/test/assets/images/kraken-logotype.png

# 4) Upload images to Amazon S3
$items = Get-ChildItem $testimagepath -Filter *.png | `
 foreach-object { 
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.FullName
     $item
 }
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToS3 -OptimizeImageItems $items -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket
$result | Format-Table

# 5) Upload public images to Amazon S3
$urls = @($urltest1)
$urlItems = $urls |  `
 foreach-object { 
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.ToString()
     $item
 }
$result = Optimize-ImageUrlToS3 -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket
$result | Format-Table

# 6) Maintain the uri when uploading public images to Amazon S3
$urls = @($urltest2)
$urlItems = $urls |  `
 foreach-object { 
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.ToString()
     $item
 }
$result = Optimize-ImageUrlToS3 -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -KeepPath $true
$result.KrakedUrl

# 7) Upload images to Azure Blob storage + Metadata + Headers + storage path
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$items = Get-ChildItem $testimagepath -Filter *.png | `
 foreach-object { 
     $metadata = @{}
     $metadata.Add('name',$_.FullName)
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.FullName
     $item.Headers = $headers
     $item.Metadata = $metadata
     $item.ExternalStoragePath = 'hello/'
     $item
 }
$result = Optimize-ImageToAzure -OptimizeImageItems $items -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer
$result | Format-Table

# 8) Upload public images to Azure Blob storage + Metadata + Headers + storage path
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$urls = @($urltest1)
$urlItems = $urls |  `
 foreach-object { 
     $metadata = @{}
     $metadata.Add('name',$_.ToString())
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.ToString()
     $item.Headers = $headers
     $item.Metadata = $metadata
     $item.ExternalStoragePath = 'hello/'
     $item
 }
$result = Optimize-ImageUrlToAzure -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
 -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer
$result | Format-Table

# 9) Upload images to Amazon S3 + Metadata + Headers + storage path
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$items = Get-ChildItem $testimagepath -Filter *.png | `
 foreach-object { 
     $metadata = @{}
     $metadata.Add('name',$_.FullName)
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.FullName
     $item.Headers = $headers
     $item.Metadata = $metadata
     $item.ExternalStoragePath = 'hello/'
     $item
 }
$files = Get-ChildItem $testimagepath -Filter *.png
$result = Optimize-ImageToS3 -OptimizeImageItems $items -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket
$result | Format-Table

# 10) Upload public images to Amazon S3 + Metadata + Headers + storage path
$headers = @{}
$headers.Add('Cache-Control','max-age=1234')
$urls = @($urltest1)
$urlItems = $urls |  `
 foreach-object { 
     $metadata = @{}
     $metadata.Add('name',$_.ToString())
     $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
     $item.Path = $_.ToString()
     $item.Headers = $headers
     $item.Metadata = $metadata
     $item.ExternalStoragePath = 'hello/'
     $item
 }
$result = Optimize-ImageUrlToS3 -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
 -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket
$result | Format-Table