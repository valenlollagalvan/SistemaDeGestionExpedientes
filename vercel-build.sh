#!/usr/bin/env bash
set -euo pipefail

DOTNET_DIR="${HOME}/.dotnet"
if [ ! -x "${DOTNET_DIR}/dotnet" ]; then
  curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
  bash /tmp/dotnet-install.sh --channel 10.0 --install-dir "${DOTNET_DIR}"
fi

export PATH="${DOTNET_DIR}:${PATH}"
rm -rf vercel-dist
dotnet publish SGE.Frontend/SGE.Frontend.csproj -c Release -o vercel-dist

if [ -n "${API_BASE_URL:-}" ]; then
  sed -i "s|__API_BASE_URL__|${API_BASE_URL}|g" vercel-dist/wwwroot/appsettings.json
fi
