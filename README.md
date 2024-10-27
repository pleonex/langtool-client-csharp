# LanguageTool .NET client

<!-- markdownlint-disable MD033 -->
<p align="center">
  <a href="https://github.com/pleonex/langtool-client-csharp/actions/workflows/build-and-release.yml">
    <img alt="Build and release" src="https://github.com/pleonex/langtool-client-csharp/actions/workflows/build-and-release.yml/badge.svg" />
  </a>
  &nbsp;
  <a href="https://choosealicense.com/licenses/mit/">
    <img alt="MIT License" src="https://img.shields.io/badge/license-MIT-blue.svg?style=flat" />
  </a>
  &nbsp;
</p>

.NET (C#) library to run checks using [LanguageTool](https://languagetool.org)
server.

> [!WARNING]  
> This is a personal project with **no support**. The project may not have an
> active development. Don't expect new features, fixes (including security
> fixes). I don't recommend using it for production environments. Feel free to
> fork and adapt. _Small_ contributions are welcome.

## Usage

This project provides the following libraries as NuGet packages (via nuget.org).
The libraries support the latest version of .NET and its LTS.

- [![PleOps.LanguageTool.Client](https://img.shields.io/nuget/v/PleOps.LanguageTool.Client?label=PleOps.LanguageTool.Client&logo=nuget)](https://www.nuget.org/packages/PleOps.LanguageTool.Client):
  client library for LanguageTool server.

**Preview releases** can be found in this
[Azure DevOps NuGet repository](https://dev.azure.com/pleonex/Pleosoft/_artifacts/feed/Pleosoft-Preview).
To use a preview release, create a file `nuget.config` in the same directory of
your solution file (.sln) with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear/>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="PleOps-Preview" value="https://pkgs.dev.azure.com/pleonex/Pleosoft/_packaging/Pleosoft-Preview/nuget/v3/index.json" />
  </packageSources>
  <packageSourceMapping>
    <packageSource key="nuget.org">
      <package pattern="*" />
    </packageSource>
    <packageSource key="PleOps-Preview">
      <package pattern="PleOps.LanguageTool.Client" />
    </packageSource>
  </packageSourceMapping>
</configuration>
```

Then restore / install as usual via Visual Studio, Rider or command-line. You
may need to restart Visual Studio for the changes to apply.

## Build

The project requires .NET 8.0 SDK to build.

To build, test and generate artifacts run:

```sh
# Build and run tests
dotnet run --project build/orchestrator

# (Optional) Create bundles (nuget, zips, docs)
dotnet run --project build/orchestrator -- --target=Bundle
```

## Release

Create a new GitHub release with a tag `v{Version}` (e.g. `v2.4`) and that's it!
This triggers a pipeline that builds and deploy the project.
