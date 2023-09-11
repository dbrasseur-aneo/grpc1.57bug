#!/usr/bin/env bash

set -e

python -m venv ./env
source ./env/bin/activate
python -m pip install -r requirement.txt

python -m grpc_tools.protoc -I . --proto_path=. --python_out=. --grpc_python_out=. testsrv.proto

dotnet build

