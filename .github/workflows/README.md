# GitHub Actions Workflows

This directory contains GitHub Actions workflows for the Kingdoms Unity project.

## Available Workflows

### 1. `staging-ci.yml` - Staging CI Pipeline
**Triggers:** PRs and pushes to `staging-CI` branch

**Purpose:** Comprehensive CI/CD pipeline for testing builds before merging to main branch.

**What it does:**
- Runs Unity tests (PlayMode and EditMode)
- Builds for multiple platforms (Windows, Linux, macOS, WebGL)
- Performs code quality checks
- Caches Unity installation and library files for faster builds
- Provides detailed summary reports

**Requirements:**
- Unity License secrets must be configured in repository settings
- Required secrets: `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`

### 2. `hello.yml` - Quick Validation
**Triggers:** Manual dispatch, pushes to main/staging-CI, PRs to main

**Purpose:** Quick validation of Unity project structure and basic checks.

**What it does:**
- Validates Unity project structure
- Checks for essential directories and files
- Reports Unity version and project statistics
- Fast execution for basic validation

### 3. `unity-activate.yml` - Unity Activation (One-time Setup)
**Triggers:** Manual dispatch only

**Purpose:** Generate Unity activation file for first-time setup.

**What it does:**
- Requests Unity activation file
- Uploads `.alf` file as artifact for manual license activation
- Only needs to be run once per repository

## Setup Instructions

### First Time Setup
1. Run the "Unity Activation" workflow manually
2. Download the `.alf` file from the workflow artifacts
3. Visit [Unity License Management](https://license.unity3d.com/manual) to generate a license
4. Add the following secrets to your GitHub repository:
   - `UNITY_LICENSE`: Your Unity license content
   - `UNITY_EMAIL`: Email associated with Unity account
   - `UNITY_PASSWORD`: Password for Unity account

### Using the Staging CI Pipeline
1. Create a branch for your changes
2. Make your changes and commit them
3. Create a PR targeting the `staging-CI` branch
4. The CI pipeline will automatically run
5. Review the build results and test results
6. If all checks pass, merge to `staging-CI`

## Build Platforms

The staging CI pipeline builds for the following platforms:
- **Windows 64-bit** (`StandaloneWindows64`)
- **Linux 64-bit** (`StandaloneLinux64`) 
- **macOS** (`StandaloneOSX`)
- **WebGL** (`WebGL`)

## Caching

The workflows use aggressive caching to speed up build times:
- Unity installation cache
- Unity Library folder cache
- Platform-specific caches

## Troubleshooting

### Common Issues

1. **Missing Unity License**
   - Error: "License activation failed"
   - Solution: Ensure Unity license secrets are properly configured

2. **Build Failures**
   - Check the build logs in the GitHub Actions tab
   - Verify all required dependencies are included
   - Check for platform-specific issues

3. **Test Failures** 
   - Review test results in the artifacts
   - Check for missing references or broken scripts
   - Verify test setup in Unity Test Framework

### Getting Help

If you encounter issues with the CI pipeline:
1. Check the workflow run logs for detailed error messages
2. Verify all required secrets are configured
3. Ensure your Unity project structure is valid
4. Check that all assets have corresponding .meta files

## Unity Version

Current Unity version: **6000.2.2f1**

Update the version in all workflow files if you upgrade Unity.