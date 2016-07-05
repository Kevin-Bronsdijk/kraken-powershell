###
$key = 'key'
$secret = 'secret'
$azureAccount = 'account'
$azureKey = 'key'
$azureContainer = 'container'
$amazonKey = 'key';
$amazonSecret = 'secret';
$amazonBucket = 'bucket';
$testFiles = 'C:\Users\Kevin\Pictures\Krakentest\*';
$modulepath = 'C:\Users\Kevin\Documents\GitHub\kraken-powershell\output\kraken.powershell.dll'
$urltest1 = 'https://seamist.blob.core.windows.net/test/powershell/app-insights.png'
###

# Import 
Import-Module $modulepath 

###

Write-Output "#########################"
Write-Output "Test: Upload + Callback, No callback url"

$files = Get-ChildItem $testFiles -Include *.png
Try
{
    Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $false -Lossy $true 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*CallBackUrl can't be empty when Wait is false*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: Url + Callback, No callback url"

$files = @($urltest1)

Try
{
    Optimize-ImageUrl -FileUrl $files -Key $key -Secret $secret -Wait $false -Lossy $true

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*CallBackUrl can't be empty when Wait is false*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: No Files -FilePath"

$files = Get-ChildItem $testFiles -Include *.jpg
Try
{
    Optimize-Image -FilePath $files.FullName -Key $key -Secret $secret -Wait $true -Lossy $true 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Cannot bind argument to parameter 'FilePath' because it is null*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: No Url's"

$files = @('')

Try
{
    Optimize-ImageUrl -FileUrl $files -Key $key -Secret $secret -Wait $true -Lossy $true

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Cannot bind argument to parameter 'FileUrl' because it is an empty string*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: Azure No FilePath and No OptimizeImageItems"

Try
{
    $files = Get-ChildItem $testFiles -Filter *.png
    Optimize-ImageToAzure -Key $key -Secret $secret -Wait $true `
     -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -AzurePath 'powershell/' 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Use -FilePath or -OptimizeImageItems*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: Azure No FileUrl and No OptimizeImageItems"

Try
{
    $files = @('https://seamist.blob.core.windows.net/test/powershell/app-insights.png')
    Optimize-ImageUrlToAzure -Key $key -Secret $secret -Wait $true `
     -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer -AzurePath 'powershell' 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Use -FileUrl or -OptimizeImageItems*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: KeepPath and AzurePath"

Try
{
    $files = @('https://seamist.blob.core.windows.net/test/powershell/app-insights.png')
    Optimize-ImageUrlToAzure -Key $key -Secret $secret -Wait $true `
     -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer  $azureContainer `
     -AzurePath 'powershell' -KeepPath $true 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Can't use -KeepPath and -AzurePath at the same time*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}



Write-Output "#########################"
Write-Output "Test: Amazon No FilePath and No OptimizeImageItems"

Try
{
    $files = Get-ChildItem $testFiles -Filter *.png
    Optimize-ImageToS3 -Key $key -Secret $secret -Wait $true `
     -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -S3Path 'powershell/' 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Use -FilePath or -OptimizeImageItems*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: Azure No FileUrl and No OptimizeImageItems"

Try
{
    $files = @('https://seamist.blob.core.windows.net/test/powershell/app-insights.png')
    Optimize-ImageUrlToS3 -Key $key -Secret $secret -Wait $true `
     -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -S3Path 'powershell/' 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Use -FileUrl or -OptimizeImageItems*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}

Write-Output "#########################"
Write-Output "Test: KeepPath and S3Path"

Try
{
    $files = @('https://seamist.blob.core.windows.net/test/powershell/app-insights.png')
    Optimize-ImageUrlToS3 -Key $key -Secret $secret -Wait $true `
     -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket `
     -S3Path 'powershell' -KeepPath $true 

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Can't use -KeepPath and -S3Path at the same time*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}


Write-Output "#########################"
Write-Output "Azure Test: (-Headers, -Metadata -AzurePath) when using -OptimizeImageItems"

Try
{
    $urls = @('https://kraken.io/assets/images/kraken-logotype.png')
    $urlItems = $urls |  `
     foreach-object { 
         $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
         $item.Path = $_.ToString()
         $item
     }
    Optimize-ImageUrlToAzure -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
     -AzureAccount $azureAccount -AzureKey $azureKey -AzureContainer $azureContainer -AzurePath 'hello/'

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Can't use shared properties (-Headers, -Metadata -AzurePath) when using -OptimizeImageItems*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}


Write-Output "#########################"
Write-Output "S3 Test: (-Headers, -Metadata -S3Path) when using -OptimizeImageItems"

Try
{
    $urls = @('https://kraken.io/assets/images/kraken-logotype.png')
    $urlItems = $urls |  `
     foreach-object { 
         $item = New-Object -TypeName Kraken.Powershell.OptimizeImageItem
         $item.Path = $_.ToString()
         $item
     }
    Optimize-ImageUrlToS3 -OptimizeImageItems $urlItems -Key $key -Secret $secret -Wait $true `
     -AmazonKey $amazonKey -AmazonSecret $amazonSecret -AmazonBucket $amazonBucket -S3Path 'hello/'

    Write-Output "Failed, no Exception"
}
Catch
{
    if ($_.Exception.Message -like "*Can't use shared properties (-Headers, -Metadata -S3Path) when using -OptimizeImageItems*")
    {
        Write-Output "Success"
    }
    else
    {
        Write-Output "Failed"
        $_.Exception.Message
    }
}