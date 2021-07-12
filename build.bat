dotnet restore
dotnet build ./src/BackendChallenge --no-restore
dotnet test ./tests/BackendChallenge.Tests --no-build
