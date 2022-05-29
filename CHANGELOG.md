# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

<!-- ## [Unreleased] -->

## [0.0.4] - 2022-05-29
### Changed
- Switched to a full release version

## [0.0.4-rc4] - 2022-05-28
### Changed
- Removed `buildToolName` from the `rember.yml` file

## [0.0.4-rc3] - 2022-05-15
### Added
- Added ability to change your git hooks path
### Changed 
- Set `AlwaysRun` to `false` by default
### Fixed
- Fixed a relative path bug

## [0.0.4-rc2] - 2022-05-13
### Changed 
- Refactored the entire codebase and started using [CliFx](https://github.com/Tyrrrz/CliFx) and [Spectre.Console](https://spectreconsole.net/)
- Removed the pre-commit option for now at least, default and only option are push hooks

## [0.0.3-beta] - 2022-01-31
### Added
- Ability to define custom tasks
- Save and restore
- Ability to load settings from a yml file

## [0.0.2-beta] - 2022-01-14
### Added
- Select if you want a build/test task at init, ability to add either one later on
- Remove existing tasks
