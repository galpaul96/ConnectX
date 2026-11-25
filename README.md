# ConnectX

Workspace scaffold for a future .NET application.

## Structure

- `src/` - Future .NET solution and application projects.
- `tests/` - Future automated test projects.
- `docs/` - Project notes and supporting documentation.

## Web Container Image

[![ConnectX Web Registry](https://github.com/galpaul96/ConnectX/actions/workflows/connectx-registry.yml/badge.svg?branch=main)](https://github.com/galpaul96/ConnectX/actions/workflows/connectx-registry.yml)

The ConnectX Web image is published to [GitHub Container Registry](https://github.com/galpaul96/ConnectX/pkgs/container/connectx-web):

```text
ghcr.io/galpaul96/connectx-web
```

Pull the latest image published from `main`:

```sh
docker pull ghcr.io/galpaul96/connectx-web:latest
```

Images are built and pushed on each push to `main`. The workflow can also be run manually from the [GitHub Actions page](https://github.com/galpaul96/ConnectX/actions/workflows/connectx-registry.yml).

Available tags:

- `latest` points to the latest image published from `main`.
- `DDMMYY.N` is the build date in the `Europe/Bucharest` time zone followed by that day's sequence number, such as `251126.1`. The first published image of a day gets `.1`, then `.2`, and so on.
- `sha-<12-character-sha>` identifies the commit used to build the image.
