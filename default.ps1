properties {
	$slnFile = "src\FluentIBatis.sln"
}

task default -depends Deploy

task Deploy -depends Build { 
	#Deployment code goes here
}

task Build -depends Clean{
	[void][System.Reflection.Assembly]::Load('Microsoft.Build.Utilities.v3.5, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
	$msbuild = [Microsoft.Build.Utilities.ToolLocationHelper]::GetPathToDotNetFrameworkFile("msbuild.exe", "VersionLatest")
	&$msbuild $slnFile
}

task Clean { 
	#Clean up the non-necessary junk here
}