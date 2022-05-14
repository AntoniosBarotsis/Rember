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

Rember is a command line tool that can run any task you want before pushing your code so you never push broken or unlinted code ever again.

## Installation

- With [choco](https://community.chocolatey.org/packages/Rember)
  ```sh
  choco install rember --pre
  ```

- With [Nuget](https://www.nuget.org/packages/Rember/)
  ```sh
  dotnet tool install --global Rember --version 0.0.4-rc2
  ```

Check the latest version just in case.

To install this right now for developing, assuming you have the required dotnet stuff, simply run the `RefreshPackage`
ps script.

## Usage

Running `rember init` from the command line automatically detects your used language and build tool, currently (hopefully) supports:

- Dotnet
- Maven
- Gradle
- NPM
- Yarn
- SBT

Use `rember -h` and `rember init -h` for more information.

### The `rember.yml` config file

The recommended way to use this tool is by creating a yaml file. This means that you have to manually add all
steps but it also means that you can add any steps you want. This makes it much easier when working in a team.

There is a sample in the repository which should be self-explanatory. 

```yml
buildToolName: Dotnet
tasks:
- name: Build
  command: dotnet build
  outputEnabled: false  # Default is true, this shows you the output you'd normally see from the command
  alwaysRun: true       # Default is false, enabling it means that you will not be asked before running it
```

In order to use it run:

```sh
rember init -f rember.yml
```

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
- v0.0.4-rc2
    - Major refactor
    - Removed the pre-commit option for now at least, default and only option are push hooks
- v0.0.4-rc3 [unreleased]
    - Improved readme a bit
    - Set `AlwaysRun` to `false` by default
    - 🚧 Waiting for feedback to address 🚧 

Feel free to open issues with feature requests, ideas or bug reports.