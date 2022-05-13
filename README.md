# Rember

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/rember) ![Chocolatey Version (including pre-releases)](https://img.shields.io/chocolatey/v/rember?include_prereleases) ![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/AntoniosBarotsis/rember) [![Publish](https://github.com/AntoniosBarotsis/Rember/actions/workflows/publish.yml/badge.svg)](https://github.com/AntoniosBarotsis/Rember/actions/workflows/publish.yml)

Rember is a command line tool that reminds and allows you to easily run builds, tests and custom tasks automatically
before
pushing code and waiting for it to break the pipeline 15 minutes later.

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

## Installation

- With [choco](https://community.chocolatey.org/packages/Rember)
  ```sh
  choco install rember --pre
  ```

- With [Nuget](https://www.nuget.org/packages/Rember/)
  ```sh
  dotnet tool install --global Rember --version 0.0.4-rc1
  ```

Check the latest version just in case.

To install this right now for developing, assuming you have the required dotnet stuff, simply run the `RefreshPackage`
ps script.

## Usage

The list of commands is getting rather lengthy so run `rember -h` instead :)

TLDR: `rember init`

### The `rember.yml` config file

The recommended way to use this tool is by creating a yaml file.

There is a sample in the repository which should be self-explanatory. 
In order to use it run:

```sh
rember init -f rember.yml
```

You should be able to add any commands you want.

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
- v0.0.4-rc1
    - Major refactor
    - Removed the pre-commit option for now at least, default and only option are push hooks

Will move to using a dev branch like a normal person when I leave beta versions :)

Feel free to open issues with feature requests, ideas or bug reports.