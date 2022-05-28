# Rember

![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/rember) ![Chocolatey Version (including pre-releases)](https://img.shields.io/chocolatey/v/rember?include_prereleases) ![GitHub (Pre-)Release Date](https://img.shields.io/github/release-date-pre/AntoniosBarotsis/rember) [![Publish](https://github.com/AntoniosBarotsis/Rember/actions/workflows/publish.yml/badge.svg)](https://github.com/AntoniosBarotsis/Rember/actions/workflows/publish.yml)

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
  choco install rember --pre
  ```

- With [Nuget](https://www.nuget.org/packages/Rember/)
  ```sh
  dotnet tool install --global Rember --version 0.0.4-rc4
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

There is a sample in the repository which should be self-explanatory. 

```yml
# hookDirectory: .husky
tasks:
- name: Build
  command: dotnet build
  outputEnabled: false  # Default is true
  alwaysRun: true       # Default is false
```

- `hookDirectory`: Optional. The hook directory should only be used if you, for whatever reason, are not using the default
  git hooks location. You can check your current hook path by running `git config core.hooksPath`, if nothing is printed
  then ignore this, otherwise set it to whatever value is printed. Keep in mind that Rember will replace
  any `pre-push` hooks you already have!

- `outputEnabled`: Controlls if you can view the logs that would normally be printed to the console by the specified command.
  Recommended to set to true for debugging purposes.

- `alwaysRun`: This is **required** to be set to true if you are using git clients (like GitKraken). If enabled, the tasks
  will run automatically instead of asking you first. If you are using git commands from the command line, it is recommended that
  you set this to false as it gives you the option to skip tasks that you know will work.

In order to use it run:

```sh
rember init -f rember.yml
```

## Contributing

Take a look at the [`CONTRIBUTING.md`](CONTRIBUTING.md) file.