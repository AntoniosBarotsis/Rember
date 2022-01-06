# Rember

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/rember)

Rember is a command line tool that allows you to easily run builds and tests automatically before
committing/pushing code and waiting for it to break the pipeline 15 minutes later.

Usage:
- rember init: Initializes a pre commit and push hook that builds and tests (more flexibility will be added later)
- rember forgor: Removes said hooks.

This is VERY early in development. I'll be adding a few more features and a lot more flexibility
to this. Eventually I'll try setting up a CD pipeline to push this to things like Chocolatey and or Homebrew.

To install this right now, assuming you have the required dotnet stuff, simply run the `RefreshPackage` ps script.