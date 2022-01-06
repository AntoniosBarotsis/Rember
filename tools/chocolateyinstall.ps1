$name = 'Package Name'
$installerType = 'exe'
$url  = 'https://github.com/AntoniosBarotsis/Rember/releases/download/v0.0.1-beta/Rember.exe'
$silentArgs = '/S'

Install-ChocolateyPackage $name $installerType $silentArgs $url