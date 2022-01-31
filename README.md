# Rember

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/rember) ![Chocolatey Version (including pre-releases)](https://img.shields.io/chocolatey/v/rember?include_prereleases) ![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/AntoniosBarotsis/rember) ![GitHub commits since latest release (by date including pre-releases)](https://img.shields.io/github/commits-since/AntoniosBarotsis/rember/latest?include_prereleases) ![GitHub contributors](https://img.shields.io/github/contributors/AntoniosBarotsis/rember) ![GitHub issues](https://img.shields.io/github/issues/AntoniosBarotsis/rember) [![Publish](https://github.com/AntoniosBarotsis/Rember/actions/workflows/publish.yml/badge.svg)](https://github.com/AntoniosBarotsis/Rember/actions/workflows/publish.yml)

Rember is a command line tool that reminds and allows you to easily run builds, tests and custom tasks automatically before
committing/pushing code and waiting for it to break the pipeline 15 minutes later. 

**Table of contents**
- [Rember](#rember)
  - [About](#about)
  - [Installation](#installation)
  - [Usage](#usage)
  - [Roadmap](#roadmap)

## About 
It detects your used language and build tool automatically, currently (hopefully) supports:

- Dotnet
- Maven
- Gradle
- NPM
- Yarn
- SBT

Building and Testing with these tools comes supported out of the box with support for creating
custom tasks planned for version 0.0.3.

## Installation

- With [choco](https://community.chocolatey.org/packages/Rember)
  ```sh
  choco install rember --pre
  ```

- With [Nuget](https://www.nuget.org/packages/Rember/)
  ```sh
  dotnet tool install --global Rember --version 0.0.3-beta1
  ```

Check the latest version just in case. A note for the versions, the beta/rc version most likely does not make a difference;
I sometimes have some issues with Choco mostly and I need to push newer versions to fix them but the code itself is the same.

## Usage

The list of commands is getting rather lengthy so run `rember -h` instead :)

This is VERY early in development. I'll be adding a few more features and a lot more flexibility
to this. 

To install this right now for developing, assuming you have the required dotnet stuff, simply run the `RefreshPackage` ps script.

To generate executables for both windows and linux run `CreateExecutables.ps1` (or just the
linux half if you can't run powershell).

## Roadmap

- v0.0.2-beta
  - Remove existing tasks
  - Select if u want build/test at init, ability to add either one later on
  - Ability to define custom tasks to run [a bit buggy, pushed back to  0.0.3]
- v0.0.3-beta
  - Completely remake how file accesses work [delayed to next release]
  - Fix custom task creation
  - Add save and restore
  - Ability to load settings from a yml file

Will move to using a dev branch like a normal person when I leave beta versions :)