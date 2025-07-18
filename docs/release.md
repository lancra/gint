# Release

New gint releases are initiated by pushing a new version tag to GitHub. Version tags match the pattern `v*`.

```shell
git tag --sign <TAG_NAME>
```

## GitHub Releases

GitHub releases are created from each tag using the refname and the message.

Sample release tag for `v1.0.0`:

```text
v1.0.0

- Add first feature.
- Add second feature.
```

### Pre-Releases

Releases are denoted as "pre-release" when the tag refname contains a hyphen.

Sample prerelease tag for `v1.0.0-preview.0`:

```text
v1.0.0-preview.0

- Add first feature.
- Add second feature.
```

### Skipping Releases

A version tag can be pushed without creating a GitHub release by setting the `Marker` trailer to `true` in the tag message.

Sample non-release version tag for `v1.0.0`:

```text
v1.0.0

- Add first feature.
- Add second feature.

Marker: true
```
