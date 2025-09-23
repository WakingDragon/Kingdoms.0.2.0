#!/bin/bash

# Unity License Validation Script
# This script helps validate that Unity license secrets are properly configured

echo "🎮 Unity License Validation Script"
echo "=================================="
echo ""

# Check if we're running in GitHub Actions
if [ "$GITHUB_ACTIONS" = "true" ]; then
    echo "✅ Running in GitHub Actions environment"
    
    # Check each required secret
    secrets_ok=true
    
    if [ -z "$UNITY_EMAIL" ]; then
        echo "❌ UNITY_EMAIL secret is not set or empty"
        secrets_ok=false
    else
        echo "✅ UNITY_EMAIL is configured"
    fi
    
    if [ -z "$UNITY_PASSWORD" ]; then
        echo "❌ UNITY_PASSWORD secret is not set or empty"
        secrets_ok=false
    else
        echo "✅ UNITY_PASSWORD is configured"
    fi
    
    if [ -z "$UNITY_LICENSE" ]; then
        echo "❌ UNITY_LICENSE secret is not set or empty"
        secrets_ok=false
    else
        echo "✅ UNITY_LICENSE is configured (${#UNITY_LICENSE} characters)"
        
        # Basic validation of license format
        if echo "$UNITY_LICENSE" | grep -q "<?xml"; then
            echo "✅ License appears to be in XML format (expected for .ulf files)"
        else
            echo "⚠️  License doesn't appear to be in XML format - please verify it's a .ulf file"
        fi
        
        if echo "$UNITY_LICENSE" | grep -q "UnityLicenseVersion"; then
            echo "✅ License contains Unity license version information"
        else
            echo "⚠️  License might not be a valid Unity license file"
        fi
    fi
    
    echo ""
    if [ "$secrets_ok" = true ]; then
        echo "🎉 All Unity license secrets are properly configured!"
        echo "You should now be able to run Unity workflows successfully."
    else
        echo "❌ Some Unity license secrets are missing or invalid."
        echo "Please check your GitHub repository secrets configuration."
        exit 1
    fi
    
else
    echo "ℹ️  This script is designed to run in GitHub Actions."
    echo "To use it locally, you would need to set the environment variables:"
    echo "  export UNITY_EMAIL='your-email@example.com'"
    echo "  export UNITY_PASSWORD='your-password'"
    echo "  export UNITY_LICENSE='contents-of-your-ulf-file'"
    echo ""
    echo "For GitHub Actions setup, please refer to docs/unity-license-fix.md"
fi

echo ""
echo "📖 For more information, see: docs/unity-license-fix.md"