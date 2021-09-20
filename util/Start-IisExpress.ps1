param (
  [Parameter(Mandatory = $true)]
  [string]$Path
)

$port = [int]([xml](Get-Content $Path\*.[cv][sb]proj)).Project.PropertyGroup.IISExpressSSLPort[0]
$url = "https://localhost:$Port"

$name = Split-Path $Path -Leaf
$physicalPath = Resolve-Path $Path

Write-Warning "$physicalPath at $url"

Push-Location "$Env:ProgramFiles/IIS Express"

# https://stackoverflow.com/a/48632693/54249
./IisExpressAdminCmd setupSslUrl -url:$url -UseSelfSigned | Write-Warning
./appcmd add site /name:"$name" /bindings:https/*:"$port":localhost /physicalPath:"$physicalPath" | Write-Warning
./appcmd list sites | Write-Warning
Start-Process -PassThru ./iisexpress "/site:$name /trace:error" | Out-Null

Pop-Location

return $url
