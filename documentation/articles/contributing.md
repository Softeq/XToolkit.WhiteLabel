# Contributing to XToolkit

This document describes contribution guidelines that are specific to XToolkit.

General contribution guidance is included in this document. Additional guidance is defined in the documents linked below.

- [Contribution Workflow](contributing-workflow.md) describes the workflow that the team uses for considering and accepting changes.

## Coding Style Changes

Follow the style used by the [.NET Foundation](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md).

Also:

* **DO NOT** send PRs for style changes. For example, do not send PRs that are focused on changing usage of ```Int32``` to ```int```.
* **DO** give priority to the current style of the project or file you're changing even if it diverges from the general guidelines.

## Pull Requests

We are happy to receive Pull Requests adding new features and solving bugs. As for new features, please contact us before doing major work. To ensure you are not working on something that will be rejected due to not fitting into the XToolkit [roadmap](https://github.com/Softeq/XToolkit.WhiteLabel/wiki/Roadmap) or ideal of the framework.

* **DO** submit all code changes via pull requests (PRs) rather than through a direct commit. PRs will be reviewed and potentially merged by the repo maintainers after a peer review that includes at least one maintainer.
* **DO** give PRs short-but-descriptive names (e.g. "Improve code coverage for System.Console by 10%", not "Fix #1234")
* **DO** refer to any relevant issues, and include [keywords](https://help.github.com/articles/closing-issues-via-commit-messages/) that automatically close issues when the PR is merged.
* **DO** tag any users that should know about and/or review the change.
* **DO** ensure each commit successfully builds.  The entire PR must pass all tests in the Continuous Integration (CI) system before it'll be merged.
* **DO** address PR feedback in an additional commit(s) rather than amending the existing commits, and only rebase/squash them when necessary.  This makes it easier for reviewers to track changes.
* **DO** assume that ["Squash and Merge"](https://github.com/blog/2141-squash-your-commits) will be used to merge your commit unless you request otherwise in the PR.
* **DO NOT** mix independent, unrelated changes in one PR. Separate real product/test code changes from larger code formatting/dead code removal changes. Separate unrelated fixes into separate PRs, especially if they are in different assemblies.

## Commit Messages

Please format commit messages as follows (based on [A Note About Git Commit Messages](http://tbaggery.com/2008/04/19/a-note-about-git-commit-messages.html)):

```
Summarize change in 50 characters or less

Provide more detail after the first line. Leave one blank line below the summary and wrap all lines at 72 characters or less.

If the change fixes an issue, leave another blank line after the final paragraph and indicate which issue is fixed in the specific format below.

Fix #42
```

Also do your best to factor commits appropriately, not too large with unrelated things in the same commit, and not too small with the same small change applied N times in N different commits.

## File Headers

The following file header is the used for Xtoolkit. Please use it for new files:

    // Developed by Softeq Development Corporation
    // http://www.softeq.com

Also, you can use bash script for set headers: `tools/set-headers.sh`