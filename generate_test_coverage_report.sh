#!/bin/bash

# install tools
if ! dotnet tool list -g  | grep  "dotnet-reportgenerator-globaltool" > /dev/null;
then
    dotnet tool install -g dotnet-reportgenerator-globaltool
fi

# run tests
test_projects=(
    Softeq.XToolkit.Common.Tests
    Softeq.XToolkit.Bindings.Tests
    Softeq.XToolkit.WhiteLabel.Tests
    Softeq.XToolkit.Remote.Tests
)

for test_project in "${test_projects[@]}"
do
    dotnet test $test_project --collect:"XPlat Code Coverage;Format=cobertura"
done

# create reports
reportgenerator \
    -reports:*.Tests/TestResults/*/coverage.cobertura.xml \
    -targetdir:coverage_report \
    -reporttypes:Html

# cleanup
rm -rf $(find . -path "./*.Tests/TestResults")

# open report in default browser
open coverage_report/index.html
