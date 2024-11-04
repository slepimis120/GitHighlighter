#!/bin/bash

# Get the current directory
current_dir=$(pwd)

# Run the PowerShell script directly
powershell -ExecutionPolicy ByPass -NoProfile -File "$current_dir/runVisualStudio.ps1"