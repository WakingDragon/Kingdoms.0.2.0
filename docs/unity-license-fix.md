# Unity License Activation Fix

This document explains the changes made to fix Unity license activation issues in GitHub Actions workflows.

## Issue Description

The GitHub Actions workflows were failing with the error:
```
No valid license activation strategy could be determined. Make sure to provide UNITY_EMAIL, UNITY_PASSWORD, and either a UNITY_SERIAL or UNITY_LICENSE.
```

## Changes Made

### 1. Fixed Environment Variable Passing

The environment variables `UNITY_LICENSE`, `UNITY_EMAIL`, and `UNITY_PASSWORD` were defined globally but were not being passed correctly to the GameCI Unity actions. 

**Before:**
```yaml
- name: Run Unity Tests
  uses: game-ci/unity-test-runner@v4
  with:
    unityVersion: 6000.2.2f1
    # ... other parameters
```

**After:**
```yaml
- name: Run Unity Tests
  uses: game-ci/unity-test-runner@v4
  env:
    UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
    UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
    UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
  with:
    unityVersion: 6000.2.2f1
    # ... other parameters
```

### 2. Added Unity 6.x Compatibility Parameters

Unity 6.0 is relatively new and may require additional configuration for GameCI compatibility. Added `customParameters: -logfile -` to improve logging and compatibility.

### 3. Updated Affected Workflows

The following workflow files were updated:
- `.github/workflows/staging-ci.yml`
- `.github/workflows/dev-build.yml`
- `.github/workflows/unity-smoke.yml`
- `.github/workflows/unity-license-test.yml` (new test workflow)

## GitHub Secrets Setup

Make sure you have the following secrets configured in your GitHub repository:

### Required Secrets

1. **UNITY_EMAIL** - Your Unity Hub account email
2. **UNITY_PASSWORD** - Your Unity Hub account password
3. **UNITY_LICENSE** - Your Unity license file content

### How to Set Up Unity License Secret

1. **Get your Unity license file:**
   - Run the "Unity Activation (one-time)" workflow to generate a `.alf` file
   - Submit the `.alf` file to Unity to get a `.ulf` license file
   - OR use an existing Unity license file from your local Unity installation

2. **Add the license to GitHub Secrets:**
   - Go to your repository → Settings → Secrets and variables → Actions
   - Click "New repository secret"
   - Name: `UNITY_LICENSE`
   - Value: Paste the entire contents of your `.ulf` file

3. **Add Unity Hub credentials:**
   - Add `UNITY_EMAIL` with your Unity Hub email
   - Add `UNITY_PASSWORD` with your Unity Hub password

## Testing the Fix

A new workflow `unity-license-test.yml` has been added to help debug license issues:

1. Go to Actions tab in your repository
2. Find "Unity License Test" workflow
3. Click "Run workflow" to test license activation
4. Check the logs to verify all secrets are configured correctly

## Unity Version Compatibility

This project uses Unity 6000.2.2f1 (Unity 6.x). If you encounter continued issues:

1. **Check GameCI compatibility:** Unity 6.x support in GameCI may still be evolving
2. **Consider downgrading:** Temporarily use Unity 2023.x LTS if critical
3. **Update GameCI actions:** Check for newer versions of the actions

## Troubleshooting

If you still encounter license issues:

1. **Verify secrets are set correctly** - Run the license test workflow
2. **Check Unity license format** - Ensure the license file content is complete
3. **Validate Unity Hub credentials** - Ensure email/password are correct
4. **Check Unity version compatibility** - Unity 6.x may need specific handling

## Additional Resources

- [GameCI Documentation](https://game.ci/docs/github/activation)
- [Unity 6.x Release Notes](https://unity.com/releases/editor/whats-new/6000.2.2)
- [GitHub Actions Secrets Documentation](https://docs.github.com/en/actions/security-guides/encrypted-secrets)