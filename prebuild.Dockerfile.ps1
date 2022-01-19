param(
[String]$Config = 'DEBUG',
[String]$RuntimeIdentifier = 'linux-x64',
[String]$tag = 'auth:prebuild',
[String]$PathToRepository = './'
)
$Publish = "$($PathToRepository)app/publish/"
dotnet publish "$($PathToRepository)Conduit.Auth.WebApi" -c $CONFIG -o $Publish -r $RuntimeIdentifier
docker build -t $tag -f "$($PathToRepository)prebuild.Dockerfile" $PathToRepository