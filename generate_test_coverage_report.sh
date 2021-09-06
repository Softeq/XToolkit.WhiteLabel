#!/bin/bash

GIT_HASH=$(git rev-parse HEAD)
COVERAGE_REPORT_PATH=temp_coverage_report_$GIT_HASH


# install tools
if ! dotnet tool list -g  | grep  "dotnet-reportgenerator-globaltool" > /dev/null;
then 
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# run tests
dotnet test \
    --filter "(FullyQualifiedName!~Droid.Tests)&(FullyQualifiedName!~iOS.Tests)" \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=cobertura \
    /p:CoverletOutput=$COVERAGE_REPORT_PATH/

# create reports
reportgenerator \
    -reports:*.Tests/$COVERAGE_REPORT_PATH/coverage.cobertura.xml \
    -targetdir:coverage_report \
    -reporttypes:Html

# cleanup
rm -rf `find . -path "./*.Tests/${COVERAGE_REPORT_PATH}"`

# open report in default browser
open coverage_report/index.html
