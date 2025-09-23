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

### 2. `dev-build.yml` - Development Build Test  
**Triggers:** Manual dispatch with platform selection

**Purpose:** On-demand builds for development and testing.

**What it does:**
- Choose specific platform to build
- Optional test execution
- Quick feedback for development iterations

### 3. `unity-smoke.yml` - Unity Smoke Build
**Triggers:** Pushes and PRs to `staging-CI` branch

**Purpose:** Quick smoke test for basic Unity functionality.

**What it does:**
- Fast Windows build verification
- Minimal setup for quick feedback

### 4. `hello.yml` - Quick Validation
**Triggers:** Manual dispatch, pushes to main/staging-CI, PRs to main

**Purpose:** Quick validation of Unity project structure and basic checks.

**What it does:**
- Validates Unity project structure
- Checks for essential directories and files
- Reports Unity version and project statistics
- Fast execution for basic validation

### 5. `unity-activate.yml` - Unity Activation (One-time Setup)
**Triggers:** Manual dispatch only

**Purpose:** Generate Unity activation file for first-time setup.

**What it does:**
- Requests Unity activation file
- Uploads `.alf` file as artifact for manual license activation
- Only needs to be run once per repository

### 6. `unity-license-test.yml` - Unity License Test
**Triggers:** Manual dispatch

**Purpose:** Debug Unity license configuration issues.

**What it does:**
- Validates all Unity secrets are configured correctly
- Tests license activation with Unity GameCI
- Provides detailed diagnostics and error reporting

## Setup Instructions

### First Time Setup
1. Run the "Unity Activation" workflow manually
2. Download the `.alf` file from the workflow artifacts
3. Visit [Unity License Management](https://license.unity3d.com/manual) to generate a license
4. Add the following secrets to your GitHub repository:
   - `UNITY_LICENSE`: Your Unity license content (.ulf file)
   - `UNITY_EMAIL`: Email associated with Unity account
   - `UNITY_PASSWORD`: Password for Unity account

### Verifying License Setup
1. Run the "Unity License Test" workflow manually
2. Check the logs to ensure all secrets are properly configured
3. Verify license format and content validation passes

### Using the Staging CI Pipeline
1. Create a branch for your changes
2. Make your changes and commit them
3. Create a PR targeting the `staging-CI` branch
4. The CI pipeline will automatically run
5. Review the build results and test results
6. If all checks pass, merge to `staging-CI`

## Recent Unity License Fix

**Issue Fixed:** Unity license activation errors in GameCI actions.

**Solution Applied:**
- Added explicit environment variables to Unity GameCI action steps
- Improved Unity 6.x compatibility with custom parameters
- Enhanced error reporting and validation

For detailed information about the fix, see: [Unity License Fix Documentation](../docs/unity-license-fix.md)

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

1. **Unity License Activation Errors**
   - Error: "No valid license activation strategy could be determined"
   - Solution: Run the "Unity License Test" workflow to diagnose
   - Verify all three secrets (`UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`) are set
   - Ensure license file content is complete (.ulf file)

2. **Missing Unity License**
   - Error: "License activation failed"
   - Solution: Ensure Unity license secrets are properly configured

3. **Build Failures**
   - Check the build logs in the GitHub Actions tab
   - Verify all required dependencies are included
   - Check for platform-specific issues

4. **Test Failures** 
   - Review test results in the artifacts
   - Check for missing references or broken scripts
   - Verify test setup in Unity Test Framework

### Getting Help

If you encounter issues with the CI pipeline:
1. Check the workflow run logs for detailed error messages
2. Run the "Unity License Test" workflow to validate configuration
3. Verify all required secrets are configured
4. Ensure your Unity project structure is valid
5. Check that all assets have corresponding .meta files

## Unity Version

Current Unity version: **6000.2.2f1** (Unity 6.x)

**Unity 6.x Compatibility:**
- Workflows include Unity 6.x specific configurations
- GameCI actions updated with compatibility parameters
- Enhanced logging for better diagnostics

Update the version in all workflow files if you upgrade Unity.

## Additional Resources

- [Unity License Fix Documentation](../docs/unity-license-fix.md) - Detailed fix explanation
- [License Validation Script](../scripts/validate-unity-license.sh) - Helper script for diagnostics
- [GameCI Documentation](https://game.ci/) - GameCI Unity actions documentation
- [Unity 6.x Documentation](https://docs.unity3d.com/6000.0/Documentation/Manual/index.html)