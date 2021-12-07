# NuGet Release Process

## Development

- Internal members:
  - Checkout from master
- External members:
  - Clone repository
- Create PR to the `master` branch

## Release XToolkit components

- Create `release/beta4` branch
- [Compare changes](https://github.com/Softeq/XToolkit.WhiteLabel/compare) (last tag <- master):
- Update all `nuget/*.nuspec` (metadata * dependencies)
- Add changes about components versions to the Doc: [NuGet releases](https://docs.google.com/spreadsheets/d/1O3aUTIkiBffVMRg9qwfqUxqlXHCMx5qV_M1k2LgPrMY/edit?usp=sharing)
- Push `release/beta4` branch to the GitHub
- Create PR to `master` branch [New beta version 4]
- Check CI validations & PR reviews (Azure Pipelines will automatically build packages)
- After CI builds manual release will be available (release artifacts)
- Manually run releases for selected components (Azure DevOps) (will be published to NuGet)
- Merge `release/<version>` branch to the `master`
- Create tag (for example`v2.0.0-beta4`)

### New milestone

- Open [Milestones](https://github.com/Softeq/XToolkit.WhiteLabel/milestones)
- Rename `vNext` milestone to `v2 beta N`, with text: [ All issues related to release v2.0 beta N of the WhiteLabel and components. ]
- Close `v2 beta N` milestone
- Create a new `vNext` milestone, with text: [ All issues related to release a future version of the WhiteLabel and components. ]

### GitHub release

- Create new Github WL Release (based on previews with new version tag)

## Update README

- Update `README.md`
- Create new PR to `master` branch

## Update Documentation

- Checkout to `master` branch
- Make changes in `documentation/*` folder
- Build documentation via `cd documentation && ./build.sh`
- Publish `_site` folder to `gh-pages` branch

---
