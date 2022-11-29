dotnet build --configuration Release

New-Item temp -Type Directory -Force

cp .\bin\Release\net472\DspILSAnalyzer.dll temp
cp .\bin\Release\net472\DspILSAnalyzer.pdb temp
cp .\icon.png temp
cp .\manifest.json temp
cp .\README.md temp


Remove-Item .\DspILSAnalyzer.zip -Force
cd temp

Compress-Archive -Path .\*.* DspILSAnalyzer.zip

mv .\DspILSAnalyzer.zip ..\

cd ..

Remove-Item -Recurse -Force temp