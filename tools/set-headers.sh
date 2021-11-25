#!/bin/bash

# How to use:
#   tools/set-headers.sh <text> <path>
#      text - text for find (exclude header duplicates)
#      path - path to file with header text
# Example:
#   sh tools/set-headers.sh "Developed" header.txt

for i in $(find . -name "*.cs" ! -path '*obj*' ! -path '*bin*' ! -name '*.designer.cs' ! -name 'AssemblyInfo.cs')
do
  if ! grep -q "$1" $i
  then
    cat $2 $i > $i.new && mv $i.new $i
  fi
done
