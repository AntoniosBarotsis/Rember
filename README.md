# Rember

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/rember) ![Chocolatey Version (including pre-releases)](https://img.shields.io/chocolatey/v/rember?include_prereleases)

Rember is a command line tool that allows you to easily run builds and tests automatically before
committing/pushing code and waiting for it to break the pipeline 15 minutes later. It detects
your used language and build tool automatically, currently (hopefully) supports:

- Dotnet
- Maven
- Gradle
- NPM
- Yarn

## Installation

- With [choco](https://community.chocolatey.org/packages/Rember)
  ```sh
  choco install rember --pre 
  ```

- With [Nuget](https://www.nuget.org/packages/Rember/)
  ```sh
  dotnet tool install --global Rember --version 0.0.1-beta
  ```

Check the latest version just in case. A note for the versions, you may have noticed that I used
4 version digits, that is because I am dumb and couldn't push them first try to their respective
platforms. The first 3 numbers are the actual version (0.0.1.1 is the same code as 0.0.1.3). This
will eventually go away as I now *believe* I figured out deployment and we move to newer versions.

It is worth noting that the choco package is currently about 1000 times larger than the nuget one
because it needed things budled with it to run so it goes without saying, if you can use the
latter, do it. If you can't then don't worry too much about it cause it's still only 10 or so mb.


## Usage

- rember init: Initializes a pre commit and push hook that builds and tests (more flexibility will be added later)
- rember forgor: Removes said hooks.

This is VERY early in development. I'll be adding a few more features and a lot more flexibility
to this. Eventually I'll try setting up a CD pipeline to push this to things like Chocolatey and or Homebrew.

To install this right now, assuming you have the required dotnet stuff, simply run the `RefreshPackage` ps script.

To generate executables for both windows and linux run `CreateExecutables.ps1` (or just the
linux half if you can't run powershell).