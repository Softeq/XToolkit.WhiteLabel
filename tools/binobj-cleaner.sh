#!/bin/bash

# How to use:
#   tools/binobj-cleaner.sh <path>
#      path - root path and cleanup entry point
# Example:
#   sh tools/binobj-cleaner.sh .

if [ -z $1 ] ; then
    echo "Root path argument is required!"
    exit 1
else
    folders_to_remove=$(find $1 -type d \( -name bin -or -name obj \))

    if [[ $folders_to_remove = "" ]]; then
        echo "No bin or obj folders to remove"
    else
        echo "Removing folders:"
        echo "$folders_to_remove"

        rm -rf $folders_to_remove
    fi
fi