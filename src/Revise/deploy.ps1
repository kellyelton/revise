$nugetRelativePath = "packages/NuGet.CommandLine.2.2.0/tools/NuGet.exe"
$projFile = "Revise.csproj"
$buildTarget = "Release"
try
{
	## Working Directory Stuff
	$Invocation = (Get-Variable MyInvocation -Scope 0).Value
	$wd = New-Object System.Uri(Split-Path $Invocation.MyCommand.Path)
	Set-Location $wd.LocalPath

	##Nuget Path
	$nugetPath = New-Object System.Uri($wd,$nugetRelativePath)

	$packExpression = $nugetPath.LocalPath + " pack " + $projFile + " -Build -Prop Configuration=" + $buildTarget
	##Build & pack the project
	Write-Output "Building & Packing Project"
	Invoke-Expression $packExpression
	if($LASTEXITCODE -ne 0){
		Write-Error "Build Failed."
		exit 1
	}
	else{
		Write-Output "Build Succeeded"
	}

	#Deploy package
	Write-Output "Deploying..."
	Move-Item *.nupkg "\\fileserver.efile.aatrix.lan\DATA (E)\Distribution\LibFeed" -force
	Write-Output "Deployed"
	exit 0
}
catch
{
	Write-Error $Error[0] "Failed to deploy"
	exit 1
}