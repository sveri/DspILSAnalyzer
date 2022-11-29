dotnet build --configuration Release

mkdir temp

cp .\bin\Release\net472\DspILSAnalyzer.dll temp
cp .\bin\Release\net472\DspILSAnalyzer.pdb temp
cp .\icon.png temp
cp .\manifest.json temp
cp .\README.md temp


Remove-Item -Recurse -Force temp