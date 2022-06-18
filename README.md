# Rember

[![Nuget Version](https://img.shields.io/nuget/v/Rember?color=brightgreen)](https://www.nuget.org/packages/Rember/) [![Chocolatey Version](https://img.shields.io/chocolatey/v/rember?color=brightgreen)](https://community.chocolatey.org/packages/Rember) ![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/AntoniosBarotsis/rember) ![GitHub Workflow Status](https://img.shields.io/github/workflow/status/AntoniosBarotsis/Rember/Publish?label=publish)

Rember is a command line tool that reminds and allows you to easily run builds, tests and custom tasks automatically
before
pushing code and waiting for it to break the pipeline 15 minutes later.
You can just init it once and forget about it!

## About

Rember is a command line tool that can run any task you want before pushing your code so you never push broken or unlinted code ever again.
You can set it up directly from your command line for a more simplistic set up, Rember
will automatically detect your language and build tool and generate a build and test
task unless specified otherwise. For custom tasks a config file must be used.

## Installation

- With [choco](https://community.chocolatey.org/packages/Rember)
  ```sh
  choco install rember
  ```

- With [Nuget](https://www.nuget.org/packages/Rember/)
  ```sh
  dotnet tool install --global Rember
  ```

Check the latest version just in case.

### Installation for Developers

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

There is a sample in the repository which should be self-explanatory:

```yml
tasks:
- name: Build
  command: dotnet build
  outputEnabled: false  # Default is true
  alwaysRun: true       # Default is false
```

You can also read [the wiki](https://github.com/AntoniosBarotsis/Rember/wiki) for a more detailed explanation.

In order to use it run:

```sh
rember init -f rember.yml
```

## Contributing

Take a look at the [`CONTRIBUTING.md`](CONTRIBUTING.md) file.